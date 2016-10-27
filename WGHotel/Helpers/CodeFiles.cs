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
            var i = int.Parse(id);
            var Code = _db.CodeFile.Find(i);
            return Code.ItemDescription;
        }
    }
}