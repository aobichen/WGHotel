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
    public class GameSiteController : BaseController
    {
        // GET: Backend/GameSite
        public ActionResult Index(int Page =1)
        {
            var currentPage = Page < 1 ? 1 : Page;
            var PageSize = 15;
            var model = new GameSiteListModel().List();
            var PageModel = model.ToPagedList(currentPage, PageSize);
            //ViewBag.GameList = new GameSiteListModel().List();
            return View(PageModel);
        }

        public ActionResult Edit(int? id)
        {
            if (id.HasValue)
            {
                var zh = _dbzh.GameSite.Find(id);
                var us = _dbus.GameSite.Where(o => o.ParentId == zh.ID).FirstOrDefault();
                var model = new GameSiteListModel();
                model.RemarkUS = us.Remark;
                model.RemarkZH = zh.Remark;
                model.SportUS = us.Sports;
                model.SportZH = us.Sports;
                model.TypeUS = us.Type;
                model.TypeZH = zh.Type;
                model.VenueUS = us.Venue;
                model.VenueZH = zh.Venue;
                model.ID = zh.ID;
                return View(model);
            }
            return View(new GameSiteListModel());
        }

        [HttpPost]
        public ActionResult Edit(GameSiteListModel model)
        {
            if (model.ID >0)
            {
                model.Edit();
                return RedirectToAction("","GameSite");
            }

            if (_dbzh.GameSite.Any(o => o.Sports == model.SportZH && o.Type == model.TypeZH && o.Venue == model.VenueZH))
            {
                ModelState.AddModelError("","已有相同項目");
                return View();
            }
            model.Create();
            return RedirectToAction("","GameSite");
        }
    }
}