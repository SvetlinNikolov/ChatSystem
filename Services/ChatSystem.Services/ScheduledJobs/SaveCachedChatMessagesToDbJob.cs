using ChatSystem.Data.Models;
using ChatSystem.Data;
using ChatSystem.Services.Constants;
using ChatSystem.Services.Services.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ChatSystem.ViewModels.Cache;

public class SaveCachedChatMessagesToDbJob : BackgroundService
{
    private readonly ILogger<SaveCachedChatMessagesToDbJob> _logger;
    private readonly IServiceScopeFactory _scopeFactory;

    public SaveCachedChatMessagesToDbJob(
        ILogger<SaveCachedChatMessagesToDbJob> logger,
        IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
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
                var cacheService = serviceProvider.GetRequiredService<ICacheService>();

                try
                {
                    // Retrieve cached messages from the cache
                    var messages = cacheService.
                        GetAllCacheEntriesWithPrefix<IEnumerable<ChatMessage>>(CacheConstants.MessageCacheKey)
                        .SelectMany(x => x.Value);

                    if (messages != null && messages.Any())
                    {
                        await dbContext.ChatMessages.AddRangeAsync(messages);
                        await dbContext.SaveChangesAsync(stoppingToken);

                        cacheService.RemoveAllCacheEntriesWithPrefix(CacheConstants.MessageCacheKey);
                        _logger.LogInformation("Cached messages saved to the database at: {time}", DateTimeOffset.Now);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred during job execution.");
                }
            }

            // Wait for a specified interval before running the task again
            await Task.Delay(ScheduledJobsConstants.SaveCachedChatMessagesToDbJobInterval, stoppingToken);
        }
    }
}
