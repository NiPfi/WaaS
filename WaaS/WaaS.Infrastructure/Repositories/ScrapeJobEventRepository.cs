using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WaaS.Business.Entities;
using WaaS.Business.Interfaces.Repositories;

namespace WaaS.Infrastructure.Repositories
{
  public class ScrapeJobEventRepository : Repository<ScrapeJobEvent, long>, IScrapeJobEventRepository
  {
    public ScrapeJobEventRepository(WaasDbContext context) : base(context)
    {
    }

    public IQueryable<ScrapeJobEvent> ReadScrapeJobEventsOfScrapeJob(long scrapeJobId)
    {
      return GetAll().Where(x => x.ScrapeJob.Id.Equals(scrapeJobId));
    }
  }
}
