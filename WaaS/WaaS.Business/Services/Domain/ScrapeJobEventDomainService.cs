using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WaaS.Business.Entities;
using WaaS.Business.Interfaces.Repositories;
using WaaS.Business.Interfaces.Services.Domain;

namespace WaaS.Business.Services.Domain
{
  public class ScrapeJobEventDomainService: IScrapeJobEventDomainService
  {
    private readonly IScrapeJobEventRepository _scrapeJobEventRepository;

    public ScrapeJobEventDomainService
      (
        IScrapeJobEventRepository scrapeJobEventRepository
      )
    {
      _scrapeJobEventRepository = scrapeJobEventRepository;
    }

    public async Task<bool> CreateAsync(ScrapeJobEvent scrapeJobEvent)
    {
      var result = false;
      if (scrapeJobEvent != null)
      {
        result = await _scrapeJobEventRepository.AddAsync(scrapeJobEvent);
      }
      return result;
    }

    public Task<ScrapeJobEvent> GetAsync(long id)
    {
      return _scrapeJobEventRepository.GetAsync(id);
    }

    public IEnumerable<ScrapeJobEvent> ReadScrapeJobEventsOfScrapeJob(int scrapeJobId)
    {
      return _scrapeJobEventRepository.ReadScrapeJobEventsOfScrapeJob(scrapeJobId);
    }

    public Task<bool> UpdateAsync(long id, Action<ScrapeJobEvent> updateAction)
    {
      return _scrapeJobEventRepository.UpdateAsync(id, updateAction);
    }

    public Task<bool> DeleteAsync(long id)
    {
      throw new NotImplementedException();
    }
  }
}
