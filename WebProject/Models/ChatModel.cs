using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication5.Models
{
    public class ChatModel
    {

        /// <summary>
        /// Users that have connected to the chat
        /// </summary>
        public List<ChatUser> Users;

        /// <summary>
        /// Messages by the users
        /// </summary>
        public List<ChatMessage> ChatHistory;

        public ChatModel()
        {
            Users = new List<ChatUser>();
            ChatHistory = new List<ChatMessage>();
        }

        public ChatModel(List<ChatMessage> ChatHistory): this()
        {
            this.ChatHistory.AddRange(ChatHistory);
        }

        public class ChatUser
        {
            public string ChatUserID;
            public string Name;
            public DateTime LoggedOnTime;
            public DateTime LastPing;
        }

        public class ChatMessage
        {
            /// <summary>
            /// If null, the message is from the server
            /// </summary>
            public String Username;
            public string Message = "";

        }
    }
}