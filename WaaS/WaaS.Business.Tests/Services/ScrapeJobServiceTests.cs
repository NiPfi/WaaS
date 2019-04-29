using AutoMapper;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WaaS.Business.Dtos;
using WaaS.Business.Entities;
using WaaS.Business.Interfaces.Repositories;
using WaaS.Business.Interfaces.Services;
using WaaS.Business.Interfaces.Services.Domain;
using WaaS.Business.Services;
using WaaS.Business.Tests.Mocks;
using Xunit;

namespace WaaS.Business.Tests.Services
{
  public class ScrapeJobServiceTests
  {

    [Fact]
    public async void CreateSucceeds()
    {

      // Arrange
      var mockScrapeJobRepository = Substitute.For<IScrapeJobRepository>();
      var mockMapper = Substitute.For<IMapper>();
      var mockUserManager = Substitute.For<MockUserManager>();
      var mockClaimsPrincipal = Substitute.For<ClaimsPrincipal>();
      var mockScrapeJobEventDomainService = Substitute.For<IScrapeJobEventDomainService>();
      var mockScraper = Substitute.For<IScraper>();

      ScrapeJobDto testScrapeJobDto = new ScrapeJobDto()
      {
        Enabled = true,
        Url = "http://www.url.trololo",
        Pattern = "hello world"
      };

      var testScrapeJob = Substitute.For<ScrapeJob>();

      mockMapper.Map<ScrapeJob>(testScrapeJobDto).Returns(testScrapeJob);
      mockMapper.Map<ScrapeJobDto>(testScrapeJob).Returns(testScrapeJobDto);

      mockScrapeJobRepository.AddAsync(Arg.Any<ScrapeJob>()).ReturnsForAnyArgs(Task.FromResult(true));

      IScrapeJobService scrapeJobService = new ScrapeJobService(mockMapper,
                                                                mockScrapeJobRepository,
                                                                mockUserManager,
                                                                mockScraper,
                                                                mockScrapeJobEventDomainService
                                                                );

      // Act
      var result = await scrapeJobService.Create(testScrapeJobDto, mockClaimsPrincipal);

      // Assert
      Assert.Equal(testScrapeJobDto, result);

    }

  }
}
