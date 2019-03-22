using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
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

    public ScrapeJobEventService
      (
      IMapper mapper,
      IScrapeJobEventRepository scrapeJobEventRepository,
      UserManager<IdentityUser> userManager
      )
    {
      _mapper = mapper;
      _scrapeJobEventRepository = scrapeJobEventRepository;
      _userManager = userManager;
    }


    public async Task<ScrapeJobEventDto> Create(ScrapeJobEventDto scrapeJobEvent)
    {
      if (!string.IsNullOrEmpty(scrapeJobEvent.Message) && scrapeJobEvent.TimeStamp != null)
      {

        var entity = _mapper.Map<ScrapeJobEvent>(scrapeJobEvent);

        var success = await _scrapeJobEventRepository.AddAsync(entity);

        if (success)
        {
          return _mapper.Map<ScrapeJobEventDto>(entity);
        }

      }

      return null;
    }

    public async Task<bool> Delete(uint id)
    {
      return await _scrapeJobEventRepository.DeleteAsync(id);
    }

    public Task<ScrapeJobEventDto> Read(uint id)
    {
      throw new NotImplementedException();
    }

    public Task<IEnumerable<ScrapeJobEventDto>> ReadScrapeJobsEventsOfScrapeJob(uint scrapeJobId)
    {
      throw new NotImplementedException();
    }

    public Task<ScrapeJobEventDto> Update(ScrapeJobEventDto scrapeJobEvent)
    {
      throw new NotImplementedException();
    }
  }
}
