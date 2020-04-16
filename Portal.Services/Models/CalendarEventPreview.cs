using System;

namespace Portal.Services.Models
{
    public class CalendarEventPreview
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Information { get; set; }
        public bool AllDay { get; set; }

        public string StartStr => Start.ToString("dd-MM-yy HH:mm");
        public string EndStr => End.ToString("dd-MM-yy HH:mm");

        public CalendarEventPreview()
        {
            
        }

        public CalendarEventPreview(string id, string title, string start)
        {
            Id = id;
            Title = title;
            var date = DateTime.Parse(start);
            Start = date;
        }

        public CalendarEventPreview(string id, string title, string start, string end, bool? allDay, string information)
        {
            var startDate = DateTime.Parse(start);
            var endDate = DateTime.Parse(end);

            Id = id;
            Title = title;
            Start = startDate;
            End = endDate;
            AllDay = allDay ?? false ;
            Information = information;
        }
    }
}
