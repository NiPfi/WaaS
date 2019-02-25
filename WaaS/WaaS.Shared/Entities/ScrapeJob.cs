using System;
using System.Collections.Generic;
using System.Text;

namespace WaaS.Shared.Entities
{
  public class ScrapeJob
  {
    public int Id { get; set; }
    public bool Enabled { get; set; }
    public string Url { get; set; }
    public string Pattern { get; set; }
    public string AlternativeEmail { get; set; }
  }
}
