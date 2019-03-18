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
  class ScrapeJobService : IScrapeJobService
  {

    private readonly IScrapeJobRepository _scrapeJobsRepository;
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
      _scrapeJobsRepository = scrapeJobsRepository;
    }

    public async Task<ScrapeJobDto> Create(ScrapeJobDto scrapeJob)
    {

      if(string.IsNullOrEmpty(scrapeJob.Url) || string.IsNullOrEmpty(scrapeJob.Pattern)){

        var entity = _mapper.Map<ScrapeJob>(scrapeJob);

        var success = await _scrapeJobsRepository.Add(entity);

        if (success)
        {
          return _mapper.Map<ScrapeJobDto>(entity);
        }

      }

      return null;

    }

    public async Task<bool> Delete(uint id)
    {
      //TODO check if dto is one of current user

      return await _scrapeJobsRepository.Delete(id);
    }

    public async Task<ScrapeJobDto> Read(uint id)
    {
      //TODO check if dto is one of current user

      var entity = await _scrapeJobsRepository.Get(id);

      if(entity != null)
      {
        return _mapper.Map<ScrapeJobDto>(entity);
      }

      return null;

    }

    public IEnumerable<ScrapeJobDto> ReadAll()
    {

      var entities = _scrapeJobsRepository.GetAll().ToList();

      if (entities.Any())
      {
        return _mapper.Map<IEnumerable<ScrapeJobDto>>(entities);
      }

      return Enumerable.Empty<ScrapeJobDto>();

    }

    public async Task<IEnumerable<ScrapeJobDto>> ReadUsersScrapeJobs(ClaimsPrincipal principal)
    {
      var idUser = await _userManager.GetUserAsync(principal);

      var entities = _scrapeJobsRepository.ReadUsersScrapeJobs(idUser.Id);

      if (entities.Any())
      {
        return _mapper.Map<IEnumerable<ScrapeJobDto>>(entities);
      }

      return Enumerable.Empty<ScrapeJobDto>();

    }

    public async Task<ScrapeJobDto> ToggleEnabled(uint id)
    {
      //TODO check if dto is one of current user

      var success = await _scrapeJobsRepository.Update(id, e => e.Enabled = !e.Enabled);

      if (success)
      {
        var updatedEntity = await _scrapeJobsRepository.Get(id);
        return _mapper.Map<ScrapeJobDto>(updatedEntity);
      }

      return null;

    }

    public async Task<ScrapeJobDto> Update(ScrapeJobDto scrapeJob)
    {
      //TODO check if dto is one of current user
      
      var success = await _scrapeJobsRepository.Update(scrapeJob.Id, e => e = _mapper.Map(scrapeJob, e));

      if (success)
      {
        var updatedEntity = await _scrapeJobsRepository.Get(scrapeJob.Id);
        return _mapper.Map<ScrapeJobDto>(updatedEntity);
      }

      return null;

    }

    #region private methods
    private bool IsOfCurrentUser(string userId, ScrapeJobDto scrapeJob)
    {
      throw new NotImplementedException();
    }

    #endregion

  }
}
