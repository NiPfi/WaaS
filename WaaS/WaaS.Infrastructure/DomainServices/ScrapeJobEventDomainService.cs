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

    public IEnumerable<ScrapeJobEvent> ReadScrapeJobEventsOfScrapeJob(long scrapeJobId)
    {
      return GetAll().Where(x => x.ScrapeJob.Id.Equals(scrapeJobId));
    }

    public ScrapeJobEvent ReadLatestScrapeJobEventOfScrapeJob(long scrapeJobId)
    {
      return ReadScrapeJobEventsOfScrapeJob(scrapeJobId).OrderByDescending(e => e.TimeStamp).FirstOrDefault();
    }

  }
}
