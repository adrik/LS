using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyMvc.Models;

namespace MyMvc.Controllers
{
    public class TestController : Controller
    {
        

        public ActionResult Test()
        {
            return new RedirectResult("/");

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

            //return View();
        }
    }
}
