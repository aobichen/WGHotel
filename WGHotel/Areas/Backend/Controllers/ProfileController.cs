using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WGHotel.Areas.Backend.Controllers
{
    public class ProfileController : Controller
    {
        // GET: Backend/Profile
        public ActionResult Index()
        {
            return View();
        }

        // GET: Backend/Profile/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Backend/Profile/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Backend/Profile/Create
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

        // GET: Backend/Profile/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Backend/Profile/Edit/5
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

        // GET: Backend/Profile/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Backend/Profile/Delete/5
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
