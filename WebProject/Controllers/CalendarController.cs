using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using WebApplication5.App_Data;
using System.Data.SqlClient;
using WebApplication5.Models;
using System.Text;

namespace WebApplication5.Controllers {
    public class CalendarController : Controller {
        //
        // GET: /Calendar/
        public ActionResult Index() {
            ViewBag.Message = "Calendar";
            return View();
        }
        
        public ActionResult StoreEvent(string EventName, string EventDescription, DateTime EventStart, DateTime EventEnd) {
            string UserID = User.Identity.GetUserId();
            MySqlConnection db = new MySqlConnection();
            db.CreateConn();

            SqlCommand cmd = new SqlCommand("AddCalendarEvent", db.Connection);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@UserId", UserID));
            cmd.Parameters.Add(new SqlParameter("@EventName", EventName));
            cmd.Parameters.Add(new SqlParameter("@EventDescription", EventDescription));
            cmd.Parameters.Add(new SqlParameter("@EventStart", EventStart));
            cmd.Parameters.Add(new SqlParameter("@EventEnd", EventEnd));

            db.Command = cmd;
            db.Command.Prepare();
            db.Command.ExecuteNonQuery();

            return RedirectToAction("Index");
        }

        public JsonResult GetEvents()
        {
            List<EventModel> events = QueryEvents();
           
            //events.Add(new EventModel {UserId="A",Name="test",Description="test",DateFrom="12-02-2015",DateTo="12-02-2015"});
            return Json(events, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DownloadEvents()
        {
            List<EventModel> events = QueryEvents();
            /*
            var cd = new System.Net.Mime.ContentDisposition
            {
                // for example foo.bak
                FileName = "events.json",

                // always prompt the user for downloading, set to true if you want 
                // the browser to try to show the file inline
                Inline = false,
            };
            
            Response.AppendHeader("Content-Disposition", cd.ToString());
             */
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(events);
            byte[] jsonEncoded = Encoding.ASCII.GetBytes(json);
            return File(jsonEncoded,"text/plain","events.json");
        }

        private List<EventModel> QueryEvents()
        {
            string userId = User.Identity.GetUserId();

            List<EventModel> events = new List<EventModel>();
            if (userId != null)
            {
                MySqlConnection conn = new MySqlConnection();
                conn.CreateConn();
                SqlCommand cmd = new SqlCommand("GetCalendarEvents", conn.Connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@UserId", userId));

                conn.DataReader = cmd.ExecuteReader();
                while (conn.DataReader.Read())
                {
                    string name = conn.DataReader["EventName"].ToString();
                    string start = conn.DataReader["EventStartTime"].ToString();
                    string end = conn.DataReader["EventEndTime"].ToString();
                    string desc = conn.DataReader["EventDescription"].ToString();
                    EventModel anEvent = new EventModel { Name = name, DateFrom = start, DateTo = end, Description = desc };
                    events.Add(anEvent);
                }
            }

            return events;
        }
	}
}