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
                                 Price = report.Price,
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
                var model = _basedb.Report.Find(id);
                var Hotel = _dbzh.Hotel.Where(o => o.UserId == CurrentUser.Id).FirstOrDefault();
                var Rooms = Hotel == null ? new List<Room>() : Hotel.Room;
                ViewBag.RoomId = new SelectList(Rooms, "ID", "Name",model.RoomID);
                var Country = _basedb.Country.ToList();
                ViewBag.Country = new SelectList(Country, "ID", "Name",model.CountryID);
            }
            else
            {
                var Hotel = _dbzh.Hotel.Where(o => o.UserId == CurrentUser.Id).FirstOrDefault();
                var Rooms = Hotel == null ? new List<Room>() : Hotel.Room;
                ViewBag.RoomId = new SelectList(Rooms, "ID", "Name");
                var Country = _basedb.Country.ToList();
                ViewBag.Country = new SelectList(Country, "Name", "Name");
            }

            

            return View();
        }

        // POST: Backend/Report/Edit/5
        [HttpPost]
        public ActionResult Edit(ReportViewModel model)
        {
            model.HotelID = _dbzh.Room.Find(model.RoomID).Hotel.ID;
            var Now = DateTime.Now;
            var UserId = CurrentUser.Id;
            model.Created = Now;
            model.Creator = UserId;
            model.Modified = Now;
            model.Modify = UserId;
            model.Room = _dbzh.Room.Find(model.RoomID).Name;
            try
            {
                if (model.ID <= 0)
                {
                    model.Create();
                }
                else
                {
                    model.Edit();
                }

                return RedirectToAction("Index");
            }
            catch
            {
                var Hotel = _dbzh.Hotel.Where(o => o.UserId == CurrentUser.Id).FirstOrDefault();
                var Rooms = Hotel == null ? new List<Room>() : Hotel.Room;
                ViewBag.RoomId = new SelectList(Rooms, "ID", "Name");
                var Country = _basedb.Country.ToList();
                ViewBag.Country = new SelectList(Country, "ID", "Name");
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
