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
    class HomeControllerTest
    {
        [Test]
        public void TestAboutIndex()
        {
            var obj = new WebApplication5.Controllers.HomeController();
            var actionResult = obj.About() as ViewResult;
            Assert.That(actionResult.ViewName, Is.EqualTo(""));
        }

        [Test]
        public void TestCalendarIndex()
        {
            var obj = new WebApplication5.Controllers.HomeController();
            var actionResult = obj.Calendar() as ViewResult;
            Assert.That(actionResult.ViewName, Is.EqualTo(""));
        }

        [Test]
        public void TestContactIndex()
        {
            var obj = new WebApplication5.Controllers.HomeController();
            var actionResult = obj.Contact() as ViewResult;
            Assert.That(actionResult.ViewName, Is.EqualTo(""));
        }



        [Test]
        public void TestUpdateStatus()
        {
            var obj = new WebApplication5.Controllers.HomeController();
            var result = (RedirectToRouteResult)obj.UpdateStatus("Test Status");
            Assert.That(result.RouteValues["action"], Is.EqualTo("Index"));
        }

    }
}
