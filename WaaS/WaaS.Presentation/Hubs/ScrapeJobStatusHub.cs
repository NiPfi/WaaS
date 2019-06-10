using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using WaaS.Business.Interfaces.Services;

namespace WaaS.Presentation.Hubs
{
  [Authorize]
  public class ScrapeJobStatusHub : Hub
  {

    private readonly IScrapeJobService _scrapeJobService;

    public ScrapeJobStatusHub(IScrapeJobService scrapeJobService)
    {
      _scrapeJobService = scrapeJobService;
    }

    public override async Task OnConnectedAsync()
    {
      var statusDto = await _scrapeJobService.ReadUsersScrapeJobsStatusAsync(Context.User);
      await Clients.Caller.SendAsync("statusInitialize", statusDto);
    }
  }
}
