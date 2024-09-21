namespace ChatAppClone.Common.Helpers
{
    public static class DateHelper
    {
        public static string TimeAgo(DateTime dateTime)
        {
            var timeSpan = DateTime.Now.Subtract(dateTime);

            if (timeSpan.TotalMinutes < 1)
                return "just now";
            if (timeSpan.TotalMinutes < 60)
                return $"{(int)timeSpan.TotalMinutes} m";
            if (timeSpan.TotalHours < 24)
                return $"{(int)timeSpan.TotalHours} h";
            if (timeSpan.TotalDays < 7)
                return $"{(int)timeSpan.TotalDays} d";
            if (timeSpan.TotalDays < 30)
                return $"{(int)(timeSpan.TotalDays / 7)} w";
            if (timeSpan.TotalDays < 365)
                return $"{(int)(timeSpan.TotalDays / 30)} mon";

            return $"{(int)(timeSpan.TotalDays / 365)} y ago";
        }
    }
}
