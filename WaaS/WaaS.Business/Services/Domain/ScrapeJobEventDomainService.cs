using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WaaS.Business.Entities;
using WaaS.Business.Interfaces.Repositories;
using WaaS.Business.Interfaces.Services.Domain;

namespace WaaS.Business.Services.Domain
{
  class ScrapeJobEventDomainService: IScrapeJobEventDomainService
  {
    private readonly IScrapeJobEventRepository _scrapeJobEventRepository;

    public ScrapeJobEventDomainService
      (
        IScrapeJobEventRepository scrapeJobEventRepository
      )
    {
      _scrapeJobEventRepository = scrapeJobEventRepository;
    }

    public async Task<bool> Create(ScrapeJobEvent scrapeJobEvent)
    {
      var result = false;
      if (scrapeJobEvent != null)
      {
        result = await _scrapeJobEventRepository.AddAsync(scrapeJobEvent);
      }
      return result;
    }
  }
}
