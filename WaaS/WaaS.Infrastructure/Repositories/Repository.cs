using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WaaS.Business.Interfaces.Entities;
using WaaS.Business.Interfaces.Repositories;

namespace WaaS.Infrastructure.Repositories
{
  public class Repository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity: class, IEntity<TKey> where TKey: IEquatable<TKey>
  {
    private readonly WaasDbContext _context;

    public Repository(WaasDbContext context)
    {
      _context = context;
    }

    private DbSet<TEntity> DbSet => _context.Set<TEntity>();

    public Task<TEntity> Get(TKey id)
    {
      return DbSet.FindAsync(id);
    }

    public IQueryable<TEntity> GetAll()
    {
      return DbSet.AsNoTracking().AsQueryable();
    }

    public Task<bool> Add(TEntity entity)
    {
      DbSet.AddAsync(entity);
      return DbSet.ContainsAsync(entity);
    }

    public async Task<bool> Delete(TKey id)
    {
      var entity = await DbSet.FindAsync(id);
      DbSet.Remove(entity);
      return !(await DbSet.ContainsAsync(entity));
    }

    public async Task<bool> Update(TKey id, Action<TEntity> changeAction)
    {
      try
      {
        var entity = await DbSet.FindAsync(id);
        changeAction(entity);
        return true;
      }
      catch
      {
        return false;
      }
    }

    public void Dispose()
    {
      _context?.Dispose();
    }
  }
}
