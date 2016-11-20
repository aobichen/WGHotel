using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WGHotel.Models;

namespace WGHotel.Areas.Backend.Models
{
    public class BedViewModel
    {
        public int ID { get; set; }
        public string NameZH { get; set; }
        public string NameEN { get; set; }

        public bool Deleted { get; set; }
        public int Creator { get; set; }

        public void Create()
        {
            var _dbZH =  new WGHotelZHEntities();
            var _dbEN = new WGHotelUSEntities();

            var Today = DateTime.Now;

            var ZH = new CodeFile();
            ZH.ItemCode = "Bed";
            ZH.ItemType = "Bed";
            ZH.ItemDescription = NameZH;
            ZH.Created = Today;
            ZH.Creator = Creator;
            ZH.Modified = Today;
            ZH.Modify = Creator;
            ZH.Remark = "Bed Type";
            _dbZH.CodeFile.Add(ZH);
            _dbZH.SaveChanges();

            var EN = new CodeFile();
            EN.ItemCode = "Bed";
            EN.ItemType = "Bed";
            EN.ItemDescription = NameEN;
            EN.Created = Today;
            EN.Creator = Creator;
            EN.Modified = Today;
            EN.Modify = Creator;
            EN.ParentId = ZH.ID;
            EN.Remark = "Bed Type";
            _dbEN.CodeFile.Add(EN);
            _dbEN.SaveChanges();
        }

        public void Edit()
        {
            var _dbZH = new WGHotelZHEntities();
            var _dbEN = new WGHotelUSEntities();

            var Today = DateTime.Now;

            var ZH = _dbZH.CodeFile.Where(o => o.ID == ID && o.ItemCode=="Bed").FirstOrDefault();
            ZH.ItemCode = "Bed";
            ZH.ItemType = "Bed";
            ZH.ItemDescription = NameZH;
           
            ZH.Modified = Today;
            ZH.Modify = Creator;
            ZH.Deleted = Deleted;
            //_dbZH.CodeFile.Add(ZH);
            _dbZH.SaveChanges();

            var ParentId = ZH.ID;

            var EN = _dbEN.CodeFile.Where(o => o.ParentId == ParentId && o.ItemCode == "Bed").FirstOrDefault();
            EN.ItemCode = "Bed";
            EN.ItemType = "Bed";
            EN.ItemDescription = NameEN;
           
            EN.Modified = Today;
            EN.Modify = Creator;
            EN.Deleted = Deleted;
            
            
            _dbEN.SaveChanges();
        }

        
    }

    public class BedModel
    {
        public List<SelectListItem> SelectList(List<int> Selected = null)
        {
            var lang = HttpContext.Current.Request.Cookies["lang"].Value.ToLower();
            var _db = new WGHotelZHEntities();
            if (lang == "us")
            {
                _db = new WGHotelZHEntities("WGHotelUSEntities");
            }
            var Items = new List<BedViewModel>();


            var Beds = _db.CodeFile.Where(o=>o.ItemType=="Bed");
            var SelectList = new List<SelectListItem>();
            foreach (var i in Beds)
            {
                SelectList.Add(item: new SelectListItem
                {
                    Text = i.ItemDescription,
                    Value = i.ID.ToString(),
                    Selected = Selected == null
                       ? false
                       : Selected.Contains(i.ID)
                });
            }



            return SelectList;
        }
    }

}