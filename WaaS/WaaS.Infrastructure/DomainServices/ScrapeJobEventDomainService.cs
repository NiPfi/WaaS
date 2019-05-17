using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WaaS.Business.Entities;
using WaaS.Business.Interfaces.Services.Domain;

namespace WaaS.Infrastructure.DomainServices
{
  public class ScrapeJobEventDomainService : BaseDomainService<ScrapeJobEvent, long>, IScrapeJobEventDomainService
  {
    public ScrapeJobEventDomainService(WaasDbContext context) : base(context)
    {
    }

    public IEnumerable<ScrapeJobEvent> ReadScrapeJobEventsOfScrapeJob(int scrapeJobId)
    {
      return GetAll().Where(x => x.ScrapeJob.Id.Equals(scrapeJobId));
    }

  }
}
