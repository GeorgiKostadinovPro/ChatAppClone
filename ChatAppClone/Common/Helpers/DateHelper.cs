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
                return $"{(int)timeSpan.TotalMinutes} m ago";
            if (timeSpan.TotalHours < 24)
                return $"{(int)timeSpan.TotalHours} h ago";
            if (timeSpan.TotalDays < 7)
                return $"{(int)timeSpan.TotalDays} d ago";
            if (timeSpan.TotalDays < 30)
                return $"{(int)(timeSpan.TotalDays / 7)} w ago";
            if (timeSpan.TotalDays < 365)
                return $"{(int)(timeSpan.TotalDays / 30)} mon ago";

            return $"{(int)(timeSpan.TotalDays / 365)} y ago";
        }

        public static string GetDate(DateTime dateTime)
        {
            return dateTime.ToString("dd MMM yyyy");
        }
    }
}
