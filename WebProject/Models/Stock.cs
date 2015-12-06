using System;

/// <summary>
/// Summary description for Class1
/// </summary>
namespace WebApplication5.Models{

    public class Stock
    {
        public string Symbol { get; set; }
        public string Name { get; set; }
        public decimal BoughtPrice { get; set; }
        public decimal SoldPrice { get; set; }
        public int NumShares { get; set; }
        public string Note { get; set; }
    }

}
