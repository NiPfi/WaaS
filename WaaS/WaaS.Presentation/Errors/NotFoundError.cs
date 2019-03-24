using System.Net;

namespace WaaS.Presentation.Errors
{
  public class NotFoundError : ApiError
  {
    public NotFoundError() : base(404, HttpStatusCode.NotFound.ToString())
    {
    }

    public NotFoundError(string message) : base(404, HttpStatusCode.NotFound.ToString(), message)
    {
    }
  }
}
