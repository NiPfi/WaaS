using System;
using System.Collections.Generic;
using System.Text;

namespace WaaS.Business.Dtos
{
  public class ScrapeJobEventDto
  {
    public uint Id { get; set; }
    public int HTTPResponseCode { get; set; }
    public int HTTPResponseTimeInMS { get; set; }
    public string Message { get; set; }
    public DateTime TimeStamp { get; set; }
  }
}
