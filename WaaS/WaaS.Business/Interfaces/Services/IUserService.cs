using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using WaaS.Business.Dtos;

namespace WaaS.Business.Interfaces.Services
{
  public interface IUserService
  {
    Task<UserDto> Create(UserDto user);
    Task<UserDto> Authenticate(string userEmail, string password);
    Task<UserDto> Update(ClaimsPrincipal principal, UserDto userDto);
    Task<UserDto> Delete(ClaimsPrincipal principal);
  }
}
