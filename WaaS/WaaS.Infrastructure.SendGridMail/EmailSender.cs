using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WaaS.Business;
using WaaS.Business.Exceptions.EmailService;

namespace WaaS.Infrastructure.SendGridMail
{
  public class EmailSender : IEmailSender
  {

    public EmailSender
      (
        ILogger<EmailSender> logger,
        IOptions<ApplicationSettings> options
      )
    {
      _logger = logger;
      Options = options.Value;
    }

    private ApplicationSettings Options { get; }
    private readonly ILogger _logger;

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
      if (string.IsNullOrWhiteSpace(Options.SendGridKey))
      {
        _logger.LogWarning("SendGrid Email sender was initialized without API Key, check if you're using the correct Email service and/or configure the API key is configured correctly");
        return;
      }
      var client = new SendGridClient(Options.SendGridKey);
      var message = new SendGridMessage()
      {
        From = new EmailAddress("noreply@waas.net", "WaaS"),
        Subject = subject,
        HtmlContent = htmlMessage
      };

      message.AddTo(new EmailAddress(email));
      message.SetClickTracking(false, false);

      Response response = await client.SendEmailAsync(message);
      if (response.StatusCode != System.Net.HttpStatusCode.Accepted)
      {
        throw new EmailServiceException($"The response of the SendGrid Service was: {response.StatusCode}");
      }

    }
  }
}
