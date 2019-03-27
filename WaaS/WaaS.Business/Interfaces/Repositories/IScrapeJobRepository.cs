using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaaS.Business.Entities;

namespace WaaS.Business.Interfaces.Repositories
{
  public interface IScrapeJobRepository : IRepository<ScrapeJob, long>
  {

    IQueryable<ScrapeJob> ReadUsersScrapeJobs(string userId);

  }
}
