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

    public Task SendRegistrationConfirmation(string email, string verificationToken)
    {
      const string EmailVerificationSubject = "Verify your email address";

      UriBuilder builder = new UriBuilder(MyHttpContext.AppBaseUrl);
      builder.Path = "/api/users/verify";
      var query = HttpUtility.ParseQueryString(builder.Query);
      query["email"] = email;
      query["verificationToken"] = verificationToken;
      builder.Query = query.ToString();
      var url = builder.ToString();

      string EmailVerificationBody = $"Thank you for registering to WaaS! <br /> <br /> <a href=\"{url}\">Verify your E-Mail address</a>";
      return _emailSender.SendEmailAsync(email, EmailVerificationSubject, EmailVerificationBody);
    }
  }
}
