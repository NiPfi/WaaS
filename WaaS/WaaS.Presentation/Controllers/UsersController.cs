using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Threading.Tasks;
using WaaS.Business;
using WaaS.Business.Dtos;
using WaaS.Business.Dtos.User;
using WaaS.Business.Exceptions;
using WaaS.Business.Exceptions.UserService;
using WaaS.Business.Interfaces.Services;
using WaaS.Presentation.Errors;

namespace WaaS.Presentation.Controllers
{
  [Authorize]
  [Route("api/[controller]")]
  [ApiController]
  public class UsersController : ControllerBase
  {
    private readonly IUserService _userService;
    private readonly ApplicationSettings _applicationSettings;

    public UsersController(IUserService userService, IOptions<ApplicationSettings> applicationSettings)
    {
      _userService = userService;
      if (applicationSettings != null)
      {
        _applicationSettings = applicationSettings.Value;
      }
    }

    // POST: api/Users
    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Register(UserCaptchaDto userCaptchaDto)
    {
      if (userCaptchaDto != null && !CaptchaResponseValid(userCaptchaDto.CaptchaResponse))
      {
        return BadRequest(new BadRequestError("Captcha was invalid"));
      }

      try
      {
        var createdUser = await _userService.CreateAsync(userCaptchaDto?.User);

        if (createdUser != null)
        {
          return Ok(createdUser);
        }
      }
      catch (IdentityUserServiceException exception)
      {
        return BadRequest(new BadRequestError(exception.ToString()));
      }

      return BadRequest(new BadRequestError("Something went wrong processing this registration"));
    }

    [AllowAnonymous]
    [HttpPost("authenticate")]
    public async Task<IActionResult> Authenticate(UserCaptchaDto userCaptchaDto)
    {
      if (userCaptchaDto != null && CaptchaResponseValid(userCaptchaDto.CaptchaResponse))
      {
        var userDto = userCaptchaDto.User;
        try
        {
          var user = await _userService.AuthenticateAsync(userDto.Email, userDto.Password);

          if (user == null)
          {
            return BadRequest(new BadRequestError("Something about this Email Password combination was incorrect"));
          }

          return Ok(user);
        }
        catch (SignInUserServiceException)
        {
          return BadRequest(new BadRequestError("Login failed. Please verify your E-Mail and Password and try again!"));
        }

      }

      return BadRequest(new BadRequestError("Captcha was invalid"));

    }

    // PUT: api/Users
    [HttpPut, Authorize]
    public async Task<IActionResult> PutUser(UserEditDto userEditDto)
    {
      if (
        userEditDto == null ||
        !string.IsNullOrWhiteSpace(userEditDto.NewEmail) && !string.IsNullOrWhiteSpace(userEditDto.NewPassword))
      {
        return BadRequest(new BadRequestError("Either a new E-Mail or the current and new password have to be set"));
      }

      if (string.IsNullOrWhiteSpace(userEditDto.NewEmail))
      {
        if (string.IsNullOrWhiteSpace(userEditDto.CurrentPassword) ||
            string.IsNullOrWhiteSpace(userEditDto.NewPassword))
        {
          return BadRequest(new BadRequestError("Either a new E-Mail or the current and new password have to be set"));
        }

        try
        {
          var pwChangeSuccessful = await _userService.UpdatePasswordAsync(User, userEditDto.CurrentPassword, userEditDto.NewPassword);
          if (pwChangeSuccessful)
          {
            return Ok();
          }
        }
        catch (IdentityUserServiceException exception)
        {
          return BadRequest(new BadRequestError(exception.ToString()));
        }


        return BadRequest(new BadRequestError("There was an error updating your password."));
      }

      try
      {
        var result = await _userService.UpdateEmailAsync(User, userEditDto.NewEmail);
        if (result != null)
        {
          return Ok(result);
        }
      }
      catch (IdentityUserServiceException exception)
      {
        return BadRequest(new BadRequestError(exception.ToString()));
      }

      return BadRequest(new BadRequestError("There was an error updating your E-Mail"));

    }

    // DELETE: api/Users
    [HttpDelete, Authorize]
    public async Task<IActionResult> DeleteUser()
    {
      var user = await _userService.DeleteAsync(User);
      if (user != null)
      {
        return Ok(user);
      }

      return BadRequest();
    }

    #region private methods

    private bool CaptchaResponseValid(string captchaResponse)
    {
      if (string.IsNullOrEmpty(captchaResponse))
      {
        return false;
      }

      var secret = _applicationSettings.ReCaptchaSecretKey;
      if (string.IsNullOrEmpty(secret))
      {
        throw new InvalidApplicationConfigurationException("ReCaptcha secret key not configured");
      }

      string googleReply;

      using (var client = new System.Net.WebClient())
      {
        googleReply = client.DownloadString(
          $"https://www.google.com/recaptcha/api/siteverify?secret={secret}&response={captchaResponse}");
      }

      return !string.IsNullOrWhiteSpace(googleReply) && JsonConvert.DeserializeObject<RecaptchaResponseDto>(googleReply).Success;
    }

    #endregion

  }
}
