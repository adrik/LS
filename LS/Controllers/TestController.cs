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

namespace MyMvc.Controllers
{
    public class TestController : Controller
    {
        

        public ActionResult Test()
        {
            //return new RedirectResult("/");

            //long half = 64 * 64 * 64;
            //long max = half * half;
            ////int max = 64 * 64 * 64 * 64 * 64;

            //Random r = new Random();
            //long code = (long)Math.Floor(r.NextDouble() * max);
            ////int code = r.Next(max - 1);
            //int k = 6;

            //ViewBag.Rnd = B64.Create(code, k);
            //ViewBag.Min = B64.Create(0, k);
            //ViewBag.Max = max;

            //ViewBag.Num = Enumerable.Range(0, 120).Select(x => CodeGen.Next());

            return View();
        }

        public ActionResult SendGcmMessage(string receiver, string message)
        {
            DbDevice device = UserFunctions.SelectAnyDevice(receiver);

            string apiKey = "AIzaSyBJW14WfDwIYpSa07b0M12c7LzycoqzYkM";
            string json = "{\"to\": \"" + device.GcmToken + "\", \"data\": {\"message\": \"" + message + "\"}}";

            var SENDER_ID = "GLT Server";
            var value = message;
            WebRequest tRequest;
            tRequest = WebRequest.Create("https://android.googleapis.com/gcm/send");
            tRequest.Method = "post";
            tRequest.ContentType = "application/json;charset=UTF-8";
            tRequest.Headers.Add(string.Format("Authorization: key={0}", apiKey));

            tRequest.Headers.Add(string.Format("Sender: id={0}", SENDER_ID));

            Byte[] byteArray = Encoding.UTF8.GetBytes(json);
            tRequest.ContentLength = byteArray.Length;

            Stream dataStream = tRequest.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            WebResponse tResponse = tRequest.GetResponse();

            dataStream = tResponse.GetResponseStream();

            StreamReader tReader = new StreamReader(dataStream);

            String sResponseFromServer = tReader.ReadToEnd();


            tReader.Close();
            dataStream.Close();
            tResponse.Close();

            return Json(sResponseFromServer);
        }
    }
}
