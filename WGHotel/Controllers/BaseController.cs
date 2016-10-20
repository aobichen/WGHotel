using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WGHotel.Models;

namespace WGHotel.Controllers
{
    public class BaseController : Controller
    {

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            var Requset = requestContext.HttpContext.Request;
            
            if (Request.Cookies["lang"]!=null && Request.Cookies["lang"].Value.ToLower().Equals("us"))
            {
                 _db = new WGHotelZHEntities("WGHotelUSEntities");
            }
            else
            {
                 _db = new WGHotelZHEntities("WGHotelZHEntities");
            }
        }

        protected WGHotelZHEntities _db;

        protected WGHotelZHEntities _dbzh;
        protected WGHotelUSEntities _dbus;
        protected WGHotelBaseEntities _basedb;
        // GET: Base
        public BaseController()
            : base()
        {
            _dbzh = new WGHotelZHEntities();
            _dbus = new WGHotelUSEntities();
            _basedb = new WGHotelBaseEntities();
        }
    }
}