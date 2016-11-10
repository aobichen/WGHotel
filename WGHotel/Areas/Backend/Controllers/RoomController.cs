using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WGHotel.Areas.Backend.Models;
using WGHotel.Controllers;
using WGHotel.Models;
using PagedList;

namespace WGHotel.Areas.Backend.Controllers
{
    [Authorize]
    public class RoomController : BaseController
    {
        public class PagedClientViewModel
        {
            public int Page { get; set; }
            public int id { get; set; }
        }
        // GET: Backend/Room
        public ActionResult Index(PagedClientViewModel Page = null)
        {
           

            if (User.IsInRole("Admin") || User.IsInRole("Admin"))
            {
                var result = (from room in _dbzh.Room
                             join hotel in _dbzh.Hotel
                             on room.HOTELID equals hotel.ID
                             
                             select new RoomList
                             {
                                 ID = room.ID,
                                 Name = room.Name,
                                 RoomType = room.RoomType,
                                 HOTELID = hotel.ID,
                                 HotelName = hotel.Name,
                                 Quantiy = room.Quantiy.Value,
                                 Sell = room.Sell.Value,
                                 BedType = room.BedType
                             }).ToList();
                var currentPage = Page.Page < 1 ? 1 : Page.Page;
                var PageSize = 15;

                var PageModel = result.ToPagedList(currentPage, PageSize);
                return View(PageModel);
            }

            var Userid = CurrentUser == null ? 0 : CurrentUser.Id;
            var Hotel = Userid > 0 ? _dbzh.Hotel.Where(o => o.UserId == Userid).FirstOrDefault() : null;
            //var model = _dbzh.Room.Where(o=>o.HOTELID == id).ToList();
            if (Page == null || Hotel == null)
            {
                return RedirectToAction("","Hotel");
            }
            var hotelId = Page != null ? Page.id : Hotel.ID;
            ViewBag.HotelID = hotelId;
            var model = (from room in _dbzh.Room
                         join hotel in _dbzh.Hotel
                         on room.HOTELID equals hotel.ID
                         where hotel.ID == hotelId
                         select new RoomList
                         {
                             ID = room.ID,
                             Name = room.Name,
                             RoomType = room.RoomType,
                             HOTELID = hotel.ID,
                             HotelName = hotel.Name,
                             Quantiy = room.Quantiy.Value,
                             Sell = room.Sell.Value,
                             BedType = room.BedType
                         }).ToList();
            var currentPage1 = Page.Page < 1 ? 1 : Page.Page;
            var PageSize1 = 15;

            var PageModel1 = model.ToPagedList(currentPage1, PageSize1);

            return View(PageModel1);
        }


        public ActionResult Create(int id)
        {
            var RoomModel = new RoomViewModel();
            RoomModel.HOTELID = id;
            //ViewBag.HotelID = id;
            ViewBag.RoomTypes = RoomModel.RoomTypeSelectList;
            ViewBag.BedTypes = RoomModel.BedTypeSelectList;
            
            ViewBag.RoomFacility = RoomModel.FacilityList;

            var key = Guid.NewGuid().GetHashCode().ToString("x");
            RoomModel.ImgKey = key;
            ViewBag.ImgKey = key;
            Session[key] = new List<ImageViewModel>();

            RoomModel.Sell = 2000;
            RoomModel.Quantiy = 1;

            return View(RoomModel);
        }

        [HttpPost]
        public ActionResult Create(RoomViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.HOTELID = model.HOTELID;
                model.Create();
                return RedirectToAction("", new {id = model.HOTELID });
            }

            var RoomModel = new RoomViewModel();
            ViewBag.RoomTypes = RoomModel.RoomTypeSelectList;
            ViewBag.BedTypes = RoomModel.BedTypeSelectList;
            ViewBag.RoomFacility = RoomModel.FacilityList;
            return View();
        }

        public ActionResult Edit(int id)
        {
            var modelzh = _dbzh.Room.Find(id);
            var modelus = _dbus.Room.Where(o => o.ParentId == modelzh.ID).FirstOrDefault();
            var model = new RoomViewModel { 
                Sell = modelzh.Sell.Value,
                BedType = modelzh.BedType,
                 Enabled = modelzh.Enabled.Value,
                 Facilities = modelzh.Facilities,
                 HOTELID = modelzh.HOTELID,
                 HasBreakfast = modelzh.HasBreakfast.Value,
                 NameUs = modelus.Name,
                 NoticeUs = modelus.Notice,
                 NoticeZh = modelzh.Notice,
                 Quantiy = modelzh.Quantiy.Value,
                 RoomType = modelzh.RoomType,
                 NameZh = modelzh.Name
            };
            var RoomModel = new RoomViewModel();
            ViewBag.RoomTypes = RoomModel.RoomTypeSelectList;
            ViewBag.BedTypes = RoomModel.BedTypeSelectList;
            ViewBag.RoomFacility = RoomModel.FacilityList;

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(RoomViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.HOTELID = 11;
                model.Create();
                return View("Edit");
            }
            return View();
        }

        public ActionResult Price(int id)
        {
            ViewBag.RoomId = id;
            return View();
        }
    }
}