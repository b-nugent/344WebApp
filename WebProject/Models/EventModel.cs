using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication5.Models
{
    public class EventModel
    {
        public string UserId { get; set; }
        public string EventId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
    }
}