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
    private readonly IScrapeJobEventService _scrapeJobEventService;
    private readonly IMapper _mapper;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IScraper _scraper;

    public ScrapeJobService
      (
      IMapper mapper,
      IScrapeJobRepository scrapeJobsRepository,
      UserManager<IdentityUser> userManager,
      IScrapeJobEventService scrapeJobEventService,
      IScraper scraper
      )
    {
      _mapper = mapper;
      _userManager = userManager;
      _scrapeJobRepository = scrapeJobsRepository;
      _scrapeJobEventService = scrapeJobEventService;
      _scraper = scraper;
    }

    public async Task<ScrapeJobDto> Create(ScrapeJobDto scrapeJob, ClaimsPrincipal principal)
    {

      if (scrapeJob != null && (!string.IsNullOrEmpty(scrapeJob.Url) && !string.IsNullOrEmpty(scrapeJob.Pattern)))
      {

        var entity = _mapper.Map<ScrapeJob>(scrapeJob);
        var idUser = await _userManager.GetUserAsync(principal);
        entity.IdentityUserForeignKey = idUser.Id;
        entity.UserSpecificId = _scrapeJobRepository.ReadUsersScrapeJobs(idUser.Id).Count() + 1;

        var success = await _scrapeJobRepository.AddAsync(entity);

        if (success)
        {
          success = await ExecuteScrapeJobAsync(entity);
        }

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

    public async Task<ScrapeJobDto> ReadUserScrapeJob(long id, ClaimsPrincipal principal)
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

    public async Task<IEnumerable<ScrapeJobDto>> ReadAllUserScrapeJobs(ClaimsPrincipal principal)
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

    public async Task<bool> ExecuteScrapeJobAsync(ScrapeJob scrapeJob)
    {
      var url = new Uri(scrapeJob.Url);
      var result = await _scraper.ExecuteAsync(url, scrapeJob.Pattern);
      result.ScrapeJob = scrapeJob;
      result.ScrapeJobForeignKey = scrapeJob.Id;
      return await _scrapeJobEventService.Create(result);
    } 

  }
}
