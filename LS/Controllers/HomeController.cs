using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyMvc.Models.DB;

namespace MyMvc.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
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
            return View();
        }

        public ActionResult CreateUser(string name)
        {
            var db = ModelContext.Instance;
            var user = new DbUser() { Login = name + "@gmail.com", Name = name };
            db.Users.Add(user);
            db.SaveChanges();
            var device = new DbDevice() { DeviceKey = name + "_devKey123", Name = name + " phone", Status = 1, UserId = user.Id };
            db.Devices.Add(device);
            db.SaveChanges();
            var location = new DbLocation() { DeviceId = device.Id, Lat = 37.707471, Lng = -122.481077, Time = DateTime.Now };
            db.Locations.Add(location);
            var relation = new DbRelation() { UserId = 1, ContactId = user.Id, GroupId = 0 };
            db.Relations.Add(relation);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Страница описания приложения.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Страница контактов.";

            return View();
        }
    }
}
