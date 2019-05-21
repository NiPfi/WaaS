using System;
using System.Collections.Generic;
using System.Text;
using WaaS.Business.Entities;

namespace WaaS.Business.Dtos.ScrapeJob
{
  class ScrapeJobStatusDto
  {
    public int ScrapeJobId { get; set; }
    public ScrapeJobEventType StatusCode { get; set; }
  }
}
