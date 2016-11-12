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

        public class SearchModel
        {
            public string word { get; set; }
            public DateTime Begin { get; set; }
            public DateTime End { get; set; }

            public string Game { get; set; }
        }

        private List<HotelViewModel> RenderImages(List<HotelViewModel> model)
        {
            if (CurrentLanguage.Equals("us"))
            {
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
            return model;
        }

        public ActionResult Index(SearchModel search=null)
        {
            var model = new List<HotelViewModel>();
            ViewBag.GameSite = new GameSiteModel().SelectList();
            #region
            if (string.IsNullOrEmpty(search.word) &&
                string.IsNullOrEmpty(search.Game)&&
                search.Begin == DateTime.MinValue &&
                search.End == DateTime.MinValue)
            {
                ViewBag.IsSearch = true;
                var Keelung = (from h in _db.Hotel
                                  where h.City == 1 && h.Room.Count>0
                                  select new HotelViewModel
                                  {
                                      ID = h.ID,
                                      Name = h.Name,
                                      //City = h.City,
                                      Game = h.Game,
                                      Sell = h.Room.Min(o => o.Sell),
                                      Tel = h.Tel,
                                      LinkUrl = h.LinkUrl
                                  }).OrderBy(x => Guid.NewGuid()).Take(8).ToList();
                ViewBag.Keelung = RenderImages(Keelung);

                var Taipei = (from h in _db.Hotel
                              where h.City == 2 && h.Room.Count > 0
                              select new HotelViewModel
                              {
                                  ID = h.ID,
                                  Name = h.Name,
                                  //City = h.City,
                                  Game = h.Game,
                                  Sell = h.Room.Min(o => o.Sell),
                                  Tel = h.Tel,
                                  LinkUrl = h.LinkUrl
                              }).OrderBy(x => Guid.NewGuid()).Take(8).ToList();
                ViewBag.Taipei = RenderImages(Taipei);

                var NewTaipei = (from h in _db.Hotel
                              where h.City == 3 && h.Room.Count > 0
                              select new HotelViewModel
                              {
                                  ID = h.ID,
                                  Name = h.Name,
                                  //City = h.City,
                                  Game = h.Game,
                                  Sell = h.Room.Min(o => o.Sell),
                                  Tel = h.Tel,
                                  LinkUrl = h.LinkUrl
                              }).OrderBy(x => Guid.NewGuid()).Take(8).ToList();
                ViewBag.NewTaipei = RenderImages(NewTaipei);

                var Taoyuan = (from h in _db.Hotel
                              where h.City == 4 && h.Room.Count > 0
                              select new HotelViewModel
                              {
                                  ID = h.ID,
                                  Name = h.Name,
                                  //City = h.City,
                                  Game = h.Game,
                                  Sell = h.Room.Min(o => o.Sell),
                                  Tel = h.Tel,
                                  LinkUrl = h.LinkUrl
                              }).OrderBy(x => Guid.NewGuid()).Take(8).ToList();
                ViewBag.Taoyuan = RenderImages(Taoyuan);

                var HsinchuCity = (from h in _db.Hotel
                               where h.City == 5 && h.Room.Count > 0
                               select new HotelViewModel
                               {
                                   ID = h.ID,
                                   Name = h.Name,
                                   //City = h.City,
                                   Game = h.Game,
                                   Sell = h.Room.Min(o => o.Sell),
                                   Tel = h.Tel,
                                   LinkUrl = h.LinkUrl
                               }).OrderBy(x => Guid.NewGuid()).Take(8).ToList();
                ViewBag.HsinchuCity = RenderImages(HsinchuCity);

                var HsinchuCountry = (from h in _db.Hotel
                                   where h.City == 6 && h.Room.Count > 0
                                   select new HotelViewModel
                                   {
                                       ID = h.ID,
                                       Name = h.Name,
                                       //City = h.City,
                                       Game = h.Game,
                                       Sell = h.Room.Min(o => o.Sell),
                                       Tel = h.Tel,
                                       LinkUrl = h.LinkUrl
                                   }).OrderBy(x => Guid.NewGuid()).Take(8).ToList();
                ViewBag.HsinchuCountry = RenderImages(HsinchuCountry);

                return View(model);
            }

            #endregion

            var CityModel = _db.City.Where(o => o.Name.Contains(search.word)).FirstOrDefault();
            var City = CityModel == null ? 0 : CityModel.ID;

            var hotel = (from h in _db.Hotel
                     where
                         //(City==0||h.City == City) ||
                     (string.IsNullOrEmpty(search.word) || h.Name.Contains(search.word))
                     && (string.IsNullOrEmpty(search.Game) || h.Game.Contains(search.Game))
                         select h).OrderBy(x => Guid.NewGuid()).ToList();
            var checkInDate = search.Begin;
            foreach(var h in hotel){
                var r = h.Room.FirstOrDefault();
                if (r != null)
                {
                   
                    var has = _basedb.RoomPrice.Where(
                        o => o.ROOMID == r.ID &&
                        System.Data.Objects.EntityFunctions.TruncateTime(o.Date) == checkInDate
                        ).FirstOrDefault();
                    if (has == null || has.SaleOff == true)
                    {
                        model.Add(new HotelViewModel
                        {
                            ID = h.ID,
                            Name = h.Name,
                            Game = h.Game,
                            Sell = h.Room.Min(o => o.Sell),
                            Tel = h.Tel,
                            LinkUrl = h.LinkUrl
                        });                      
                    }
                }
            }
            
          
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
            detail.Address = model.Address;
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

            var gamesites = model.Game.Split(',').ToList();

            var NearHotels = _db.Hotel.Where(o => gamesites.Contains(o.Game) && o.ID != model.ID)
                .Select(o => new HotelDetail
            {
                ID = o.ID,
                Address = o.Address,
                Name = o.Name,
              ParentId = o.ParentId == null ? 0 : o.ParentId.Value
            }).OrderBy(x => Guid.NewGuid()).Take(5).ToList();

            

            foreach (var item in NearHotels)
            {
                var ReferIdZH = item.ID;
                if (CurrentLanguage.Equals("us"))
                {
                    ReferIdZH = item.ParentId;
                }
                item.Images = _basedb.ImageStore.Where(o => o.ReferIdZH == ReferIdZH).ToList();

            }
            ViewBag.NearHotels = NearHotels;
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