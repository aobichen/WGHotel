using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WGHotel.Areas.Backend.Models;
using WGHotel.Controllers;
using WGHotel.Models;
using PagedList;
namespace WGHotel.Areas.Backend.Controllers
{
    public class AdminController : BaseController
    {
        public ActionResult Index(string SearchString="",int Page=1)
        {

           // var model = _dbzh.Hotel.ToList();

            var model = (from h in _db.Hotel
                         where string.IsNullOrEmpty(SearchString) || h.Name.Contains(SearchString)
                         select new HotelListViewModel
                         {
                            City = h.City,
                             Game = h.Game,
                             ID = h.ID,
                             Name = h.Name,
                            UserId = h.UserId
                         }).ToList().OrderBy(o => o.ID);
                         
            var currentPage = Page < 1 ? 1 : Page;
            var PageSize = 30;

            var PageModel = model.ToPagedList(currentPage, PageSize);

            //var currentPage = Page < 1 ? 1 : Page;
            //var PageSize = 10;
            //var model = _db.Scenic.ToList().OrderByDescending(o => o.ID);
            //var result = model.ToPagedList(currentPage, PageSize);
            return View(PageModel);

           
        }
        // GET: Backend/Admin
        //public ActionResult Index()
        //{

        //    var model = new AccountHotelViewModel();

        //    var AccountAndImgKey = Guid.NewGuid().GetHashCode().ToString("x");
        //    model.Account = AccountAndImgKey.ToUpper();
        //    model.Password = Guid.NewGuid().GetHashCode().ToString("x");
        //    ViewBag.HotelFacility = model.HotelFacility;
        //    ViewBag.ImgKey = AccountAndImgKey;
        //    Session[AccountAndImgKey] = new List<ImageViewModel>();
        //    return View(model);
        //}

        [HttpPost]
        public ActionResult Create(AccountHotelViewModel model)
        {
            if (Request["HotelFacility"] != null)
            {
                var a = Request["HotelFacility"].ToString();
            }

            var account = new RegisterViewModel { UserName = model.Account, Password = model.Password, ConfirmPassword=model.Password };

            var aw = new AccountController().Register(account);

            _dbzh.Hotel.Add(new Hotel { 
                Name = model.Namezh,
                Address = model.Addresszh,
                Area = model.Area,
                City = model.City,
                Facilities = "",
                Features = model.Featurezh,
                Enabled = true,
                LinkUrl = model.LinkUrl,
                Game = model.Game
            });

            _dbus.Hotel.Add(new Hotel
            {
                Name = model.Nameus,
                Address = model.Addressus,
                Area = model.Area,
                City = model.City,
                Facilities = "",
                Features = model.Featureus,
                Enabled = true,
                LinkUrl = model.LinkUrl,
                Game = model.Game
            });
            return View();
        }
    }
}