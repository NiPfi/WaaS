using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using WaaS.Business;
using WaaS.Business.Interfaces.Repositories;
using WaaS.Business.Interfaces.Services;
using WaaS.Business.Interfaces.Services.Domain;
using WaaS.Business.Services;
using WaaS.Business.Services.Domain;
using WaaS.Infrastructure;
using WaaS.Infrastructure.Repositories;
using WaaS.Infrastructure.Scraper;
using WaaS.Infrastructure.ScrapeScheduler;
using WaaS.Infrastructure.SendGridMail;
using WaaS.Presentation.Middlewares.HttpContext;

namespace WaaS.Presentation
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
      CanSendEmail = !string.IsNullOrWhiteSpace(Configuration.GetValue<string>("ApplicationSettings:SendGridKey"));
    }

    public IConfiguration Configuration { get; }
    public bool CanSendEmail { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      var applicationSettings = Configuration.GetSection("ApplicationSettings");
      services.Configure<ApplicationSettings>(applicationSettings);

      services.AddDefaultIdentity<IdentityUser>(config =>
      {
        if (CanSendEmail)
        {
          config.SignIn.RequireConfirmedEmail = true;
        }
      })
        .AddEntityFrameworkStores<WaasDbContext>()
        .AddDefaultTokenProviders();

      services.Configure<IdentityOptions>(options =>
      {
        options.Password.RequiredLength = 8;

        options.Lockout.AllowedForNewUsers = false;

        options.User.RequireUniqueEmail = true;

        options.ClaimsIdentity.UserIdClaimType = JwtRegisteredClaimNames.Sub;
      });

      JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
      services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
          options.SaveToken = true;
          options.TokenValidationParameters = new TokenValidationParameters
          {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidateTokenReplay = true,

            ValidIssuer = applicationSettings.Get<ApplicationSettings>().JwtIssuer,
            ValidAudience = applicationSettings.Get<ApplicationSettings>().JwtIssuer,
            IssuerSigningKey =
              new SymmetricSecurityKey(Encoding.UTF8.GetBytes(applicationSettings.Get<ApplicationSettings>().JwtSecret)),
            ClockSkew = TimeSpan.Zero
          };
        });

      services.AddResponseCompression();

      services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

      ServiceProvider serviceProvider = services.BuildServiceProvider();
      IHostingEnvironment env = serviceProvider.GetService<IHostingEnvironment>();
      if (env.IsProduction())
      {
        services.AddSpaStaticFiles(configuration =>
        {
          configuration.RootPath = "ClientApp/dist";
        });
      }

      services.AddAutoMapper();

      services.AddDbContext<WaasDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("WaasDbContext")), ServiceLifetime.Transient);

      services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
      services.AddSingleton<HttpClient, HttpClient>();

      services.AddTransient<IEmailSender, EmailSender>();
      //services.AddTransient<IScraper, SimpleHtmlRegexScraper>();
      services.AddScoped<IScraper, CachedHtmlRegexScraper>();

      services.AddScoped<IEmailService, EmailService>();
      services.AddScoped<IUserService, UserService>();
      services.AddScoped<IScrapeJobRepository, ScrapeJobRepository>();
      services.AddScoped<IScrapeJobService, ScrapeJobService>();
      services.AddScoped<IScrapeJobEventRepository, ScrapeJobEventRepository>();
      services.AddScoped<IScrapeJobEventDomainService, ScrapeJobEventDomainService>();
      services.AddScoped<IScrapeJobEventService, ScrapeJobEventService>();

      services.AddHostedService<SimpleTimedScrapeScheduler>();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      if (env.IsDevelopment())
      {
        IdentityModelEventSource.ShowPII = true;
        app.UseDeveloperExceptionPage();
      }
      else
      {
        app.UseExceptionHandler("/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();

        app.UseResponseCompression();
      }

      app.UseHttpContext();

      app.UseAuthentication();

      app.UseHttpsRedirection();

      if (env.IsProduction())
      {
        app.UseStaticFiles();
        app.UseSpaStaticFiles();
      }

      app.UseMvc(routes =>
      {
        routes.MapRoute(
                  name: "default",
                  template: "{controller}/{action=Index}/{id?}");
      });

      app.UseSpa(spa =>
      {
        // To learn more about options for serving an Angular SPA from ASP.NET Core,
        // see https://go.microsoft.com/fwlink/?linkid=864501

        spa.Options.SourcePath = "ClientApp";

        spa.UseSpaPrerendering(options =>
        {
          
          options.BootModulePath = $"{spa.Options.SourcePath}/dist-server/main.js";
          options.BootModuleBuilder = env.IsDevelopment()
              ? new AngularCliBuilder(npmScript: "build:ssr")
              : null;
          options.ExcludeUrls = new[] { "/sockjs-node" };
          options.SupplyData = (context, data) =>
          {
            data["cookie"] = context.Request.Cookies;
          };
        });

        if (env.IsDevelopment())
        {
          spa.UseProxyToSpaDevelopmentServer("http://localhost:4200");
        }
      });
    }
  }
}
