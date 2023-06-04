namespace ChatSystem.Services.Constants
{
    public static class ScheduledJobsConstants
    {
        public static TimeSpan SaveCachedChatMessagesToDbJobInterval { get; } = TimeSpan.FromMinutes(1);

        public static TimeSpan SaveCachedChatMessagesToDbJobTimeout { get; } = TimeSpan.FromMinutes(1);
    }
}
