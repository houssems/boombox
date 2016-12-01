using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Services.Protocols;
using Newtonsoft.Json.Linq;
using Twilio;

namespace MusikApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(string type = "", string query = "")
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        

        
    }
}
