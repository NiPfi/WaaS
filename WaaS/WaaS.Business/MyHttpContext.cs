using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace WaaS.Business
{
  public class MyHttpContext
  {
    private static IHttpContextAccessor m_httpContextAccessor;
    public static HttpContext Current => m_httpContextAccessor.HttpContext;
    public static string AppBaseUrl => $"{Current.Request.Scheme}://{Current.Request.Host}{Current.Request.PathBase}";
    public static void Configure(IHttpContextAccessor contextAccessor)
    {
      m_httpContextAccessor = contextAccessor;
    }
  }
}
