using System;
using System.Runtime.Serialization;

namespace WaaS.Business.Exceptions.EmailService
{
  /// <summary>
  /// Is thrown if a method in the <see cref="WaaS.Business.Exceptions.EmailService"/> can't fulfill its task
  /// </summary>
  [Serializable]
  public class EmailServiceException : ServiceException
  {
    protected EmailServiceException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public EmailServiceException(string message) : base(message)
    {
    }

    public EmailServiceException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public EmailServiceException()
    {
    }
  }
}
