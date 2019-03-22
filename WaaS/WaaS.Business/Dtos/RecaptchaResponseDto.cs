using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WaaS.Business.Dtos
{
  /// <summary>
  /// Represents the answer of Google's ReCaptcha service for JSOn deserialization.
  /// </summary>
  public class RecaptchaResponseDto
  {
    /// <summary>
    /// boolean representing if captcha verification has been successful.
    /// </summary>
    [JsonProperty("success")]
    public bool Success { get; set; }

    /// <summary>
    /// https://developers.google.com/recaptcha/docs/verify#error-code-reference
    /// </summary>
    [JsonProperty("error-codes")]
    public IEnumerable<string> ErrorCodes { get; set; }

    /// <summary>
    /// timestamp of the challenge load (ISO format yyyy-MM-dd'T'HH:mm:ssZZ)
    /// </summary>
    [JsonProperty("challenge_ts")]
    public DateTime ChallengeTs { get; set; }

    /// <summary>
    /// the hostname of the site where the reCAPTCHA was solved
    /// </summary>
    [JsonProperty("hostname")]
    public string Hostname { get; set; }
  }
}
