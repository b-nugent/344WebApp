using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Web.Mvc;
using System.Web;
using Moq;
using WebApplication5.Models;

namespace NUnitTests
{
    [TestFixture]
    class StockModelTest
    {
        [Test]
        public void TestNewStock()
        {
            var stock = new Mock<Stock>();
            stock.SetupProperty(x => x.Symbol,"GOOG");
            stock.SetupProperty(x => x.Name, "Alphabet");
            stock.SetupProperty(x => x.NumShares, 25);
            stock.SetupProperty(x => x.Note, "Don't be evil");

            Stock myStock = stock.Object;
            Assert.AreEqual("GOOG", myStock.Symbol);
            Assert.AreEqual("Alphabet", myStock.Name);
            Assert.AreEqual("Don't be evil", myStock.Note);
            Assert.AreEqual(25, myStock.NumShares);
        }
    }
}
