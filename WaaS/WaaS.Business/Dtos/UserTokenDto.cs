using System;
using System.Collections.Generic;
using System.Text;
using WaaS.Business.Dtos.User;

namespace WaaS.Business.Dtos
{
  public class UserTokenDto
  {
    public UserDto User { get; set; }
    public string VerificationToken { get; set; }
  }
}
