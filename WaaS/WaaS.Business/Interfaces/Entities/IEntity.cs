using System;

namespace WaaS.Business.Interfaces.Entities
{
  public interface IEntity<TKey>
    where TKey : IEquatable<TKey>
  {
    TKey Id { get; set; }
  }
}
