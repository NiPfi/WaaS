using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Microsoft.AspNetCore.Identity;

namespace WaaS.Business.Exceptions.UserService
{
  /// <summary>
  /// Is thrown when a method generating <see cref="IdentityResult"/> was unsuccessful
  /// </summary>
  [Serializable]
  public class IdentityUserServiceException: UserServiceException
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
  }
}
