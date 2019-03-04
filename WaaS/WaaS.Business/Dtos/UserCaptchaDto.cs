using System;
using System.Collections.Generic;
using System.Text;

namespace WaaS.Business.Dtos
{
  public class UserCaptchaDto
  {
    public UserDto User { get; set; }
    public string CaptchaResponse { get; set; }
  }
}
