using System;
using System.Collections.Generic;
using System.Text;

namespace WaaS.Business.Dtos
{
  public class EmailTokenDto
  {
    public string Email { get; set; }
    public string VerificationToken { get; set; }
  }
}
