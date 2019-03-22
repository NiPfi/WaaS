using System;
using System.Runtime.Serialization;

namespace WaaS.Business.Exceptions.UserService
{
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
