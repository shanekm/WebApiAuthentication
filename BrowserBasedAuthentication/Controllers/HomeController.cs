using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BrowserBasedAuthentication.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var identity = User.Identity;
            return View();
        }
    }
}