using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace WaaS.Presentation.Hubs
{
  public class ScrapeJobStatusHub : Hub
  {
    public async Task RegisterStatusListener()
    {
      await Clients.Caller.SendCoreAsync("statusUpdate", null);
    }
  }
}
