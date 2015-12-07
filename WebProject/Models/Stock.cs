using System;

/// <summary>
/// Summary description for Class1
/// </summary>
namespace WebApplication5.Models{

    public interface StockInterface { }

    public class Stock
    {
        public virtual string Symbol { get; set; }
        public virtual string Name { get; set; }
        public virtual decimal BoughtPrice { get; set; }
        public virtual decimal SoldPrice { get; set; }
        public virtual int NumShares { get; set; }
        public virtual string Note { get; set; }
    }

}
