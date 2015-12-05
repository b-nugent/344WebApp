using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using WebApplication5.Models;
using WebApplication5.App_Data;
using System.Data.SqlClient;

namespace WebApplication5.Controllers
{
public class ChatController : Controller
{
    static ChatModel chatModel;
    private Object myLock = new Object();
    int msgID = 1;
    /// <summary>
    /// When the method is called with no arguments, just return the view
    /// When argument logOn is true, a user logged on
    /// When argument logOff is true, a user closed their browser or navigated away (log off)
    /// When argument chatMessage is specified, the user typed something in the chat
    /// </summary>
    public ActionResult Index(string user,bool? logOn, bool? logOff, string chatMessage)
    {
        string UserID = User.Identity.GetUserId();
        
        try
        {
            if (chatModel == null) chatModel = new ChatModel();

            if (!Request.IsAjaxRequest())
            {
                //first time loading
                return View(chatModel);
            }
            else if (logOn != null && (bool)logOn)
            {
                //check if nickname already exists
                if (chatModel.Users.FirstOrDefault(u => u.ChatUserID == User.Identity.GetUserId()) != null)
                {
                    throw new Exception("You are already logged into chat");
                }
                
                else
                {
                    #region create new user and add to lobby
                    chatModel.Users.Add( new ChatModel.ChatUser()
                    {
                        ChatUserID = UserID,
                        Name = User.Identity.GetUserName(),
                        LoggedOnTime = DateTime.Now,
                        LastPing = DateTime.Now
                    });

                    
                    #endregion

                }

                return PartialView("Lobby", chatModel);
            }
            else if (logOff != null && (bool)logOff)
            {
                LogOffUser( chatModel.Users.FirstOrDefault( u=>u.ChatUserID==User.Identity.GetUserId()) );
                return PartialView("Lobby", chatModel);
            }
            else
            {

                ChatModel.ChatUser currentUser = chatModel.Users.FirstOrDefault(u => u.ChatUserID == User.Identity.GetUserId());

                //remember each user's last ping time
                currentUser.LastPing = DateTime.Now;

                #region remove inactive users
                List<ChatModel.ChatUser> removeThese = new List<ChatModel.ChatUser>();
                foreach (Models.ChatModel.ChatUser usr in chatModel.Users)
                {
                    TimeSpan span = DateTime.Now - usr.LastPing;
                    if (span.TotalSeconds > 15)
                        removeThese.Add(usr);
                }
                foreach (ChatModel.ChatUser usr in removeThese)
                {
                    LogOffUser(usr);
                }
                #endregion

                #region if there is a new message, append it to the chat
                if (!string.IsNullOrEmpty(chatMessage))
                {

                    lock(myLock){
                    
                    MySqlConnection conn = new MySqlConnection();
                    conn.CreateConn();
                    SqlCommand cmd = new SqlCommand("AddChatMessage", conn.Connection);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@UserId", UserID));
                    cmd.Parameters.Add(new SqlParameter("@MessageContent", chatMessage));

                    conn.Command = cmd;
                    conn.Command.Prepare();
                    conn.Command.ExecuteNonQuery();
                    

                    foreach(ChatModel.ChatUser usr in chatModel.Users){
                        SqlCommand cmd2 = new SqlCommand("AddReceivedMessage", conn.Connection);
                        cmd2.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd2.Parameters.Add(new SqlParameter("@ReceivedUserID", usr.ChatUserID));
                        cmd2.Parameters.Add(new SqlParameter("@MessageID", msgID));

                        conn.Command = cmd2;
                        conn.Command.Prepare();
                        conn.Command.ExecuteNonQuery();

                    }
                    conn.CloseConn();

                    }
                }
                #endregion

                ChatModel userChatModel = new ChatModel(chatModel.ChatHistory);
                //stored procedure select 

                MySqlConnection selectConn = new MySqlConnection();
                selectConn.CreateConn();
                SqlCommand command = new SqlCommand("GetChatMessages", selectConn.Connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@UserID", UserID));

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    ChatModel.ChatUser msgSender;
                    string username = reader.GetString(reader.GetOrdinal("UserID")); 
                    msgSender = chatModel.Users.FirstOrDefault(u => u.ChatUserID == username); 
                    username = msgSender.Name;
                    userChatModel.ChatHistory.Add(new ChatModel.ChatMessage{
                        
                        Message = reader.GetString(reader.GetOrdinal("MessageContent")),
                        Username = username
                    });
                }

                return PartialView("ChatHistory", userChatModel);
            }
        }
        catch (Exception ex)
        {
            //return error to AJAX function
            Response.StatusCode = 500;
            return Content(ex.Message);
        }
    }

    /// <summary>
    /// Remove this user from the lobby
    /// </summary>
    /// <param name="user"></param>
    public void LogOffUser(ChatModel.ChatUser user)
    {
        chatModel.Users.Remove(user);
    }

}
}
