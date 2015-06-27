using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyMvc.Models;
using MyMvc.Models.DB;
using System.Net;
using System.Text;
using System.IO;
using MyMvc.Services.DataContracts.V2;

namespace MyMvc.Controllers
{
    public class TestController : Controller
    {
        public ActionResult Test()
        {
            return View();
        }

        public ActionResult SendGcmMessage(string receiver, string message)
        {
            DbDevice device = UserFunctions.SelectAnyDevice(receiver);

            if (device.GcmToken == null)
                return Json("Gcm token not set");

            Gcm.SendData(device.GcmToken, new ServerMessage { t = ServerMessageType.BadCode, c = message });

            return Json(string.Empty);
        }
    }
}
