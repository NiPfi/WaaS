using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using WaaS.Business.Interfaces.Entities;
using WaaS.Business.Interfaces.Repositories;

namespace WaaS.Infrastructure.Repositories
{
  public class Repository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : class, IEntity<TKey> where TKey : IEquatable<TKey>
  {
    protected readonly WaasDbContext _context;

    public Repository(WaasDbContext context)
    {
      _context = context;
    }

    protected DbSet<TEntity> DbSet => _context.Set<TEntity>();

    public Task<TEntity> GetAsync(TKey id)
    {
      return DbSet.FindAsync(id);
    }

    public IQueryable<TEntity> GetAll()
    {
      return DbSet.AsNoTracking().AsQueryable();
    }

    public async Task<bool> AddAsync(TEntity entity)
    {
      await DbSet.AddAsync(entity);
      return 1 == await _context.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(TKey id)
    {
      var entity = await DbSet.FindAsync(id);
      DbSet.Remove(entity);

      return 1 == await _context.SaveChangesAsync();
    }

    public async Task<bool> UpdateAsync(TKey id, Action<TEntity> changeAction)
    {
      var entity = await DbSet.FindAsync(id);
      changeAction(entity);

      return 1 == await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (disposing)
      {
        _context.Dispose();
      }
    }
  }
}
