using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace WaaS.Business.Exceptions.UserService
{
  class SignInUserServiceException: UserServiceException
  {
    public SignInResult SignInResult { get; }

    public SignInUserServiceException(SignInResult signInResult)
    {
      SignInResult = signInResult;
    }
  }
}
