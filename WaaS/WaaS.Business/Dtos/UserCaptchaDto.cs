using System;
using System.Collections.Generic;
using System.Text;

namespace WaaS.Business.Dtos
{
  /// <summary>
  /// Represents a user dto transmission protected by a captcha
  /// </summary>
  public class UserCaptchaDto
  {
    public UserDto User { get; set; }
    public string CaptchaResponse { get; set; }
  }
}
