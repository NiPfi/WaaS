using System;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WaaS.Business.Dtos;
using WaaS.Business.Entities;
using WaaS.Business.Interfaces.Repositories;
using WaaS.Business.Interfaces.Services;

namespace WaaS.Business.Services
{
  public class ScrapeJobService : IScrapeJobService
  {

    private readonly IScrapeJobRepository _scrapeJobRepository;
    private readonly IMapper _mapper;
    private readonly UserManager<IdentityUser> _userManager;

    public ScrapeJobService
      (
      IMapper mapper,
      IScrapeJobRepository scrapeJobsRepository,
      UserManager<IdentityUser> userManager
      )
    {
      _mapper = mapper;
      _userManager = userManager;
      _scrapeJobRepository = scrapeJobsRepository;
    }

    public async Task<ScrapeJobDto> Create(ScrapeJobDto scrapeJob, ClaimsPrincipal principal)
    {

      if (scrapeJob != null && (!string.IsNullOrEmpty(scrapeJob.Url) && !string.IsNullOrEmpty(scrapeJob.Pattern)))
      {

        var entity = _mapper.Map<ScrapeJob>(scrapeJob);
        var idUser = await _userManager.GetUserAsync(principal);
        entity.IdentityUserForeignKey = idUser.Id;
        var highestId = _scrapeJobRepository
          .ReadUsersScrapeJobs(idUser.Id)
          .OrderByDescending(j => j.UserSpecificId)
          .FirstOrDefault()?.UserSpecificId;
        entity.UserSpecificId = (highestId ?? 0) + 1;

        var success = await _scrapeJobRepository.AddAsync(entity);

        if (success)
        {
          return _mapper.Map<ScrapeJobDto>(entity);
        }

      }

      return null;

    }

    public async Task<bool> Delete(long id, ClaimsPrincipal principal)
    {
      var idUser = await _userManager.GetUserAsync(principal);
      if (await ScrapeJobIsOfUser(id, idUser.Id))
      {
        return await _scrapeJobRepository.DeleteAsync(id);
      }

      return false;

    }

    public async Task<ScrapeJobDto> Read(long id, ClaimsPrincipal principal)
    {
      var idUser = await _userManager.GetUserAsync(principal);
      if (await ScrapeJobIsOfUser(id, idUser.Id))
      {
        var entity = await _scrapeJobRepository.GetAsync(id);

        if (entity != null)
        {
          return _mapper.Map<ScrapeJobDto>(entity);
        }
      }

      return null;

    }

    public IEnumerable<ScrapeJobDto> ReadAll()
    {

      var entities = _scrapeJobRepository.GetAll().ToList();

      if (entities.Any())
      {
        return _mapper.Map<IEnumerable<ScrapeJobDto>>(entities);
      }

      return Enumerable.Empty<ScrapeJobDto>();

    }

    public async Task<IEnumerable<ScrapeJobDto>> ReadUsersScrapeJobs(ClaimsPrincipal principal)
    {
      var idUser = await _userManager.GetUserAsync(principal);

      var entities = _scrapeJobRepository.ReadUsersScrapeJobs(idUser.Id);

      if (entities.Any())
      {
        return _mapper.Map<IEnumerable<ScrapeJobDto>>(entities);
      }

      return Enumerable.Empty<ScrapeJobDto>();

    }

    public async Task<ScrapeJobDto> ToggleEnabled(long id, ClaimsPrincipal principal)
    {
      var idUser = await _userManager.GetUserAsync(principal);
      if (await ScrapeJobIsOfUser(id, idUser.Id))
      {
        var success = await _scrapeJobRepository.UpdateAsync(id, e => e.Enabled = !e.Enabled);

        if (success)
        {
          var updatedEntity = await _scrapeJobRepository.GetAsync(id);
          return _mapper.Map<ScrapeJobDto>(updatedEntity);
        }
      }

      return null;

    }

    public async Task<ScrapeJobDto> Update(ScrapeJobDto scrapeJob, ClaimsPrincipal principal)
    {
      var idUser = await _userManager.GetUserAsync(principal);
      if (await ScrapeJobIsOfUser(scrapeJob.Id, idUser.Id))
      {
        var success = await _scrapeJobRepository.UpdateAsync(scrapeJob.Id, e => _mapper.Map(scrapeJob, e));

        if (success)
        {
          var updatedEntity = await _scrapeJobRepository.GetAsync(scrapeJob.Id);
          return _mapper.Map<ScrapeJobDto>(updatedEntity);
        }
      }

      return null;

    }

    public async Task<bool> ScrapeJobIsOfUser(long scrapeJobId, string userId)
    {
      var scrapeJobEntity = await _scrapeJobRepository.GetAsync(scrapeJobId);

      return userId.Equals(scrapeJobEntity.IdentityUser.Id, StringComparison.InvariantCulture);
    }


  }
}
