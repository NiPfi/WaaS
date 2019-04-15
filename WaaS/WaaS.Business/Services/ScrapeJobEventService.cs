using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WaaS.Business.Dtos;
using WaaS.Business.Entities;
using WaaS.Business.Interfaces.Repositories;
using WaaS.Business.Interfaces.Services;

namespace WaaS.Business.Services
{
  public class ScrapeJobEventService : IScrapeJobEventService
  {

    private readonly IScrapeJobEventRepository _scrapeJobEventRepository;
    private readonly IMapper _mapper;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IScrapeJobService _scrapeJobService;

    public ScrapeJobEventService
      (
      IMapper mapper,
      IScrapeJobEventRepository scrapeJobEventRepository,
      UserManager<IdentityUser> userManager,
      IScrapeJobService scrapeJobService
      )
    {
      _mapper = mapper;
      _scrapeJobEventRepository = scrapeJobEventRepository;
      _userManager = userManager;
      _scrapeJobService = scrapeJobService;
    }


    public async Task<ScrapeJobEventDto> Create(ScrapeJobEventDto scrapeJobEvent)
    {
      if (scrapeJobEvent != null
          && !string.IsNullOrEmpty(scrapeJobEvent.Message)
          && scrapeJobEvent.TimeStamp != null)
      {

        var entity = _mapper.Map<ScrapeJobEvent>(scrapeJobEvent);

        var success = await Create(entity);

        if (success)
        {
          return _mapper.Map<ScrapeJobEventDto>(entity);
        }

      }

      return null;
    }

    async Task<bool> Create(ScrapeJobEvent scrapeJobEvent)
    {
      var result = false;
      if (scrapeJobEvent != null)
      {
        result = await _scrapeJobEventRepository.AddAsync(scrapeJobEvent);
      }
      return result;
    }

    public async Task<bool> Delete(long id, ClaimsPrincipal principal)
    {
      var idUser = await _userManager.GetUserAsync(principal);
      var entity = await _scrapeJobEventRepository.GetAsync(id);

      if (entity != null && await _scrapeJobService.ScrapeJobIsOfUser(entity.ScrapeJob.Id, idUser.Id))
      {
        return await _scrapeJobEventRepository.DeleteAsync(id);
      }

      return false;

    }

    public async Task<ScrapeJobEventDto> Read(long id, ClaimsPrincipal principal)
    {
      var idUser = await _userManager.GetUserAsync(principal);
      var entity = await _scrapeJobEventRepository.GetAsync(id);

      if (entity != null && await _scrapeJobService.ScrapeJobIsOfUser(entity.ScrapeJob.Id, idUser.Id))
      {
        return _mapper.Map<ScrapeJobEventDto>(entity);
      }

      return null;
    }

    public async Task<IEnumerable<ScrapeJobEventDto>> ReadScrapeJobEventsOfScrapeJob(long scrapeJobId, ClaimsPrincipal principal)
    {

      var scrapeJob = await _scrapeJobService.ReadUserScrapeJob(scrapeJobId, principal);

      if(scrapeJob != null)
      {
        var entities = _scrapeJobEventRepository.ReadScrapeJobEventsOfScrapeJob(scrapeJob.Id);

        if (entities.Any())
        {
          return _mapper.Map<IEnumerable<ScrapeJobEventDto>>(entities);
        }

      }

      return Enumerable.Empty<ScrapeJobEventDto>();
    }

    public async Task<ScrapeJobEventDto> Update(ScrapeJobEventDto scrapeJobEvent, ClaimsPrincipal principal)
    {
      var idUser = await _userManager.GetUserAsync(principal);
      var entity = await _scrapeJobEventRepository.GetAsync(scrapeJobEvent.Id);

      if (entity != null && await _scrapeJobService.ScrapeJobIsOfUser(entity.ScrapeJob.Id, idUser.Id))
      {
        var success = await _scrapeJobEventRepository.UpdateAsync(scrapeJobEvent.Id, e => e = _mapper.Map(scrapeJobEvent, e));

        if (success)
        {
          var updatedEntity = await _scrapeJobEventRepository.GetAsync(scrapeJobEvent.Id);
          return _mapper.Map<ScrapeJobEventDto>(updatedEntity);
        }
      }

      return null;
    }


  }
}
