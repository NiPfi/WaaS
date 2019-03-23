namespace WaaS.Business.Dtos.User
{
  /// <summary>
  /// Represents a user in transmission over the JSON REST API
  /// </summary>
  public class UserDto
  {
    public string Email { get; set; }
    public string Password { get; set; }

    /// <summary>
    /// [optional] The JWT that is generated and verified by the backend.
    /// </summary>
    public string Token { get; set; }
  }
}
