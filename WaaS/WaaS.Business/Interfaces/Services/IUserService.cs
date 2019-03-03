using System.Threading.Tasks;
using WaaS.Business.Dtos;

namespace WaaS.Business.Interfaces.Services
{
  public interface IUserService
  {
    Task<UserDto> Authenticate(string userEmail, string password);
  }
}
