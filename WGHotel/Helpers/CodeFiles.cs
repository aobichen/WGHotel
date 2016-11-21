using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WGHotel.Models;

namespace WGHotel.Helpers
{
    public class CodeFiles
    {
        static WGHotelZHEntities _db = new WGHotelZHEntities();
        public static string GetCodeFileDescription(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return string.Empty;
            }
            var i = int.Parse(id);
            var Code = _db.CodeFile.Find(i);
            if (Code == null)
            {
                return string.Empty;
            }
            return Code.ItemDescription;
        }

        public static string GetCodeFileForBed(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return string.Empty;
            }
            var IDs = id.Split(',').Select(int.Parse).ToList();
            
            var Bed = string.Empty;
            var Code = _db.CodeFile.Where(o => IDs.Contains(o.ID) && o.ItemType == "Bed" && o.Deleted == false).ToList();
            if (Code == null || Code.Count <=0)
            {
                return string.Empty;
            }

            var Beds = Code.Select(o => o.ItemDescription).ToList();
            Bed = string.Join(",",Beds);

            if (HttpContext.Current.Request.Cookies["lang"] != null && HttpContext.Current.Request.Cookies["lang"].ToString().ToLower()!="zh")
            {
                using(var db = new WGHotelUSEntities()){
                    var ENBeds = Code.Select(o => o.ID).ToList();
                    var Code1 = db.CodeFile.Where(o => ENBeds.Contains(o.ParentId.Value) && o.ItemType == "Bed" && o.Deleted == false).ToList();
                    if (Code1 == null|| Code1.Count()<=0)
                    {
                        return string.Empty;
                    }
                    Beds = Code.Select(o => o.ItemDescription).ToList();
                    Bed = string.Join(",", Bed);
                }
            }
            return Bed;
        }
    }
}