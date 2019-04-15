using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WaaS.Business.Entities;

namespace WaaS.Business.Interfaces.Services
{
  public interface IScraper
  {
    Task<ScrapeJobEvent> ExecuteAsync(Uri url, string searchPattern);
  }
}
