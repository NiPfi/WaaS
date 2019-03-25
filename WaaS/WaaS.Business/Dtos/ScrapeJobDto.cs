using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WaaS.Business.Dtos
{
  public class ScrapeJobDto
  {
    public int Id { get; set; }
    public bool Enabled { get; set; }
    public string Url { get; set; }
    public string Pattern { get; set; }
    public string AlternativeEmail { get; set; }
  }
}
