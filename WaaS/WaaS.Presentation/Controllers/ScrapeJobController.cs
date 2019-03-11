using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WaaS.Business.Dtos;
using WaaS.Business.Interfaces.Services;

namespace WaaS.Presentation.Controllers
{
  [Authorize]
  [Route("api/[controller]")]
  [ApiController]
  public class ScrapeJobController : Controller
  {

    private readonly IScrapeJobService _scrapeJobService;

    public ScrapeJobController(IScrapeJobService scrapeJobService)
    {
      _scrapeJobService = scrapeJobService;
    }

    [HttpPost, Authorize]
    public Task<IActionResult> CreateScrapeJob(ScrapeJobDto scrapeJobDto)
    {
      throw new NotImplementedException();
    }

    [HttpGet ,Authorize]
    public Task<IActionResult> GetUsersScrapeJobs()
    {
      throw new NotImplementedException();
    }

    [HttpPut, Authorize]
    public Task<IActionResult> PutScrapeJob(ScrapeJobDto scrapeJobDto)
    {
      throw new NotImplementedException();
    }

    [HttpDelete, Authorize]
    public Task<IActionResult> DeleteScrapeJob(ScrapeJobDto scrapeJobDto)
    {
      throw new NotImplementedException();
    }

  }
}
