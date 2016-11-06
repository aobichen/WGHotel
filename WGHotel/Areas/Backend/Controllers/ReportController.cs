using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WGHotel.Areas.Backend.Controllers
{
    public class ReportController : Controller
    {
        // GET: Backend/Report
        public ActionResult Index()
        {
            return View();
        }

        // GET: Backend/Report/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Backend/Report/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Backend/Report/Create
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

        // GET: Backend/Report/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Backend/Report/Edit/5
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
