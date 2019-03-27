using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WaaS.Business.Entities;
using WaaS.Business.Interfaces;

namespace WaaS.Infrastructure
{
  public sealed class WaasDbContext : IdentityDbContext<IdentityUser>
  {
    public DbSet<ScrapeJob> ScrapeJobs { get; set; }
    public DbSet<ScrapeJobEvent> ScrapeJobEvents { get; set; }

    public WaasDbContext(DbContextOptions options) : base(options)
    {
      if (Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
      {
        Database.Migrate();
      }
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      var scrapeJobsBuilder = builder.Entity<ScrapeJob>();
      scrapeJobsBuilder.ToTable("ScrapeJob");
      scrapeJobsBuilder.HasAlternateKey(sj => new {sj.UserSpecificId, sj.IdentityUserForeignKey});

      builder.Entity<ScrapeJobEvent>().ToTable("ScrapeJobEvent");

      base.OnModelCreating(builder);
    }
  }
}
