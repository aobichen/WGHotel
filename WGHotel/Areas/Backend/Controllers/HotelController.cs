using System;
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
    [Authorize]
    public class HotelController : BaseController
    {
       
        public ActionResult Index(string SearchString="",int Page=1)
        {

            var IsHotel = User.IsInRole("Hotel");
            var IsAdmin = User.IsInRole("Admin");
            ViewBag.ViewMessage = string.IsNullOrEmpty(SearchString) ? "目前無任何資料":"沒有任何搜尋資訊";
           // var model = _dbzh.Hotel.ToList();
            if (!IsAdmin)
            {
                SearchString = string.Empty;
                Page = 1;
            }
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
                         }).OrderBy(o => o.ID).ToList();

                if (!IsAdmin)
                {
                    model = model.Where(o => o.UserId == CurrentUser.Id).ToList();
                }
                         
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
        [Authorize(Roles = "Admin,System")]
        public ActionResult Create(int? id)
        {
            
           
            var model = new AccountHotelViewModel();

            var AccountAndImgKey = Guid.NewGuid().GetHashCode().ToString("x");
            model.Account = AccountAndImgKey.ToUpper();
            model.Password = Guid.NewGuid().GetHashCode().ToString("x");
            ViewBag.HotelFacility = new Facilities().Facility();
            ViewBag.ImgKey = AccountAndImgKey;
            Session[AccountAndImgKey] = new List<ImageViewModel>();
            //model.ImgKey = AccountAndImgKey;
            ViewBag.GameSites = new GameSiteModel().SelectList();
            ViewBag.City = new GameSiteModel().Citys();
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,System")]
        public ActionResult Create(AccountHotelViewModel model)
        {
            
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
                var UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var user = new ApplicationUser { UserName = model.Account, Email = model.Password };
                ApplicationRoleManager _roleManager =  HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
                var CurrentRole = "Hotel";
          
               UserManager.Create(user, model.Password);
               UserManager.AddToRole(user.Id, CurrentRole);
               var id = UserManager.FindByName(model.Account).Id;
                model.UserId = id;
                model.Create();
                return RedirectToAction("Index");
            }


           ViewBag.HotelFacility = new Facilities().Facility();
            ViewBag.ImgKey = model.ImgKey;
            Session[model.ImgKey] = new List<ImageViewModel>();
            //model.ImgKey = AccountAndImgKey;
            ViewBag.GameSites = new GameSiteModel().SelectList();
            ViewBag.City = new GameSiteModel().Citys();
            ModelState.AddModelError("", "錯誤!請檢查資料");
            return View();
        }

        [Authorize(Roles = "Admin,System")]
        public ActionResult Edit(int id)
        {
            //var zh = _dbzh.Hotel.Find(id);
            //var us = _dbus.Hotel.Find()
            var ZHmodel = _dbzh.Hotel.Find(id);
            var USmodel = _dbus.Hotel.Find(id);

            if (ZHmodel == null || USmodel==null)
            {
                return View("Index");
            }

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
            ViewBag.HotelFacility = new Facilities().Facility("zh",Facilies);
            ViewBag.GameSites = new GameSiteModel().SelectList(GameSite);
            ViewBag.City = new GameSiteModel().Citys(ZHmodel.City);
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,System")]
        public ActionResult Edit(AccountHotelViewModel model)
        {
            //var zh = _dbzh.Hotel.Find(id);
            //var us = _dbus.Hotel.Find()
            var ZHmodel = _dbzh.Hotel.Find(model.ID);
            var USmodel = _dbus.Hotel.Find(model.ID);

            var Manager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var u = Manager.FindById(ZHmodel.UserId).UserName;

            //var model = new AccountHotelViewModel();
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
            ViewBag.HotelFacility = new Facilities().Facility("zh",Facilies);
            ViewBag.GameSites = new GameSiteModel().SelectList(GameSite);
            ViewBag.City = new GameSiteModel().Citys(ZHmodel.City);
            return View(model);
        }

        public ActionResult MyHotel(int id)
        {
            var ZHmodel = _dbzh.Hotel.Find(id);

            if (CurrentUser.Id != ZHmodel.UserId)
            {
                return RedirectToAction("","Hotel");
            }

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
            model.City = ZHmodel.City;
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
            ViewBag.HotelFacility = new Facilities().Facility("zh",Facilies);
            ViewBag.GameSites = new GameSiteModel().SelectList(GameSite);
            ViewBag.City = new GameSiteModel().Citys(ZHmodel.City);
            return View(model);
            
        }
        
    }
}