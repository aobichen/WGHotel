using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WGHotel.Models;

namespace WGHotel.Areas.Backend.Models
{
    public class GameSiteListModel
    {

        public int ID { get; set; }
        public string SportZH { get; set; }
        public string TypeZH { get; set; }
        public string VenueZH { get; set; }
        public string RemarkZH { get; set; }

        public string SportUS { get; set; }
        public string TypeUS { get; set; }
        public string VenueUS { get; set; }
        public string RemarkUS { get; set; }

        public void Create()
        {
            var dbZH = new WGHotelZHEntities();
            var dbUS = new WGHotelUSEntities();

            var GameZH = new GameSite();
            GameZH.Remark = RemarkZH;
            GameZH.Sports = SportZH;
            GameZH.Type = TypeZH;
            GameZH.Venue = VenueZH;
            dbZH.GameSite.Add(GameZH);
            dbZH.SaveChanges();
            var GameUS = new GameSite();
            GameUS.ParentId = GameZH.ID;
            GameUS.Remark = RemarkUS;
            GameUS.Sports = SportUS;
            GameUS.Type = TypeUS;
            GameUS.Venue = VenueUS;
            dbUS.GameSite.Add(GameUS);
            dbUS.SaveChanges();
        }

        public void Edit()
        {
            var dbZH = new WGHotelZHEntities();
            var dbUS = new WGHotelUSEntities();
            var zh = dbZH.GameSite.Find(ID);
            var us = dbUS.GameSite.Where(o => o.ParentId == zh.ID).FirstOrDefault();
            zh.Remark = RemarkZH;
            zh.Sports = SportZH;
            zh.Type = TypeZH;
            zh.Venue = VenueZH;
            dbZH.SaveChanges();
            us.Venue = VenueUS;
            us.Remark = RemarkUS;
            us.Sports = SportUS;
            us.Type = TypeUS;
            dbUS.SaveChanges();
        }

        public List<GameSiteListModel> List()
        {
            var GameZH = new List<GameSite>();
            using (var db = new WGHotelZHEntities())
            {
                GameZH = db.GameSite.ToList();
            }

            var GameUS = new List<GameSite>();
            using (var db = new WGHotelUSEntities())
            {
                GameUS = db.GameSite.ToList();
            }

            var Games = new List<GameSiteListModel>();

            for (var i =0 ;i<GameZH.Count;i++)
            {
                Games.Add(new GameSiteListModel 
                {
                    RemarkZH = GameZH[i].Remark,
                    RemarkUS= GameUS[i].Remark,
                    SportUS = GameUS[i].Sports,
                    SportZH = GameZH[i].Sports,
                    TypeUS = GameUS[i].Type,
                    TypeZH = GameZH[i].Type,
                    VenueUS = GameUS[i].Venue,
                    VenueZH = GameZH[i].Venue,
                    ID = GameZH[i].ID
                });
            }

            return Games;
        }

    }
    public class GameSiteViewModel
    {
        public int ID { get; set; }
        public bool Checked { get; set; }
        public string Value { get; set; }

       
    }

    public class GameSiteModel
    {
        private WGHotelZHEntities db = new WGHotelZHEntities();
        public GameSiteModel()
        {
            if (db == null)
            {
                db = new WGHotelZHEntities();
            }

        }
        public List<GameSiteViewModel> List(List<string> checkeds=null)
        {
            var Items = new List<GameSiteViewModel>();

           
                var Games = db.GameSite.ToList();
                
                foreach (var item in Games)
                {
                    var id = item.ID.ToString();
                    bool IsChecked = (checkeds == null || checkeds.Count <= 0) ? false : (checkeds.Contains(id) ? true : false);
                    Items.Add(new GameSiteViewModel { Value = item.Venue, ID = item.ID, Checked = IsChecked });
                }
            

            return Items;
        }

        public List<SelectListItem> SelectList(List<int> Selected = null)
        {
            var Items = new List<GameSiteViewModel>();


            var Games = db.GameSite.ToList();
            var SelectList = new List<SelectListItem>();
            foreach (var i in Games)
            {
                SelectList.Add(item: new SelectListItem
                {
                    Text = string.Format("{0}/{1}", i.Venue,i.Sports),
                    Value = i.ID.ToString(),
                    Selected = Selected == null
                       ? false
                       : Selected.Contains(i.ID)
                });
            }

            

            return SelectList;
        }

        public SelectList Citys(int id=0)
        {
            var city = new List<City>();
            using (var db = new WGHotelZHEntities())
            {
               city = db.City.ToList();
            }

            var List = new SelectList(city,"ID", "Name",id);
            return List;
        }
        
    }
}