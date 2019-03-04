using System;
using System.Linq;
using System.Threading.Tasks;

namespace WaaS.Business.Interfaces.Repositories
{
  public interface IRepository<TEntity, in TKey>: IDisposable where TKey: IEquatable<TKey>
  {
    Task<TEntity> Get(TKey id);
    IQueryable<TEntity> GetAll();
    Task<bool> Add(TEntity entity);
    Task<bool> Delete(TKey id);
    Task<bool> Update(TKey id, Action<TEntity> changeAction);
  }
}
