using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WGHotel.Areas.Backend.Controllers
{
    public class AdminController : Controller
    {
        // GET: Backend/Admin
        public ActionResult Index()
        {
            return View();
        }
    }
}