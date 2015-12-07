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

            List<Stock> Top5 = Top5Stocks();
            model.Top5 = Top5;

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


        public Dictionary<string, List<Stock>> GetStockHistoryForDownload(string userId)
        {
            Dictionary<string, List<Stock>> dict = new Dictionary<string, List<Stock>>();


            MySqlConnection conn = new MySqlConnection();
            conn.CreateConn();
            SqlCommand cmd = new SqlCommand("GetStockHistory", conn.Connection);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@UserId", userId));

            conn.DataReader = cmd.ExecuteReader();
            while (conn.DataReader.Read())
            {
                Stock userStock = new Stock();
                string sname = conn.DataReader["StockName"].ToString();
                int quantity = Convert.ToInt16(conn.DataReader["Quantity"]);
                decimal price = Convert.ToDecimal(conn.DataReader["TransactionPrice"]);
                int hasSold = Convert.ToInt16(conn.DataReader["HasSold"]);

                if (hasSold == 1)
                {
                    userStock.SoldPrice = price;
                    userStock.BoughtPrice = 0;
                }

                else
                {
                    userStock.BoughtPrice = price;
                    userStock.SoldPrice = 0;
                }
                userStock.Symbol = sname;
                userStock.NumShares = quantity;
                if (dict.ContainsKey(sname))
                {
                    dict[sname].Add(userStock);
                }
                else
                {
                    List<Stock> userTransactions = new List<Stock>();
                    userTransactions.Add(userStock);
                    dict.Add(sname, userTransactions);
                }

            }

            return dict;
        }
        public List<Stock> Top5Stocks()
        {
            List<Stock> Top5 = new List<Stock>();
            List<Stock> TransactionList = new List<Stock>();
            string userId = User.Identity.GetUserId();
            Dictionary<string, List<Stock>> history = GetStockHistoryForDownload(userId);
            foreach (string key in history.Keys){
                foreach (Stock stockValues in history[key]){
                    TransactionList.Add(stockValues);
                }
                Top5.Add(calculateEarned(TransactionList));
            }
            return Top5;
        }

        public Stock calculateEarned(List<Stock> transactions)
        {
            Stock finalStock = new Stock();
            finalStock.Symbol = transactions[0].Symbol;
            foreach (Stock s in transactions)
            {
                if (s.SoldPrice == 0)
                {
                    finalStock.SoldPrice -= (s.BoughtPrice * s.NumShares);
                    finalStock.NumShares += s.NumShares;
                }
                if (s.BoughtPrice == 0)
                {
                    finalStock.SoldPrice += (s.SoldPrice * s.NumShares);
                    finalStock.NumShares -= s.NumShares;
                }
            }
            return finalStock;
        }
	}
}