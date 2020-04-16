using System;

namespace Portal.Website.Model
{
    public class CalendarEvent
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public bool AllDay { get; set; }
        public string Information { get; set; }

        public CalendarEvent()
        {

        }

        public CalendarEvent(string id, string title, DateTime start, DateTime end, bool allDay, string information)
        {
            Title = title;
            Start = start;
            End = end;
            AllDay = allDay;
            Information = information;
        }
    }
}
