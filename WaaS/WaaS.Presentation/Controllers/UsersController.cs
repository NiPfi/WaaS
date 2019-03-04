using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WaaS.Business.Dtos;
using WaaS.Business.Interfaces.Services;

namespace WaaS.Presentation.Controllers
{
  [Authorize]
  [Route("api/[controller]")]
  [ApiController]
  public class UsersController : ControllerBase
  {
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
      _userService = userService;
    }

    // POST: api/Users
    [AllowAnonymous]
    [HttpPost]
    public async Task<ActionResult<UserDto>> Register(UserDto user)
    {
      var createdUser = await _userService.Create(user);


      if (createdUser != null)
      {
        return Ok(createdUser);
      } else
      {
        return BadRequest();
      }
    }

    [AllowAnonymous]
    [HttpPost("authenticate")]
    public async Task<ActionResult<UserDto>> Authenticate(UserDto userDto)
    {

      var user = await _userService.Authenticate(userDto.Email, userDto.Password);

      if (user == null)
      {
        return NotFound();
      }

      return user;

    }

    // PUT: api/Users/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutUser(int id, UserDto userDto)
    {

      //_context.Entry(user).State = EntityState.Modified;

      //try
      //{
      //  await _context.SaveChangesAsync();
      //}
      //catch (DbUpdateConcurrencyException)
      //{
      //  if (!UserExists(id))
      //  {
      //    return NotFound();
      //  }
      //  else
      //  {
      //    throw;
      //  }
      //}

      return NoContent();
    }

    //// DELETE: api/Users/5
    //[HttpDelete("{id}")]
    //public async Task<ActionResult<User>> DeleteUser(int id)
    //{
    //  var user = await _context.Users.FindAsync(id);
    //  if (user == null)
    //  {
    //    return NotFound();
    //  }

    //  _context.Users.Remove(user);
    //  await _context.SaveChangesAsync();

    //  return user;
    //}

    //private bool UserExists(int id)
    //{
    //  return _context.Users.Any(e => e.Id == id);
    //}
  }
}
