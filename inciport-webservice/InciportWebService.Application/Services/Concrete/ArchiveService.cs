using InciportWebService.Application;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InciportWebService.Application {

  /// <summary>
  /// Runs a job once a day that archives incident reports if eligible.
  /// </summary>
  public class ArchiveService : BackgroundService {
    private const int ARCHIVE_AT_HOUR = 2; // Archive at 2 AM
    private TimeSpan _archiveAtTimeOfDay = TimeSpan.FromHours(ARCHIVE_AT_HOUR);

    private readonly IServiceScopeFactory _scopeFactory;

    public ArchiveService(IServiceScopeFactory scopeFactory) {
      _scopeFactory = scopeFactory;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken) {
      Console.WriteLine("Archive service started");

      return Task.Run(async () => {
        while (!stoppingToken.IsCancellationRequested) {
          Console.WriteLine("Running archiving task...");
          await ExecuteArchiveJobAsync();
          DateTimeOffset currentTime = DateTimeOffset.UtcNow;
          TimeSpan timeToExecute = currentTime.GetTimeUntilTimeOfDay(_archiveAtTimeOfDay);
          Console.WriteLine($"Time until next execution is {timeToExecute} which will be on {currentTime.Add(timeToExecute)}");
          await Task.Delay(timeToExecute, stoppingToken);
        }
      }, stoppingToken);
    }

    private async Task ExecuteArchiveJobAsync() {
      DateTimeOffset currentTime = DateTimeOffset.UtcNow;
      using (IServiceScope scope = _scopeFactory.CreateScope()) {
        IApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
        foreach (IncidentReportEntity report in dbContext.IncidentReportEntities) {
          report.ArchiveIfEligible(currentTime);
        }
        await dbContext.SaveChangesAsync();
      }
    }

    public override async Task StopAsync(CancellationToken cancellationToken) {
      Console.WriteLine("Archive service stopped");
      await Task.CompletedTask;
    }
  }
}