using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Microsoft.AspNetCore.Identity;

namespace WaaS.Business.Exceptions.UserService
{
  [Serializable]
  public class IdentityUserServiceException: UserServiceException
  {

    public IEnumerable<IdentityError> IdentityErrors { get; }

    protected IdentityUserServiceException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public IdentityUserServiceException(IEnumerable<IdentityError> identityErrors) 
    {
      IdentityErrors = identityErrors;
    }
  }
}
