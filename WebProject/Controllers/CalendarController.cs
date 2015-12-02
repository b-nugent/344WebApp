﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using WebApplication5.App_Data;
using System.Data.SqlClient;

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

            SqlCommand cmd = new SqlCommand("AddEvent", db.Connection);
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
	}
}