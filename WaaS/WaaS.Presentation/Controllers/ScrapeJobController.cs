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
    public async Task<IActionResult> CreateScrapeJob(ScrapeJobDto scrapeJobDto)
    {
      var scrapeJob = await _scrapeJobService.Create(scrapeJobDto);
      if (scrapeJob != null)
      {
        return Ok(scrapeJob);
      }

      return BadRequest();
    }

    [HttpGet ,Authorize]
    public IActionResult GetUsersScrapeJobs()
    {
      //TODO get current user from token
      var scrapeJobs =  _scrapeJobService.ReadUsersScrapeJobs(new UserDto());
      if (scrapeJobs.Any())
      {
        return Ok(scrapeJobs);
      }

      return BadRequest();
    }

    [HttpPut, Authorize]
    public async Task<IActionResult> PutScrapeJob(ScrapeJobDto scrapeJobDto)
    {
      var scrapeJob = await _scrapeJobService.Update(scrapeJobDto);
      if (scrapeJob != null)
      {
        return Ok(scrapeJob);
      }

      return BadRequest();
    }

    [HttpDelete, Authorize]
    public async Task<IActionResult> DeleteScrapeJob(ScrapeJobDto scrapeJobDto)
    {
      var success = await _scrapeJobService.Delete(scrapeJobDto.Id);
      if (success)
      {
        return Ok();
      }

      return BadRequest();
    }

  }
}
