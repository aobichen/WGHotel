using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
                us.Facility.Add(FactitlUS);
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

    public class Facilities
    {
        public List<SelectListItem> Facility(string lang = "zh", List<int> Selected = null)
        {
            var model = new List<Facility>();
            var SelectList = new List<SelectListItem>();

            var db = new WGHotelZHEntities();
            if (lang.Equals("us"))
            {
                db = new WGHotelZHEntities("WGHotelUSEntities");
            }
          
                model = db.Facility.ToList();
                SelectList = new List<SelectListItem>();
                foreach (var i in model)
                {
                    SelectList.Add(item: new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.ID.ToString(),
                        Selected = Selected == null
                           ? false
                           : Selected.Contains(i.ID)
                    });
                }
                
            return SelectList;
            
        }
    }
}