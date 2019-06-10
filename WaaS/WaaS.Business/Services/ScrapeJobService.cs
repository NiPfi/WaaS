using System;
using System.Collections;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using WaaS.Business.Dtos;
using WaaS.Business.Dtos.ScrapeJob;
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
    private readonly IEmailService _emailService;

    public ScrapeJobService(
      IMapper mapper,
      UserManager<IdentityUser> userManager,
      IScraper scraper,
      IScrapeJobEventDomainService scrapeJobEventDomainService,
      IScrapeJobDomainService scrapeJobDomainService,
      IUnitOfWork unitOfWork,
      IEmailService emailService)
    {
      _mapper = mapper;
      _userManager = userManager;
      _scraper = scraper;
      _scrapeJobEventDomainService = scrapeJobEventDomainService;
      _scrapeJobDomainService = scrapeJobDomainService;
      _unitOfWork = unitOfWork;
      _emailService = emailService;
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
          await ExecuteScrapeJobAsync(entity);
          return _mapper.Map<ScrapeJobDto>(entity);
        }

      }

      return null;

    }

    public async Task<bool> Delete(long userSpecificId, ClaimsPrincipal principal)
    {
      var idUser = await _userManager.GetUserAsync(principal);
      var scrapeJob = GetScrapeJobByUserSpecificId(userSpecificId, idUser.Id);

      if (scrapeJob != null)
      {
        await _scrapeJobDomainService.DeleteAsync(scrapeJob.Id);
        return await _unitOfWork.CommitAsync();
      }
      else
      {
        return false;
      }

    }

    public async Task<ScrapeJob> ReadUserScrapeJob(long userSpecificId, ClaimsPrincipal principal)
    {
      var idUser = await _userManager.GetUserAsync(principal);
      var scrapeJob = GetScrapeJobByUserSpecificId(userSpecificId, idUser.Id);

      if (scrapeJob != null)
      {
        return scrapeJob;
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
    

    public async Task<ScrapeJobDto> Update(ScrapeJobDto scrapeJobDto, ClaimsPrincipal principal)
    {
      var idUser = await _userManager.GetUserAsync(principal);
      var scrapeJob = GetScrapeJobByUserSpecificId(scrapeJobDto.Id, idUser.Id);

      if (scrapeJob != null)
      {
        await _scrapeJobDomainService.UpdateAsync(scrapeJob.Id, e => _mapper.Map(scrapeJobDto, e));
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
        result.Type = ScrapeJobEventType.Error;
        result.Message = ex.Message;
        result.Url = scrapeJob.Url;
        result.TimeStamp = DateTime.UtcNow;
      }
      result.ScrapeJobForeignKey = scrapeJob.Id;
      result.ScrapeJob = scrapeJob;

      if (result.Type.Equals(ScrapeJobEventType.Match))
      {
        await SendScrapeSuccessEmail(result);
      }

      await _scrapeJobEventDomainService.AddAsync(result);
      return await _unitOfWork.CommitAsync();

    }

    private async Task SendScrapeSuccessEmail(ScrapeJobEvent result)
    {
      var email = string.IsNullOrWhiteSpace(result.ScrapeJob.AlternativeEmail) ? result.ScrapeJob.AlternativeEmail : result.ScrapeJob.IdentityUser.Email;

      await _emailService.SendScrapeSuccessAsync(email, result);
    }

    public async Task<IEnumerable<ScrapeJobStatusDto>> ReadUsersScrapeJobsStatusAsync(ClaimsPrincipal principal)
    {
      var idUser = await _userManager.GetUserAsync(principal);
      var jobs = _scrapeJobDomainService.ReadUsersEnabledScrapeJobs(idUser.Id);
      List<ScrapeJobStatusDto> resultList = new List<ScrapeJobStatusDto>();
      foreach (ScrapeJob scrapeJob in jobs)
      {
        var scrapeJobEvent = _scrapeJobEventDomainService.ReadLatestScrapeJobEventOfScrapeJob(scrapeJob.Id);
        if (scrapeJobEvent != null)
        {
          var statusDto = new ScrapeJobStatusDto
          {
            ScrapeJobId = scrapeJob.UserSpecificId,
            StatusCode = scrapeJobEvent.Type
          };
          resultList.Add(statusDto);
        }
      }

      return resultList;
    }

    private ScrapeJob GetScrapeJobByUserSpecificId(long userSpecificId, string userId)
    {
      return _scrapeJobDomainService.GetAll().Where(j => j.UserSpecificId == userSpecificId && j.IdentityUserForeignKey == userId).FirstOrDefault();
    }

  }
}
