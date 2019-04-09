using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WaaS.Business;

namespace WaaS.Infrastructure.SendGridMail
{
  public class EmailSender : IEmailSender
  {

    public EmailSender(IOptions<ApplicationSettings> options)
    {
      Options = options.Value;
    }

    private ApplicationSettings Options { get; }

    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
      var client = new SendGridClient(Options.SendGridKey);
      var message = new SendGridMessage()
      {
        From = new EmailAddress("noreply@waas.net", "WaaS"),
        Subject = subject,
        HtmlContent = htmlMessage
      };

      message.AddTo(new EmailAddress(email));
      message.SetClickTracking(false, false);

      return client.SendEmailAsync(message);
    }
  }
}
