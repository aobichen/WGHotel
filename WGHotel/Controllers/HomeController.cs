using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WGHotel.Models;


namespace WGHotel.Controllers
{
    public class HomeController : BaseController
    {//
        public ActionResult Index()
        {
            var model = (from h in _db.Hotel
                         select new HotelViewModel
                         {
                             ID = h.ID,
                             Name = h.Name,
                             City = h.City,
                             Game = h.Game,
                             Price = "100",
                             Tel = h.Tel
                             //LinkUrl = h.LinkUrl
                         }).Take(40).ToList();

            if(CurrentLanguage.Equals("us")){
                foreach (var m in model)
                {

                    m.Image = _basedb.ImageStore.Where(o => o.ReferIdUS == m.ID).FirstOrDefault().ID;
                
                
                }
            }
            else
            {
                foreach (var m in model)
                {
                    var image = _basedb.ImageStore.Where(o => o.ReferIdZH == m.ID).FirstOrDefault();
                    m.Image = image == null ? 0 : image.ID;
                }
            }

            return View(model);
        }


        public ActionResult HotelFirstImage(int id)
        {
            var image = _basedb.ImageStore.Where(o=>o.ID == id).FirstOrDefault();
            
            byte[] img = image == null ? new ImageDAO().EmptyImageForHotel() : image.Image;
            var Extension = image == null ? "jpg" : image.Extension.Replace(".", "");
            var imgtype = string.Format("image/{0}", Extension);
            return File(img,imgtype);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}