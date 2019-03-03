using WaaS.Business.Interfaces.Entities;

namespace WaaS.Business.Entities
{
  public class User: IEntity<uint>
  {
    public uint Id { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string PasswordSalt { get; set; }
  }
}
