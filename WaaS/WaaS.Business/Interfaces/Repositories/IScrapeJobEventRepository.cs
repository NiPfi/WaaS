using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WaaS.Business.Entities;

namespace WaaS.Business.Interfaces.Repositories
{
  public interface IScrapeJobEventRepository : IRepository<ScrapeJobEvent, uint>
  {

    IQueryable<ScrapeJobEvent> ReadScrapeJobEventsOfScrapeJob(uint scrapeJobId);

  }
}
