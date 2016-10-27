using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WGHotel.Models;

namespace WGHotel.Areas.Backend.Models
{
    public class CodeFiles
    {
        protected WGHotelZHEntities _db;
        public CodeFiles()
            : base()
        {
            if (_db == null)
            {
                _db = new WGHotelZHEntities();
            }
        }



        public List<CodeFile> GetHotelFacility(){
            var CodeType = "HF";
            var Items = _db.CodeFile.Where(o=>o.ItemType == CodeType).ToList();
            return Items;
        }

        

    }
}