using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using WebApplication5.App_Data;
using System.Data.SqlClient;

namespace WebApplication5.Controllers
{
    public class StockController : Controller
    {
        public ActionResult Index()
        {
            // This is a message that can be called on the Stock's page.
            ViewBag.Message = "Stock";
            return View();
        }

        public ActionResult AddNote(string stock, string note)
        {
            string userId = User.Identity.GetUserId();

            MySqlConnection conn = new MySqlConnection();
            conn.CreateConn();
            SqlCommand cmd = new SqlCommand("AddStockNote", conn.Connection);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@UserId", userId));
            cmd.Parameters.Add(new SqlParameter("@StockName", stock));
            cmd.Parameters.Add(new SqlParameter("@StockNote", note));

            conn.Command = cmd;
            conn.Command.Prepare();
            conn.Command.ExecuteNonQuery();

            return RedirectToAction("Index");
        }

        public ActionResult GetNote(string stock, string note)
        {
            string userId = User.Identity.GetUserId();

            MySqlConnection conn = new MySqlConnection();
            conn.CreateConn();
            SqlCommand cmd = new SqlCommand("GetStockNote", conn.Connection);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@UserId", userId));
            cmd.Parameters.Add(new SqlParameter("@StockName", stock));
            cmd.Parameters.Add(new SqlParameter("@StockNote", note));

            conn.Command = cmd;
            conn.Command.Prepare();
            conn.Command.ExecuteNonQuery();

            return RedirectToAction("Index");
        }

        public ActionResult IndividualStock()
        {
            // This is a message that can be called on the Stock's page.
            ViewBag.Message = "IndividualStock";

            return View();
        }
    }
}