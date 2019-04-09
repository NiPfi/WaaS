using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
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

    public Task SendRegistrationConfirmation(string email, string code)
    {
      const string EmailVerificationSubject = "Verify your email address";
      string EmailVerificationBody = $"Thank you for registering to WaaS! Your verification code is: <em>{code}</em>";
      return _emailSender.SendEmailAsync(email, EmailVerificationSubject, EmailVerificationBody);
    }
  }
}
