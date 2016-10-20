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

        public string ImgKey { get; set; }

        public void Create(int userId)
        {
            var ZHdb = new WGHotelZHEntities();
            var HotelZh = new Hotel
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
            };
             
            ZHdb.Hotel.Add(HotelZh);           
            ZHdb.SaveChanges();
            var ReferIdZH = HotelZh.ID;

            var USdb = new WGHotelUSEntities();
            var HotelUs = new Hotel
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
            };
            USdb.Hotel.Add(HotelUs);

            USdb.SaveChanges();
             var ReferIdUS = HotelUs.ID;
            var Session = HttpContext.Current.Session;

            if (Session[ImgKey] != null)
            {
                var Basedb = new WGHotelBaseEntities();
                var Now = DateTime.Now;
                var images = (List<ImageViewModel>)Session[ImgKey];
                foreach(var img in images)
                {
                    Basedb.ImageStore.Add(new ImageStore
                    { 
                         Created = Now,
                         Extension = img.Extension,
                         Deleted = false,
                         ReferIdUS = ReferIdUS,
                         ReferIdZH = ReferIdZH,
                         Type = "Hotel",
                         Name = img.Name,
                        Image = img.Image
                    });
                }
                Basedb.SaveChanges();
            }
           
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