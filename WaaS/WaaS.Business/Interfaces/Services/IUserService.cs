using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using WaaS.Business.Dtos;
using WaaS.Business.Dtos.User;

namespace WaaS.Business.Interfaces.Services
{
  public interface IUserService
  {
    Task<UserDto> CreateAsync(UserDto user);
    Task ResendConfirmationMailAsync(string email);
    Task<UserDto> AuthenticateAsync(string userEmail, string password);
    Task RequestEmailChangeAsync(ClaimsPrincipal principal, string newEmail);
    Task<UserDto> UpdateEmailAsync(ClaimsPrincipal principal, string newEmail, string token);
    Task<UserDto> VerifyEmailAsync(string email, string verificationToken);
    Task RequestResetPasswordAsync(string email);
    Task<UserDto> ResetPasswordAsync(string email, string newPassword, string token);
    Task<bool> UpdatePasswordAsync(ClaimsPrincipal principal, string currentPassword, string newPassword);
    Task<UserDto> DeleteAsync(ClaimsPrincipal principal);
  }
}
