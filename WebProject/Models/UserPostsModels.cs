using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication5.Models
{
    public class UserPostsModels
    {
        public IList<UserPost> posts {get; set;}
    }

    public class UserPost
    {
        public string type { get; set; }
        public string createdTime { get; set; }
        public string message { get; set; }
    }
}