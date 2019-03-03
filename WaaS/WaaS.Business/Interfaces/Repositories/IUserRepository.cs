using System.Dynamic;
using System.Threading.Tasks;
using WaaS.Business.Entities;

namespace WaaS.Business.Interfaces.Repositories
{
  public interface IUserRepository: IRepository<User, uint>
  {
    Task<User> Get(string email);
  }
}
