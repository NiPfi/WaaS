using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WaaS.Business.Entities;

namespace WaaS.Business.Interfaces.Repositories
{
  public interface IScrapeJobEventRepository : IRepository<ScrapeJobEvent, long>
  {

    IQueryable<ScrapeJobEvent> ReadScrapeJobEventsOfScrapeJob(long scrapeJobId);

  }
}
