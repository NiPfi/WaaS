using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WaaS.Business.Dtos;
using WaaS.Business.Interfaces.Services;
using WaaS.Presentation.Errors;

namespace WaaS.Presentation.Controllers
{
  [Authorize]
  [Route("api/[controller]")]
  [ApiController]
  public class ScrapeJobController : ControllerBase
  {

    private readonly IScrapeJobService _scrapeJobService;

    public ScrapeJobController(IScrapeJobService scrapeJobService)
    {
      _scrapeJobService = scrapeJobService;
    }

    [HttpPost, Authorize]
    public async Task<IActionResult> CreateScrapeJob(ScrapeJobDto scrapeJobDto)
    {
      var scrapeJob = await _scrapeJobService.Create(scrapeJobDto, User);
      if (scrapeJob != null)
      {
        return Ok(scrapeJob);
      }

      return BadRequest(new BadRequestError("Could not create ScrapeJob"));
    }

    [HttpGet ,Authorize]
    public async Task<IActionResult> GetUsersScrapeJobs()
    {
      var scrapeJobs =  await _scrapeJobService.ReadAllUserScrapeJobs(User);
      if (scrapeJobs != null)
      {
        return Ok(scrapeJobs);
      }

      return BadRequest(new BadRequestError("Could not get the users ScrapeJobs"));
    }

    [HttpPut, Authorize]
    public async Task<IActionResult> PutScrapeJob(ScrapeJobDto scrapeJobDto)
    {
      var scrapeJob = await _scrapeJobService.Update(scrapeJobDto, User);
      if (scrapeJob != null)
      {
        return Ok(scrapeJob);
      }

      return BadRequest(new BadRequestError("Could not update ScrapeJob"));
    }

    [HttpDelete, Authorize]
    public async Task<IActionResult> DeleteScrapeJob(ScrapeJobDto scrapeJobDto)
    {
      var success = await _scrapeJobService.Delete(scrapeJobDto.Id, User);
      if (success)
      {
        return Ok();
      }

      return BadRequest(new BadRequestError("Could not delete Scrape Job"));
    }

  }
}
