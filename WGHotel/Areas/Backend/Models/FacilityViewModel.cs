using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WGHotel.Areas.Backend.Models
{
    public class FacilityModel
    {
        public int ID { get; set; }

        public string NameZH { get; set; }

        public string NameUS { get; set; }

        public bool Enabled { get; set; }
    }
}