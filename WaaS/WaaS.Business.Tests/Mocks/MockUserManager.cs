using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using System;
using System.Threading.Tasks;

namespace WaaS.Business.Tests.Mocks
{
  public class MockUserManager : UserManager<IdentityUser>
  {
    public MockUserManager()
      : base
      (
        Substitute.For<IUserStore<IdentityUser>>(),
        Substitute.For<IOptions<IdentityOptions>>(),
        Substitute.For<IPasswordHasher<IdentityUser>>(),
        new IUserValidator<IdentityUser>[0],
        new IPasswordValidator<IdentityUser>[0],
        Substitute.For<ILookupNormalizer>(),
        Substitute.For<IdentityErrorDescriber>(),
        Substitute.For<IServiceProvider>(),
        Substitute.For<ILogger<UserManager<IdentityUser>>>()
      )
    { }
  }
}
