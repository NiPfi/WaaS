using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WaaS.Business.Dtos;
using WaaS.Business.Exceptions.UserService;
using WaaS.Business.Interfaces.Services;

namespace WaaS.Business.Services
{
  public class UserService : IUserService
  {

    private readonly IMapper _mapper;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;

    private readonly ApplicationSettings _applicationSettings;
    private readonly DateTime _tokenExpirationDate = DateTime.UtcNow.AddHours(12);

    public UserService
    (
      IOptions<ApplicationSettings> applicationSettings,
      IMapper mapper,
      SignInManager<IdentityUser> signInManager,
      UserManager<IdentityUser> userManager
      )
    {
      _mapper = mapper;
      _signInManager = signInManager;
      _userManager = userManager;
      _applicationSettings = applicationSettings.Value;
    }

    public async Task<UserDto> CreateAsync(UserDto user)
    {
      if (!string.IsNullOrEmpty(user.Email) && !string.IsNullOrEmpty(user.Password))
      {
        var userEntity = _mapper.Map<IdentityUser>(user);
        var result = await _userManager.CreateAsync(userEntity, user.Password).ConfigureAwait(false);

        if (result.Succeeded)
        {
          userEntity.PasswordHash = null;
          return _mapper.Map<UserDto>(userEntity);
        }

        throw new IdentityUserServiceException(result.Errors);

      }

      throw new UserServiceException("Both E-Mail and Password are required");

    }

    public async Task<UserDto> AuthenticateAsync(string userEmail, string password)
    {
      var result = await _signInManager.PasswordSignInAsync(userEmail, password, false, false).ConfigureAwait(false);

      if (result.Succeeded)
      {
        var user = await _userManager.FindByEmailAsync(userEmail).ConfigureAwait(false);
        var token = GenerateJwtToken(user);

        var userDto = _mapper.Map<UserDto>(user);
        userDto.Password = null;
        userDto.Token = token;

        return userDto;

      }

      throw new SignInUserServiceException(result);

    }

    public async Task<UserDto> UpdateAsync(ClaimsPrincipal principal, UserDto userDto)
    {
      var idUser = await _userManager.GetUserAsync(principal).ConfigureAwait(false);
      IdentityResult result;
      if (userDto.Password != null)
      {
        var token = await _userManager.GeneratePasswordResetTokenAsync(idUser).ConfigureAwait(false);
        result = await _userManager.ResetPasswordAsync(idUser, token, userDto.Password).ConfigureAwait(false);
      }
      else
      {
        idUser.UserName = userDto.Email;
        var token = await _userManager.GenerateChangeEmailTokenAsync(idUser, userDto.Email).ConfigureAwait(false);
        result = await _userManager.ChangeEmailAsync(idUser, userDto.Email, token).ConfigureAwait(false);
      }

      if (result.Succeeded)
      {
        userDto.Password = null;
        userDto.Token = GenerateJwtToken(idUser);
        return userDto;
      }

      return null;
    }

    public async Task<UserDto> DeleteAsync(ClaimsPrincipal principal)
    {
      var idUser = await _userManager.GetUserAsync(principal).ConfigureAwait(false);

      var result = await _userManager.DeleteAsync(idUser).ConfigureAwait(false);

      if (result.Succeeded)
      {
        return _mapper.Map<UserDto>(idUser);
      }

      return null;
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
          new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
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
