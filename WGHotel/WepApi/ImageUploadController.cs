using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WGHotel.Models;

namespace WGHotel.WepApi
{
    public class ImageUploadController : ApiController
    {
        // GET: ImageUpload
        [HttpPost]
        [Route("ImageUpload")]
        public object HotelImageUpload()
        {
            HttpRequestMessage request = this.Request;
            if (!request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            } 
            var Images = new List<ImageViewModel>();
            
            var Current = HttpContext.Current;
            var key = Current.Request["key"];         

            if (Current.Session[key] != null)
            {

                Images = (List<ImageViewModel>)Current.Session[key];
                //Images = Images.Select(o => { o.Hotel = null; return o; }).ToList();
            }


            var ImgDao = new ImageDAO();

            for (int i = 0; i < HttpContext.Current.Request.Files.Count; i++)
            {
                var file = HttpContext.Current.Request.Files[i];                
                var subName = Path.GetExtension(file.FileName);
                //var fileName = Guid.NewGuid().GetHashCode().ToString("x");    
                var fileName = file.FileName;
                var image = ImgDao.FileToByte(file);
                Images.Add(new ImageViewModel { Image = image, Name = fileName, Extension = subName });
            }

            var Message = "完成上傳";
            
            Current.Session[key] = Images;
            var data = JsonConvert.SerializeObject(Images);
            return Json(new { data = data, message = Message });
        }


        public class ImageModel
        {
            public string image { get; set; }
            public string name { get; set; }
        }

        [HttpPost]
        [Route("ImageUpload1")]
        public object HotelImageUpload1(ImageModel data)
        {
            var a = "";
            //byte[] bytes = Convert.FromBase64String(file);
            //HttpRequestMessage request = this.Request;
            //if (!request.Content.IsMimeMultipartContent())
            //{
            //    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            //}
            //var Images = new List<ImageViewModel>();

            //var Current = HttpContext.Current;
            //var key = Current.Request["key"];

            //if (Current.Session[key] != null)
            //{

            //    Images = (List<ImageViewModel>)Current.Session[key];
            //    //Images = Images.Select(o => { o.Hotel = null; return o; }).ToList();
            //}


            //var ImgDao = new ImageDAO();

            //for (int i = 0; i < HttpContext.Current.Request.Files.Count; i++)
            //{
            //    var file = HttpContext.Current.Request.Files[i];
            //    var subName = Path.GetExtension(file.FileName);
            //    //var fileName = Guid.NewGuid().GetHashCode().ToString("x");    
            //    var fileName = file.FileName;
            //    var image = ImgDao.FileToByte(file);
            //    Images.Add(new ImageViewModel { Image = image, Name = fileName, Extension = subName });
            //}

            //var Message = "完成上傳";

            //Current.Session[key] = Images;
            //var data = JsonConvert.SerializeObject(Images);
            return Json(new {  message = "OK" });
        }

        [HttpPost]
        [Route("ImageDelete")]
        public object HotelImageDelete(ImageViewModel data)
        {
            
            if (HttpContext.Current.Session[data.SessionKey] != null)
            {
                var images = (List<ImageViewModel>)HttpContext.Current.Session[data.SessionKey];
                var img = new ImageViewModel();
                if (images.Count > 0)
                {
                    img = images.Where(o => o.Name == data.Name).FirstOrDefault();
                    if (img != null)
                    {
                        images.Remove(images.Where(o => o.Name == img.Name).FirstOrDefault());
                        new ImageDAO().Delete(img.Name);

                        using (var db = new WGHotelBaseEntities())
                        {
                            var dbimg = db.ImageStore.Where(o => o.Name == img.Name).FirstOrDefault();
                            if (dbimg != null)
                            {
                                db.ImageStore.Remove(dbimg);
                                db.SaveChanges();
                            }
                        }
                    }                    
                }

                

                HttpContext.Current.Session[data.SessionKey] = images;
              
            }

            return Json(new { success = true });
        }
    }
}
