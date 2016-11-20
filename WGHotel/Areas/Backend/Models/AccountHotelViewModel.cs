using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WGHotel.Models;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity; // Maybe this one too


namespace WGHotel.Areas.Backend.Models
{
    public class AccountHotelViewModel
    {
        public int ID { get; set; }
        [Required]
        public string Account { get; set; }
         [Required]
        public string Password { get; set; }
        [Required(ErrorMessage="必填項目")]
        public string Namezh { get; set; }
         [Required]
        public string Nameus { get; set; }
        public string Featurezh { get; set; }
        public string Featureus { get; set; }
         [Required]
        public string Addresszh { get; set; }
         [Required]
        public string Addressus { get; set; }

        [Required]
        public string LinkUrl { get; set; }
        [Required]
        public int City { get; set; }

        public string Area { get; set; }

        [Required]
        public string Tel { get; set; }

        public string Game { get; set; }

        public string Facilies { get; set; }

        public int UserId { get; set; }
        //public List<CodeFile> HotelFacility { get { return new CodeFiles().GetHotelFacility(); } }

        public string ImgKey { get; set; }

        public void Create()
        {
           
            var ZHdb = new WGHotelZHEntities();
            var zhHotel = new Hotel();
            zhHotel.Name = Namezh;
            zhHotel.Address = Addresszh;
            zhHotel.Area = Area;
            zhHotel.City = City;
            zhHotel.Facilities = Facilies;
            zhHotel.Features = Featurezh;
            zhHotel.Enabled = true;
            zhHotel.LinkUrl = LinkUrl;
            zhHotel.Game = Game;
            zhHotel.Tel = Tel;
            zhHotel.UserId = UserId;


            ZHdb.Hotel.Add(zhHotel);           
            ZHdb.SaveChanges();
            var ReferIdZH = zhHotel.ID;
            var ReferIdUS = 0;
            try
            {
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
                    UserId = UserId,
                    Tel = Tel,
                    ParentId = zhHotel.ID
                };
                USdb.Hotel.Add(HotelUs);

                USdb.SaveChanges();
                ReferIdUS = HotelUs.ID;

                var Session = HttpContext.Current.Session;

                if (Session[ImgKey] != null)
                {
                    var Basedb = new WGHotelBaseEntities();
                    var Now = DateTime.Now;
                    var images = (List<ImageViewModel>)Session[ImgKey];
                    foreach (var img in images)
                    {
                        var fileName = Guid.NewGuid().GetHashCode().ToString("x");
                        Basedb.ImageStore.Add(new ImageStore
                        {
                            Created = Now,
                            Extension = img.Extension,
                            Deleted = false,
                            ReferIdUS = ReferIdUS,
                            ReferIdZH = ReferIdZH,
                            Type = "Hotel",
                            Name = fileName,
                            Image = img.Image
                        });
                    }
                    Basedb.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                var ZH = ZHdb.Hotel.Find(zhHotel.ID);
                if (ZH != null)
                {
                    ZHdb.Hotel.Remove(ZH);
                    ZHdb.SaveChanges();
                }
            }
             
        
           
        }

        public void Edit()
        {

            var ZHdb = new WGHotelZHEntities();
            var zhHotel = ZHdb.Hotel.Find(ID);
            zhHotel.Name = Namezh;
            zhHotel.Address = Addresszh;
            zhHotel.Area = Area;
            zhHotel.City = City;
            zhHotel.Facilities = Facilies;
            zhHotel.Features = Featurezh;
            zhHotel.Enabled = true;
            zhHotel.LinkUrl = LinkUrl;
            zhHotel.Game = Game;
            zhHotel.Tel = Tel;
            zhHotel.UserId = UserId;


            ZHdb.Hotel.Add(zhHotel);
            ZHdb.SaveChanges();
            var ReferIdZH = zhHotel.ID;

            var USdb = new WGHotelUSEntities();
            var HotelUs = USdb.Hotel.Find(zhHotel.ParentId);

            HotelUs.Name = Nameus;
            HotelUs.Address = Addressus;
            HotelUs.Area = Area;
            HotelUs.City = City;
            HotelUs.Facilities = Facilies;
            HotelUs.Features = Featureus;
            HotelUs.Enabled = true;
            HotelUs.LinkUrl = LinkUrl;
            HotelUs.Game = Game;
            HotelUs.UserId = UserId;
            HotelUs.Tel = Tel;
            HotelUs.ParentId = zhHotel.ID;
            
            USdb.Hotel.Add(HotelUs);

            USdb.SaveChanges();
            var ReferIdUS = HotelUs.ID;
            var Session = HttpContext.Current.Session;

            if (Session[ImgKey] != null)
            {
                var Basedb = new WGHotelBaseEntities();
                var Now = DateTime.Now;
                var images = (List<ImageViewModel>)Session[ImgKey];
                var dbImg = Basedb.ImageStore.Where(o => o.ReferIdZH == zhHotel.ID);
                var ImgNames = dbImg.Select(o=>o.Name).ToList();
                images = images.Where(o => !ImgNames.Contains(o.Name)).ToList();
                foreach (var img in images)
                {
                    var fileName = Guid.NewGuid().GetHashCode().ToString("x");
                    Basedb.ImageStore.Add(new ImageStore
                    {
                        Created = Now,
                        Extension = img.Extension,
                        Deleted = false,
                        ReferIdUS = ReferIdUS,
                        ReferIdZH = ReferIdZH,
                        Type = "Hotel",
                        Name = fileName,
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

        public string Tel { get; set; }
        public string Name { get; set; }
        
        //public string Feature { get; set; }
        
        //public string Address { get; set; }
       

        public string LinkUrl { get; set; }
        public string City { get; set; }

        //public string Area { get; set; }

        public string Game { get; set; }

       
    }
}