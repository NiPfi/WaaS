using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace WaaS.Business.Exceptions
{
  /// <summary>
  /// Is thrown when a application service cannot fulfill its task
  /// </summary>
  [Serializable]
  public class ServiceException: Exception
  {
    public ServiceException()
    {
    }

    protected ServiceException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public ServiceException(string message) : base(message)
    {
    }

    public ServiceException(string message, Exception innerException) : base(message, innerException)
    {
    }
  }
}
