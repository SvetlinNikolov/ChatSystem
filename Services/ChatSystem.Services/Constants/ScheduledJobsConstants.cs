namespace ChatSystem.Services.Constants
{
    public static class ScheduledJobsConstants
    {
        public static TimeSpan SaveCachedChatMessagesToDbJobInterval { get; } = TimeSpan.FromSeconds(30);

        public static TimeSpan SaveCachedChatMessagesToDbJobTimeout { get; } = TimeSpan.FromMinutes(1);
    }
}
