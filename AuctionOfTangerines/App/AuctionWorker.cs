using Core.Interfaces;
using Core.Models;
using Hangfire;
using Infrastructure.Repository.Interfaces;

namespace AuctionOfTangerines;

public class AuctionWorker: BackgroundService
{
    private Random rnd = new();
    protected readonly IServiceProvider _serviceProvider;

    public AuctionWorker(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _ = ClearOldAuctionJob(stoppingToken);
        _ = GenerateTangerineJob(stoppingToken);
    }

    private async Task GenerateTangerineJob(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await GenerateTangerine();
            await Task.Delay(TimeSpan.FromHours(3), stoppingToken);
        }
    }

    private async Task ClearOldAuctionJob(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await NotificationJob();
            await ClearOldTangerines();
            await ClearOldBet();
            await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
        }
    }

    private async Task NotificationJob()
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();
            BackgroundJob.Enqueue(() => notificationService.SendWinEmailNotification());
        }
    }

    private async Task GenerateTangerine()
    {
        List<Tangerine> _tangerines = new();
        for (int i = 0; i < 11; i++)
        {
            _tangerines.Add(new Tangerine()
            {
                Id = new int(),
                Name = $"tangerine {i}",
                CurrentPrice = rnd.Next(10,100)
            });
        }

        await AddTangerineList(_tangerines);
    }
    
    private async Task ClearOldTangerines()
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var tangerineRepository = scope.ServiceProvider.GetRequiredService<ITangerineRepository>();
            await tangerineRepository.ClearTangerines();
        }
    }
    
    private async Task ClearOldBet()
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var betRepository = scope.ServiceProvider.GetRequiredService<IBetRepository>();
            await betRepository.DeleteBet();
        }
    }

    private async Task<bool> AddTangerineList(List<Tangerine> _tangerines)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var tangerineRepository = scope.ServiceProvider.GetRequiredService<ITangerineRepository>();
            await tangerineRepository.AddTangerineList(_tangerines);
            return true;
        }
    }
}