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
  public class ScrapeJobEventController : ControllerBase
  {

    private readonly IScrapeJobEventService _scrapeJobEventService;

    public ScrapeJobEventController(IScrapeJobEventService scrapeJobEventService)
    {
      _scrapeJobEventService = scrapeJobEventService;
    }

    [HttpPost, Authorize]
    public async Task<IActionResult> CreateScrapeJobEvent(ScrapeJobEventDto scrapeJobEventDto)
    {
      var scrapeJobEvent = await _scrapeJobEventService.Create(scrapeJobEventDto);
      if (scrapeJobEvent != null)
      {
        return Ok(scrapeJobEvent);
      }

      return BadRequest();
    }


    [HttpPut, Authorize]
    public async Task<IActionResult> PutScrapeJobEvent(ScrapeJobEventDto scrapeJobEventDto)
    {
      var scrapeJobEvent = await _scrapeJobEventService.Update(scrapeJobEventDto, User);
      if (scrapeJobEvent != null)
      {
        return Ok(scrapeJobEvent);
      }

      return BadRequest();
    }

    [HttpDelete, Authorize]
    public async Task<IActionResult> DeleteScrapeJobEvent(ScrapeJobEventDto scrapeJobEventDto)
    {
      var success = await _scrapeJobEventService.Delete(scrapeJobEventDto.Id, User);
      if (success)
      {
        return Ok();
      }

      return BadRequest();
    }

    [HttpGet, Authorize]
    public async Task<IActionResult> GetScrapeJobEventsOfScrapeJob(uint scrapeJobId)
    {
      var scrapeJobEvents = await _scrapeJobEventService.ReadScrapeJobEventsOfScrapeJob(scrapeJobId, User);
      if (scrapeJobEvents != null)
      {
        return Ok(scrapeJobEvents);
      }

      return BadRequest();
    }


  }

}
