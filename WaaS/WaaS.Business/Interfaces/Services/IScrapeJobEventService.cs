using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WaaS.Business.Dtos;

namespace WaaS.Business.Interfaces.Services
{
  public interface IScrapeJobEventService
  {
    Task<ScrapeJobEventDto> Create(ScrapeJobEventDto scrapeJobEvent);
    Task<IEnumerable<ScrapeJobEventDto>> ReadScrapeJobEventsOfScrapeJob(long scrapeJobId, ClaimsPrincipal principal);
    Task<ScrapeJobEventDto> Read(long id, ClaimsPrincipal principal);
    Task<ScrapeJobEventDto> Update(ScrapeJobEventDto scrapeJobEvent, ClaimsPrincipal principal);
    Task<bool> Delete(long id, ClaimsPrincipal principal);
  }
}
