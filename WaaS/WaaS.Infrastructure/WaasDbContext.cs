using Microsoft.EntityFrameworkCore;
using WaaS.Business.Entities;
using WaaS.Business.Interfaces;

namespace WaaS.Infrastructure
{
  public sealed class WaasDbContext : DbContext
  {
    public DbSet<ScrapeJob> ScrapeJobs { get; set; }

    public WaasDbContext(DbContextOptions options) : base(options)
    {
      Database.Migrate();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<ScrapeJob>().ToTable("ScrapeJob");
    }
  }
}
