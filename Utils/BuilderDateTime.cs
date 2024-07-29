using System;

namespace SNAMP.Utils
{
    public static class BuilderDateTime
    {
        public static string DateTimeToString(DateTime date)
        {
            TimeSpan diff = DateTime.UtcNow - date.ToUniversalTime();

            if (diff.TotalMinutes < 60)
                return $"{Math.Ceiling(diff.TotalMinutes)} мин. назад";

            else if (diff.TotalHours < 24)
                return $"{Math.Ceiling(diff.TotalHours)} ч. назад";

            else if (diff.TotalDays < 30)
                return $"{Math.Ceiling(diff.TotalDays)} д. назад";

            else if (diff.TotalDays / 12 < 12)
                return $"{Math.Ceiling(diff.TotalDays / 12)} м. назад";

            else if (diff.TotalDays / 365 < 5)
                return $"{Math.Ceiling(diff.TotalDays / 365)} г. назад";

            else
                return $"{Math.Ceiling(diff.TotalDays / 365)} лет назад";
        }
    }
}
