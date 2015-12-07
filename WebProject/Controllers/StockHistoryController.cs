using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using WebApplication5.App_Data;
using System.Data.SqlClient;
using WebApplication5.Models;
using System.Net;
using System.Text;

namespace WebApplication5.Controllers
{
    public class StockHistoryController : Controller
    {
        StockHistory userHistory = new StockHistory();
        public ActionResult Index()
        {
            // This is a message that can be called on the Stock's page.
            string userId = User.Identity.GetUserId();
            if (userId != null)
            {
                GetStockHistory(userId);
            }
            ViewBag.Message = "Stock History";
            return View(userHistory);
        }

        public StockHistory GetStockHistory(string userId)
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
            userHistory.userHistory = dict;
            return userHistory;
        }

        public void UploadTransaction(string stock, int numShares, decimal price, int buySell)
        {
            string userId = User.Identity.GetUserId();
            var transactionType = 0;

            if (buySell == 1)
            {
                transactionType = 1;
            }

            if (userId != null)
            {
                MySqlConnection conn = new MySqlConnection();
                conn.CreateConn();
                SqlCommand cmd = new SqlCommand("CreateStockTransaction", conn.Connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@UserId", userId));
                cmd.Parameters.Add(new SqlParameter("@StockName", stock));
                cmd.Parameters.Add(new SqlParameter("@Quantity", numShares));
                cmd.Parameters.Add(new SqlParameter("@TransactionPrice", price));
                cmd.Parameters.Add(new SqlParameter("@HasSold", transactionType));

                conn.Command = cmd;
                conn.Command.Prepare();
                conn.Command.ExecuteNonQuery();

            }
            //return RedirectToAction("index");
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

        public ActionResult DownloadStocks()
        {
            string userId = User.Identity.GetUserId();
            Dictionary<string, List<Stock>> stockHistory = GetStockHistoryForDownload(userId);

            string json = Newtonsoft.Json.JsonConvert.SerializeObject(stockHistory);
            byte[] jsonEncoded = Encoding.ASCII.GetBytes(json);
            return File(jsonEncoded, "text/plain", "stocks.json");
        }

        public ActionResult UploadStocks(HttpPostedFileBase file)
        {
            if (file.ContentLength > 0)
            {
                byte[] fileContents = new byte[file.ContentLength];
                file.InputStream.Read(fileContents, 0, file.ContentLength);

                string json = Encoding.ASCII.GetString(fileContents);

                try
                {
                    Dictionary<string, List<Stock>> stocks = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, List<Stock>>>(json);
                    foreach (string key in stocks.Keys)
                    {
                        foreach (Stock s in stocks[key])
                        {
                            if (s.SoldPrice == 0)
                            {
                                UploadTransaction(s.Symbol, s.NumShares, s.BoughtPrice, 0);
                            }

                            else if (s.BoughtPrice == 0)
                            {
                                UploadTransaction(s.Symbol, s.NumShares, s.SoldPrice, 1);

                            }

                        }
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


            return RedirectToAction("Index");
        }

        public ActionResult DeleteStockHistory()
        {
            string userId = User.Identity.GetUserId();

            if (userId != null)
            {
                MySqlConnection conn = new MySqlConnection();
                conn.CreateConn();
                SqlCommand cmd = new SqlCommand("DeleteStockHistory", conn.Connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@UserId", userId));


                conn.Command = cmd;
                conn.Command.Prepare();
                conn.Command.ExecuteNonQuery();

            }
            return RedirectToAction("index");
        }
    }
}