using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyMvc.Models.DB;
using WebMatrix.WebData;

namespace MyMvc.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
                if (WebSecurity.CurrentUserId == 19)
                    return View("Master");
                else
                    return View();
            else
                return View("Login");
        }

        public ActionResult DeviceTemplate()
        {
            return View();
        }

        public ActionResult Randomize()
        {
            if (User.Identity.IsAuthenticated && WebSecurity.CurrentUserId == 19)
                return View();
            else
                return new RedirectResult("/");
        }

        public ActionResult CreateUser(string name)
        {
            return new RedirectResult("/");

            //var db = ModelContext.Instance;
            //var user = new DbUser() { Login = name + "@gmail.com", Name = name, PasswordHash = string.Empty, Code = string.Empty };
            //db.Users.Add(user);
            //db.SaveChanges();
            //var device = new DbDevice() { DeviceKey = name + "_devKey123", Name = name + " phone", Status = 1, UserId = user.Id };
            //db.Devices.Add(device);
            //db.SaveChanges();
            //var location = new DbLocation() { DeviceId = device.Id, Lat = 37.707471, Lng = -122.481077, Time = DateTime.Now };
            //db.Locations.Add(location);
            //var relation = new DbRelation() { UserId = 19, ContactId = user.Id, GroupId = 0 };
            //db.Relations.Add(relation);
            //var relation2 = new DbRelation() { UserId = user.Id, ContactId = 19, GroupId = 0 };
            //db.Relations.Add(relation2);
            //db.SaveChanges();

            //return RedirectToAction("Index");
        }
    }
}
