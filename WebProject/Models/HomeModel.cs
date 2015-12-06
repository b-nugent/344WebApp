using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication5.Models
{
    public class HomeModel
    {
        public UserPostsModels Posts { get; set; }

        public List<EventModel> Events { get; set; }

    }
}