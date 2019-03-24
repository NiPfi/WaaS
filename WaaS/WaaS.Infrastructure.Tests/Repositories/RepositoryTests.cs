using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using WaaS.Business.Entities;
using WaaS.Business.Interfaces.Repositories;
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
      var options = new DbContextOptionsBuilder<WaasDbContext>()
        .UseInMemoryDatabase("Get_ScrapeJob_Database")
        .Options;

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

    private static ScrapeJob GenerateTestEntity()
    {
      return new ScrapeJob
      {
        Id = 321,
        AlternativeEmail = "test@email.com",
        Enabled = true,
        Pattern = "testPattern",
        Url = "www.tests.com",
        IdentityUserForeignKey = "someGUID",
        UserSpecificId = 1
      };
    }
  }
}
