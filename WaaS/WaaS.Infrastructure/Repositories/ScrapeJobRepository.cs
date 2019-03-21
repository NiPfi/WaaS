using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaaS.Business.Entities;
using WaaS.Business.Interfaces.Repositories;

namespace WaaS.Infrastructure.Repositories
{
  public class ScrapeJobRepository : Repository<ScrapeJob, uint>, IScrapeJobRepository
  {
    public ScrapeJobRepository(WaasDbContext context) : base(context)
    {
    }

    public IQueryable<ScrapeJob> ReadUsersScrapeJobs(string userId)
    {

      return GetAll().Where(x => x.IdentityUser.Id.Equals(userId));

    }
  }
}
