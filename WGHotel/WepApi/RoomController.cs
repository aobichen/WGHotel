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

        public DateTime Start { get; set; }

        public DateTime End { get; set; }
    }
    public class RoomController : ApiController
    {
        [HttpPost]
        [Route("GetRoomPrice/{id}")]
        public List<CalendarEvent> HotelImageUpload(int id)
        {
            var d = id == 1 ? 5 : 10;
            var start = DateTime.Now.AddDays(-d);
            var end = DateTime.Now.AddDays(d);
            DateTime epoc = new DateTime(1970, 1, 1);
            List<CalendarEvent> events = new List<CalendarEvent>(); 
            for (var date = start; date < end; date = date.AddDays(1.0))
            {
                var beginDay = DateTime.Parse(date.ToShortDateString() + " 00:00:00");
                var endDay = DateTime.Parse(date.ToShortDateString() + " 00:00:00");
                events.Add(new CalendarEvent { Title = "Event"+id.ToString(), Start = beginDay, End = endDay });
            }
            return events;
        }
    }
}
