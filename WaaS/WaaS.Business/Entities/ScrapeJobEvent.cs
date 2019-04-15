using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;
using System.Text;
using WaaS.Business.Interfaces.Entities;

namespace WaaS.Business.Entities
{
  public enum ScrapeJobEventType
  {
    Error,
    Match,
    NoMatch
  }

  public class ScrapeJobEvent : IEntity<long>
  {
    public long Id { get; set; }
    public HttpStatusCode HttpResponseCode { get; set; }
    public long HttpResponseTimeInMs { get; set; }
    public string Message { get; set; }
    public DateTime TimeStamp { get; set; }
    public ScrapeJobEventType Type { get; set; }

    public long ScrapeJobForeignKey { get; set; }

    [ForeignKey("ScrapeJobForeignKey")]
    public virtual ScrapeJob ScrapeJob { get; set; }
  }
}
