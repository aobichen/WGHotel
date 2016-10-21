using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WGHotel.Models
{
    public class HotelViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Price { get; set; }
        public string City { get; set; }
        public string Game { get; set; }

        public string Tel { get; set; }
        public int Image { get; set; }
    }
}