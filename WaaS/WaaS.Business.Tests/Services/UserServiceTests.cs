using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using NSubstitute;
using NSubstitute.Extensions;
using WaaS.Business.Dtos;
using WaaS.Business.Interfaces.Services;
using WaaS.Business.Services;
using WaaS.Business.Tests.Mocks;
using Xunit;

namespace WaaS.Business.Tests.Services
{
  public class UserServiceTests
  {
    [Fact]
    public async void CreateSucceeds()
    {
      // Arrange
      var mockApplicationSettings = Substitute.For<IOptions<ApplicationSettings>>();
      var mockMapper = Substitute.For<IMapper>();
      var mockSignInManager = Substitute.For<MockSignInManager>();
      var mockUserManager = Substitute.For<MockUserManager>();

      var testEmail = "test@test.com";
      var testPassword = "This should not ever be visible to the Presentation Layer!";

      UserDto testUserDto = new UserDto
      {
        Email = testEmail,
        Password = testPassword
      };

      IdentityUser testIdentityUser = Substitute.For<IdentityUser>();

      IdentityResult testIdentityResult = IdentityResult.Success;

      mockMapper.Map<IdentityUser>(testUserDto).Returns(testIdentityUser);
      mockMapper.Map<UserDto>(testIdentityUser).Returns(testUserDto);

      mockUserManager.CreateAsync(Arg.Any<IdentityUser>(), Arg.Any<string>()).ReturnsForAnyArgs(Task.FromResult(testIdentityResult));

      IUserService userService = new UserService(mockApplicationSettings, mockMapper, mockSignInManager, mockUserManager);

      // Act
      var result = await userService.Create(testUserDto);

      // Assert
      await mockUserManager.Received().CreateAsync(testIdentityUser, testPassword);
      Assert.Equal(testUserDto, result);

    }
  }
}
