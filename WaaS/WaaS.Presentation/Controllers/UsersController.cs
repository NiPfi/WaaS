using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using WaaS.Business;
using WaaS.Business.Dtos;
using WaaS.Business.Interfaces.Services;
using WaaS.Business.Exceptions;
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
      _applicationSettings = applicationSettings.Value;
    }

    // POST: api/Users
    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Register(UserCaptchaDto userCaptchaDto)
    {

      if (CaptchaResponseValid(userCaptchaDto.CaptchaResponse))
      {

        var createdUser = await _userService.Create(userCaptchaDto.User);


        if (createdUser != null)
        {
          return Ok(createdUser);
        }
        else
        {
          return BadRequest(new BadRequestError("Something about this Email Password combination was incorrect"));
        }
      }
      return BadRequest(new BadRequestError("Captcha was invalid"));
    }

    [AllowAnonymous]
    [HttpPost("authenticate")]
    public async Task<IActionResult> Authenticate(UserCaptchaDto userCaptchaDto)
    {
      if (CaptchaResponseValid(userCaptchaDto.CaptchaResponse))
      {
        var userDto = userCaptchaDto.User;
        var user = await _userService.Authenticate(userDto.Email, userDto.Password);

        if (user == null)
        {
          return BadRequest(new BadRequestError("Something about this Email Password combination was incorrect"));
        }

        return Ok(user);
      }

      return BadRequest(new BadRequestError("Captcha was invalid"));

    }

    // PUT: api/Users
    [HttpPut, Authorize]
    public async Task<IActionResult> PutUser(UserDto userDto)
    {
      var user = await _userService.Update(User, userDto);
      if (user != null)
      {
        return Ok(user);
      }

      return BadRequest();
    }

    // DELETE: api/Users
    [HttpDelete, Authorize]
    public async Task<IActionResult> DeleteUser()
    {
      var user = await _userService.Delete(User);
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

      if (!string.IsNullOrWhiteSpace(googleReply))
      {
        return JsonConvert.DeserializeObject<RecaptchaResponseDto>(googleReply).Success;
      }

      return false;
    }

    #endregion

  }
}
