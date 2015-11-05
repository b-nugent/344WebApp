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
    public class HomeController : Controller {
        public ActionResult Index() {
            UserPostsModels model = new UserPostsModels();
            if (Session["access_token"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

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
                model.posts = posts;
            }
            catch (FacebookOAuthException e) { }

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

        public ActionResult IndividualStock()
        {
            // This is a message that can be called on the Stock's page.
            ViewBag.Message = "IndividualStock";

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

    }

}