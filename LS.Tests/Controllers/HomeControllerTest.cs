﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyMvc;
using MyMvc.Controllers;

namespace MyMvc.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index()
        {
            // Упорядочение
            HomeController controller = new HomeController();

            // Действие
            ViewResult result = controller.Index() as ViewResult;

            // Утверждение
            Assert.AreEqual("Измените этот шаблон, чтобы быстро приступить к работе над приложением ASP.NET MVC.", result.ViewBag.Message);
        }
    }
}
