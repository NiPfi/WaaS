using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WaaS.Business;

namespace WaaS.Presentation.Middlewares.HttpContext
{
  public static class HttpContextExtensions
  {
    public static IApplicationBuilder UseHttpContext(this IApplicationBuilder app)
    {
      MyHttpContext.Configure(app.ApplicationServices.GetRequiredService<IHttpContextAccessor>());
      return app;
    }
  }
}
