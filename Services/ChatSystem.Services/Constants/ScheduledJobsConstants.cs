namespace ChatSystem.Services.Constants
{
    public static class ScheduledJobsConstants
    {
        public static TimeSpan SaveCachedChatMessagesToDbJobInterval { get; } = TimeSpan.FromSeconds(20);

        public static TimeSpan SaveCachedChatMessagesToDbJobTimeout { get; } = TimeSpan.FromMinutes(2);
    }
}
