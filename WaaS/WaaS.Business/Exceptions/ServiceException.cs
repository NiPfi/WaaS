using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace WaaS.Business.Exceptions
{
  [Serializable]
  public class ServiceException: Exception
  {
    protected ServiceException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public ServiceException(string message) : base(message)
    {
    }

    public ServiceException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public ServiceException()
    {
    }
  }
}
