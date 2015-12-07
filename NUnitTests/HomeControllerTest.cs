using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Web.Mvc;
using System.Web;
using Moq;
using System.Security.Principal;

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

        public static Mock<HttpContextBase> MockContext(IPrincipal principal = null)
        {
            var context = new Mock<HttpContextBase>();
            var request = new Mock<HttpRequestBase>();
            var response = new Mock<HttpResponseBase>();
            var session = new Mock<HttpSessionStateBase>();
            var server = new Mock<HttpServerUtilityBase>();

            context.Setup(ctx => ctx.Request).Returns(request.Object);
            context.Setup(ctx => ctx.Response).Returns(response.Object);
            context.Setup(ctx => ctx.Session).Returns(session.Object);
            context.Setup(ctx => ctx.Server).Returns(server.Object);

            if (principal != null)
                context.Setup(ctx => ctx.User).Returns(principal);

            return context;
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
