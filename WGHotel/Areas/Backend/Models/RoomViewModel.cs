﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
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

            if(!Directory.Exists(HttpContext.Current.Server.MapPath("/UserFolder")))
                {
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath("/UserFolder"));
                };
        }

        private WGHotelZHEntities _db { get; set; }
        private WGHotelUSEntities db { get; set; }
        public int HOTELID { get; set; }

        public int ID { get; set; }
        public string NameZh { get; set; }
        public string NoticeZh { get; set; }

        public string NameUs { get; set; }
        public string NoticeUs { get; set; }

        public string FeatureUs { get; set; }
        public string FeatureZh { get; set; }

        public string RoomType { get; set; }
        public string BedType { get; set; }
        public List<string> Beds { get; set; }
        public string Facilities { get; set; }
        public bool HasBreakfast { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.###}", ApplyFormatInEditMode = true)]
        public decimal Sell { get; set; }
        public int Quantiy { get; set; }
        public bool Enabled { get; set; }

        public string ImgKey { get; set; }
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
            var types = _db.CodeFile.Where(o => o.ItemType == "Bed" && o.Deleted == false).ToList();
            BedTypeSelectList = new SelectList(types, "ID", "ItemDescription", BedType);            
        }

        public void Create()
        {
            var ZHID = 0;
            var USID = 0;
            var ZHHotelID = 0;
            var BedTypes = string.Empty;
            if (Beds != null && Beds.Count > 0)
            {
                BedTypes = string.Join(",", Beds);
            }
            using (var db = new WGHotelZHEntities())
            {

                var Room = new Room();
                Room.Name = NameZh;
                Room.Notice = NoticeZh;
                Room.Feature = FeatureZh;
                Room.BedType = BedTypes;
                Room.RoomType = RoomType;
                Room.Sell = Sell;
                Room.Enabled = true;
                Room.HasBreakfast = HasBreakfast;
                Room.HOTELID = HOTELID;
                Room.Facilities = string.Empty;
                Room.Quantiy = Quantiy;
                db.Room.Add(Room);
                db.SaveChanges();
                ZHID = Room.ID;
                ZHHotelID = Room.HOTELID;
            }

            try
            {

                using (var db = new WGHotelUSEntities())
                {
                    var ENBed = new List<string>();
                    if (Beds != null && Beds.Count > 0)
                    {

                        var ENBeds = Beds.Select(int.Parse).ToList();
                        foreach (var b in ENBeds)
                        {
                            var data = db.CodeFile.Where(o => o.ItemType == "Bed" && o.ParentId == b).FirstOrDefault();
                            if (data != null)
                            {
                                ENBed.Add(data.ID.ToString());
                            }
                        }
                    }
                    var HotelID = db.Hotel.Where(o => o.ParentId == ZHHotelID).FirstOrDefault().ID;
                    var Room = new Room();
                    Room.Name = NameUs;
                    Room.Feature = FeatureUs;
                    Room.Notice = NoticeUs;
                    Room.BedType = (ENBed == null || ENBed.Count <= 0) ? string.Empty : string.Join(",", ENBed);
                    Room.RoomType = RoomType;
                    Room.Sell = Sell;
                    Room.Enabled = true;
                    Room.HasBreakfast = HasBreakfast;
                    Room.HOTELID = HotelID;
                    Room.Facilities = string.Empty;
                    Room.Quantiy = Quantiy;
                    Room.ParentId = ZHID;
                    db.Room.Add(Room);
                    db.SaveChanges();
                    USID = Room.ID;
                }

                SaveImageStore(ZHID, USID);
            }
            catch(Exception ex)
            {
                using (var db = new WGHotelZHEntities())
                {
                    var remove = db.Hotel.Where(o => o.ID == ZHID).FirstOrDefault();
                    if (remove != null)
                    {
                        db.Hotel.Remove(remove);
                        db.SaveChanges();
                    }

                }
            }

        }

        public void Edit()
        {
            db = new WGHotelUSEntities();
            var ZHModel = _db.Room.Find(ID);
            if (ZHModel == null)
            {
                return;
            }
            var USModel = db.Room.Where(o => o.ParentId == ZHModel.ID).FirstOrDefault();
            if (USModel == null)
            {
                return;
            }

            ZHModel.Name = NameZh;
            ZHModel.Notice = NoticeZh;

            USModel.Name = NameUs;
            USModel.Notice = NameUs;

            //Room.Notice = NoticeZh;
            //ZHModel.BedType = BedType;
            var BedTypes = string.Empty;
            if (Beds!=null && Beds.Count > 0){
                BedTypes = string.Join(",",Beds);
            }
            ZHModel.BedType = BedTypes;
            ZHModel.RoomType = RoomType;
            ZHModel.Sell = Sell;
            ZHModel.Enabled = true;
            ZHModel.HasBreakfast = HasBreakfast;
            ZHModel.HOTELID = HOTELID;
            ZHModel.Facilities = string.Empty;
            ZHModel.Quantiy = Quantiy;
            //_db.Room.Add(ZHModel);
            //_db.SaveChanges();

            try
            {
                var ENBed = new List<string>();
                if (Beds != null && Beds.Count > 0)
                {
                    
                    var ENBeds = Beds.Select(int.Parse).ToList();
                    foreach (var b in ENBeds)
                    {
                       var data = db.CodeFile.Where(o => o.ItemType == "Bed" && o.ParentId == b).FirstOrDefault();
                       if (data != null)
                       {
                           ENBed.Add(data.ID.ToString());
                       }
                    }
                }
               

                var ENHotelId = db.Room.Where(o => o.ParentId == ZHModel.ID).FirstOrDefault().ID;

                USModel.BedType = (ENBed == null || ENBed.Count <= 0) ? string.Empty : string.Join(",", ENBed);
                USModel.RoomType = RoomType;
                USModel.Sell = Sell;
                USModel.Enabled = true;
                USModel.HasBreakfast = HasBreakfast;
                //USModel.HOTELID = ENHotelId;
                USModel.Facilities = string.Empty;
                USModel.Quantiy = Quantiy;
                //db.Room.Add(USModel);
                db.SaveChanges();
                _db.SaveChanges();
                SaveImageStore(ZHModel.ID, USModel.ID);
            }
            catch(Exception ex)
            {

            }


        }

        public void SaveImageStore(int ZHID, int USID)
        {
            var Session = HttpContext.Current.Session;

            if (Session[ImgKey] != null)
            {
                var Basedb = new WGHotelBaseEntities();
                var Now = DateTime.Now;
                var images = (List<ImageViewModel>)Session[ImgKey];
                var dbimages = Basedb.ImageStore.Where(o => o.ReferIdZH == ZHID && o.ReferIdUS == USID).ToList();
                foreach (var img in images)
                {
                    if(!dbimages.Any(o=>o.Name==img.Name)){
                                           
                        var fileName = Guid.NewGuid().GetHashCode().ToString("x");
                        //var file = string.Format("/UserFolder/{0}{1}",fileName, img.Extension);
                        //var path = HttpContext.Current.Server.MapPath(file);
                        //File.WriteAllBytes(path, img.Image);
                        Basedb.ImageStore.Add(new ImageStore
                        {
                            // Path = file,
                            Created = Now,
                            Extension = img.Extension,
                            Deleted = false,
                            ReferIdUS = USID,
                            ReferIdZH = ZHID,
                            Type = "Room",
                            Name = fileName,
                            Image = img.Image
                        });
                     }
                }
                Basedb.SaveChanges();
            }
        }
    }

    public class RoomFacilitiesCheckList
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public bool Checked { get; set; }
    }

    public class RoomList{
        public int ID { get; set; }
        public string Name { get; set; }
        public string HotelName {get;set;}
        public int HOTELID { get; set; }
        public string RoomType { get; set; }
        public string BedType { get; set; }
        public int Quantiy { get; set; }
        public decimal Sell { get; set; }
    }
}