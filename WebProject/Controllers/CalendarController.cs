using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication5.App_Data;

namespace WebApplication5.Controllers {
    public class CalendarController : Controller {
        //
        // GET: /Calendar/
        public ActionResult Index() {
            ViewBag.Message = "Calendar";
            return View();
        }

        public ActionResult StoreEvent(string EventName, string EventDescription, DateTime EventStart, DateTime EventEnd) {
            MySqlConnection db = new MySqlConnection();
            db.CreateConn();
            
            return RedirectToAction("Index");
        }
	}
}