using System;
using System.Collections.Generic;
using System.Net;
using StockRetriever;
using StockDescription;

namespace StockInfo
{
    public class StockHandler
    {
        public static void getInfo(string ticker)
        {
            string csvData;

            using (WebClient web = new WebClient())
            {
                csvData = web.DownloadString(makeString(ticker));
            }

            List<Stock> prices = YahooFinance.Parse(csvData);

            foreach (Stock price in prices)
            {
                System.Console.WriteLine(string.Format("{0} ({1})  Current Price:{2} Day's High:{3} Day's Low:{4} Year High: {5} YearLow:{6}",price.Name,price.Symbol,price.CurrentPrice,price.DaysHigh,price.DaysLow,price.YearHigh,price.YearLow));
            }



        }
        private static string makeString(string ticker)
        {
            string finalString = "http://finance.yahoo.com/d/quotes.csv?s=";
            finalString += ticker.ToUpper();
            finalString += "&f=snahgkj";
            return finalString;
        }
    }

}