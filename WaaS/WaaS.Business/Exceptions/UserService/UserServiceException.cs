using System;
using System.Runtime.Serialization;

namespace WaaS.Business.Exceptions.UserService
{
  /// <summary>
  /// Is thrown if a method in the <see cref="WaaS.Business.Exceptions.UserService"/> can't fulfill its task
  /// </summary>
  [Serializable]
  public class UserServiceException: ServiceException
  {
    protected UserServiceException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public UserServiceException(string message) : base(message)
    {
    }

    public UserServiceException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public UserServiceException()
    {
    }
  }
}
