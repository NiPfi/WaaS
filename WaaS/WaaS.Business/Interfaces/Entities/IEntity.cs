using System;

namespace WaaS.Business.Interfaces.Entities
{
  /// <summary>
  /// Represents an entity that is uniquely identifiable by its generically typed key
  /// </summary>
  /// <typeparam name="TKey"></typeparam>
  public interface IEntity<TKey>
    where TKey : IEquatable<TKey>
  {
    /// <summary>
    /// The generically typed key identifying this entity
    /// </summary>
    TKey Id { get; set; }
  }
}
