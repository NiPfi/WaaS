using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using WaaS.Business.Interfaces.Entities;

namespace WaaS.Business.Entities
{
  public class ScrapeJobEvent : IEntity<long>
  {
    public long Id { get; set; }
    public int HttpResponseCode { get; set; }
    public int HttpResponseTimeInMs { get; set; }
    public string Message { get; set; }
    public DateTime TimeStamp { get; set; }

    public long ScrapeJobForeignKey { get; set; }

    [ForeignKey("ScrapeJobForeignKey")]
    public virtual ScrapeJob ScrapeJob { get; set; }
  }
}
