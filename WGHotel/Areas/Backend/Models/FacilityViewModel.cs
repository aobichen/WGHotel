using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WGHotel.Models;

namespace WGHotel.Areas.Backend.Models
{
    public class FacilityModel
    {
        public int ID { get; set; }

        public string NameZH { get; set; }

        public string NameUS { get; set; }

        public bool Enabled { get; set; }

        public void Create()
        {
            var FactitlyZH = new Facility();
            FactitlyZH.Name = NameZH;
            FactitlyZH.Enabled = true;
            using (var zh = new WGHotelZHEntities())
            {
                zh.Facility.Add(FactitlyZH);
                zh.SaveChanges();
            }

            var FactitlUS = new Facility();
            FactitlUS.Name = NameUS;
            FactitlUS.Enabled = true;
            FactitlUS.ParentId = FactitlyZH.ID;
            using (var us = new WGHotelUSEntities())
            {
                us.Facility.Add(FactitlyZH);
                us.SaveChanges();
            }
        }

        public void Edit()
        {
            var FactitlyZH = new Facility();
            using (var zh = new WGHotelZHEntities())
            {
                FactitlyZH = zh.Facility.Find(ID);
                FactitlyZH.Name = NameZH;
                FactitlyZH.Enabled = Enabled;
                zh.SaveChanges();
            }

            var FactitlyUS = new Facility();
            using (var us = new WGHotelUSEntities())
            {
                FactitlyUS = us.Facility.Where(o => o.ParentId == FactitlyZH.ID).First();
                FactitlyUS.Name = NameUS;
                FactitlyUS.Enabled = Enabled;
                us.SaveChanges();
            }
        }
    }
}