using Laserbeam.BusinessObject.Common;
using Laserbeam.DataManager.Interfaces.Common;
using Laserbeam.DataManager.Interfaces.Core;
using Laserbeam.EntityManager.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using OpenTokSDK;
using System.Threading.Tasks;

namespace Laserbeam.DataManager.Common
{
    public class ChatBoxRepository : IChatBoxRepository
    {
        #region Fields
        // Author         :  
        // Creation Date  :  
        /// <summary>
        /// Object of Base Repository
        /// </summary>
        private IBaseRepository m_baseRepository;

        #endregion

        #region Constructors
        // Author         :  
        // Creation Date  :  
        /// <summary>
        /// Initializes objects used in this class
        /// </summary>
        /// <param name="baseRepository">Base Repository Object</param>
        public ChatBoxRepository(IBaseRepository baseRepository)
        {
            m_baseRepository = baseRepository;
        }

        public int GetAppuserStatus(int UserNum)
        {
            Thread.Sleep(TimeSpan.FromSeconds(0.50));
            int Status = m_baseRepository.GetQuery<AppUser>().Where(a => a.UserNum == UserNum).Select(s => s.ChatStatus).FirstOrDefault() ?? 1;
            return Status;
        }
        #endregion
        public List<ChatAccountModel> GetChatUserDetails(int loggedInUserNum,AppConfigModel appConfig)
        {
            OpenTok openTok = new OpenTok(appConfig.TokBoxApiKey, appConfig.TokBoxSecretKey);
            var appUser = (
                 from app in m_baseRepository.GetQuery<AppUser>()
                 join appStatus in m_baseRepository.GetQuery<AppUserStatu>(x => x.UserStatus == "Active") on app.AppUserStatusID equals appStatus.AppUserStatusID
                 select app).ToList();
            var chat = m_baseRepository.GetQuery<ChatDetail>().Where(x => x.ViewedStatus == false && x.ReceiverUserNum == loggedInUserNum).ToList();
            var sessionDetails = m_baseRepository.GetQuery<ChatSessionDetail>().ToList();

            var GetManagerDetails = (from app in appUser
                                     let chatData = chat.Where(x => x.SenderUserNum == app.UserNum).Count()
                                     let session = sessionDetails.Count()>0?sessionDetails.Where(x=>( (x.FromUserNum == app.UserNum && x.ToUserNum == loggedInUserNum) || (x.ToUserNum == app.UserNum && x.FromUserNum == loggedInUserNum))).FirstOrDefault():null
                                     select new ChatAccountModel
                                     {
                                         EmployeeNum = app.EmployeeNum ?? 0,
                                         UserName = app.UserName,
                                         Usernum = app.UserNum,
                                         UserStatus = (app.LastLoginDt == null)?0:((app.IsLoggedIn??0==1)?(app.ChatStatus??1):0),
                                         unReadChatCount = chatData,
                                         ApiKey = appConfig.TokBoxApiKey,
                                         sessionID = session!=null? session.ChatSessionID:"Empty",
                                         token = session != null ? session.Token : "Empty",
                                     }).Distinct().OrderBy(X => X.UserName).ToList();

            foreach (var item in GetManagerDetails.Where(x => x.sessionID == "Empty").ToList())
            {

                item.sessionID = openTok.CreateSession().Id;
                ChatSessionDetail data = new ChatSessionDetail();
                data.FromUserNum = loggedInUserNum;
                data.ToUserNum = item.Usernum;
                data.ChatSessionID = item.sessionID;
                m_baseRepository.Add<ChatSessionDetail>(data);
                m_baseRepository.SaveChanges();
            }
            foreach (var item in GetManagerDetails)
            {
                var epochTimeInSeconds = (DateTime.UtcNow.AddHours(3) - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
                string connectionMetadata = "userNum=" + loggedInUserNum + ",sessionID=" + item.sessionID;
                item.token = openTok.GenerateToken(item.sessionID, Role.PUBLISHER, epochTimeInSeconds, connectionMetadata);
            }
            return GetManagerDetails;
        }

        public List<ChatStatus> GetStatusList()
        {
            var ChatStatusList = m_baseRepository.GetQuery<ChatUserStatus>().ToList();

            List<ChatStatus> ChatList = ChatStatusList.Select(e => new ChatStatus
            {
                ID = e.ID,
                ChatStatus1 = e.ChatStatus,
            }).ToList();
            return ChatList;
        }

        public async Task UpdateStatus(int ID, int UserNum)
        {
            AppUser appuser = m_baseRepository.GetQuery<AppUser>().Where(a => a.UserNum == UserNum).FirstOrDefault();
            appuser.ChatStatus = ID;
            m_baseRepository.Edit<AppUser>(appuser);
            await m_baseRepository.SaveChangesAsync();
        }

        public List<ChatDetails> GetChatDetails(int loggedInUserNum,int selectedUserNum,AppConfigModel appConfig)
        {
            var appUser = m_baseRepository.GetQuery<AppUser>(x => x.UserNum == loggedInUserNum || x.UserNum == selectedUserNum);
            var selectedUser = appUser.Where(x => x.UserNum == selectedUserNum).FirstOrDefault();
            var chatdata = m_baseRepository.GetQuery<ChatDetail>().Where(x => x.ViewedStatus == false && x.ReceiverUserNum == loggedInUserNum && x.SenderUserNum == selectedUserNum).ToList();
            if (chatdata.Count() > 0)
            {
                foreach(var item in chatdata)
                {
                    item.ViewedStatus = true;
                    m_baseRepository.Edit<ChatDetail>(item);
                    m_baseRepository.SaveChanges();
                }
            }
            var sessionID = m_baseRepository.GetQuery<ChatSessionDetail>().Where(chat => ((chat.FromUserNum == loggedInUserNum && chat.ToUserNum == selectedUserNum) ||
                         (chat.FromUserNum == selectedUserNum && chat.ToUserNum == loggedInUserNum))).Select(x => x.ChatSessionID).FirstOrDefault() ?? "";
            //var sessionData = m_baseRepository.GetQuery<ChatSessionDetail>().Where(chat => ((chat.FromUserNum == loggedInUserNum && chat.ToUserNum == selectedUserNum) ||
            //             (chat.FromUserNum == selectedUserNum && chat.ToUserNum == loggedInUserNum))).FirstOrDefault() ;
            //var sessionID = sessionData.ChatSessionID;
            //var Token = sessionData.Token;
            var Token = "";
            if (sessionID != "")
            {
                OpenTok openTok = new OpenTok(appConfig.TokBoxApiKey, appConfig.TokBoxSecretKey);
                var epochTimeInSeconds = (DateTime.UtcNow.AddHours(3) - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
                string connectionMetadata = "userNum=" + loggedInUserNum + ",sessionID=" + sessionID;
                Token = openTok.GenerateToken(sessionID, Role.PUBLISHER, epochTimeInSeconds, connectionMetadata);
            }
            var result= (from chat in m_baseRepository.GetQuery<ChatDetail>()
                          where ((chat.SenderUserNum == loggedInUserNum && chat.ReceiverUserNum == selectedUserNum) ||
                               (chat.SenderUserNum == selectedUserNum && chat.ReceiverUserNum == loggedInUserNum))
                          select new ChatDetails
                          {
                              ChatDetailNum = chat != null ?chat.ChatDetailNum : 0,
                              SenderUserNum = chat != null ? appUser.Where(x => x.UserNum == chat.SenderUserNum).Select(x => x.UserNum).FirstOrDefault() : 0,
                              SenderUserName = chat != null ? appUser.Where(x=>x.UserNum == chat.SenderUserNum).Select(x=>x.UserName).FirstOrDefault() : "",
                              SenderUserShortName = chat != null ? appUser.Where(x => x.UserNum == chat.SenderUserNum).Select(x => x.UserName).FirstOrDefault().Substring(0, 1) : "",
                              Chat = chat != null ? chat.Chat : "",
                              Attachment = chat != null ? chat.Attachement : "",
                              FileType = chat != null ? chat.FileType??0 : 0,
                              FileName = chat != null ? chat.FileName : "",
                              FilePath=  chat != null ? (chat.Attachement.Contains("Images") ? (chat.Attachement.Replace("~", "../../") + "/" + chat.ChatDetailNum + "_" + chat.FileName) : chat.Attachement + "/"+chat.ChatDetailNum+"_"+chat.FileName) : "",
                              CreatedDate = chat != null ? chat.CreatedDate : DateTime.Now,
                              Time = chat != null ? chat.CreatedDate.ToString() : "",
                              userChatStatus = chat != null ? (selectedUser.LastLoginDt == null) ? 0 : ((selectedUser.IsLoggedIn ?? 0 == 1) ? (selectedUser.ChatStatus ?? 1) : 0)  : 0,
                              SessionID = sessionID,
                              ApiKey=appConfig.TokBoxApiKey,
                              Token = Token
                          }).ToList();
            if(result.Count()==0)
            {
                List<ChatDetails> a = new List<ChatDetails>();
                a.Add(new ChatDetails { userChatStatus = (selectedUser.LastLoginDt == null) ? 0 : ((selectedUser.IsLoggedIn ?? 0 == 1) ? (selectedUser.ChatStatus ?? 1) : 0), SessionID = sessionID, Token = Token, ApiKey = appConfig.TokBoxApiKey });
                return a;
            }
            return result;
        }

        public List<ChatDetails> UpdateChat(int loggedInUserNum, int selectedUserNum, string chat, string PathName, int FileTYpe,string FileName, AppConfigModel appConfig)
        {
            ChatDetail a = new ChatDetail();
            a.SenderUserNum = loggedInUserNum;
            a.ReceiverUserNum = selectedUserNum;
            a.Chat = chat;
            a.Attachement = PathName;
            a.FileType = FileTYpe;
            a.FileName = FileName;
            a.ViewedStatus = false;
            a.CreatedDate = DateTime.Now;
            m_baseRepository.Add<ChatDetail>(a);
            m_baseRepository.SaveChanges();
            var primaryKey = a.ChatDetailNum;
            if(FileTYpe==3)
                return GetChatDetails(loggedInUserNum, selectedUserNum, appConfig);
            else
            {
                var ChatDetail = (from x in m_baseRepository.GetQuery<ChatDetail>()
                                  where x.ChatDetailNum == primaryKey
                                  select new ChatDetails
                                  {
                                      ChatDetailNum = x.ChatDetailNum
                                  }).ToList();
                return ChatDetail;
            }
        }

        public List<ChatDetails> GetSearchChatDetails(int loggedInUserNum, int selectedUserNum, string Chat)
        {
            var appUser = m_baseRepository.GetQuery<AppUser>(x => x.UserNum == loggedInUserNum || x.UserNum == selectedUserNum);
            var selectedUser = appUser.Where(x => x.UserNum == selectedUserNum).FirstOrDefault();
            var result = (from chat in m_baseRepository.GetQuery<ChatDetail>()
                          where ((chat.SenderUserNum == loggedInUserNum && chat.ReceiverUserNum == selectedUserNum) ||
                               (chat.SenderUserNum == selectedUserNum && chat.ReceiverUserNum == loggedInUserNum)) && (chat.Chat.Contains(Chat))
                          select new ChatDetails
                          {
                              ChatDetailNum = chat != null ? chat.ChatDetailNum : 0,
                              SenderUserNum = chat != null ? appUser.Where(x => x.UserNum == chat.SenderUserNum).Select(x => x.UserNum).FirstOrDefault() : 0,
                              SenderUserName = chat != null ? appUser.Where(x => x.UserNum == chat.SenderUserNum).Select(x => x.UserName).FirstOrDefault() : "",
                              SenderUserShortName = chat != null ? appUser.Where(x => x.UserNum == chat.SenderUserNum).Select(x => x.UserName).FirstOrDefault().Substring(0, 1) : "",
                              Chat = chat != null ? chat.Chat : "",
                              Attachment = chat != null ? chat.Attachement : "",
                              FileType = chat != null ? chat.FileType ?? 0 : 0,
                              FileName = chat != null ? chat.FileName : "",
                              FilePath = chat != null ? (chat.Attachement.Contains("Images") ? (chat.Attachement.Replace("~", "../../") + "/" + chat.ChatDetailNum + "_" + chat.FileName) : chat.Attachement + "/" + chat.ChatDetailNum + "_" + chat.FileName) : "",
                              CreatedDate = chat != null ? chat.CreatedDate : DateTime.Now,
                              Time = chat != null ? chat.CreatedDate.ToString() : "",
                              userChatStatus = chat != null ? (selectedUser.LastLoginDt == null) ? 0 : ((selectedUser.IsLoggedIn ?? 0 == 1) ? (selectedUser.ChatStatus ?? 1) : 0) : 0
                          }).ToList();
            if (result.Count() == 0)
            {
                List<ChatDetails> a = new List<ChatDetails>();
                a.Add(new ChatDetails { userChatStatus = (selectedUser.LastLoginDt == null) ? 0 : ((selectedUser.IsLoggedIn ?? 0 == 1) ? (selectedUser.ChatStatus ?? 1) : 0) });
                return a;
            }
            return result;
        }
    }
}
