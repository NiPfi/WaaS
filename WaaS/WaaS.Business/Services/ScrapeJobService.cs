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
        var idUser = await _userManager.GetUserAsync(principal).ConfigureAwait(false);
        entity.IdentityUserForeignKey = idUser.Id;
        entity.UserSpecificId = _scrapeJobRepository.ReadUsersScrapeJobs(idUser.Id).Count() + 1;

        var success = await _scrapeJobRepository.AddAsync(entity).ConfigureAwait(false);

        if (success)
        {
          return _mapper.Map<ScrapeJobDto>(entity);
        }

      }

      return null;

    }

    public async Task<bool> Delete(long id, ClaimsPrincipal principal)
    {
      var idUser = await _userManager.GetUserAsync(principal).ConfigureAwait(false);
      if (await ScrapeJobIsOfUser(id, idUser.Id).ConfigureAwait(false))
      {
        return await _scrapeJobRepository.DeleteAsync(id).ConfigureAwait(false);
      }

      return false;

    }

    public async Task<ScrapeJobDto> Read(long id, ClaimsPrincipal principal)
    {
      var idUser = await _userManager.GetUserAsync(principal).ConfigureAwait(false);
      if (await ScrapeJobIsOfUser(id, idUser.Id).ConfigureAwait(false))
      {
        var entity = await _scrapeJobRepository.GetAsync(id).ConfigureAwait(false);

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
      var idUser = await _userManager.GetUserAsync(principal).ConfigureAwait(false);

      var entities = _scrapeJobRepository.ReadUsersScrapeJobs(idUser.Id);

      if (entities.Any())
      {
        return _mapper.Map<IEnumerable<ScrapeJobDto>>(entities);
      }

      return Enumerable.Empty<ScrapeJobDto>();

    }

    public async Task<ScrapeJobDto> ToggleEnabled(long id, ClaimsPrincipal principal)
    {
      var idUser = await _userManager.GetUserAsync(principal).ConfigureAwait(false);
      if (await ScrapeJobIsOfUser(id, idUser.Id).ConfigureAwait(false))
      {
        var success = await _scrapeJobRepository.UpdateAsync(id, e => e.Enabled = !e.Enabled).ConfigureAwait(false);

        if (success)
        {
          var updatedEntity = await _scrapeJobRepository.GetAsync(id).ConfigureAwait(false);
          return _mapper.Map<ScrapeJobDto>(updatedEntity);
        }
      }

      return null;

    }

    public async Task<ScrapeJobDto> Update(ScrapeJobDto scrapeJob, ClaimsPrincipal principal)
    {
      var idUser = await _userManager.GetUserAsync(principal).ConfigureAwait(false);
      if (await ScrapeJobIsOfUser(scrapeJob.Id, idUser.Id).ConfigureAwait(false))
      {
        var success = await _scrapeJobRepository.UpdateAsync(scrapeJob.Id, e => _mapper.Map(scrapeJob, e)).ConfigureAwait(false);

        if (success)
        {
          var updatedEntity = await _scrapeJobRepository.GetAsync(scrapeJob.Id).ConfigureAwait(false);
          return _mapper.Map<ScrapeJobDto>(updatedEntity);
        }
      }

      return null;

    }

    public async Task<bool> ScrapeJobIsOfUser(long scrapeJobId, string userId)
    {
      var scrapeJobEntity = await _scrapeJobRepository.GetAsync(scrapeJobId).ConfigureAwait(false);

      return userId.Equals(scrapeJobEntity.IdentityUser.Id, StringComparison.InvariantCulture);
    }


  }
}
