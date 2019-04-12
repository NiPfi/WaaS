using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WaaS.Business.Interfaces.Services
{
  public interface IEmailService
  {
    /// <summary>
    /// Sends a confirmation mail containing a confirmation token to a registered user so his address can be confirmed
    /// </summary>
    /// <param name="email">The recipient E-Mail address</param>
    /// <param name="verificationToken">The token to verify the email address with</param>
    /// <returns></returns>
    Task SendRegistrationConfirmation(string email, string verificationToken);
  }
}
