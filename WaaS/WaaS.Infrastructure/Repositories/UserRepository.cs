using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WaaS.Business.Entities;
using WaaS.Business.Interfaces.Repositories;

namespace WaaS.Infrastructure.Repositories
{
  class UserRepository: Repository<User, uint>, IUserRepository
  {
    private readonly WaasDbContext _context;

    public UserRepository(WaasDbContext context) : base(context)
    {
      _context = context;
    }

    public Task<User> Get(string email)
    {
      return _context.Users.SingleOrDefaultAsync(u => u.Email == email);
    }
  }
}
