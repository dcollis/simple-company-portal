using System;
using System.Collections.Generic;
using Portal.Services.Models;

namespace Portal.Website.Model
{
    public class CalendarSummary
    {
        public DateTime Date { get; set; }
        public List<CalendarEventPreview> Events { get; set; }
    }
}
