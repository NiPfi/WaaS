using System;
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
  public class ScrapeJobService : IScrapeJobService
  {

    private readonly IMapper _mapper;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IScraper _scraper;
    private readonly IScrapeJobDomainService _scrapeJobDomainService;
    private readonly IScrapeJobEventDomainService _scrapeJobEventDomainService;
    private readonly IUnitOfWork _unitOfWork;

    public ScrapeJobService
      (
      IMapper mapper,
      UserManager<IdentityUser> userManager,
      IScraper scraper, IScrapeJobEventDomainService scrapeJobEventDomainService,
      IScrapeJobDomainService scrapeJobDomainService, IUnitOfWork unitOfWork)
    {
      _mapper = mapper;
      _userManager = userManager;
      _scraper = scraper;
      _scrapeJobEventDomainService = scrapeJobEventDomainService;
      _scrapeJobDomainService = scrapeJobDomainService;
      _unitOfWork = unitOfWork;
    }

    public async Task<ScrapeJobDto> Create(ScrapeJobDto scrapeJob, ClaimsPrincipal principal)
    {

      if (scrapeJob != null && (!string.IsNullOrEmpty(scrapeJob.Url) && !string.IsNullOrEmpty(scrapeJob.Pattern)))
      {

        var entity = _mapper.Map<ScrapeJob>(scrapeJob);
        var idUser = await _userManager.GetUserAsync(principal);
        entity.IdentityUserForeignKey = idUser.Id;
        var highestId = _scrapeJobDomainService
          .ReadUsersScrapeJobs(idUser.Id)
          .OrderByDescending(j => j.UserSpecificId)
          .FirstOrDefault()?.UserSpecificId;
        entity.UserSpecificId = (highestId ?? 0) + 1;
        entity.Enabled = true;

        await _scrapeJobDomainService.AddAsync(entity);
        var success = await _unitOfWork.CommitAsync();

        if (success)
        {
          ExecuteScrapeJob(entity);
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
        await _scrapeJobDomainService.DeleteAsync(id);
        return await _unitOfWork.CommitAsync();
      }

      return false;

    }

    public async Task<ScrapeJobDto> ReadUserScrapeJob(long id, ClaimsPrincipal principal)
    {
      var idUser = await _userManager.GetUserAsync(principal);
      if (await ScrapeJobIsOfUser(id, idUser.Id))
      {
        var entity = await _scrapeJobDomainService.GetAsync(id);

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

      var entities = _scrapeJobDomainService.ReadUsersScrapeJobs(idUser.Id);

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
        await _scrapeJobDomainService.UpdateAsync(id, e => e.Enabled = !e.Enabled);
        var success = await _unitOfWork.CommitAsync();

        if (success)
        {
          var updatedEntity = await _scrapeJobDomainService.GetAsync(id);
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
        await _scrapeJobDomainService.UpdateAsync(scrapeJob.Id, e => _mapper.Map(scrapeJob, e));
        var success = await _unitOfWork.CommitAsync();

        if (success)
        {
          var updatedEntity = await _scrapeJobDomainService.GetAsync(scrapeJob.Id);
          return _mapper.Map<ScrapeJobDto>(updatedEntity);
        }
      }

      return null;

    }

    public async Task<bool> ScrapeJobIsOfUser(long scrapeJobId, string userId)
    {
      var scrapeJobEntity = await _scrapeJobDomainService.GetAsync(scrapeJobId);

      if (scrapeJobEntity == null || userId == null)
      {
        return false;
      }

      return userId.Equals(scrapeJobEntity.IdentityUser.Id, StringComparison.InvariantCulture);
    }

    public async Task<bool> ExecuteScrapeJobAsync(ScrapeJob scrapeJob)
    {
      if (scrapeJob == null)
      {
        return false;
      }

      var result = new ScrapeJobEvent();
      
      try
      {
        var url = new Uri(scrapeJob.Url);
        result = await _scraper.ExecuteAsync(url, scrapeJob.Pattern);
      }
      catch (UriFormatException ex)
      {
        await _scrapeJobDomainService.UpdateAsync(scrapeJob.Id, job => job.Enabled = false);

        result.Type = ScrapeJobEventType.Error;
        result.Message = ex.Message;
        result.Url = scrapeJob.Url;
        result.TimeStamp = DateTime.UtcNow;
      }
      result.ScrapeJobForeignKey = scrapeJob.Id;

      await _scrapeJobEventDomainService.AddAsync(result);
      return await _unitOfWork.CommitAsync();

    }

    public void ExecuteScrapeJob(ScrapeJob scrapeJob)
    {
      Task.Factory.StartNew(() => ExecuteScrapeJobAsync(scrapeJob));
    }
  }
}
