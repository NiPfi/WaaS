using AutoMapper;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using NSubstitute.Core.Arguments;
using WaaS.Business.Dtos;
using WaaS.Business.Entities;
using WaaS.Business.Interfaces;
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
      var mockScrapeJobDomainService = Substitute.For<IScrapeJobDomainService>();
      var mockMapper = Substitute.For<IMapper>();
      var mockUserManager = Substitute.For<MockUserManager>();
      var mockClaimsPrincipal = Substitute.For<ClaimsPrincipal>();
      var mockScrapeJobEventDomainService = Substitute.For<IScrapeJobEventDomainService>();
      var mockScraper = Substitute.For<IScraper>();
      var mockUnitOfWork = Substitute.For<IUnitOfWork>();
      var mockEmailService = Substitute.For<IEmailService>();

      const string testUrl = "http://www.url.trololo";
      const string testPattern = "hello world";

      ScrapeJobDto testScrapeJobDto = new ScrapeJobDto()
      {
        Id = 100,
        Enabled = true,
        Url = testUrl,
        Pattern = testPattern
      };

      var testScrapeJob = new ScrapeJob
      {
        Id = 333,
        UserSpecificId = 100,
        Enabled = true,
        Url = testUrl,
        Pattern = testPattern
      };

      mockMapper.Map<ScrapeJob>(testScrapeJobDto).Returns(testScrapeJob);
      mockMapper.Map<ScrapeJobDto>(testScrapeJob).Returns(testScrapeJobDto);

      mockScraper.ExecuteAsync(new Uri(testUrl), testPattern).Returns(new ScrapeJobEvent {
        Url = testUrl
        });

      mockScrapeJobDomainService.AddAsync(Arg.Any<ScrapeJob>()).ReturnsForAnyArgs(Task.FromResult(testScrapeJob));
      mockUnitOfWork.CommitAsync().Returns(Task.FromResult(true));

      IScrapeJobService scrapeJobService = new ScrapeJobService(mockMapper,
                                                                mockUserManager,
                                                                mockScraper,
                                                                mockScrapeJobEventDomainService,
                                                                mockScrapeJobDomainService,
                                                                mockUnitOfWork,
                                                                mockEmailService
                                                                );

      // Act
      var result = await scrapeJobService.Create(testScrapeJobDto, mockClaimsPrincipal);

      // Assert
      await mockUnitOfWork.Received().CommitAsync();
      Assert.Equal(testScrapeJobDto, result);

    }

  }
}
