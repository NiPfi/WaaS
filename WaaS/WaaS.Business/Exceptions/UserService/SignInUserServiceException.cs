using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace WaaS.Business.Exceptions.UserService
{
  /// <summary>
  /// Is thrown when a user cannot be authenticated
  /// </summary>
  [Serializable]
  public class SignInUserServiceException: UserServiceException
  {
    public SignInResult SignInResult { get; }

    public SignInUserServiceException(SignInResult signInResult)
    {
      SignInResult = signInResult;
    }
    protected SignInUserServiceException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public SignInUserServiceException(string message) : base(message)
    {
    }

    public SignInUserServiceException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public SignInUserServiceException()
    {
    }
  }
}
