using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication5.Models
{
    public class EventModel
    {
        public virtual string UserId { get; set; }
        public virtual string EventId { get; set; }
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual string DateFrom { get; set; }
        public virtual string DateTo { get; set; }
    }
}