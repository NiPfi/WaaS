using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using WaaS.Business.Dtos;

namespace WaaS.Business.Interfaces.Services
{
  public interface IUserService
  {
    Task<UserDto> CreateAsync(UserDto user);
    Task<UserDto> AuthenticateAsync(string userEmail, string password);
    Task<UserDto> UpdateAsync(ClaimsPrincipal principal, UserDto userDto);
    Task<UserDto> DeleteAsync(ClaimsPrincipal principal);
  }
}
