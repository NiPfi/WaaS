using System;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using WaaS.Business.Interfaces.Entities;

namespace WaaS.Business.Entities
{
  public class ScrapeJob: IEntity<long>
  {
    public long Id { get; set; }
    public int UserSpecificId { get; set; }
    public bool Enabled { get; set; }
    public string Url { get; set; }
    public string Pattern { get; set; }
    public string AlternativeEmail { get; set; }

    public string IdentityUserForeignKey { get; set; }

    [ForeignKey("IdentityUserForeignKey")]
    public virtual IdentityUser IdentityUser { get; set; }
    public virtual ICollection<ScrapeJobEvent> Events { get; set; }

    public override bool Equals(object obj)
    {
      if(obj is IEntity<long> other)
      {
        return Id == other.Id;
      }

      return false;
    }

    public override int GetHashCode() => HashCode.Combine(Id);

  }
}
