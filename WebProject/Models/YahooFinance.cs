using System;
using StockDescription;
using System.Collections.Generic;

namespace StockRetriever
{
    public static class YahooFinance
    {
        public static List<Stock> Parse(string csvData)
        {
            List<Stock> prices = new List<Stock>();

            string[] rows = csvData.Replace("\r", "").Split('\n');

            foreach (string row in rows)
            {
                if (string.IsNullOrEmpty(row)) continue;

                string[] cols = row.Split(',');

                Stock s = new Stock();
                s.Symbol = cols[0];
                s.Name = cols[1];
                s.CurrentPrice = Convert.ToDecimal(cols[2]);
                s.DaysHigh = Convert.ToDecimal(cols[3]);
                s.DaysLow = Convert.ToDecimal(cols[4]);
                s.YearHigh = Convert.ToDecimal(cols[5]);
                s.YearLow = Convert.ToDecimal(cols[6]);

                prices.Add(s);
            }

            return prices;
        }
    }
}

