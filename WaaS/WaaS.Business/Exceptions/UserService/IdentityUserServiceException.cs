using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace WaaS.Business.Exceptions.UserService
{
  /// <summary>
  /// Is thrown when a method generating <see cref="IdentityResult"/> was unsuccessful
  /// </summary>
  [Serializable]
  public class IdentityUserServiceException : UserServiceException
  {

    /// <summary>
    /// Contains all the errors responsible for the exception to be thrown
    /// </summary>
    public IEnumerable<IdentityError> IdentityErrors { get; }

    protected IdentityUserServiceException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public IdentityUserServiceException(IEnumerable<IdentityError> identityErrors)
    {
      IdentityErrors = identityErrors;
    }

    public IdentityUserServiceException(string message) : base(message)
    {
    }

    public IdentityUserServiceException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public IdentityUserServiceException()
    {
    }

    public override string ToString()
    {
      if (IdentityErrors == null || !IdentityErrors.Any())
      {
        return base.ToString();
      }

      var builder = new StringBuilder();

      var i = 0;
      foreach (IdentityError identityError in IdentityErrors)
      {
        builder.Append(identityError.Description);
        i++;
        if (i < IdentityErrors.Count())
        {
          builder.Append("\n");
        }
      }

      return builder.ToString();
    }
  }
}
