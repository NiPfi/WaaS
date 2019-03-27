using System;
using System.Collections.Generic;
using System.Text;

namespace WaaS.Business.Dtos
{
  public class ScrapeJobEventDto
  {
    public long Id { get; set; }
    public int HttpResponseCode { get; set; }
    public int HttpResponseTimeInMs { get; set; }
    public string Message { get; set; }
    public DateTime TimeStamp { get; set; }
  }
}
