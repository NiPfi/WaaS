using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace WaaS.Business.Exceptions
{
  /// <summary>
  /// Is thrown when the application is misconfigured, meaning that some required configuration parameters haven't been set
  /// </summary>
  [Serializable]
  public class InvalidApplicationConfigurationException: Exception
  {
    public InvalidApplicationConfigurationException()
    {
    }

    public InvalidApplicationConfigurationException(string message) : base(message)
    {
    }

    public InvalidApplicationConfigurationException(string message, Exception innerException) : base(message, innerException)
    {
    }

    protected InvalidApplicationConfigurationException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
  }
}
