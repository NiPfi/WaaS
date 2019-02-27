using System.Threading.Tasks;
using WaaS.Business.Dtos;
using WaaS.Shared.Entities;

namespace WaaS.Business.Interfaces
{
  public interface IUserService
  {
    Task<UserDto> Authenticate(string userEmail, string password);
  }
}
