using System.Threading.Tasks;
using System.Transactions;

namespace WaaS.Business.Interfaces
{
  public interface IUnitOfWork
  {
    bool Commit();
    Task<bool> CommitAsync();
  }
}
