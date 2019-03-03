using Microsoft.EntityFrameworkCore;
using WaaS.Business.Entities;

namespace WaaS.Infrastructure
{
  public sealed class WaasDbContext : DbContext
  {
    public DbSet<User> Users { get; set; }
    public DbSet<ScrapeJob> ScrapeJobs { get; set; }

    public WaasDbContext(DbContextOptions options) : base(options)
    {
      Database.Migrate();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<User>().ToTable("User");
      modelBuilder.Entity<ScrapeJob>().ToTable("ScrapeJob");
    }
  }
}
