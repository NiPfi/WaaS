using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WaaS.Business.Dtos.User;
using WaaS.Business.Exceptions.UserService;
using WaaS.Business.Interfaces.Services;

namespace WaaS.Business.Services
{
  public class UserService : IUserService
  {

    private readonly IMapper _mapper;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;

    private readonly IEmailService _emailService;
    private readonly ApplicationSettings _applicationSettings;
    private readonly DateTime _tokenExpirationDate = DateTime.UtcNow.AddHours(12);

    public UserService
      (
        IOptions<ApplicationSettings> applicationSettings,
        IMapper mapper,
        SignInManager<IdentityUser> signInManager,
        UserManager<IdentityUser> userManager,
        IEmailService emailService
      )
    {
      _mapper = mapper;
      _signInManager = signInManager;
      _userManager = userManager;
      _emailService = emailService;

      if (applicationSettings != null)
      {
        _applicationSettings = applicationSettings.Value;
      }
    }

    public async Task<UserDto> CreateAsync(UserDto user)
    {
      if (user == null || string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password))
      {
        throw new UserServiceException("Both E-Mail and Password are required");
      }

      var userEntity = _mapper.Map<IdentityUser>(user);
      userEntity.PasswordHash = null;
      var result = await _userManager.CreateAsync(userEntity, user.Password);

      if (!result.Succeeded)
      {
        throw new IdentityUserServiceException(result.Errors);
      }

      await SendEmailConfirmationMailAsync(userEntity);
      return _mapper.Map<UserDto>(userEntity);

    }

    public async Task ResendConfirmationMailAsync(string email)
    {
      var userEntity = await _userManager.FindByEmailAsync(email);
      if (userEntity == null || await _userManager.IsEmailConfirmedAsync(userEntity))
      {
        throw new UserServiceException($"Could not send verification E-Mail to '{email}' because the user either doesn't exist or his address was already verified");
      }

      await SendEmailConfirmationMailAsync(userEntity);

    }

    private async Task SendEmailConfirmationMailAsync(IdentityUser userEntity)
    {
      var code = await _userManager.GenerateEmailConfirmationTokenAsync(userEntity);
      await _emailService.SendRegistrationConfirmationAsync(userEntity.Email, code);
    }

    public async Task<UserDto> AuthenticateAsync(string userEmail, string password)
    {
      var result = await _signInManager.PasswordSignInAsync(userEmail, password, false, false);

      if (!result.Succeeded)
      {
        throw new SignInUserServiceException(result);
      }

      var user = await _userManager.FindByEmailAsync(userEmail);
      var token = GenerateJwtToken(user);

      var userDto = _mapper.Map<UserDto>(user);
      userDto.Password = null;
      userDto.Token = token;

      return userDto;

    }

    public async Task RequestEmailChangeAsync(ClaimsPrincipal principal, string newEmail)
    {
      IdentityUser idUser = await _userManager.GetUserAsync(principal);
      if (newEmail != null)
      {
        idUser.UserName = newEmail;
        var token = await _userManager.GenerateChangeEmailTokenAsync(idUser, newEmail);
        await _emailService.SendMailChangeConfirmationAsync(newEmail, token);
      }

    }

    public async Task<UserDto> UpdateEmailAsync(ClaimsPrincipal principal, string newEmail, string token)
    {
      if (newEmail == null || token == null)
      {
        return null;
      }

      IdentityUser idUser = await _userManager.GetUserAsync(principal);
      IdentityResult result = await _userManager.ChangeEmailAsync(idUser, newEmail, token);

      if (!result.Succeeded)
      {
        throw new IdentityUserServiceException(result.Errors);
      }

      var userDto = new UserDto
      {
        Email = newEmail,
        Token = GenerateJwtToken(idUser)
      };

      return userDto;
    }

    public async Task<UserDto> VerifyEmailAsync(string email, string verificationToken)
    {
      IdentityUser idUser = await _userManager.FindByEmailAsync(email);
      var result = await _userManager.ConfirmEmailAsync(idUser, verificationToken);

      if (result.Succeeded)
      {
        return new UserDto{Email = email};
      }

      throw new IdentityUserServiceException(result.Errors);
    }

    public async Task RequestResetPasswordAsync(string email)
    {
      var idUser = await _userManager.FindByEmailAsync(email);
      var passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(idUser);
      await _emailService.SendPasswordResetConfirmationAsync(email, passwordResetToken);
    }

    public async Task<UserDto> ResetPasswordAsync(string email, string newPassword, string token)
    {
      var idUser = await _userManager.FindByEmailAsync(email);
      var result = await _userManager.ResetPasswordAsync(idUser, token, newPassword);

      if (result.Succeeded)
      {
        return new UserDto{Email = email};
      }

      throw new IdentityUserServiceException(result.Errors);
    }

    public async Task<bool> UpdatePasswordAsync(ClaimsPrincipal principal, string currentPassword,
      string newPassword)
    {
      IdentityUser idUser = await _userManager.GetUserAsync(principal);
      IdentityResult result = await _userManager.ChangePasswordAsync(idUser, currentPassword, newPassword);

      if (!result.Succeeded)
      {
        throw new IdentityUserServiceException(result.Errors);
      }

      return result.Succeeded;
    }

    public async Task<UserDto> DeleteAsync(ClaimsPrincipal principal)
    {
      var idUser = await _userManager.GetUserAsync(principal);

      var result = await _userManager.DeleteAsync(idUser);

      return result.Succeeded ? _mapper.Map<UserDto>(idUser) : null;
    }

    #region private methods

    private string GenerateJwtToken(IdentityUser user)
    {
      var tokenHandler = new JwtSecurityTokenHandler();
      var key = Encoding.UTF8.GetBytes(_applicationSettings.JwtSecret);
      var tokenDescriptor = new SecurityTokenDescriptor
      {
        Subject = new ClaimsIdentity(new[]
        {
          new Claim(JwtRegisteredClaimNames.Sub, user.Id),
          new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
          new Claim(JwtRegisteredClaimNames.Email, user.Email),
          new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("D", CultureInfo.InvariantCulture))
        }),
        Expires = _tokenExpirationDate,
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
        Issuer = _applicationSettings.JwtIssuer,
        Audience = _applicationSettings.JwtIssuer
      };

      return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
    }
  }

  #endregion

}
