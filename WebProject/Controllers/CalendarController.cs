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
            InsertEvent(EventName, EventDescription, EventStart, EventEnd);

            return RedirectToAction("Index");
        }

        public JsonResult GetEvents()
        {
            List<EventModel> events = QueryEvents();
           
            return Json(events, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DownloadEvents()
        {
            List<EventModel> events = QueryEvents();
            
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(events);
            byte[] jsonEncoded = Encoding.ASCII.GetBytes(json);
            return File(jsonEncoded,"text/plain","events.json");
        }

        public ActionResult UploadEvents(HttpPostedFileBase file)
        {
            if (file.ContentLength > 0)
            {
                byte[] fileContents = new byte[file.ContentLength];
                file.InputStream.Read(fileContents, 0, file.ContentLength);

                string json = Encoding.ASCII.GetString(fileContents);

                try
                {
                    List<EventModel> events = Newtonsoft.Json.JsonConvert.DeserializeObject<List<EventModel>>(json);
                    foreach (EventModel e in events)
                    {
                        InsertEvent(e.Name, e.Description, Convert.ToDateTime(e.DateFrom), Convert.ToDateTime(e.DateTo));
                    }
                }
                catch (Newtonsoft.Json.JsonReaderException ex)
                {
                    ModelState.AddModelError("Upload", "Please upload a valid JSON file.");
                }

            }
            else
            {
                ModelState.AddModelError("Upload", "File has no content.");
            }

            
            return RedirectToAction("Index", new { uniqueUri = Request.RequestContext.RouteData.Values["uniqueUri"] });
        }

        private void InsertEvent(string EventName, string EventDescription, DateTime EventStart, DateTime EventEnd)
        {
            string UserID = User.Identity.GetUserId();
            if (UserID != null)
            {
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
            }
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