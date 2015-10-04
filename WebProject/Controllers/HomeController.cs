using Facebook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication5.CustomAttributes;
using WebApplication5.Models;

namespace WebApplication5.Controllers
{
    [LogInAttribute]
    [RequireHttps]
    public class HomeController : Controller {
        public ActionResult Index() {
            UserPostsModels model = new UserPostsModels();
            var client = new FacebookClient(Session["access_token"].ToString());
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
            model.posts = posts;

            return View(model);
        }

        public ActionResult About() {
            ViewBag.Message = "Your application description page.";

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

        public ActionResult Stock() {
            // This is a message that can be called on the Stock's page.
            ViewBag.Message = "Stock";
            return View();
        }
    }

}