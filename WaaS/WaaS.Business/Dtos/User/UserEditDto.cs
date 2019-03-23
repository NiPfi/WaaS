using System;
using System.Collections.Generic;
using System.Text;

namespace WaaS.Business.Dtos.User
{
  public class UserEditDto
  {
    public string NewEmail { get; set; }
    public string CurrentPassword { get; set; }
    public string NewPassword { get; set; }
  }
}
