using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace WaaS.Presentation.Hubs
{
  public class ScrapeJobStatusHub : Hub
  {

    public override Task OnConnectedAsync()
    {
      Clients.Caller.SendAsync("statusUpdate");
      return base.OnConnectedAsync();
    }
  }
}
