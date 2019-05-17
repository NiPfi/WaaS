using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WaaS.Business;
using WaaS.Business.Entities;
using WaaS.Business.Interfaces.Services;
using WaaS.Business.Interfaces.Services.Domain;

namespace WaaS.Infrastructure.ScrapeScheduler
{
  public sealed class SimpleTimedScrapeScheduler : IHostedService, IDisposable
  {

    private readonly ILogger _logger;
    private readonly ApplicationSettings _applicationSettings;

    private CancellationTokenSource _cancellationTokenSource;
    private Timer _timer;
    private Task _runningScrapeTask;

    public IServiceProvider Services { get; }

    public SimpleTimedScrapeScheduler(
        IServiceProvider services,
        ILogger<SimpleTimedScrapeScheduler> logger,
        IOptions<ApplicationSettings> applicationSettings)
    {
      Services = services;
      _logger = logger;

      if (applicationSettings != null)
      {
        _applicationSettings = applicationSettings.Value;
      }
    }


    public Task StartAsync(CancellationToken cancellationToken)
    {
      _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

      _timer = new Timer(ExecuteTask, null, TimeSpan.FromMinutes(_applicationSettings.InitialScrapeDelayMins), TimeSpan.FromMinutes(_applicationSettings.ScrapeIntervalMins));

      _logger.LogDebug("ScrapeJob timer has been scheduled");
      return Task.CompletedTask;
    }

    private void ExecuteTask(object state)
    {
      _runningScrapeTask = PerformScrapesAsync();
    }

    private async Task PerformScrapesAsync()
    {
      _logger.LogInformation("Starting scheduled ScrapeJob execution");

      using (var scope = Services.CreateScope())
      {
        var scrapeJobService = scope.ServiceProvider.GetRequiredService<IScrapeJobService>();

        var scrapeJobs = scope.ServiceProvider.GetRequiredService<IScrapeJobDomainService>().GetAll().AsEnumerable();

        foreach (ScrapeJob scrapeJob in scrapeJobs)
        {
          // Do not continue scraping if cancellation has been requested
          if (_cancellationTokenSource.IsCancellationRequested)
          {
            _logger.LogInformation($"Scraping has been cancelled, was about to scrape job: {scrapeJob}");
            return;
          }

          _logger.LogDebug($"Scraping Job: {scrapeJob}");
          await scrapeJobService.ExecuteScrapeJobAsync(scrapeJob);
        }
      }

      _logger.LogInformation("Scraping has been finished");
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
      _timer.Change(Timeout.Infinite, 0);

      if (_runningScrapeTask == null)
      {
        return;
      }

      // Request cancellation, so service won't be killed forcefully
      _cancellationTokenSource.Cancel();

      _logger.LogInformation("Cancellation requested");

      await Task.WhenAny(_runningScrapeTask, Task.Delay(-1, cancellationToken));

      cancellationToken.ThrowIfCancellationRequested();
    }

    public void Dispose()
    {
      _timer?.Dispose();
    }

  }
}
