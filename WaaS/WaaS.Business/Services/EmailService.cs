using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using WaaS.Business.Interfaces.Services;

namespace WaaS.Business.Services
{
  public class EmailService : IEmailService
  {

    public EmailService
      (
        IEmailSender emailSender
      )
    {
      _emailSender = emailSender;
    }

    private readonly IEmailSender _emailSender;

    public Task SendRegistrationConfirmationAsync(string email, string verificationToken)
    {
      const string emailVerificationSubject = "Verify your email address";

      UriBuilder builder = new UriBuilder(MyHttpContext.AppBaseUrl) {Path = "/verify-registration"};
      var query = HttpUtility.ParseQueryString(builder.Query);
      query["email"] = email;
      query["verificationToken"] = verificationToken;
      builder.Query = query.ToString();
      var url = builder.ToString();

      string emailVerificationBody = "Thank you for registering to WaaS!<br /><br />" +
                                     $"<a href=\"{url}\">Verify your E-Mail address</a>";
      return _emailSender.SendEmailAsync(email, emailVerificationSubject, emailVerificationBody);
    }

    public Task SendMailChangeConfirmationAsync(string email, string verificationToken)
    {
      const string emailVerificationSubject = "Confirm your new email address";

      UriBuilder builder = new UriBuilder(MyHttpContext.AppBaseUrl) {Path = "/verify-mail-change"};
      var query = HttpUtility.ParseQueryString(builder.Query);
      query["email"] = email;
      query["verificationToken"] = verificationToken;
      builder.Query = query.ToString();
      var url = builder.ToString();

      string emailVerificationBody = "An E-Mail address change was requested.<br /> <br />" +
                                     $"<a href=\"{url}\">Confirm your new E-Mail address</a>";
      return _emailSender.SendEmailAsync(email, emailVerificationSubject, emailVerificationBody);
    }
    public Task SendPasswordResetConfirmationAsync(string email, string verificationToken)
    {
      const string emailVerificationSubject = "Reset your password";

      UriBuilder builder = new UriBuilder(MyHttpContext.AppBaseUrl) {Path = "/verify-password-reset"};
      var query = HttpUtility.ParseQueryString(builder.Query);
      query["email"] = email;
      query["verificationToken"] = verificationToken;
      builder.Query = query.ToString();
      var url = builder.ToString();

      string emailVerificationBody = "A password reset was requested.<br /> <br />" +
                                     $"<a href=\"{url}\">Set new password</a>";
      return _emailSender.SendEmailAsync(email, emailVerificationSubject, emailVerificationBody);
    }
  }
}
