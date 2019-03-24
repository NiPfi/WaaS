using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using WaaS.Business.Entities;
using WaaS.Infrastructure.Repositories;
using Xunit;

namespace WaaS.Infrastructure.Tests.Repositories
{
  public class RepositoryTests
  {
    [Fact]
    public async Task GetAsyncSucceeds()
    {
      // Arrange
      var options = GetTestDbContextOptions("Get_ScrapeJob_Database");

      var testEntity = GenerateTestEntity();

      using (var context = new WaasDbContext(options))
      {
        await context.AddAsync(testEntity);
        await context.SaveChangesAsync();
      }

      // Act
      using (var context = new WaasDbContext(options))
      {
        var testRepository = new Repository<ScrapeJob, uint>(context);
        var result = await testRepository.GetAsync(testEntity.Id);

        // Assert
        Assert.Equal(testEntity, result);
      }
    }

    [Fact]
    public async Task GetAllSucceeds()
    {
      // Arrange
      var options = GetTestDbContextOptions("GetAll_ScrapeJob_Database");

      const int count = 100;
      var testScrapeJobs = Enumerable.Range(1, count).Select(e => GenerateTestEntity((uint)e)).ToArray();
      using (var context = new WaasDbContext(options))
      {
        foreach (ScrapeJob scrapeJob in testScrapeJobs)
        {
          await context.AddAsync(scrapeJob);
        }
        await context.SaveChangesAsync();
      }

      // Act
      using (var context = new WaasDbContext(options))
      {
        var testRepository = new Repository<ScrapeJob, uint>(context);
        var result = testRepository.GetAll();

        Assert.Equal(count, await result.CountAsync());
        Assert.Equal(testScrapeJobs.AsQueryable(), result);
      }
    }

    [Fact]
    public async Task AddAsyncSucceeds()
    {
      // Arrange
      var options = GetTestDbContextOptions("Add_ScrapeJob_Database");

      var testEntity = GenerateTestEntity();

      // Act
      bool result;
      using (var context = new WaasDbContext(options))
      {
        var testRepository = new Repository<ScrapeJob, uint>(context);
        result = await testRepository.AddAsync(testEntity);
      }

      // Assert
      Assert.True(result);
      using (var context = new WaasDbContext(options))
      {
        var check = await context.ScrapeJobs.FindAsync(testEntity.Id);
        Assert.Equal(testEntity, check);
      }
    }


    [Fact]
    public async Task DeleteAsyncSucceeds()
    {
      // Arrange
      var options = GetTestDbContextOptions("Delete_ScrapeJob_Database");
      var testEntity = GenerateTestEntity();

      using (var context = new WaasDbContext(options))
      {
        await context.AddAsync(testEntity);
        await context.SaveChangesAsync();
      }

      // Act
      bool result;
      using (var context = new WaasDbContext(options))
      {
        var testRepository = new Repository<ScrapeJob, uint>(context);
        result = await testRepository.DeleteAsync(testEntity.Id);
      }

      // Assert
      Assert.True(result);
      using (var context = new WaasDbContext(options))
      {
        Assert.Null(await context.ScrapeJobs.FindAsync(testEntity.Id));
      }
    }

    [Fact]
    public async Task UpdateAsyncSucceeds()
    {
      // Arrange
      var options = GetTestDbContextOptions("Update_ScrapeJob_Database");
      var testEntity = GenerateTestEntity();

      using (var context = new WaasDbContext(options))
      {
        await context.AddAsync(testEntity);
        await context.SaveChangesAsync();
      }

      // Act
      bool result;
      using (var context = new WaasDbContext(options))
      {
        var testRepository = new Repository<ScrapeJob, uint>(context);
        result = await testRepository.UpdateAsync(testEntity.Id, job =>
        {
          job.Pattern = "updatedPattern";
          job.Url = "updatedUrl";
          job.Enabled = !testEntity.Enabled;
        });
      }

      // Assert
      Assert.True(result);
      using (var context = new WaasDbContext(options))
      {
        var updatedEntity = await context.ScrapeJobs.FindAsync(testEntity.Id);

        Assert.NotEqual(testEntity.Pattern, updatedEntity.Pattern);
        Assert.NotEqual(testEntity.Url, updatedEntity.Url);
        Assert.NotEqual(testEntity.Enabled, updatedEntity.Enabled);
      }
    }

    private static DbContextOptions<WaasDbContext> GetTestDbContextOptions(string databaseName)
    {
      return new DbContextOptionsBuilder<WaasDbContext>()
        .UseInMemoryDatabase(databaseName)
        .Options;
    }

    private static ScrapeJob GenerateTestEntity(uint id = 123)
    {
      return new ScrapeJob
      {
        Id = id,
        AlternativeEmail = "test@email.com",
        Enabled = true,
        Pattern = "testPattern",
        Url = "www.tests.com",
        IdentityUserForeignKey = "someGUID",
        UserSpecificId = id
      };
    }

  }
}
