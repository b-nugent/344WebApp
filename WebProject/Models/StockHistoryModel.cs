using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication5.Models
{
    public class StockHistory
    {
        public Dictionary<string, List<Stock>> userHistory { get; set; }
    }
}