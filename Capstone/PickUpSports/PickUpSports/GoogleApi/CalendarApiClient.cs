using System;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using PickUpSports.Interface;

namespace PickUpSports.GoogleApi
{
    public class CalendarApiClient : ICalendarApiClient
    {
        private readonly CalendarService _calendarService;
        private readonly string _calendarId;
        public CalendarApiClient(CalendarService calendarService, string calendarId)
        {
            _calendarService = calendarService;
            _calendarId = calendarId;
        }


        public void InsertEvent()
        {
            Event newEvent = new Event()
            {
                Summary = "Google I/O 2015",
                Location = "800 Howard St., San Francisco, CA 94103",
                Description = "A chance to hear more about Google's developer products.",
                Start = new EventDateTime()
                {
                    DateTime = DateTime.Parse("2019-05-28T09:00:00-07:00"),
                    TimeZone = "America/Los_Angeles",
                },
                End = new EventDateTime()
                {
                    DateTime = DateTime.Parse("2019-05-28T17:00:00-07:00"),
                    TimeZone = "America/Los_Angeles",
                },
                Creator =  new Event.CreatorData
                {
                    Email = "shaynuhcon@gmail.com",
                    DisplayName = "shayna"
                }
            };
            

            EventsResource.InsertRequest request = _calendarService.Events.Insert(newEvent, _calendarId);
            Event createdEvent = request.Execute();
            Console.WriteLine("Event created: {0}", createdEvent.HtmlLink);
        }

        public void GetEvents()
        {

        }

        private void SetRole()
        {
            AclRule body = new AclRule();
            body.Role = "writer";
            body.Scope = new AclRule.ScopeData();
            body.Scope.Type = "user";
            body.Scope.Value = "shaynuhcon@gmail.com";

            _calendarService.Acl.Insert(body, _calendarId).Execute();
        }
    }
}