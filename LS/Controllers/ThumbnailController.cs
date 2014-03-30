using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyMvc.Models.DB;
using WebMatrix.WebData;

namespace MyMvc.Controllers
{
    public class ThumbnailController : Controller
    {
        public ActionResult Load(int id)
        {
            var db = ModelContext.Instance;
            int userId = WebSecurity.CurrentUserId;
            var icon = (from t in db.Thumbnails
                        where t.UserId == id && (id == userId || db.Relations.Any(x => x.UserId == userId && x.ContactId == id))
                        select t).FirstOrDefault();
            if (icon == null)
            {
                icon = (from t in db.Thumbnails
                        where t.UserId == 0
                        select t).FirstOrDefault();
            }
            return File(icon.Content, icon.ContentType);
        }
    }
}
