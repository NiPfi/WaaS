using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WaaS.Business.Dtos;
using WaaS.Business.Dtos.ScrapeJob;
using WaaS.Business.Entities;

namespace WaaS.Business.Interfaces.Services
{
  public interface IScrapeJobService
  {
    Task<ScrapeJobDto> Create(ScrapeJobDto scrapeJob, ClaimsPrincipal principal);
    Task<IEnumerable<ScrapeJobDto>> ReadAllUserScrapeJobs(ClaimsPrincipal principal);
    Task<ScrapeJob> ReadUserScrapeJob(long id, ClaimsPrincipal principal);
    Task<ScrapeJobDto> Update(ScrapeJobDto scrapeJob, ClaimsPrincipal principal);
    Task<bool> Delete(long id, ClaimsPrincipal principal);
    Task<bool> ScrapeJobIsOfUser(long scrapeJobId, string userId);

    /// <summary>
    /// Executes a given scrape job and saves its result as a <see cref="ScrapeJobEvent"/>
    /// </summary>
    /// <param name="scrapeJob">The <see cref="ScrapeJob"/> to execute</param>
    /// <returns></returns>
    Task<bool> ExecuteScrapeJobAsync(ScrapeJob scrapeJob);

    Task<IEnumerable<ScrapeJobStatusDto>> ReadUsersScrapeJobsStatusAsync(ClaimsPrincipal principal);

  }
}
