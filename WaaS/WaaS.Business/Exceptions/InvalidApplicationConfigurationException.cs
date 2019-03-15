using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace WaaS.Business.Exceptions
{
  [Serializable]
  public class InvalidApplicationConfigurationException: Exception
  {
    public InvalidApplicationConfigurationException(string message) : base(message)
    {
    }

    protected InvalidApplicationConfigurationException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
  }
}
