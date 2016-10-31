using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WGHotel.Areas.Backend.Models;
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
                             //City = h.City,
                             Game = h.Game,
                             Sell = h.Room.Min(o => o.Sell),
                             Tel = h.Tel,                            
                             LinkUrl = h.LinkUrl
                         }).OrderBy(x => Guid.NewGuid()).Take(40).ToList();

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

            var a = new GameSiteModel().SelectList();
           ViewBag.GameSite = new GameSiteModel().SelectList();

            return View(model);
        }

        public ActionResult Detail(int id)
        {
            
            var model = _db.Hotel.Find(id);

            if (model == null)
            {
                return RedirectToAction("Home");
            }

            var detail = new HotelDetail();
            detail.ID = model.ID;
            detail.Images = _basedb.ImageStore.Where(o => o.ReferIdZH == model.ID && o.Type == "Hotel").ToList();
            detail.LinkUrl = model.LinkUrl;
            detail.Name = model.Name;
            detail.Tel = model.Tel;
            detail.Feature = model.Features;
            var Facilities = model.Facilities.Split(',').Select(Int32.Parse).ToList();
            detail.Facilities = _db.CodeFile.Where(o => Facilities.Contains(o.ID)).Select(p=>p.ItemDescription).ToList();
            detail.City = _db.City.Where(o => o.ID == model.City).FirstOrDefault().Name;
            var rooms = model.Room.Select(o => o.ID).ToList();
            detail.Rooms = (from r in _db.Room
                            where rooms.Contains(r.ID)
                            select new RoomViewList
                            {
                                Feature = r.Feature,
                                Notice = r.Notice,
                                ID = r.ID,
                                BedType = r.BedType,
                                LinkUrl = model.LinkUrl,
                                //Images = _basedb.ImageStore.Where(o => o.ID == r.ID && o.Type == "Room").ToList(),
                                Name = r.Name,
                                Quantiy = r.Quantiy,
                                RoomType = r.RoomType,
                                Sell = r.Sell,
                                HasBreakfast = r.HasBreakfast.Value
                            }).ToList();

           
            
            foreach (var r in detail.Rooms)
            {
               
                r.Images = _basedb.ImageStore.Where(o => o.ReferIdZH == r.ID && o.Type == "Room").ToList();
            }

            return View(detail);
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