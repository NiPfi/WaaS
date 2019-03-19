using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using WaaS.Business.Interfaces.Entities;

namespace WaaS.Business.Entities
{
  public class ScrapeJobEvent : IEntity<uint>
  {
    public uint Id { get; set; }
    public int HTTPResponseCode { get; set; }
    public int HTTPResponseTimeInMS { get; set; }
    public string Message { get; set; }
    public DateTime TimeStamp { get; set; }

    public uint ScrapeJobForeignKey { get; set; }

    [ForeignKey("ScrapeJobForeignKey")]
    public virtual ScrapeJob ScrapeJob { get; set; }
  }
}
