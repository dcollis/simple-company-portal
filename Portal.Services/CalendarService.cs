using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Graph;
using Portal.Services.GraphServices;
using Portal.Services.Models;

namespace Portal.Services
{
    public class CalendarService
    {
        private readonly string _groupCalendarGroupId;
        private readonly string _sharedCalendarId;
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _tenantId;

        private readonly bool _useSharedCalendar;

        public CalendarService(string groupCalendarGroupId, string sharedCalendarId, bool useSharedCalendar, string clientId, string clientSecret, string tenantId)
        {
            _groupCalendarGroupId = groupCalendarGroupId;
            _sharedCalendarId = sharedCalendarId;
            _clientId = clientId;
            _clientSecret = clientSecret;
            _tenantId = tenantId;

            _useSharedCalendar = useSharedCalendar;
        }

        public async Task<List<CalendarEventPreview>> GetEvents(string username, string accessToken, DateTime start, DateTime end)
        {
            if (_useSharedCalendar)
            {
                return await GetUserEvents(start, end);
            }

            return await GetGroupEvents(username, accessToken, start, end);
        }

        public async Task<List<CalendarEventPreview>> GetEvents(string username, string accessToken, int numEvents)
        {
            if (_useSharedCalendar)
            {
                return await GetUserEvents(numEvents);
            }

            return await GetGroupEvents(username, accessToken, numEvents);
        }

        public async Task<List<CalendarEventPreview>> GetUserEvents(DateTime start, DateTime end)
        {
            var client = new GraphServiceClient(new AzureAuthenticationProvider(_clientId, _clientSecret, _tenantId));

            // the end of the event should be after the start filter - this way we get events that are currently running.
            // the start time should be less than the end time - this way we get events that start in the window we require but end outside the window
            var filter = $"End/DateTime+ge+'{start.ToUniversalTime():o}'+and+Start/DateTime+le+'{end.ToUniversalTime():o}'";
            var events = await client.Users[_sharedCalendarId].Events.Request().Select("id,subject,start,end,isallday,bodypreview").Filter(filter).OrderBy("Start/DateTime").GetAsync();
            var eventsPreviews = events.Select(x => new CalendarEventPreview(x.Id, x.Subject, x.Start.DateTime, x.End.DateTime, x.IsAllDay, x.BodyPreview)).ToList();
            return eventsPreviews;
        }

        public async Task<List<CalendarEventPreview>> GetUserEvents(int numEvents)
        {
            var client = new GraphServiceClient(new AzureAuthenticationProvider(_clientId, _clientSecret, _tenantId));
            var filter = $"End/DateTime+ge+'{DateTime.UtcNow:o}'";
            var events = await client.Users[_sharedCalendarId].Events.Request().Select("id,subject,start,end,isallday,bodypreview").Filter(filter).OrderBy("Start/DateTime").Top(numEvents).GetAsync();
            var eventsPreviews = events.Select(x => new CalendarEventPreview(x.Id, x.Subject, x.Start.DateTime, x.End.DateTime, x.IsAllDay, x.BodyPreview)).ToList();
            return eventsPreviews;
        }

        public async Task<List<CalendarEventPreview>> GetGroupEvents(string username, string accessToken, DateTime start, DateTime end)
        {
            var client = new GraphServiceClient(new AzureAuthenticationBehalfOfProvider(accessToken, username, _clientId, _clientSecret, _tenantId));

            // the end of the event should be after the start filter - this way we get events that are currently running.
            // the start time should be less than the end time - this way we get events that start in the window we require but end outside the window
            var filter = $"End/DateTime+ge+'{start.ToUniversalTime():o}'+and+Start/DateTime+le+'{end.ToUniversalTime():o}'";
            var events = await client.Groups[_groupCalendarGroupId].Events.Request().Select("id,subject,start,end,isallday,bodypreview").Filter(filter).OrderBy("Start/DateTime").GetAsync();
            var eventsPreviews = events.Select(x => new CalendarEventPreview(x.Id, x.Subject, x.Start.DateTime, x.End.DateTime, x.IsAllDay, x.BodyPreview)).ToList();
            return eventsPreviews;
        }

        public async Task<List<CalendarEventPreview>> GetGroupEvents(string username, string accessToken, int numEvents)
        {
            var client = new GraphServiceClient(new AzureAuthenticationBehalfOfProvider(accessToken, username, _clientId, _clientSecret, _tenantId));
            var filter = $"End/DateTime+ge+'{DateTime.UtcNow:o}'";
            var events = await client.Groups[_groupCalendarGroupId].Events.Request().Select("id,subject,start,end,isallday,bodypreview").Filter(filter).OrderBy("Start/DateTime").Top(numEvents).GetAsync();
            var eventsPreviews = events.Select(x => new CalendarEventPreview(x.Id, x.Subject, x.Start.DateTime, x.End.DateTime, x.IsAllDay, x.BodyPreview)).ToList();
            return eventsPreviews;
        }
    }
}
