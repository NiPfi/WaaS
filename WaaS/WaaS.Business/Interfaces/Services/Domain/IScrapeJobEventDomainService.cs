using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WaaS.Business.Entities;

namespace WaaS.Business.Interfaces.Services.Domain
{
  public interface IScrapeJobEventDomainService
  {
    Task<bool> CreateAsync(ScrapeJobEvent scrapeJobEvent);
    Task<ScrapeJobEvent> GetAsync(long id);
    IEnumerable<ScrapeJobEvent> ReadScrapeJobEventsOfScrapeJob(int scrapeJobId);
    Task<bool> UpdateAsync(long id, Action<ScrapeJobEvent> updateAction);
    Task<bool> DeleteAsync(long id);
  }
}
