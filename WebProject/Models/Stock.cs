using System;

/// <summary>
/// Summary description for Class1
/// </summary>
namespace StockDescription{

    public class Stock
    {
        public string Symbol { get; set; }
        public string Name { get; set; }
        public decimal CurrentPrice { get; set; }
        public decimal DaysHigh { get; set; }
        public decimal DaysLow { get; set; }
        public decimal YearHigh { get; set; }
        public decimal YearLow { get; set; }
    }

}
