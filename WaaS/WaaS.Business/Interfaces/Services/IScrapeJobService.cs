using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WaaS.Business.Dtos;

namespace WaaS.Business.Interfaces.Services
{
  public interface IScrapeJobService
  {
    Task<ScrapeJobDto> Create(ScrapeJobDto scrapeJob, ClaimsPrincipal principal);
    Task<IEnumerable<ScrapeJobDto>> ReadAllUserScrapeJobs(ClaimsPrincipal principal);
    Task<ScrapeJobDto> ReadUserScrapeJob(long id, ClaimsPrincipal principal);
    Task<ScrapeJobDto> Update(ScrapeJobDto scrapeJob, ClaimsPrincipal principal);
    Task<bool> Delete(long id, ClaimsPrincipal principal);
    Task<ScrapeJobDto> ToggleEnabled(long id, ClaimsPrincipal principal);
    Task<bool> ScrapeJobIsOfUser(long scrapeJobId, string userId);

  }
}
