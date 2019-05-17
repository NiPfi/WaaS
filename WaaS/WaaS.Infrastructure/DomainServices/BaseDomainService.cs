using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WaaS.Business.Interfaces.Entities;

namespace WaaS.Infrastructure.DomainServices
{
  public class BaseDomainService<TEntity, TKey> : IDisposable where TEntity : class, IEntity<TKey> where TKey : IEquatable<TKey>
  {
    protected readonly WaasDbContext Context;

    public BaseDomainService(WaasDbContext context)
    {
      Context = context;
    }

    protected DbSet<TEntity> DbSet => Context.Set<TEntity>();

    public Task<TEntity> GetAsync(TKey id)
    {
      return DbSet.FindAsync(id);
    }

    public IQueryable<TEntity> GetAll()
    {
      return DbSet.AsNoTracking().AsQueryable();
    }

    public async Task<TEntity> AddAsync(TEntity entity)
    {
      return (await DbSet.AddAsync(entity)).Entity;
    }

    public async Task<TEntity> DeleteAsync(TKey id)
    {
      var entity = await DbSet.FindAsync(id);
      return DbSet.Remove(entity).Entity;
    }

    public async Task<TEntity> UpdateAsync(TKey id, Action<TEntity> changeAction)
    {
      var entity = await DbSet.FindAsync(id);
      changeAction(entity);
      return entity;
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
        Context.Dispose();
      }
    }
  }
}
