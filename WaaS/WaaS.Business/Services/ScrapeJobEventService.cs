using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WaaS.Business.Dtos;
using WaaS.Business.Entities;
using WaaS.Business.Interfaces;
using WaaS.Business.Interfaces.Services;
using WaaS.Business.Interfaces.Services.Domain;

namespace WaaS.Business.Services
{
  public class ScrapeJobEventService : IScrapeJobEventService
  {
    private readonly IMapper _mapper;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IScrapeJobService _scrapeJobService;
    private readonly IScrapeJobEventDomainService _scrapeJobEventDomainService;
    private readonly IUnitOfWork _unitOfWork;

    public ScrapeJobEventService
      (
        IMapper mapper,
        UserManager<IdentityUser> userManager,
        IScrapeJobService scrapeJobService,
        IScrapeJobEventDomainService scrapeJobEventDomainService, IUnitOfWork unitOfWork)
    {
      _mapper = mapper;
      _userManager = userManager;
      _scrapeJobService = scrapeJobService;
      _scrapeJobEventDomainService = scrapeJobEventDomainService;
      _unitOfWork = unitOfWork;
    }


    public async Task<ScrapeJobEventDto> Create(ScrapeJobEventDto scrapeJobEvent)
    {
      if (scrapeJobEvent != null
          && !string.IsNullOrEmpty(scrapeJobEvent.Message))
      {

        var entity = _mapper.Map<ScrapeJobEvent>(scrapeJobEvent);

        await _scrapeJobEventDomainService.AddAsync(entity);
        var success = await _unitOfWork.CommitAsync();

        if (success)
        {
          return _mapper.Map<ScrapeJobEventDto>(entity);
        }

      }

      return null;
    }

    public async Task<bool> Delete(long id, ClaimsPrincipal principal)
    {
      var idUser = await _userManager.GetUserAsync(principal);
      var entity = await _scrapeJobEventDomainService.GetAsync(id);

      if (entity != null && await _scrapeJobService.ScrapeJobIsOfUser(entity.ScrapeJob.Id, idUser.Id))
      {
        await _scrapeJobEventDomainService.DeleteAsync(id);
        return await _unitOfWork.CommitAsync();
      }

      return false;

    }

    public async Task<ScrapeJobEventDto> Read(long id, ClaimsPrincipal principal)
    {
      var idUser = await _userManager.GetUserAsync(principal);
      var entity = await _scrapeJobEventDomainService.GetAsync(id);

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
        var entities = _scrapeJobEventDomainService.ReadScrapeJobEventsOfScrapeJob(scrapeJob.Id);

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
      var entity = await _scrapeJobEventDomainService.GetAsync(scrapeJobEvent.Id);

      if (entity != null && await _scrapeJobService.ScrapeJobIsOfUser(entity.ScrapeJob.Id, idUser.Id))
      {
        await _scrapeJobEventDomainService.UpdateAsync(scrapeJobEvent.Id, e => _mapper.Map(scrapeJobEvent, e));
        var success = await _unitOfWork.CommitAsync();

        if (success)
        {
          var updatedEntity = await _scrapeJobEventDomainService.GetAsync(scrapeJobEvent.Id);
          return _mapper.Map<ScrapeJobEventDto>(updatedEntity);
        }
      }

      return null;
    }


  }
}
