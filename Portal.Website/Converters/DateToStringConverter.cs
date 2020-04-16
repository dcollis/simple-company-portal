using System;

namespace Portal.Website.Converters
{
    public static class DateToStringConverter
    {
        public static string Convert(DateTime date)
        {
            var diff = DateTime.Now - date;

            if (diff < TimeSpan.FromMinutes(1) && diff.Seconds == 1)
            {
                return $"{diff.Seconds} second ago";
            }
            if (diff < TimeSpan.FromMinutes(1))
            {
                return $"{diff.Seconds} seconds ago";
            }
            if (diff < TimeSpan.FromHours(1) && diff.Minutes == 1)
            {
                return $"{diff.Minutes} minute ago";
            }
            if (diff < TimeSpan.FromHours(1))
            {
                return $"{diff.Minutes} minutes ago";
            }
            if (diff < TimeSpan.FromHours(24) && diff.Hours == 1)
            {
                return $"{diff.Hours} hour ago";
            }
            if (diff < TimeSpan.FromHours(24))
            {
                return $"{diff.Hours} hours ago";
            }

            return $"{date:D}";
        }
    }
}
