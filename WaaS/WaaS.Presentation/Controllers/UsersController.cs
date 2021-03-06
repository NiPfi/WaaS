using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
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
    private readonly ILogger _logger;

    public UsersController(IUserService userService, IOptions<ApplicationSettings> applicationSettings, ILogger<UsersController> logger)
    {
      _userService = userService;
      _logger = logger;
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
    [HttpPost("verify")]
    public async Task<IActionResult> Verify(EmailTokenDto dto)
    {
      if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.VerificationToken))
      {
        return BadRequest(new BadRequestError("Email and a verification token have to be set to verify an email address"));
      }

      try
      {
        return Ok(await _userService.VerifyEmailAsync(dto.Email, dto.VerificationToken));
      }
      catch (IdentityUserServiceException exception)
      {
        return BadRequest(new BadRequestError(exception.ToString()));
      }
    }

    [AllowAnonymous]
    [HttpPost("resend-confirmation-email")]
    public async Task<IActionResult> ResendConfirmationEmail(UserCaptchaDto userCaptchaDto)
    {
      if (userCaptchaDto != null && CaptchaResponseValid(userCaptchaDto.CaptchaResponse))
      {
        if (userCaptchaDto.User == null || string.IsNullOrWhiteSpace(userCaptchaDto.User.Email))
        {
          return BadRequest(new BadRequestError("Email has to be set to resend a confirmation email"));
        }

        try
        {
          await _userService.ResendConfirmationMailAsync(userCaptchaDto.User.Email);
        }
        catch (UserServiceException exception)
        {
          _logger.LogWarning(exception.Message);
        }
        return Ok();
      }
      return BadRequest(new BadRequestError("Captcha was invalid"));
    }

    [AllowAnonymous]
    [HttpPost("verify-reset-password")]
    public async Task<IActionResult> VerifyResetPasswordRequest(UserCaptchaDto userCaptchaDto)
    {
      if (userCaptchaDto != null && CaptchaResponseValid(userCaptchaDto.CaptchaResponse))
      {
        if (userCaptchaDto.User == null || string.IsNullOrWhiteSpace(userCaptchaDto.User.Email))
        {
          return BadRequest(new BadRequestError("Email has to be set"));
        }

        try
        {
          await _userService.RequestResetPasswordAsync(userCaptchaDto.User.Email);
        }
        catch (UserServiceException exception)
        {
          _logger.LogWarning(exception.Message);
        }
        return Ok();
      }
      return BadRequest(new BadRequestError("Captcha was invalid"));
    }

    [AllowAnonymous]
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword(UserTokenDto userTokenDto)
    {

      if (userTokenDto?.User == null ||
          string.IsNullOrWhiteSpace(userTokenDto.VerificationToken) ||
          string.IsNullOrWhiteSpace(userTokenDto.User.Email) ||
          string.IsNullOrWhiteSpace(userTokenDto.User.Password))
      {
        return BadRequest(new BadRequestError("Verification token, Email and Password have to be set"));
      }

      try
      {
        var user = await _userService.ResetPasswordAsync(userTokenDto.User.Email, userTokenDto.User.Password, userTokenDto.VerificationToken);
        return Ok(user);
      }
      catch (IdentityUserServiceException exception)
      {
        return BadRequest(new BadRequestError(exception.ToString()));
      }

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
        catch (SignInUserServiceException exception)
        {
          if (exception.SignInResult.IsNotAllowed)
          {
            return BadRequest(new BadRequestError("You need to confirm your E-Mail address before logging in!"));
          }
          return BadRequest(new BadRequestError("Login failed. Please verify your E-Mail and Password and try again!"));
        }

      }

      return BadRequest(new BadRequestError("Captcha was invalid"));

    }

    [HttpPost("verify-mail-change")]
    public async Task<IActionResult> VerifyMailChange(EmailTokenDto dto)
    {

      if (dto == null || string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.VerificationToken))
      {
        return BadRequest(new BadRequestError("Email and verification token have to be set to verify mail change"));
      }

      try
      {
        var result = await _userService.UpdateEmailAsync(User, dto.Email, dto.VerificationToken);
        if (result == null)
        {
          return BadRequest();
        }

        return Ok(result);
      }
      catch (IdentityUserServiceException exception)
      {
        return BadRequest(new BadRequestError(exception.ToString()));
      }
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
        await _userService.RequestEmailChangeAsync(User, userEditDto.NewEmail);
        return Ok();
      }
      catch (IdentityUserServiceException exception)
      {
        return BadRequest(new BadRequestError(exception.ToString()));
      }

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
