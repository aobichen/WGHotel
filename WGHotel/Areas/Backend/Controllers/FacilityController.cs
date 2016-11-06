using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WGHotel.Areas.Backend.Models;
using WGHotel.Controllers;
using PagedList;
namespace WGHotel.Areas.Backend.Controllers
{
    public class FacilityController : BaseController
    {
        // GET: Backend/Facility
        public ActionResult Index(int Page=1)
        {
            var ZH = _dbzh.Facility.ToList();
            var US = _dbus.Facility.ToList();
            var model = new List<FacilityModel>();
            foreach (var item in ZH)
            {
                model.Add(new FacilityModel {
                     ID = item.ID,
                     NameZH = item.Name,
                     NameUS = US.Where(o=>o.ParentId == item.ID).First().Name,
                     Enabled = item.Enabled
                });
            }

            var currentPage = Page < 1 ? 1 : Page;
            var PageSize = 15;
            //currentPage = !string.IsNullOrEmpty(SearchString) ? 1 : currentPage;
           // var model = _basedb.Country.Where(o => string.IsNullOrEmpty(SearchString) || o.Name.Contains(SearchString)).ToList();

            var PageModel = model.ToPagedList(currentPage, PageSize);

            return View(PageModel);
        }

        // GET: Backend/Facility/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Backend/Facility/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Backend/Facility/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Backend/Facility/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Backend/Facility/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Backend/Facility/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Backend/Facility/Delete/5
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
