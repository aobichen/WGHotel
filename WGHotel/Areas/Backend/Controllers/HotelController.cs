﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using WGHotel.Controllers;
using WGHotel.Areas.Backend.Models;
using WGHotel.Models;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity; // Maybe this one too



namespace WGHotel.Areas.Backend.Controllers
{
    public class HotelController : BaseController
    {
        public ActionResult Index(string SearchString="",int Page=1)
        {

            ViewBag.ViewMessage = string.IsNullOrEmpty(SearchString) ? "目前無任何資料":"沒有任何搜尋資訊";
           // var model = _dbzh.Hotel.ToList();
            
            var model = (from h in _db.Hotel
                         join c in _db.City on h.City equals c.ID
                         where string.IsNullOrEmpty(SearchString) || h.Name.Contains(SearchString)
                         select new HotelListViewModel
                         {
                            City = c.Name,
                            Game = h.Game,
                            ID = h.ID,
                            Name = h.Name,
                            UserId = h.UserId,
                            Tel = h.Tel
                         }).ToList().OrderBy(o => o.ID);
                         
            var currentPage = Page < 1 ? 1 : Page;
            var PageSize = 15;

            var PageModel = model.ToPagedList(currentPage, PageSize);

            //var currentPage = Page < 1 ? 1 : Page;
            //var PageSize = 10;
            //var model = _db.Scenic.ToList().OrderByDescending(o => o.ID);
            //var result = model.ToPagedList(currentPage, PageSize);
            return View(PageModel);

           
        }

        
         //GET: Backend/Admin
        public ActionResult Create(int? id)
        {
            
           
            var model = new AccountHotelViewModel();

            var AccountAndImgKey = Guid.NewGuid().GetHashCode().ToString("x");
            model.Account = AccountAndImgKey.ToUpper();
            model.Password = Guid.NewGuid().GetHashCode().ToString("x");
            ViewBag.HotelFacility = new CodeFiles().GetHotelFacility();
            ViewBag.ImgKey = AccountAndImgKey;
            Session[AccountAndImgKey] = new List<ImageViewModel>();
            //model.ImgKey = AccountAndImgKey;
            ViewBag.GameSites = new GameSiteModel().SelectList();
            ViewBag.City = new GameSiteModel().Citys();
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(AccountHotelViewModel model)
        {
    //        var errors = ModelState
    //.Where(x => x.Value.Errors.Count > 0)
    //.Select(x => new { x.Key, x.Value.Errors })
    //.ToArray();
            var Facility = string.Empty;
            if (Request["HotelFacility"] != null)
            {
                Facility = Request["HotelFacility"].ToString();
            }
            model.Facilies = Facility;


            var GameSite = string.Empty;
            if (Request["GameSite"] != null)
            {
                GameSite = Request["GameSite"].ToString();
            }

            model.Game = GameSite;

           if (ModelState.IsValid)
            {
                var Manager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var user = new ApplicationUser { UserName = model.Account, Email = model.Account };
                Manager.Create(user, model.Password);
                var id = Manager.FindByName(model.Account).Id;
                model.UserId = id;
                model.Create();
                return RedirectToAction("Create");
            }

          
            ViewBag.HotelFacility = new CodeFiles().GetHotelFacility();
            ViewBag.ImgKey = model.ImgKey;
            Session[model.ImgKey] = new List<ImageViewModel>();
            //model.ImgKey = AccountAndImgKey;
            ViewBag.GameSites = new GameSiteModel().List();
            ViewBag.City = new GameSiteModel().Citys();
            ModelState.AddModelError("", "錯誤!請檢查資料");
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            //var zh = _dbzh.Hotel.Find(id);
            //var us = _dbus.Hotel.Find()
            var ZHmodel = _dbzh.Hotel.Find(id);
            var USmodel = _dbus.Hotel.Find(id);

            var Manager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var u = Manager.FindById(ZHmodel.UserId).UserName;

            var model = new AccountHotelViewModel();
            model.Account = u;
            model.Addressus = USmodel.Address;
            model.Addresszh = ZHmodel.Address;
            model.Area = ZHmodel.Area;
            model.Featureus = USmodel.Features;
            model.Featurezh = ZHmodel.Features;
            model.Tel = ZHmodel.Tel;
            model.Nameus = USmodel.Name;
            model.Namezh = ZHmodel.Name;
            model.LinkUrl = ZHmodel.LinkUrl;
            var sessionkey = Guid.NewGuid().GetHashCode().ToString("x");
            ViewBag.ImgKey = sessionkey;
            
            var Imgs = _basedb.ImageStore.Where(o => o.ReferIdZH == ZHmodel.ID && o.Type == "Hotel").Select(o => new ImageViewModel
            {
                ReferIdZH = o.ReferIdZH.Value,
                Extension = o.Extension,
                Image = o.Image,
                Name = o.Name,
                SessionKey = sessionkey,
                Type = o.Type

            }).ToList();

            Session[sessionkey] = Imgs;
            //model.Facilies = 
            var Facilies = ZHmodel.Facilities.Split(',').Select(int.Parse).ToList();
            var GameSite = ZHmodel.Game.Split(',').Select(int.Parse).ToList();
            ViewBag.HotelFacility = new CodeFiles().GetHotelFacility(Facilies);
            ViewBag.GameSites = new GameSiteModel().SelectList(GameSite);
            ViewBag.City = new GameSiteModel().Citys(ZHmodel.City);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(AccountHotelViewModel model)
        {
            //var zh = _dbzh.Hotel.Find(id);
            //var us = _dbus.Hotel.Find()
            var ZHmodel = _dbzh.Hotel.Find(id);
            var USmodel = _dbus.Hotel.Find(id);

            var Manager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var u = Manager.FindById(ZHmodel.UserId).UserName;

            var model = new AccountHotelViewModel();
            model.Account = u;
            model.Addressus = USmodel.Address;
            model.Addresszh = ZHmodel.Address;
            model.Area = ZHmodel.Area;
            model.Featureus = USmodel.Features;
            model.Featurezh = ZHmodel.Features;
            model.Tel = ZHmodel.Tel;
            model.Nameus = USmodel.Name;
            model.Namezh = ZHmodel.Name;
            model.LinkUrl = ZHmodel.LinkUrl;
            var sessionkey = Guid.NewGuid().GetHashCode().ToString("x");
            ViewBag.ImgKey = sessionkey;

            var Imgs = _basedb.ImageStore.Where(o => o.ReferIdZH == ZHmodel.ID && o.Type == "Hotel").Select(o => new ImageViewModel
            {
                ReferIdZH = o.ReferIdZH.Value,
                Extension = o.Extension,
                Image = o.Image,
                Name = o.Name,
                SessionKey = sessionkey,
                Type = o.Type

            }).ToList();

            Session[sessionkey] = Imgs;
            //model.Facilies = 
            var Facilies = ZHmodel.Facilities.Split(',').Select(int.Parse).ToList();
            var GameSite = ZHmodel.Game.Split(',').Select(int.Parse).ToList();
            ViewBag.HotelFacility = new CodeFiles().GetHotelFacility(Facilies);
            ViewBag.GameSites = new GameSiteModel().SelectList(GameSite);
            ViewBag.City = new GameSiteModel().Citys(ZHmodel.City);
            return View(model);
        }
        
    }
}