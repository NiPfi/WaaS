using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using WaaS.Business;
using WaaS.Business.Dtos;
using WaaS.Business.Interfaces.Services;

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
      }
      return BadRequest();
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
          return Unauthorized();
        }

        return Ok(user);
      }

      return Unauthorized();

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

    private bool CaptchaResponseValid(string captchaResponse)
    {
      if (string.IsNullOrEmpty(captchaResponse))
      {
        return false;
      }

      var secret = _applicationSettings.ReCaptchaSecretKey;
      if (string.IsNullOrEmpty(secret))
      {
        throw new Exception("uninitialized secret");
      }

      var client = new System.Net.WebClient();

      var googleReply = client.DownloadString(
        $"https://www.google.com/recaptcha/api/siteverify?secret={secret}&response={captchaResponse}");

      return JsonConvert.DeserializeObject<RecaptchaResponseDto>(googleReply).Success;
    }
  }
}
