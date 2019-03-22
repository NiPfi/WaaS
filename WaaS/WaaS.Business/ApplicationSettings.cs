namespace WaaS.Business
{
  /// <summary>
  /// Represents the application settings that can be configured in the appsettings.json file or environment variables respectively. To be used in the options pattern.
  /// </summary>
  public class ApplicationSettings
  {
    /// <summary>
    /// Symmetric key that will be used to sign the JWT.
    /// </summary>
    public string JwtSecret { get; set; }

    /// <summary>
    /// URLs for valid JWT issuers. Limits which hosts are valid to issue JWT for this application.
    /// </summary>
    public string JwtIssuer { get; set; }

    /// <summary>
    /// The secret key for Google's ReCaptcha service.
    /// </summary>
    public string ReCaptchaSecretKey { get; set; }
  }
}
