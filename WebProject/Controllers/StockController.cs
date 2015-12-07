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
            string noteString = "";
            Stock StockNote = new Stock();

            if (userId != null)
            {
                MySqlConnection conn = new MySqlConnection();
                conn.CreateConn();
                SqlCommand cmd = new SqlCommand("GetStockNote", conn.Connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@UserId", userId));
                cmd.Parameters.Add(new SqlParameter("@StockName", stock));
                cmd.Parameters.Add(new SqlParameter("@StockNote", note));

                conn.DataReader = cmd.ExecuteReader();
                while (conn.DataReader.Read())
                {
                    noteString = conn.DataReader["StockNote"].ToString();
                }
                
                StockNote.Note = noteString;
                //StockNote.Note = "Test";
            }
            //StockNote.Note = "Test";
            return View(StockNote);
        }

        public ActionResult BuyStock(string stock, string buy)
        {
            string userId = User.Identity.GetUserId();
            Stock StockNote = new Stock();
            var transactionType = 0;
            decimal transPrice = 0;
            int numTrans = 0;

            if (stock != null | stock != "")
            {
                transPrice = QueryStockPrice(stock);
            }

            if (buy != "")
            {
                numTrans = Convert.ToInt16(buy);
            }

            if (userId != null & numTrans > 0)
            {
                MySqlConnection conn = new MySqlConnection();
                conn.CreateConn();
                SqlCommand cmd = new SqlCommand("CreateStockTransaction", conn.Connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@UserId", userId));
                cmd.Parameters.Add(new SqlParameter("@StockName", stock.ToUpper()));
                cmd.Parameters.Add(new SqlParameter("@Quantity", numTrans));
                cmd.Parameters.Add(new SqlParameter("@TransactionPrice", transPrice));
                cmd.Parameters.Add(new SqlParameter("@HasSold", transactionType));

                conn.Command = cmd;
                conn.Command.Prepare();
                conn.Command.ExecuteNonQuery();

            }
            return RedirectToAction("index");
        }

        public ActionResult SellStock(string stock, string sell)
        {
            string userId = User.Identity.GetUserId();
            Stock StockNote = new Stock();
            var transactionType = 1;
            decimal transPrice = 0;
            int numTrans = 0;

            if (sell != "")
            {
                numTrans = Convert.ToInt16(sell);
            }

            if (stock != null | stock != "")
            {
                transPrice = QueryStockPrice(stock);
            }

            if (userId != null & checkIfStocksExist(stock, userId, numTrans) & numTrans > 0)
            {
                MySqlConnection conn = new MySqlConnection();
                conn.CreateConn();
                SqlCommand cmd = new SqlCommand("CreateStockTransaction", conn.Connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@UserId", userId));
                cmd.Parameters.Add(new SqlParameter("@StockName", stock.ToUpper()));
                cmd.Parameters.Add(new SqlParameter("@Quantity", numTrans));
                cmd.Parameters.Add(new SqlParameter("@TransactionPrice", transPrice));
                cmd.Parameters.Add(new SqlParameter("@HasSold", transactionType));

                conn.Command = cmd;
                conn.Command.Prepare();
                conn.Command.ExecuteNonQuery();

            }
            return RedirectToAction("index");
        }

        public void UploadTransaction(string stock, int numShares, decimal price, int buySell)
        {
            string userId = User.Identity.GetUserId();
            var transactionType = 0;

            if (buySell == 1){
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

        public decimal QueryStockPrice(string ticker)
        {
            WebClient client = new WebClient();
            string finalString = "http://finance.yahoo.com/d/quotes.csv?s=";
            finalString += ticker.ToUpper();
            finalString += "&f=a";
            string price = client.DownloadString(finalString);
            try
            {
                return Convert.ToDecimal(price);
            }
            catch (FormatException){
                return 0;
            }
        }

        public Dictionary<string,List<List<decimal>>> GetTransactionHistory(string stock, string userId)
        {
            Dictionary<string,List<List<decimal>>> dict = new Dictionary<string, List<List<decimal>>>();
            List<List<decimal>> transactionList = new List<List<decimal>>();
            MySqlConnection conn = new MySqlConnection();
            conn.CreateConn();
            SqlCommand cmd = new SqlCommand("GetStockTransaction", conn.Connection);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@UserId", userId));
            cmd.Parameters.Add(new SqlParameter("@StockName", stock));

            conn.DataReader = cmd.ExecuteReader();
            while (conn.DataReader.Read())
            {
                List<decimal> stockValues = new List<decimal>();
                string sname = conn.DataReader["StockName"].ToString();
                int quantity = Convert.ToInt16(conn.DataReader["Quantity"]);
                decimal price = Convert.ToDecimal(conn.DataReader["TransactionPrice"]);
                int hasSold = Convert.ToInt16(conn.DataReader["HasSold"]);

                if (hasSold == 1)
                {
                    price = -price;
                }

                while (quantity > 0)
                {
                    stockValues.Add(price);
                    quantity -= 1;
                }
                if(dict.ContainsKey(stock)){
                    dict[stock].Add(stockValues);
                }
                else{
                    transactionList.Add(stockValues);
                    dict.Add(stock, transactionList);
                }

            }

            return dict;
        }

        Boolean checkIfStocksExist(string key, string userId, int numToSell){
            int numShares = 0;
            Dictionary<string, List<List<decimal>>> stockDict = GetTransactionHistory(key, userId);
            List<List<decimal>> stockList;
            if (stockDict.ContainsKey(key))
            {
                stockList = stockDict[key];
                foreach (List<decimal> transaction in stockList)
                {
                    foreach (decimal price in transaction)
                    {
                        if (price > 0)
                        {
                            numShares++;
                        }
                        else
                        {
                            numShares--;
                        }
                    }
                }
                if (numShares >= numToSell)
                {
                    return true;
                }
            }
            return false;
        }

        public List<string> GetUniqueStocks()
        {
            List<string> uniqueStocks = new List<string>();
            MySqlConnection conn = new MySqlConnection();
            conn.CreateConn();
            string userId = User.Identity.GetUserId();
            if (userId != null)
            {
                SqlCommand cmd = new SqlCommand("GetUniqueStocks", conn.Connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@UserId", userId));

                conn.DataReader = cmd.ExecuteReader();
                while (conn.DataReader.Read())
                {
                    string sname = conn.DataReader["StockName"].ToString();
                    uniqueStocks.Add(sname);

                }
            }

            return uniqueStocks;
        }

        public Dictionary<string, List<Stock>> GetStockHistory(string userId)
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
            Dictionary<string,List<Stock>> stockHistory = GetStockHistory(userId);

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
                    Dictionary<string,List<Stock>> stocks = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string,List<Stock>>>(json);
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


            return RedirectToAction("Index", new { uniqueUri = Request.RequestContext.RouteData.Values["uniqueUri"] });
        }

        public ActionResult IndividualStock()
        {
            // This is a message that can be called on the Stock's page.
            ViewBag.Message = "IndividualStock";

            return View();
        }
    }
}