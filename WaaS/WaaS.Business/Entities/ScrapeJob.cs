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
  }
}
