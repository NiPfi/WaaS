using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WaaS.Business.Dtos;
using WaaS.Business.Interfaces;
using WaaS.Infrastructure;

namespace WaaS.Business.Services
{
  public class UserService : IUserService
  {

    private WaasDbContext _context;

    private readonly ApplicationSettings _applicationSettings;
    private readonly DateTime _tokenExpirationDate = DateTime.UtcNow.AddHours(12);

    public UserService(WaasDbContext context, IOptions<ApplicationSettings> applicationSettings)
    {
      _context = context;
      _applicationSettings = applicationSettings.Value;
    }

    public async Task<UserDto> Authenticate(string userEmail, string password)
    {
      var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == userEmail);

      var returnUserDto = new UserDto();

      if (user != null)
      {
        returnUserDto.Email = user.Email;
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_applicationSettings.JwtSecret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
          Subject = new ClaimsIdentity(new[]
          {
            new Claim(ClaimTypes.Name, user.Id.ToString()) 
          }),
          Expires = _tokenExpirationDate,
          SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        returnUserDto.Token = tokenHandler.WriteToken(token);

      }

      return returnUserDto;

    }
  }
}
