using System;
using System.Linq;
using System.Threading.Tasks;
using WaaS.Business.Interfaces.Entities;

namespace WaaS.Business.Interfaces.Repositories
{
  /// <summary>
  /// Represents a Repository that handles storage and retrieval of <see cref="IEntity{TKey}"/>
  /// </summary>
  /// <typeparam name="TEntity"></typeparam>
  /// <typeparam name="TKey"></typeparam>
  public interface IRepository<TEntity, in TKey>: IDisposable where TKey: IEquatable<TKey>
  {
    /// <summary>
    /// Gets a specific <see cref="IEntity{TKey}"/>
    /// </summary>
    /// <param name="id">The unique identifier <see cref="TKey"/> of an <see cref="IEntity{TKey}"/></param>
    /// <returns>A <see cref="Task{TResult}"/> that will resolve to the entity</returns>
    Task<TEntity> Get(TKey id);

    /// <summary>
    /// Gets an <see cref="IQueryable{TEntity}"/> referring to all items stored in this repository
    /// </summary>
    /// <returns>An <see cref="IQueryable{T}"/> referring to all <see cref="IEntity{TKey}"/> objects stored in this repository</returns>
    IQueryable<TEntity> GetAll();

    /// <summary>
    /// Adds an <see cref="IEntity{TKey}"/> to this repository
    /// </summary>
    /// <param name="entity">The <see cref="IEntity{TKey}"/> that is to be added to this repository</param>
    /// <returns>A <see cref="Task{TResult}"/> that will resolve to a boolean that is true if the operation was successful, false otherwise</returns>
    Task<bool> Add(TEntity entity);

    /// <summary>
    /// Removes an <see cref="IEntity{TKey}"/> from this repository
    /// </summary>
    /// <param name="id">The unique identifier <see cref="TKey"/> of an <see cref="IEntity{TKey}"/></param>
    /// <returns>A <see cref="Task{TResult}"/> that will resolve to a boolean that is true if the operation was successful, false otherwise</returns>
    Task<bool> Delete(TKey id);

    /// <summary>
    /// Updates an <see cref="IEntity{TKey}"/> with new values
    /// </summary>
    /// <param name="id">The unique identifier <see cref="TKey"/> of an <see cref="IEntity{TKey}"/></param>
    /// <param name="changeAction">An <see cref="Action{T}"/> lambda expression that can be used to insert the updated values</param>
    /// <returns></returns>
    Task<bool> Update(TKey id, Action<TEntity> changeAction);
  }
}
