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
    Task<ScrapeJobDto> Create(ScrapeJobDto scrapeJob);
    IEnumerable<ScrapeJobDto> ReadAll();
    Task<IEnumerable<ScrapeJobDto>> ReadUsersScrapeJobs(ClaimsPrincipal principal);
    Task<ScrapeJobDto> Read(uint id, ClaimsPrincipal principal);
    Task<ScrapeJobDto> Update(ScrapeJobDto scrapeJob, ClaimsPrincipal principal);
    Task<bool> Delete(uint id, ClaimsPrincipal principal);
    Task<ScrapeJobDto> ToggleEnabled(uint id, ClaimsPrincipal principal);
  }
}
