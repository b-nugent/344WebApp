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

    /// <summary>
    /// When the method is called with no arguments, just return the view
    /// When argument logOn is true, a user logged on
    /// When argument logOff is true, a user closed their browser or navigated away (log off)
    /// When argument chatMessage is specified, the user typed something in the chat
    /// </summary>
    public ActionResult Index(bool? logOn, bool? logOff, string chatMessage)
    {
        string UserID = User.Identity.GetUserId();

        if (UserID != null)
        {
            try
            {
                if (chatModel == null) chatModel = new ChatModel();
                ChatModel.ChatUser currentUser = chatModel.Users.FirstOrDefault(u => u.ChatUserID == User.Identity.GetUserId());
                if (logOn != null && (bool)logOn)
                {
                    return CheckLoggedInUser(UserID, chatModel);
                }

                else if (logOff != null && (bool)logOff)
                {
                    LogOffUser(chatModel.Users.FirstOrDefault(u => u.ChatUserID == User.Identity.GetUserId()));
                    ChatViewModel chatViewModel = new ChatViewModel();
                    chatViewModel.Users = chatModel.Users;
                    chatViewModel.ChatHistory = currentUser.ChatHistory;
                    return PartialView("Lobby", chatViewModel);
                }
                else
                {
                    

                    //remember each user's last ping time
                    currentUser.LastPing = DateTime.Now;
                    
                    #region remove inactive users
                    RemoveInactiveUsers(chatModel);
                    #endregion

                    #region if there is a new message, append it to the chat
                    if (!string.IsNullOrEmpty(chatMessage))
                    {
                        AddMessage(UserID, chatMessage, chatModel);
                    }
                    #endregion
                    ChatViewModel chatViewModel = new ChatViewModel();
                    chatViewModel.Users = chatModel.Users;
                    chatViewModel.ChatHistory = currentUser.ChatHistory;
                    return PartialView("ChatHistory", chatViewModel);
                }
            }
            catch (Exception ex)
            {
                //return error to AJAX function
                Response.StatusCode = 500;
                return Content(ex.Message);
            }
        }
        else
        {
            return View();
        }
    }

       

    /// <summary>
    /// Remove this user from the lobby
    /// </summary>
    /// <param name="user"></param>
    public static void LogOffUser(ChatModel.ChatUser user)
    {
        ChatModel.ChatUser userToRemove = null;
        if (chatModel != null)
        {

            foreach (ChatModel.ChatUser aUser in chatModel.Users)
            {
                if (aUser.ChatUserID.Equals(user.ChatUserID))
                {
                    userToRemove = aUser;
                }
            }
            chatModel.Users.Remove(userToRemove);
        }
       
    }

    private ActionResult CheckLoggedInUser(string UserID, ChatModel chatModel)
    {
        if (chatModel.Users.FirstOrDefault(u => u.ChatUserID == User.Identity.GetUserId()) == null)
        {
            #region create new user and add to lobby
            
                ChatModel.ChatUser currentUser = new ChatModel.ChatUser()
            {
                ChatUserID = UserID,
                Name = User.Identity.GetUserName(),
                LoggedOnTime = DateTime.Now,
                LastPing = DateTime.Now
            };
                chatModel.Users.Add(currentUser);

            //stored procedure select 
            MySqlConnection selectConn = new MySqlConnection();
            selectConn.CreateConn();
            SqlCommand command = new SqlCommand("GetChatMessages", selectConn.Connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@UserID", UserID));

            selectConn.DataReader = command.ExecuteReader();
            while (selectConn.DataReader.Read())
            {
                currentUser.ChatHistory.Add(new ChatModel.ChatMessage
                {
                    Message = selectConn.DataReader["MessageContent"].ToString(),
                    Username = selectConn.DataReader["UserName"].ToString()
                });
            }
            #endregion
        }
        ChatModel.ChatUser curUser = chatModel.Users.FirstOrDefault(u => u.ChatUserID == User.Identity.GetUserId());
        ChatViewModel chatViewModel = new ChatViewModel();
        chatViewModel.Users = chatModel.Users;
        chatViewModel.ChatHistory = curUser.ChatHistory;
        return PartialView("Lobby", chatViewModel);
    }

    private void RemoveInactiveUsers(ChatModel chatModel)
    {
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
    }

    private void AddMessage(string UserID, string chatMessage, ChatModel chatModel)
    {
        DateTime currentTime = DateTime.Now;
            MySqlConnection conn = new MySqlConnection();
            conn.CreateConn();
            SqlCommand cmd = new SqlCommand("AddChatMessage", conn.Connection);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@UserId", UserID));
            cmd.Parameters.Add(new SqlParameter("@MessageContent", chatMessage));
            cmd.Parameters.Add(new SqlParameter("@TimeReceived", currentTime));

            conn.Command = cmd;
            conn.Command.Prepare();
            conn.Command.ExecuteNonQuery();


            foreach (ChatModel.ChatUser usr in chatModel.Users)
            {
                SqlCommand cmd2 = new SqlCommand("AddReceivedMessage", conn.Connection);
                cmd2.CommandType = System.Data.CommandType.StoredProcedure;
                cmd2.Parameters.Add(new SqlParameter("@ReceivedUserID", usr.ChatUserID));
                cmd2.Parameters.Add(new SqlParameter("@MessageContent", chatMessage));
                cmd2.Parameters.Add(new SqlParameter("@CurrentTime", currentTime));

                conn.Command = cmd2;
                conn.Command.Prepare();
                conn.Command.ExecuteNonQuery();
                usr.ChatHistory.Add(new ChatModel.ChatMessage { Message = chatMessage, Username = User.Identity.Name });

            }
            conn.CloseConn();
            
        }
    }
}
