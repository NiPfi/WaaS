using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using WaaS.Business.Dtos;
using WaaS.Business.Entities;

namespace WaaS.Business.Interfaces.Services
{
  public interface IUserService
  {
    Task<IdentityUser> Create(UserDto user);
    Task<UserDto> Authenticate(string userEmail, string password);
  }
}
