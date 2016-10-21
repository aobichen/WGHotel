using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WGHotel.WepApi
{
    public class CalendarEvent
    {
        public string Title { get; set; }
        public string Price { get; set; }
        public bool Off { get; set; }

        public double Start { get; set; }

        public double End { get; set; }
    }
    public class RoomController : ApiController
    {
        [HttpPost]
        [Route("GetRoomPrice/{id}")]
        public List<CalendarEvent> HotelImageUpload(int id)
        {
            var start = DateTime.Now.AddDays(-10);
            var end = DateTime.Now.AddDays(10);
            DateTime epoc = new DateTime(1970, 1, 1);
            List<CalendarEvent> events = new List<CalendarEvent>(); 
            for (var date = start; date < end; date = date.AddDays(1.0))
            {
                var beginDay = DateTime.Parse(date.ToShortDateString() + " 00:00:00");
                var endDay = DateTime.Parse(date.ToShortDateString() + " 00:00:00");
                events.Add(new CalendarEvent { Title = "Event 1", Start = beginDay.Subtract(epoc).TotalSeconds, End = endDay.Subtract(epoc).TotalSeconds });
            }
            return events;
        }
    }
}
