using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WaaS.Business.Dtos;
using WaaS.Business.Interfaces.Services;

namespace WaaS.Business.Services
{
  public class ScrapeJobEventService : IScrapeJobEventService
  {
    public Task<ScrapeJobEventDto> Create(ScrapeJobEventDto scrapeJobEvent)
    {
      throw new NotImplementedException();
    }

    public Task<bool> Delete(uint id)
    {
      throw new NotImplementedException();
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
