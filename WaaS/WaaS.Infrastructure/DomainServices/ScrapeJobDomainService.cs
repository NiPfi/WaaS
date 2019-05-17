using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WaaS.Business.Entities;
using WaaS.Business.Interfaces.Services.Domain;

namespace WaaS.Infrastructure.DomainServices
{
  public class ScrapeJobDomainService : BaseDomainService<ScrapeJob, long>, IScrapeJobDomainService
  {
    public ScrapeJobDomainService(WaasDbContext context) : base(context)
    {
    }

    public IQueryable<ScrapeJob> ReadUsersScrapeJobs(string userId)
    {
      return GetAll().Where(x => x.IdentityUser.Id.Equals(userId, StringComparison.InvariantCulture));
    }

  }
}
