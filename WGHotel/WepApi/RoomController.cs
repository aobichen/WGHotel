using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WGHotel.Areas.Backend.Models;
using WGHotel.Models;

namespace WGHotel.WepApi
{
    public class CalendarEvent
    {
        public string Title { get; set; }
        public decimal Price { get; set; }
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
            var Room = new PRDate();
            var Begin = DateTime.Parse(Room.Begin);
            var End = DateTime.Parse(Room.End);
            decimal Price = 0;
            List<CalendarEvent> Events = new List<CalendarEvent>();
            using (var db = new WGHotelZHEntities())
            {
                Price = db.Room.Find(id).Sell.Value;

                Events =  (from room in db.RoomPrice where room.ROOMID == id
                          select new CalendarEvent
                            {
                                   Start = room.Date,
                                   //End = room.Date.AddDays(1),
                                   Off = room.SaleOff,
                                   Price = room.Price

                            }).ToList();
            }
           
            DateTime epoc = new DateTime(1970, 1, 1);
            List<CalendarEvent> events = new List<CalendarEvent>();
            if (Events == null || Events.Count <= 0)
            {
                for (var date = Begin; date < End; date = date.AddDays(1.0))
                {
                    var beginDay = DateTime.Parse(date.ToShortDateString() + " 00:00:00");
                    var endDay = DateTime.Parse(date.ToShortDateString() + " 00:00:00");
                    events.Add(new CalendarEvent { Title = "Event" + id.ToString(), Start = beginDay, End = endDay });
                }
            }
            else
            {
                for (var date = Begin; date < End; date = date.AddDays(1.0))
                {
                     
                    var beginDay = DateTime.Parse(date.ToShortDateString() + " 00:00:00");
                    var endDay = DateTime.Parse(date.ToShortDateString() + " 00:00:00");
                    var Off = true;
                    decimal CurrentPrice = 0;
                    var Current = Events.Where(o => o.Start == beginDay).FirstOrDefault();
                    if (Current != null)
                    {
                        CurrentPrice = Current.Price;
                        Off = Current.Off;
                    }
                    else
                    {
                        CurrentPrice = Price;
                        Off = true;
                    }
                    events.Add(new CalendarEvent { Title = "Event" + id.ToString(), Start = beginDay, End = endDay, Off = Off, Price = CurrentPrice });
                }
            }
            return events;
        }

        [HttpPost]
        [Route("RoomSave/{id}")]
        public List<CalendarEvent> RoomPriceSave(int id)
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
                events.Add(new CalendarEvent { Title = "Event" + id.ToString(), Start = beginDay, End = endDay });
            }
            return events;
        }
    }
}
