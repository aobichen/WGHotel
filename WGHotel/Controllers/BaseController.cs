using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WGHotel.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using WGHotel.Areas.Backend.Models;
namespace WGHotel.Controllers
{
    public class BaseController : Controller
    {

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            var Requset = requestContext.HttpContext.Request;
            UserId = requestContext.HttpContext.User.Identity.GetUserId<int>();
            if (Request.Cookies["lang"]!=null && Request.Cookies["lang"].Value.ToLower().Equals("us"))
            {
                 _db = new WGHotelZHEntities("WGHotelUSEntities");
                 CurrentLanguage = Request.Cookies["lang"].Value.ToLower();
                
            }
            else
            {
                 _db = new WGHotelZHEntities("WGHotelZHEntities");
                 CurrentLanguage = "zh";
                 if (Request.Cookies["lang"] == null)
                 {
                     HttpCookie cookie = new HttpCookie("lang","zh");
                     Request.Cookies.Add(cookie);
                 }
                 else
                 {
                     Request.Cookies["lang"].Value = "zh";
                 }
                 
            }
            ViewBag.lang = CurrentLanguage;
            
        }

        protected WGHotelZHEntities _db;

        protected WGHotelZHEntities _dbzh;
        protected WGHotelUSEntities _dbus;
        protected WGHotelBaseEntities _basedb;

        protected string CurrentLanguage;

        

        private int UserId { get; set; }

        // GET: Base
        public BaseController()
            : base()
        {
            _dbzh = new WGHotelZHEntities();
            _dbus = new WGHotelUSEntities();
            _basedb = new WGHotelBaseEntities();
           
            //var RoomEditDate = new PRDate();
            //var RoomEditBeginDate = DateTime.Parse(RoomEditDate.Begin);
            //var RoomEditEndDate = DateTime.Parse(RoomEditDate.End);
            //ViewBag.GameSite = new GameSiteModel().SelectList();
        }

        protected ApplicationDbContext Account_db
        {

            get { return new ApplicationDbContext(); }
        }

        protected ApplicationUser CurrentUser
        {
            get
            {

                return Account_db.Users.Where(o => o.Id == UserId).FirstOrDefault();
            }
        }
    }
}