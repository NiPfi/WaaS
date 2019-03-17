using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaaS.Business.Dtos;

namespace WaaS.Business.Interfaces.Services
{
  public interface IScrapeJobService
  {
    Task<ScrapeJobDto> Create(ScrapeJobDto scrapeJob);
    IEnumerable<ScrapeJobDto> ReadAll();
    IEnumerable<ScrapeJobDto> ReadUsersScrapeJobs(UserDto user);
    Task<ScrapeJobDto> Read(uint id);
    Task<ScrapeJobDto> Update(ScrapeJobDto scrapeJob);
    Task<bool> Delete(uint id);
    Task<ScrapeJobDto> ToggleEnabled(uint id);
  }
}
