using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WaaS.Business.Entities;

namespace WaaS.Business.Interfaces.Services.Domain
{
  public interface IScrapeJobEventDomainService
  {
    Task<bool> Create(ScrapeJobEvent scrapeJobEvent);
    Task GetAsync(long id);
  }
}
