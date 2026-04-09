using GeoBlocker.BLL.Services.Abstraction;
using GeoBlocker.DAL.Repo.Abstraction;

namespace GeoBlocker.PL.Helper
{
    public class TemporalBlockCleanupService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogService _logger;
        private static readonly TimeSpan _interval = TimeSpan.FromMinutes(2);

        public TemporalBlockCleanupService(IServiceProvider serviceProvider, ILogService logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.AddLog($"Temporal Block Cleanup Service started. Will run every {_interval.TotalMinutes} minutes.");

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(_interval, stoppingToken);

                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var repo = scope.ServiceProvider.GetRequiredService<ICountryRepo>();
                    repo.RemoveExpiredTemporalBlocks();
                    _logger.AddLog($"[{DateTime.Now}] Temporal block cleanup completed.");
                }
                catch (Exception ex) when (!stoppingToken.IsCancellationRequested)
                {
                    _logger.AddLog($"Error during temporal block cleanup , ex : {ex.Message}");
                }
            }

            _logger.AddLog("Temporal Block Cleanup Service stopped.");
        }
    }
}
