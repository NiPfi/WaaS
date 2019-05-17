using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WaaS.Business.Entities;

namespace WaaS.Business.Interfaces.Services.Domain
{
  public interface IScrapeJobEventDomainService: IBaseDomainService<ScrapeJobEvent, long>
  {
    IEnumerable<ScrapeJobEvent> ReadScrapeJobEventsOfScrapeJob(int scrapeJobId);
  }
}
