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
using System.Globalization;

namespace WebApplication5.Controllers {
    public class CalendarController : Controller {
        //
        // GET: /Calendar/
        public ActionResult Index() {
            ViewBag.Message = "Calendar";
            CalendarModel c = new CalendarModel();
            c.populateLists();
            return View(c);
        }

        /// <summary>
        /// Translates the various time forms from the view into two DateTime variables, the data then gets passed to the InsertEvent function.
        /// </summary>
        /// <param name="c"></param>
        /// <param name="EventName"></param>
        /// <param name="EventDescription"></param>
        /// <param name="EventStart"></param>
        /// <param name="EventEnd"></param>
        /// <returns></returns>
        public ActionResult TranslateEventTime(CalendarModel c, string EventName, string EventDescription, string EventStart, string EventEnd) {
            // Translating user input into event start datetime variable
            int startHour = c.startHourVal;
            string startHourText;
            if (startHour.ToString().Length == 1) {
                startHourText = "0" + startHour.ToString();
            } else {
                startHourText = startHour.ToString();
            }
            if (c.startTimeframeText == null) {
                c.startTimeframeText = "AM";
            }
            if (c.startMinuteText == null) {
                c.startMinuteText = "00";
            }
            if (c.startTimeframeText == "PM" && startHour != 12) {
                startHour += 12;
                startHourText = startHour.ToString();
            }
            if (startHour == 12 && c.startTimeframeText == "AM") {
                startHourText = "00";
            }
            var startTime = startHourText + ":" + c.startMinuteText + ":00";
            startTime = EventStart + " " + startTime;

            // Translating user input into event end datetime variable
            int endHour = c.endHourVal;
            string endHourText;
            if (endHour.ToString().Length == 1) {
                endHourText = "0" + endHour.ToString();
            } else {
                endHourText = endHour.ToString();
            }
            if (c.endTimeframeText == null) {
                c.endTimeframeText = "AM";
            }
            if (c.endMinuteText == null) {
                c.endMinuteText = "00";
            }
            if (c.endTimeframeText == "PM" && endHour != 12) {
                endHour += 12;
                endHourText = endHour.ToString();
            }
            if (endHour == 12 && c.endTimeframeText == "AM") {
                endHourText = "00";
            }
            var endTime = endHourText + ":" + c.endMinuteText + ":00";
            endTime = EventEnd + " " + endTime;
            
            // Validate the start time and endtime to make sure they are actually dates in a calendar.
            DateTime dtStart;
            DateTime dtEnd;
            if (DateTime.TryParseExact(startTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out dtStart) == true && 
                DateTime.TryParseExact(endTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out dtEnd) == true) {
                InsertEvent(EventName, EventDescription, dtStart, dtEnd);
            }
            return RedirectToAction("Index");
        }

        public JsonResult GetEvents()
        {
            List<EventModel> events = QueryEvents();
           
            return Json(events, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteEvent(CalendarModel c, string EventName, string EventDescription, string EventStart, string EventEnd) {
            int EventID = c.currentEventID;
            string UserID = User.Identity.GetUserId();
            if (UserID != null) {
                MySqlConnection db = new MySqlConnection();
                db.CreateConn();

                SqlCommand cmd = new SqlCommand("DeleteCalendarEvent", db.Connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@UserId", UserID));
                cmd.Parameters.Add(new SqlParameter("@EventID", EventID));
                cmd.Parameters.Add(new SqlParameter("@EventName", EventName));
                cmd.Parameters.Add(new SqlParameter("@EventDescription", EventDescription));
                cmd.Parameters.Add(new SqlParameter("@EventStart", EventEnd));
                cmd.Parameters.Add(new SqlParameter("@EventEnd", EventStart));

                db.Command = cmd;
                db.Command.Prepare();
                db.Command.ExecuteNonQuery();
            }
            
            return RedirectToAction("Index");
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

        private void InsertEvent(string EventName, string EventDescription, DateTime dtStart, DateTime dtEnd)
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
                cmd.Parameters.Add(new SqlParameter("@EventStart", dtStart));
                cmd.Parameters.Add(new SqlParameter("@EventEnd", dtEnd));

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