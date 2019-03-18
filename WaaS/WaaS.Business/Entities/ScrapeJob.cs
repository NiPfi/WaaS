using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using WaaS.Business.Interfaces.Entities;

namespace WaaS.Business.Entities
{
  public class ScrapeJob: IEntity<uint>
  {
    public uint Id { get; set; }
    public bool Enabled { get; set; }
    public string Url { get; set; }
    public string Pattern { get; set; }
    public string AlternativeEmail { get; set; }

    public string IdentityUserForeignKey { get; set; }

    [ForeignKey("IdentityUserForeignKey")]
    public virtual IdentityUser IdentityUser { get; set; }
    public virtual ICollection<ScrapeJobEvent> Events { get; set; }
  }
}
