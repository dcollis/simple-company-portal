using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Portal.Services;
using Portal.Services.Models;
using Portal.Website.Converters;
using Portal.Website.Model;

namespace Portal.Website.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalendarController : ControllerBase
    {
        private readonly CalendarService _calendarService;

        public CalendarController(CalendarService calendarService)
        {
            this._calendarService = calendarService;
        }

        [Route("getevents")]
        public Task<List<CalendarEventPreview>> GetEvents(DateTime start, DateTime end)
        {
            GetUsernameAndToken(out var username, out var token);
            var events = _calendarService.GetEvents(username, token, start, end);
            return events;
        }

        [Route("getnextevents")]
        public async Task<List<CalendarSummary>> GetNextEvents(int numEvents)
        {
            GetUsernameAndToken(out var username, out var token);
            var events = await _calendarService.GetEvents(username, token, numEvents);
            var dateToEvent = new Dictionary<string, List<CalendarEventPreview>>();
            foreach (var e in events)
            {
                var key = e.Start.ToString("yyyyMMdd");
                if (!dateToEvent.ContainsKey(key))
                {
                    dateToEvent.Add(key, new List<CalendarEventPreview>());
                }
                dateToEvent[key].Add(e);
            }

            var keys = dateToEvent.Keys.OrderBy(k => k);

            return keys.Select(e => CalendarEventPreviewToCalendarSummary.Convert(dateToEvent[e])).ToList();
        }

        private void GetUsernameAndToken(out string username, out string token)
        {
            var tokens = Request.Headers["Authorization"];
            token = tokens[0].Split(' ')[1];
            username = User.Identity.Name;
        }
    }
}