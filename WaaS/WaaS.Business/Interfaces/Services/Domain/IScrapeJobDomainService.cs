using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaaS.Business.Entities;

namespace WaaS.Business.Interfaces.Services.Domain
{
  public interface IScrapeJobDomainService: IBaseDomainService<ScrapeJob, long>
  {
    IQueryable<ScrapeJob> ReadUsersScrapeJobs(string userId);
    IQueryable<ScrapeJob> ReadUsersEnabledScrapeJobs(string userId);
    IEnumerable<ScrapeJob> ReadAllEnabledScrapeJobs();
  }
}
