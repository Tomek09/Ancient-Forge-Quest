using System;

namespace AncientForgeQuest.Utility
{
    public static class TimeExtension
    {
        public static TimeSpan IncreaseDeltaTime(this TimeSpan timeSpan, float deltaTime)
        {
            return TimeSpan.FromTicks(timeSpan.Ticks + (long)(deltaTime * TimeSpan.TicksPerSecond));
        }

        public static TimeSpan DecreaseDeltaTime(this TimeSpan timeSpan, float deltaTime)
        {
            return TimeSpan.FromTicks(timeSpan.Ticks - (long)(deltaTime * TimeSpan.TicksPerSecond));
        }
        
        public static bool IsCompleted(this TimeSpan timeSpan)
        {
            return timeSpan <= TimeSpan.Zero;
        }
    }
}
