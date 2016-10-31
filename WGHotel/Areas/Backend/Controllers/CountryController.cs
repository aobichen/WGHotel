using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WGHotel.Controllers;
using WGHotel.Models;

namespace WGHotel.Areas.Backend.Controllers
{
    public class CountryController : BaseController
    {
        // GET: Backend/Country
        public ActionResult Index()
        {
            ViewBag.Country = _basedb.Country.ToList();
            return View();
        }

        public ActionResult Edit(int? id)
        {
            if (id.HasValue)
            {
                var model = _basedb.Country.Find(id);
                return View(model);
            }
          
            //ViewBag.Country = _basedb.Country.ToList();
            return View(new Country());
        }

        [HttpPost]
        public ActionResult Edit(Country model)
        {
            if (model.ID != 0)
            {
                var result = _basedb.Country.Find(model.ID);
                result.Name = model.Name;
                result.NUSF = model.NUSF;
                result.Tel1 = model.Tel1;
                result.Tel2 = model.Tel2;
                result.Telmobile = model.Telmobile;
                result.Fax = model.Fax;
                _basedb.SaveChanges();
                return RedirectToAction("","Country");
            }
            if (_basedb.Country.Any(o => o.Name == model.Name))
            {
                ModelState.AddModelError("","資料已存在");
                return View();
            }

            _basedb.Country.Add(model);
            _basedb.SaveChanges();
            return View("Edit");
        }
    }
}