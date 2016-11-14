using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WGHotel.Areas.Backend.Models;
using WGHotel.Controllers;
using PagedList;
using WGHotel.Models;

namespace WGHotel.Areas.Backend.Controllers

{
    [Authorize]
    public class ReportController : BaseController
    {
        // GET: Backend/Report
        public ActionResult Index(int Page=1)
        {
            var IsAdmin = (User.IsInRole("Admin") || User.IsInRole("SystemAdmin"))?true:false;
            var model = (from report in _basedb.Report
                         where IsAdmin || report.Creator == CurrentUser.Id
                         select new ReportViewModel
                             {
                                 CheckInDate = report.CheckInDate,
                                 Price = report.Price==null?0:report.Price,
                                 Country = report.Country,
                                 ID = report.ID,
                                 NumOfPeople = report.NumOfPeople,
                                Room = report.Room
                             }).OrderByDescending(o=>o.CheckInDate).ToList();

            var currentPage = Page < 1 ? 1 : Page;
            var PageSize = 15;

            var PageModel = model.ToPagedList(currentPage, PageSize);
            return View(PageModel);
        }

       

        // GET: Backend/Report/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id.HasValue)
            {
                var result = new ReportViewModel();
                var model = _basedb.Report.Where(o => o.ID == id).Select(o =>
                     new ReportViewModel
                     {
                         CheckInDate = o.CheckInDate,
                         Price = o.Price,
                         OtherCost = o.OtherCost,
                         Other = o.Other,
                         FoodCost = o.FoodCost,
                         Food = o.Food,
                         Country = o.Country,
                         CountryID = o.CountryID,
                         HotelID = o.HotelID,
                         ID = o.ID,
                         NumOfPeople = o.NumOfPeople,
                         Remark = o.Remark,
                         Room = o.Room,
                         RoomID = o.RoomID,
                         UserType = o.UserType
                     }).FirstOrDefault();
                    
                var Hotel = _dbzh.Hotel.Where(o => o.UserId == CurrentUser.Id).FirstOrDefault();
                model.HotelID = Hotel.ID;

                model.RoomOfReport = _basedb.ReportRooms.Where(o => o.ID == model.ID).ToList();
                
                var Rooms = Hotel == null ? new List<Room>() : Hotel.Room;
                ViewBag.RoomId = new SelectList(Rooms, "ID", "Name",model.RoomID);
                var Country = _basedb.Country.ToList();
                ViewBag.Country = new SelectList(Country, "ID", "Name",model.CountryID);
                ViewBag.UserType = model.UserType;
                return View(model);
            }
            else
            {
                var Hotel = _dbzh.Hotel.Where(o => o.UserId == CurrentUser.Id).FirstOrDefault();
                var Rooms = Hotel == null ? new List<Room>() : Hotel.Room;
                ViewBag.RoomId = new SelectList(Rooms, "ID", "Name");
                var Country = _basedb.Country.ToList();
                ViewBag.Country = new SelectList(Country, "ID", "Name");
                var model = new ReportViewModel();

                model.HotelID = Hotel.ID;
                return View(model);
            }

            

            //return View();
        }

        // POST: Backend/Report/Edit/5
        [HttpPost]
        public ActionResult Edit(ReportViewModel model)
        {

            //#region
            //if (model.Price == null || model.CheckInDate == DateTime.MinValue || string.IsNullOrEmpty(model.Room))
            //{
            //    var result = new ReportViewModel();
            //    var model1 = _basedb.Report.Where(o => o.ID == model.ID).Select(o =>
            //         new ReportViewModel
            //         {
            //             CheckInDate = o.CheckInDate,
            //             Price = o.Price,
            //             OtherCost = o.OtherCost,
            //             Other = o.Other,
            //             FoodCost = o.FoodCost,
            //             Food = o.Food,
            //             Country = o.Country,
            //             CountryID = o.CountryID,
            //             HotelID = o.HotelID,
            //             ID = o.ID,
            //             NumOfPeople = o.NumOfPeople,
            //             Remark = o.Remark,
            //             Room = o.Room,
            //             RoomID = o.RoomID,
            //             UserType = o.UserType
            //         }).FirstOrDefault();

            //    var Hotel = _dbzh.Hotel.Where(o => o.UserId == CurrentUser.Id).FirstOrDefault();
            //    model.HotelID = Hotel.ID;

            //    model.RoomOfReport = _basedb.ReportRooms.Where(o => o.ID == model.ID).ToList();

            //    var Rooms = Hotel == null ? new List<Room>() : Hotel.Room;
            //    ViewBag.RoomId = new SelectList(Rooms, "ID", "Name", model.RoomID);
            //    var Country = _basedb.Country.ToList();
            //    ViewBag.Country = new SelectList(Country, "ID", "Name", model.CountryID);
            //    ViewBag.UserType = model.UserType;
            //    return View(model1);
            //}
            //#endregion

            //model.HotelID = _dbzh.Room.Find(model.HotelID).Hotel.ID;
            if (!_dbzh.Hotel.Any(o => o.ID == model.HotelID && o.UserId == CurrentUser.Id))
            {
                return View();
            }
            var Now = DateTime.Now;
            var UserId = CurrentUser.Id;
            model.Created = Now;
            model.Creator = UserId;
            model.Modified = Now;
            model.Modify = UserId;
            //model.CheckInDate = Now;
        
           
            //model.Room = _dbzh.Room.Find(model.RoomID).Name;
            try
            {
                if (model.ID <= 0)
                {
                    model.Create();
                    return RedirectToAction("Index");
                }
                else
                {
                    model.Edit();
                    return RedirectToAction("Edit", new { id = model.ID });
                }

                
            }
            catch(Exception ex)
            {
                var Hotel = _dbzh.Hotel.Where(o => o.UserId == CurrentUser.Id).FirstOrDefault();
                var Rooms = Hotel == null ? new List<Room>() : Hotel.Room;
                ViewBag.RoomId = new SelectList(Rooms, "ID", "Name");
                var Country = _basedb.Country.ToList();
                ViewBag.Country = new SelectList(Country, "ID", "Name");
                ModelState.AddModelError("","編輯未完成，請檢查資料");
                return View();
            }
        }

        // GET: Backend/Report/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Backend/Report/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
