using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using NSubstitute;
using NSubstitute.Extensions;
using WaaS.Business.Dtos;
using WaaS.Business.Dtos.User;
using WaaS.Business.Exceptions.UserService;
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
    private readonly SignInManager<IdentityUser> _mockSignInManager;
    private readonly UserManager<IdentityUser> _mockUserManager;
    private readonly IUserService _userService;
    private readonly IdentityUser _mockIdentityUser;
    private readonly UserDto _testUserDto;
    private readonly ClaimsPrincipal _mockClaimsPrincipal;
    private IEmailService _mockEmailService;


    public UserServiceTests()
    {
      var testApplicationSettings = new ApplicationSettings
      {
        JwtIssuer = "testIssuer",
        JwtSecret = "testSecret_u5^ZifjhB3w@%@d9orGUf^%qfEZWp8wc!aYREEgGff#Cmg8daL%5ZQix!IR7zE#2nb3Sjot3NVb#Fy%C^C8JKLu&ra$yjk5yQsHMDW^Bs@z9W$U43bLa&gn^AzsqH975",
        ReCaptchaSecretKey = "testReCaptchaSecretKey"
      };

      IOptions<ApplicationSettings> mockApplicationSettings = Options.Create(testApplicationSettings);

      _mockIdentityUser = Substitute.For<IdentityUser>();
      _mockIdentityUser.Email.Returns(TestUserEmail);
      _mockIdentityUser.PasswordHash.Returns(TestUserPassword);

      _testUserDto = new UserDto
      {
        Email = TestUserEmail,
        Password = TestUserPassword
      };

      var mockMapper = Substitute.For<IMapper>();
      mockMapper.Map<IdentityUser>(_testUserDto).Returns(_mockIdentityUser);
      mockMapper.Map<UserDto>(_mockIdentityUser).Returns(_testUserDto);

      _mockClaimsPrincipal = Substitute.For<ClaimsPrincipal>();
      _mockSignInManager = Substitute.For<MockSignInManager>();

      _mockUserManager = Substitute.For<MockUserManager>();
      _mockUserManager.GetUserAsync(_mockClaimsPrincipal).Returns(_mockIdentityUser);
      _mockUserManager.GenerateEmailConfirmationTokenAsync(Arg.Any<IdentityUser>()).Returns("testToken");

      _mockEmailService = Substitute.For<IEmailService>();
      _mockEmailService.SendRegistrationConfirmation(Arg.Any<string>(), Arg.Any<string>());

      _userService = new UserService(mockApplicationSettings, mockMapper, _mockSignInManager, _mockUserManager, _mockEmailService);
    }

    [Fact]
    public async Task CreateSucceeds()
    {
      // Arrange
      _mockUserManager.CreateAsync(Arg.Any<IdentityUser>(), Arg.Any<string>()).ReturnsForAnyArgs(Task.FromResult(IdentityResult.Success));

      // Act
      var result = await _userService.CreateAsync(_testUserDto);

      // Assert
      await _mockUserManager.Received().CreateAsync(_mockIdentityUser, TestUserPassword);
      Assert.Equal(_testUserDto, result);
    }

    [Fact]
    public async Task AuthenticateSucceeds()
    {
      // Arrange
      _mockSignInManager.PasswordSignInAsync(TestUserEmail, TestUserPassword, Arg.Any<bool>(), Arg.Any<bool>())
        .Returns(SignInResult.Success);
      _mockUserManager.FindByEmailAsync(TestUserEmail).Returns(_mockIdentityUser);

      // Act
      var result = await _userService.AuthenticateAsync(TestUserEmail, TestUserPassword);

      // Assert
      await _mockSignInManager.Received()
        .PasswordSignInAsync(TestUserEmail, TestUserPassword, Arg.Any<bool>(), Arg.Any<bool>());
      Assert.Equal(TestUserEmail, result.Email);
      Assert.Null(result.Password);
      Assert.False(string.IsNullOrWhiteSpace(result.Token));
    }

    [Fact]
    public async Task CreateWithoutUsernameFails()
    {
      // Arrange
      _testUserDto.Email = null;

      // Act
      await Assert.ThrowsAsync<UserServiceException>(() => _userService.CreateAsync(_testUserDto));
    }

    [Fact]
    public async Task CreateWithoutPasswordFails()
    {
      // Arrange
      _testUserDto.Password = null;

      // Act
      await Assert.ThrowsAsync<UserServiceException>(() => _userService.CreateAsync(_testUserDto));
    }

    [Fact]
    public async Task UpdateEmailSucceeds()
    {
      // Arrange
      const string newEmail = "new-mail@test.com";
      const string testEmailResetToken = "testEmailResetToken";

      _mockUserManager.GenerateChangeEmailTokenAsync(_mockIdentityUser, newEmail)
        .Returns(testEmailResetToken);
      _mockUserManager.ChangeEmailAsync(_mockIdentityUser, newEmail, testEmailResetToken)
        .Returns(IdentityResult.Success);

      // Act
      var result = await _userService.UpdateEmailAsync(_mockClaimsPrincipal, newEmail, testEmailResetToken);

      // Assert
      await _mockUserManager.Received().ChangeEmailAsync(_mockIdentityUser, newEmail, testEmailResetToken);
      Assert.Equal(newEmail, result.Email);
      Assert.Null(result.Password);
      Assert.False(string.IsNullOrWhiteSpace(result.Token));
    }

    [Fact]
    public async Task UpdatePasswordSucceeds()
    {
      // Arrange
      const string currentPassword = "testCurrentPassword";
      const string newPassword = "testNewPassword";

      _mockUserManager.ChangePasswordAsync(_mockIdentityUser, currentPassword, newPassword)
        .Returns(IdentityResult.Success);

      // Act
      var successful = await _userService.UpdatePasswordAsync(_mockClaimsPrincipal, currentPassword, newPassword);

      // Assert
      await _mockUserManager.Received().ChangePasswordAsync(_mockIdentityUser, currentPassword, newPassword);
      Assert.True(successful);
    }

    [Fact]
    public async Task DeleteSucceeds()
    {
      // Arrange
      _mockUserManager.DeleteAsync(_mockIdentityUser).Returns(IdentityResult.Success);

      // Act
      UserDto result = await _userService.DeleteAsync(_mockClaimsPrincipal);

      // Assert
      await _mockUserManager.Received().DeleteAsync(_mockIdentityUser);
      Assert.Equal(TestUserEmail, result.Email);
    }
  }
}
