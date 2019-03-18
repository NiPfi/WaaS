using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WaaS.Business.Dtos;

namespace WaaS.Business.Interfaces.Services
{
  public interface IScrapeJobEventService
  {
    Task<ScrapeJobEventDto> Create(ScrapeJobEventDto scrapeJobEvent);
    Task<IEnumerable<ScrapeJobEventDto>> ReadScrapeJobsEventsOfScrapeJob(uint scrapeJobId);
    Task<ScrapeJobEventDto> Read(uint id);
    Task<ScrapeJobEventDto> Update(ScrapeJobEventDto scrapeJobEvent);
    Task<bool> Delete(uint id);
  }
}
