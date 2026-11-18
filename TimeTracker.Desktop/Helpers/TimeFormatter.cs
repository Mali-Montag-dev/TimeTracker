namespace TimeTracker.Desktop.Helpers
{
    public static class TimeFormatter
    {
        public static string Format(double minutes)
        {
            var time = TimeSpan.FromMinutes(minutes);

            if (time.TotalHours >= 1)
                return $"{(int)time.TotalHours} שע' {time.Minutes} דק'";

            if (time.TotalMinutes >= 1)
                return $"{time.Minutes} דק'";

            return "פחות מדקה";
        }
    }
}