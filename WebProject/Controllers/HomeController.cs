using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication5.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Meet the Team";

            return View();
        }
        
        // The link to the calendar page.
        public ActionResult Calendar() {
            // This is a message that can be called on the Calendar's page.
            ViewBag.Message = "Calendar";

            return View();
        }

        public ActionResult Stocks()
        {
            ViewBag.Message = "My Stocks";

            return View();
        }
    }
}