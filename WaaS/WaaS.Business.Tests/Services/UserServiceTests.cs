using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    private const string TestUserEmail = "test@test.com";
    private const string TestUserPassword = "This should not ever be visible to the Presentation Layer!";
    private readonly IMapper _mockMapper;
    private readonly SignInManager<IdentityUser> _mockSignInManager;
    private readonly UserManager<IdentityUser> _mockUserManager;
    private readonly IUserService _userService;
    private readonly IdentityUser _testIdentityUser;
    private readonly UserDto _testUserDto;


    public UserServiceTests()
    {
      var testApplicationSettings = new ApplicationSettings
      {
        JwtIssuer = "testIssuer",
        JwtSecret = "testSecret_u5^ZifjhB3w@%@d9orGUf^%qfEZWp8wc!aYREEgGff#Cmg8daL%5ZQix!IR7zE#2nb3Sjot3NVb#Fy%C^C8JKLu&ra$yjk5yQsHMDW^Bs@z9W$U43bLa&gn^AzsqH975",
        ReCaptchaSecretKey = "testReCaptchaSecretKey"
      };

      IOptions<ApplicationSettings> mockApplicationSettings = Options.Create(testApplicationSettings);

      _testIdentityUser = Substitute.For<IdentityUser>();
      _testIdentityUser.Email.Returns(TestUserEmail);
      _testIdentityUser.PasswordHash.Returns(TestUserPassword);

      _testUserDto = new UserDto
      {
        Email = TestUserEmail,
        Password = TestUserPassword
      };

      _mockMapper = Substitute.For<IMapper>();
      _mockMapper.Map<IdentityUser>(_testUserDto).Returns(_testIdentityUser);
      _mockMapper.Map<UserDto>(_testIdentityUser).Returns(_testUserDto);

      _mockSignInManager = Substitute.For<MockSignInManager>();
      _mockUserManager = Substitute.For<MockUserManager>();
      _userService = new UserService(mockApplicationSettings, _mockMapper, _mockSignInManager, _mockUserManager);
    }

    [Fact]
    public async Task CreateSucceeds()
    {
      // Arrange
      _mockUserManager.CreateAsync(Arg.Any<IdentityUser>(), Arg.Any<string>()).ReturnsForAnyArgs(Task.FromResult(IdentityResult.Success));

      // Act
      var result = await _userService.CreateAsync(_testUserDto);

      // Assert
      await _mockUserManager.Received().CreateAsync(_testIdentityUser, TestUserPassword);
      Assert.Equal(_testUserDto, result);
    }

    [Fact]
    public async Task AuthenticateSucceeds()
    {
      // Arrange
      _mockSignInManager.PasswordSignInAsync(TestUserEmail, TestUserPassword, Arg.Any<bool>(), Arg.Any<bool>())
        .Returns(SignInResult.Success);
      _mockUserManager.FindByEmailAsync(TestUserEmail).Returns(_testIdentityUser);

      // Act
      var result = await _userService.AuthenticateAsync(TestUserEmail, TestUserPassword);

      // Assert
      Assert.Equal(TestUserEmail, result.Email);
      Assert.Null(result.Password);
      Assert.False(string.IsNullOrEmpty(result.Token));

    }
  }
}
