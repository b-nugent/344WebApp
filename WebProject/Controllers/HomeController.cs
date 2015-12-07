using Facebook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication5.CustomAttributes;
using WebApplication5.Models;
using Microsoft.AspNet.Identity;
using WebApplication5.App_Data;
using System.Data.SqlClient;

namespace WebApplication5.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller {
        public ActionResult Index() {
            //ViewBag.ReturnUrl = returnUrl;
            HomeModel model = new HomeModel();
            UserPostsModels postsModel = new UserPostsModels();
            if (Session["access_token"] != null)
            {
                var client = new FacebookClient(Session["access_token"].ToString());
                try
                {
                    dynamic fbresult = client.Get("/" + Session["uid"].ToString() + "/feed?fields=from,created_time,type,message");
                    IList<UserPost> posts = new List<UserPost>();
                    foreach (var res in fbresult)
                    {
                        if (res.Value != null)
                        {
                            foreach (var value in res.Value)
                            {
                                if ((value.Key != "previous" && value.Key != "next"))
                                {
                                    if (value["type"] == "status" && value["from"]["id"] == Session["uid"].ToString())
                                    {
                                        UserPost aPost = new UserPost();
                                        aPost.type = value["type"];
                                        aPost.createdTime = value["created_time"];
                                        aPost.message = value["message"];
                                        posts.Add(aPost);
                                    }
                                }
                            }
                        }
                    }
                    postsModel.posts = posts;
                }
                catch (FacebookOAuthException e) { }
            }
            List<EventModel> events = GetTodaysEvents();
            model.Posts = postsModel;
            model.Events = events;

            return View(model);
        }

        public ActionResult About() {
            ViewBag.Message = "About Our Application";

            return View();
        }

        public ActionResult Contact() {
            ViewBag.Message = "Meet the Team";

            return View();
        }

        // The link to the calendar page.
        public ActionResult Calendar() {
            // This is a message that can be called on the Calendar's page.
            ViewBag.Message = "Calendar";

            return View();
        }

        public ActionResult UpdateStatus(string status)
        {
            if (status == "")
            {
                return RedirectToAction("Index");
            }

            var client = new FacebookClient(Session["access_token"].ToString());
            var args = new Dictionary<string, object>();
            args["message"] = status;
            try
            {
                dynamic fbresult = client.Post("/" + Session["uid"].ToString() + "/feed", args);
            }
            catch (FacebookOAuthException e)
            {
                return View("Error");
            }

            return RedirectToAction("Index");
        }

        private List<EventModel> GetTodaysEvents()
        {
            List<EventModel> eventsOnToday = new List<EventModel>();
            List<EventModel> usersEvents = QueryEvents();

            foreach (EventModel anEvent in usersEvents)
            {
                if (Convert.ToDateTime(anEvent.DateFrom).Day.Equals(DateTime.Now.Day))
                {
                    eventsOnToday.Add(anEvent);
                }
            }

            return eventsOnToday;
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