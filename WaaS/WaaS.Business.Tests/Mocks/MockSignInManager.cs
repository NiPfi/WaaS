using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;

namespace WaaS.Business.Tests.Mocks
{
  public class MockSignInManager : SignInManager<IdentityUser>
  {
    public MockSignInManager()
      : base
      (
        Substitute.For<MockUserManager>(),
        Substitute.For<IHttpContextAccessor>(),
        Substitute.For<IUserClaimsPrincipalFactory<IdentityUser>>(),
        Substitute.For<IOptions<IdentityOptions>>(),
        Substitute.For<ILogger<SignInManager<IdentityUser>>>(),
        Substitute.For<IAuthenticationSchemeProvider>()
      )
    { }
  }
}
