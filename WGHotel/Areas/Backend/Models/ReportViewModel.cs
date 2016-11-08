using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WGHotel.Areas.Backend.Models
{
    using System;
    using System.Collections.Generic;
    using WGHotel.Models;
    public partial class ReportViewModel
    {
        public int ID { get; set; }
        public string Country { get; set; }
        public int CountryID { get; set; }
        public int HotelID { get; set; }
        public string Room { get; set; }
       // public int RoomID { get; set; }
        public int Creator { get; set; }
        public System.DateTime Modified { get; set; }

        public int Modify { get; set; }
        public System.DateTime Created { get; set; }
        public System.DateTime CheckInDate { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<int> NumOfPeople { get; set; }
        public string Remark { get; set; }

        public int RoomID { get; set; }

        public void Create(){
            var db = new WGHotelBaseEntities();
            var Model = new Report();
            Model.Created = Created;
            Model.Creator = Creator;
            Model.CountryID = CountryID;
            //var country = db.Country.Find(CountryID).Name;
            Model.Country = Country;
            Model.CheckInDate = CheckInDate;
            Model.HotelID = HotelID;
            Model.NumOfPeople = NumOfPeople;
            Model.Price = Price;
            Model.RoomID = RoomID;
            Model.Modified = Modified;
            Model.Modify = Modify;
            Model.Room = Room;
            db.Report.Add(Model);
            db.SaveChanges();
        }

        public void Edit()
        {
            var db = new WGHotelBaseEntities();
            var Model = db.Report.Find(ID);
            Model.Modified = Modified;
            Model.Modify = Modify;
            //var Model = new Report();
            //Model.Created = Created;
            //Model.Creator = Creator;
            Model.CountryID = CountryID;
            var country = db.Country.Find(CountryID).Name;
            Model.Country = country;
            Model.CheckInDate = CheckInDate;
            Model.HotelID = HotelID;
            Model.NumOfPeople = NumOfPeople;
            Model.Price = Price;
            Model.RoomID = RoomID;
            Model.Room = Room;
            //db.Report.Add(Model);
            db.SaveChanges();
        }
    }
}