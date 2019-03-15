using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaaS.Business.Dtos;
using WaaS.Business.Entities;
using WaaS.Business.Interfaces.Repositories;
using WaaS.Business.Interfaces.Services;

namespace WaaS.Business.Services
{
  class ScrapeJobService : IScrapeJobService
  {

    private readonly IRepository<ScrapeJob, uint> _scrapeJobsRepository;

    public ScrapeJobService(IRepository<ScrapeJob, uint> scrapeJobsRepository)
    {
      _scrapeJobsRepository = scrapeJobsRepository;
    }

    public Task<ScrapeJobDto> Create(ScrapeJobDto scrapeJob)
    {
      throw new NotImplementedException();
    }

    public Task<bool> Delete(uint id)
    {
      throw new NotImplementedException();
    }

    public Task<ScrapeJobDto> Read(uint id)
    {
      throw new NotImplementedException();
    }

    public IQueryable<ScrapeJobDto> ReadAll()
    {
      throw new NotImplementedException();
    }

    public Task<ScrapeJobDto> ToggleEnabled(uint id)
    {
      throw new NotImplementedException();
    }

    public Task<ScrapeJobDto> Update(ScrapeJobDto scrapeJob)
    {
      throw new NotImplementedException();
    }
  }
}
