using MyMvc.Models;
using MyMvc.Services.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace MyMvc.Controllers
{
    public class LocationController : Controller
    {
        public ActionResult GetUserInfo()
        {
            return new JsonResult()
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = UserFunctions.SelectUser(
                    WebSecurity.CurrentUserId,
                    x => new DeviceInfo { uid = x.User.Id, uname = x.User.Login, dname = x.Device.Name, id = x.Device.Id, lat = x.Location.Lat, lng = x.Location.Lng }).ToArray()
            };
        }

        public ActionResult GetContactsInfo()
        {
            return new JsonResult()
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = UserFunctions.SelectContacts(
                    WebSecurity.CurrentUserId,
                    x => new DeviceInfo { uid = x.User.Id, uname = x.User.Login, dname = x.Device.Name, id = x.Device.Id, lat = x.Location.Lat, lng = x.Location.Lng },
                    false).ToArray()
            };
        }

        public ActionResult GetMasterContactsInfo(string name)
        {
            return new JsonResult()
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = UserFunctions.SelectMasterContactsForUser(
                    WebSecurity.CurrentUserId,
                    name,
                    x => new DeviceInfo { uid = x.User.Id, uname = x.User.Login, dname = x.Device.Name, id = x.Device.Id, lat = x.Location.Lat, lng = x.Location.Lng }).ToArray()
            };
        }

        public ActionResult GetLocationsForDemo(int skip, int take)
        {
            var data = 
                UserFunctions.SelectLocationsForDemo(WebSecurity.CurrentUserId)
                    .OrderBy(x => x.DeviceId)
                    .Select(x => new { lat = x.Lat, lng = x.Lng })
                    .Skip(skip)
                    .Take(take)
                    .ToArray();

            return new JsonResult()
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = data
            };
        }

        public ActionResult GetAllLocations()
        {
            return new JsonResult()
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = UserFunctions.SelectAll(
                    WebSecurity.CurrentUserId,
                    x => new DeviceLocation { id = x.Device.Id, lat = x.Location.Lat, lng = x.Location.Lng },
                    true).ToArray()
            };
        }

        public ActionResult Randomize()
        {
            var db = Models.DB.ModelContext.Instance;
            Random r = new Random();

            foreach (var user in db.Users.ToList())
            {
                var loc = UserFunctions.SelectUser(user.Id, x => x.Location).FirstOrDefault();

                double dlat = (r.NextDouble() - 0.5) / 10;
                double dlng = (r.NextDouble() - 0.5) / 10;

                UserFunctions.UpdateLocation(user.Id, loc.Lat + dlat, loc.Lng + dlng);
            }

            return new JsonResult();
        }

        public ActionResult GetMyLocations()
        {
            var db = Models.DB.ModelContext.Instance;

            DateTime yesterday = DateTime.Now.AddDays(-1);

            var query = (
                from d in db.Devices
                join u in db.Users on d.UserId equals u.Id
                join l in db.Locations on d.Id equals l.DeviceId
                where u.Id == WebSecurity.CurrentUserId && l.Time > yesterday
                orderby l.Time
                select new DeviceLocation() { id = d.Id, lat = l.Lat, lng = l.Lng }).ToList();

            return new JsonResult()
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = query.ToArray()
            };
        }
    }
}
