using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WaaS.Business.Dtos
{
  public class RecaptchaResponseDto
  {
    [JsonProperty("success")]
    public bool Success { get; set; }

    [JsonProperty("error-codes")]
    public IEnumerable<string> ErrorCodes { get; set; }

    [JsonProperty("challenge_ts")]
    public DateTime ChallengeTs { get; set; }

    [JsonProperty("hostname")]
    public string Hostname { get; set; }
  }
}
