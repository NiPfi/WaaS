using System;
using System.Collections.Generic;
using System.Text;

namespace WaaS.Business.Dtos.User
{
  /// <summary>
  /// Represents changes to a users account data
  /// </summary>
  public class UserEditDto
  {
    public string NewEmail { get; set; }
    public string CurrentPassword { get; set; }
    public string NewPassword { get; set; }
  }
}
