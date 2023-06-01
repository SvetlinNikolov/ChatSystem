using System;
using System.Threading;
using System.Threading.Tasks;
using ChatSystem.Data;
using ChatSystem.Data.Models;
using ChatSystem.Services.Constants;
using ChatSystem.Services.Services.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

public class SaveCachedChatMessagesToDbJob : BackgroundService
{
    private readonly ILogger<SaveCachedChatMessagesToDbJob> _logger;
    private readonly ICacheService _cacheService;
    private readonly IServiceScopeFactory _scopeFactory;

    public SaveCachedChatMessagesToDbJob(
        ILogger<SaveCachedChatMessagesToDbJob> logger,
        ICacheService cacheService,
        IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _cacheService = cacheService;
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                var dbContext = serviceProvider.GetRequiredService<ChatDbContext>();

                try
                {
                    using var timeoutCts = new CancellationTokenSource(ScheduledJobsConstants.SaveCachedChatMessagesToDbJobTimeout);
                    using var combinedCts = CancellationTokenSource.CreateLinkedTokenSource(stoppingToken, timeoutCts.Token);

                    // Retrieve cached messages from the cache
                    var messages = _cacheService.Get<IEnumerable<ChatMessage>>(CacheConstants.MessageCacheKey);

                    if (messages != null && messages.Any())
                    {
                        await dbContext.ChatMessages.AddRangeAsync(messages);
                        await dbContext.SaveChangesAsync(combinedCts.Token);

                        _cacheService.RemoveFromCache<IEnumerable<ChatMessage>>(CacheConstants.MessageCacheKey);
                        _logger.LogInformation("Cached messages saved to the database at: {time}", DateTimeOffset.Now);
                    }
                }

                catch (OperationCanceledException ex)
                {
                    _logger.LogError(ex, "Job execution timed out.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred during job execution.");
                }

                // Wait for a specified interval before running the task again
                await Task.Delay(ScheduledJobsConstants.SaveCachedChatMessagesToDbJobInterval, stoppingToken);
            }
        }
    }
}
