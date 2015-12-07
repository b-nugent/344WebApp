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
    class EventModelTest
    {
        [Test]
        public void TestEventModel()
        {
            var eventModel = new Mock<EventModel>();
            eventModel.SetupProperty(x => x.EventId, "25");
            eventModel.SetupProperty(x => x.UserId, "1");
            eventModel.SetupProperty(x => x.Name,"Event");
            eventModel.SetupProperty(x => x.Description, "Test Event");
            

            EventModel myEvent = eventModel.Object;
            Assert.AreEqual("Event", myEvent.Name);
            Assert.AreEqual("Test Event", myEvent.Description);
            Assert.AreEqual("1", myEvent.UserId);
            Assert.AreEqual("25", myEvent.EventId);
        }
    }
}
