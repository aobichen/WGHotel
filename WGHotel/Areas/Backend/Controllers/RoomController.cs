using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WGHotel.Areas.Backend.Models;
using WGHotel.Controllers;

namespace WGHotel.Areas.Backend.Controllers
{
    public class RoomController : BaseController
    {
        // GET: Backend/Room
        public ActionResult Index()
        {
            //var model = _dbzh.Room.Where();
            var model = _dbzh.Room.ToList();
            return View(model);
        }


        public ActionResult Create(int id)
        {
            var RoomModel = new RoomViewModel();
            ViewBag.RoomTypes = RoomModel.RoomTypeSelectList;
            ViewBag.BedTypes = RoomModel.BedTypeSelectList;
            ViewBag.RoomFacility = RoomModel.FacilityList;
            return View();
        }

        [HttpPost]
        public ActionResult Create(RoomViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.HOTELID = 11;
                model.Create();
                return View();
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
    }
}