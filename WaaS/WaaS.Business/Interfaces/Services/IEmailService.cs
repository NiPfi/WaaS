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
    Task SendRegistrationConfirmationAsync(string email, string verificationToken);

    /// <summary>
    /// Sends a email address change confirmation mail containing a confirmation token to a registered user so his address can be confirmed
    /// </summary>
    /// <param name="email">The recipients new E-Mail address</param>
    /// <param name="verificationToken">The token to verify the email address with</param>
    /// <returns></returns>
    Task SendMailChangeConfirmationAsync(string email, string verificationToken);

    /// <summary>
    /// Sends a password reset mail containing a confirmation token to a registered user so his password can be reset
    /// </summary>
    /// <param name="email">The recipient E-Mail address</param>
    /// <param name="verificationToken">The token to verify the password change request with</param>
    /// <returns></returns>
    Task SendPasswordResetConfirmationAsync(string email, string verificationToken);
  }
}
