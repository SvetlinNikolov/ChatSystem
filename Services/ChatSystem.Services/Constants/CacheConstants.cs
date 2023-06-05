using System.Net.NetworkInformation;

namespace ChatSystem.Services.Constants
{
    public static class CacheConstants
    {
        public const string MessageCacheKey = "Chat_Messages_Scheduled_For_Background_Saving";

        public static TimeSpan GlobalCachingTime { get; } = TimeSpan.FromMinutes(10);

        public static int MaximumAllowedMessagesInCache = 5;
    }
}