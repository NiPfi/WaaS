using System;
using System.Collections.Generic;
using System.Text;
using WaaS.Business.Entities;
using WaaS.Business.Interfaces.Repositories;

namespace WaaS.Infrastructure.Repositories
{
  public class ScrapeJobEventRepository : Repository<ScrapeJobEvent, uint>, IScrapeJobEventRepository
  {
    public ScrapeJobEventRepository(WaasDbContext context) : base(context)
    {
    }

  }
}
