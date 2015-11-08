using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace NUnitTests
{
    [TestFixture]
    public class StockControllerTest
    {
        [Test]
        public void TestGetStockName()
        {
            string result = StockInfo.StockHandler.makeString("goog");
            Assert.AreEqual(result, "http://finance.yahoo.com/d/quotes.csv?s=GOOG&f=snahgkj");
        }
        
    }
}
