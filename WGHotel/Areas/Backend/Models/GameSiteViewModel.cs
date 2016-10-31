using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WGHotel.Models;

namespace WGHotel.Areas.Backend.Models
{
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

        public SelectList SelectList(List<string> checkeds = null)
        {
            var Items = new List<GameSiteViewModel>();


            var Games = db.GameSite.ToList();

            foreach (var item in Games)
            {
                var id = item.ID.ToString();
                bool IsChecked = (checkeds == null || checkeds.Count <= 0) ? false : (checkeds.Contains(id) ? true : false);
                Items.Add(new GameSiteViewModel { Value = item.Venue+"/"+item.Sports, ID = item.ID, Checked = IsChecked });
            }

            var List = new SelectList(Items, "ID", "Value");

            return List;
        }

        public SelectList Citys()
        {
            var city = new List<City>();
            using (var db = new WGHotelZHEntities())
            {
               city = db.City.ToList();
            }

            var List = new SelectList(city,"ID", "Name");
            return List;
        }
        
    }
}