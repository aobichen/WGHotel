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
    [Authorize(Roles = "Admin,System")]
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
        public ActionResult Edit(int? id)
        {
            if (id.HasValue)
            {
                var model = new FacilityModel();
                var zh = _dbzh.Facility.Find(id);
                var us = _dbus.Facility.Where(o => o.ParentId == zh.ID).First();
                model.ID = zh.ID;
                model.NameZH = zh.Name;
                model.NameUS = us.Name;
                model.Enabled = zh.Enabled;
                return View(model);
            }
            return View();
        }

        // POST: Backend/Facility/Create
        [HttpPost]
        public ActionResult Edit(FacilityModel model)
        {
            if (model.ID <= 0)
            {
                model.Create();
                return RedirectToAction("","Facility");
            }
            else
            {
                model.Edit();
            }
            return View();
        }

       

    }
}
