using System.Collections.Generic;
using System.Linq;
using Portal.Services.Models;
using Portal.Website.Model;

namespace Portal.Website.Converters
{
    public static class CalendarEventPreviewToCalendarSummary
    {
        /// <summary>
        /// Converts event previews (they should all be on the same day) to a summary
        /// </summary>
        /// <param name="previews"></param>
        /// <returns></returns>
        public static CalendarSummary Convert(List<CalendarEventPreview> previews)
        {
            var preview = previews.First();
            var allEvents = previews.Select(x => x).ToList();
            var summary = new CalendarSummary()
            {
                Date = preview.Start,
                Events = allEvents,
            };

            return summary;
        }
    }
}
