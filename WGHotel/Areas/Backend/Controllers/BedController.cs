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
    public class BedController : BaseController
    {
        //
        // GET: /Backend/Bed/
        public ActionResult Index(int Page=1)
        {
            
            var model =  new List<BedViewModel>();
            var ZH = _dbzh.CodeFile.Where(o => o.ItemType == "Bed").ToList();
            foreach(var item in ZH)
            {
                var EN = _dbus.CodeFile.Where(o=>o.ParentId == item.ID).FirstOrDefault();
                model.Add(new BedViewModel{ 
                    ID = item.ID,
                    NameZH = item.ItemDescription,
                    NameEN = EN.ItemDescription,
                    Deleted = item.Deleted
                });
            }
            
            var currentPage = Page < 1 ? 1 : Page;
            var PageSize = 15;
            //currentPage = !string.IsNullOrEmpty(SearchString) ? 1 : currentPage;
            // var model = _basedb.Country.Where(o => string.IsNullOrEmpty(SearchString) || o.Name.Contains(SearchString)).ToList();

            var PageModel = model.ToPagedList(currentPage, PageSize);
            return View(PageModel);
        }

        public ActionResult Edit(int? id)
        {
            if (id.HasValue)
            {
                var ZH = _dbzh.CodeFile.Where(o => o.ItemType == "Bed" && o.ID == id).FirstOrDefault();

                if (ZH == null)
                {
                    return View();
                }

                var EN = _dbus.CodeFile.Where(o => o.ItemType == "Bed" && o.ParentId == ZH.ID ).FirstOrDefault();
                if (ZH == null)
                {
                    return View();
                }

                var model = new BedViewModel();
                model.ID = ZH.ID;
                model.NameZH = ZH.ItemDescription;
                model.NameEN = EN.ItemDescription;
                model.Deleted = ZH.Deleted;
                return View(model);
            }
            return View();
        }

        [HttpPost]
        public ActionResult Edit(BedViewModel model)
        {
            model.Creator = CurrentUser.Id;
             if (model.ID > 0)
                {
                    model.Edit();
                    return RedirectToAction("Index");
                }
                else
                {
                    model.Create();
                    return RedirectToAction("Index");
                }
            
            return View();
        }
	}
}