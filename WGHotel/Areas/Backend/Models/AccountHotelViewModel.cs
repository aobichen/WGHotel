using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WGHotel.Models;

namespace WGHotel.Areas.Backend.Models
{
    public class AccountHotelViewModel
    {
        public string Account { get; set; }
        public string Password { get; set; }
        
        public string Namezh { get; set; }
        public string Nameus { get; set; }
        public string Featurezh { get; set; }
        public string Featureus { get; set; }
        public string Addresszh { get; set; }
        public string Addressus { get; set; }

        public string LinkUrl { get; set; }
        public string City { get; set; }

        public string Area { get; set; }

        public string Game { get; set; }

        public string Facilies { get; set; }
        public List<CodeFile> HotelFacility { get { return new CodeFiles().GetHotelFacility(); } }

        public void Create(int userId)
        {
            var ZHdb = new WGHotelZHEntities();
            ZHdb.Hotel.Add(new Hotel
            {
                Name = Namezh,
                Address = Addresszh,
                Area = Area,
                City = City,
                Features = Featurezh,
                Enabled = true,
                LinkUrl = LinkUrl,
                Facilities = Facilies,
                Game = Game,
                UserId = userId
            });

            ZHdb.SaveChanges();

            var USdb = new WGHotelUSEntities();
            USdb.Hotel.Add(new Hotel
            {
                Name = Nameus,
                Address = Addressus,
                Area = Area,
                City = City,
                Facilities = Facilies,
                Features = Featureus,
                Enabled = true,
                LinkUrl = LinkUrl,
                Game = Game,
                UserId = userId
            });

            USdb.SaveChanges();

           // HttpContext.Current.Response.Redirect("~/Backend/Admin/Index",true);
          //  return;
        }
    }

    public class HotelListViewModel
    {
        public int ID { get; set; }
        public int UserId { get; set; }
        

        public string Name { get; set; }
        
        //public string Feature { get; set; }
        
        //public string Address { get; set; }
       

        public string LinkUrl { get; set; }
        public string City { get; set; }

        //public string Area { get; set; }

        public string Game { get; set; }

       
    }
}