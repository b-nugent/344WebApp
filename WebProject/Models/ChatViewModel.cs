using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication5.Models
{
    public class ChatViewModel

    {
        public List<ChatModel.ChatUser> Users;
        public List<ChatModel.ChatMessage> ChatHistory;
    }
}