using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using MyMvc.Filters;
using MyMvc.Models.DB;

namespace MyMvc.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class AccountController : Controller
    {
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string returnUrl)
        {
            return new ExternalLoginResult("Google", Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
        }

        [AllowAnonymous]
        public ActionResult ExternalLoginCallback(string returnUrl)
        {
            AuthenticationResult result = OAuthWebSecurity.VerifyAuthentication(Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
            if (result.IsSuccessful)
            {
                DbUser u = ModelContext.Instance.Users.FirstOrDefault(x => x.Login == result.UserName);
                if (WebSecurity.Login(result.UserName, null))
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            return View();
        }
       

        internal class ExternalLoginResult : ActionResult
        {
            public ExternalLoginResult(string provider, string returnUrl)
            {
                Provider = provider;
                ReturnUrl = returnUrl;
            }

            public string Provider { get; private set; }
            public string ReturnUrl { get; private set; }

            public override void ExecuteResult(ControllerContext context)
            {
                OAuthWebSecurity.RequestAuthentication(Provider, ReturnUrl);
            }
        }
    }
}
