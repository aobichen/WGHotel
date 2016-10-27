using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WGHotel.Models;

namespace WGHotel.Areas.Backend.Models
{
    public class RoomViewModel
    {
        public RoomViewModel()
        {
            if (_db == null)
            {
                _db = new WGHotelZHEntities();
            }
            RoomTypes();
            Facility();
            BedTypes();
        }

        private WGHotelZHEntities _db { get; set; }
        private WGHotelUSEntities db { get; set; }
        public int HOTELID { get; set; }

        public int ID { get; set; }
        public string NameZh { get; set; }
        public string NoticeZh { get; set; }

        public string NameUs { get; set; }
        public string NoticeUs { get; set; }

        public string RoomType { get; set; }
        public string BedType { get; set; }
        public string Facilities { get; set; }
        public bool HasBreakfast { get; set; }
        public decimal Sell { get; set; }
        public int Quantiy { get; set; }
        public bool Enabled { get; set; }

        public SelectList RoomTypeSelectList { get; set; }
        public SelectList BedTypeSelectList { get; set; }

        public List<RoomFacilitiesCheckList> FacilityList { get; set; }


        private void Facility()
        {
            var List = _db.CodeFile.Where(o => o.ItemType == "RF").ToList();
            List<RoomFacilitiesCheckList> RF = new List<RoomFacilitiesCheckList>();
            var strFacilities = Facilities != null ? Facilities.Split(',').ToList():new List<string>();
            FacilityList = new List<RoomFacilitiesCheckList>();
            foreach (var item in List)
            {
                var ischecked = List.Where(o => strFacilities.Contains(o.ID.ToString())).Count() > 0 ? true : false;
                FacilityList.Add(new RoomFacilitiesCheckList { Checked = ischecked, ID = item.ID, Name = item.ItemDescription });
            }
        }

        private void RoomTypes()
        {
           var types = _db.CodeFile.Where(o => o.ItemType == "Room").ToList();
            RoomTypeSelectList = new SelectList(types, "ID", "ItemDescription", RoomType);
            
        }

        private void BedTypes()
        {
            var types = _db.CodeFile.Where(o => o.ItemType == "Bed").ToList();
            BedTypeSelectList = new SelectList(types, "ID", "ItemDescription", BedType);            
        }

        public void Create()
        {
            var ZHID = 0;
            var USID = 0;
            using (var db = new WGHotelZHEntities())
            {

                var Room = new Room();
                Room.Name = NameZh;
                Room.Notice = NoticeZh;
                Room.BedType = BedType;
                Room.RoomType = RoomType;
                Room.Sell = Sell;
                Room.Enabled = true;
                Room.HasBreakfast = HasBreakfast;
                Room.HOTELID = HOTELID;
                Room.Facilities = Facilities;
                Room.Quantiy = Quantiy;
                db.Room.Add(Room);
                db.SaveChanges();
                ZHID = Room.ID;
            }

            using (var db = new WGHotelUSEntities())
            {

                var Room = new Room();
                Room.Name = NameUs;
                Room.Notice = NoticeUs;
                Room.BedType = BedType;
                Room.RoomType = RoomType;
                Room.Sell = Sell;
                Room.Enabled = true;
                Room.HasBreakfast = HasBreakfast;
                Room.HOTELID = HOTELID;
                Room.Facilities = Facilities;
                Room.Quantiy = Quantiy;
                Room.ParentId = ZHID;
                db.Room.Add(Room);
                db.SaveChanges();
                USID = Room.ID;
            }
        }

        public void Edit()
        {
            db = new WGHotelUSEntities();
            var ZHModel = _db.Room.Find(ID);
            if(ZHModel==null){
                return;
            }
            var USModel = db.Room.Where(o => o.ParentId == ZHModel.ID).FirstOrDefault();
            if (USModel == null)
            {
                return;
            }

            ZHModel.Name = NameZh;
            ZHModel.Notice = NoticeZh;
           
            USModel.Name = NameZh;
            USModel.Notice = NoticeZh;

            //Room.Notice = NoticeZh;
            ZHModel.BedType = BedType;
            ZHModel.RoomType = RoomType;
            ZHModel.Sell = Sell;
            ZHModel.Enabled = true;
            ZHModel.HasBreakfast = HasBreakfast;
            ZHModel.HOTELID = HOTELID;
            ZHModel.Facilities = Facilities;
            ZHModel.Quantiy = Quantiy;
            _db.Room.Add(ZHModel);
            _db.SaveChanges();

            USModel.BedType = BedType;
            USModel.RoomType = RoomType;
            USModel.Sell = Sell;
            USModel.Enabled = true;
            USModel.HasBreakfast = HasBreakfast;
            USModel.HOTELID = HOTELID;
            USModel.Facilities = Facilities;
            USModel.Quantiy = Quantiy;
            db.Room.Add(USModel);
            db.SaveChanges();
        }
    }

    public class RoomFacilitiesCheckList
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public bool Checked { get; set; }
    }
}