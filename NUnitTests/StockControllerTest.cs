using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Web.Mvc;
using System.Web;

namespace NUnitTests
{
    [TestFixture]
    public class StockControllerTest
    {
        [Test]
        public void TestIndividualStock()
        {
            string expected = "";
            var controller = new WebApplication5.Controllers.StockController();
            var result = controller.IndividualStock() as ViewResult;
            Assert.AreEqual(expected, result.ViewName);
        }
        
    }
}
