// Copyright (c) 2015 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.

// Component Name  :    CommunicationRepository
// Description     : 	Repository for Communication
// Author          :	Shaheena Shaik
// Creation Date   : 	12-April-2017

using Laserbeam.BusinessObject.Common;
using Laserbeam.Constant.HR;
using Laserbeam.DataManager.Interfaces.Common;
using Laserbeam.DataManager.Interfaces.Core;
using Laserbeam.EntityManager.Common;
using Laserbeam.Libraries.Core.Interfaces.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Laserbeam.DataManager.Common
{
    public class CommunicationRepository : ICommunicationRepository
    {
        #region Fields
        // Author         :  Shaheena Shaik
        // Creation Date  :  12-April-2017
        /// <summary>
        /// Object of Base Repository
        /// </summary>
        private IBaseRepository m_baseRepository;
        private ITenantCacheProvider m_tenantCacheProvider;
        #endregion

        #region Constructors
        // Author         :  Shaheena Shaik
        // Creation Date  : 12-April-2017
        /// <summary>
        /// Initializes objects used in this class
        /// </summary>
        /// <param name="baseRepository"></param>

        public CommunicationRepository(IBaseRepository baseRepository, ITenantCacheProvider tenantCacheProvider)            
        {
            m_baseRepository = baseRepository;
            m_tenantCacheProvider = tenantCacheProvider;
        }
        
        #endregion

        #region Public Methods

        public IQueryable<AppEmail> GetAppEmailInfo()
        {
            return m_baseRepository.GetQuery<AppEmail>().Where(x => x.Active == true);
        }

        // Author       : Shaheena Shaik
        //Creation Date :18/April-2017
        /// <summary>
        /// Binding ManageCommunicationTemplate Tree view values 
        /// </summary>
        /// <returns>Returning a list of values </returns>
        public IQueryable<ManageCommunicationTemplate> GetBindManageCommunicationTreeView()
        {
            var appEmail = m_baseRepository.GetQuery<AppEmail>().ToList();
            List<AppEmailDetails> Actionrequired = new List<AppEmailDetails>();
            Actionrequired = appEmail.Where(x => x.UserTemplate == false).Select(x => new AppEmailDetails { AppEmailID = x.AppEmailID, EmailSubject = x.EmailSubject, EmailBody = x.EmailBody }).ToList();
            if (Actionrequired.Count > 0)
            {
                Actionrequired.Add(new AppEmailDetails { EmailSubject = "Tool Generated Template", AppEmailID = 555555 });
            }

            List<AppEmailDetails> Others = new List<AppEmailDetails>();
            Others = appEmail.Where(x => x.UserTemplate == true).Select(x => new AppEmailDetails { AppEmailID = x.AppEmailID, EmailSubject = x.EmailSubject, EmailBody = x.EmailBody }).ToList();

            if (Others.Count > 0)
            {
                Others.Add(new AppEmailDetails { EmailSubject = "Administer Template", AppEmailID = 666666 });
            }

            List<ManageCommunicationTemplate> actionRequiredList = (from i in Actionrequired
                                                                 select new ManageCommunicationTemplate
                                                                 {
                                                                     EmailSubject = i.EmailSubject,
                                                                     AppEmailId = i.AppEmailID,
                                                                     EmailBody = i.EmailBody,
                                                                     IsTreeTop = (555555 == i.AppEmailID),
                                                                     ParentMenuNum = (555555 == i.AppEmailID) ? 0 : 555555,
                                                                     OrderByTree = 1
                                                                 }).ToList();

            List<ManageCommunicationTemplate> orderList = (from i in Others
                                                           select new ManageCommunicationTemplate
                                                           {
                                                               EmailSubject = i.EmailSubject,
                                                               AppEmailId = i.AppEmailID,
                                                               EmailBody = i.EmailBody,
                                                               IsTreeTop = (666666 == i.AppEmailID),
                                                               ParentMenuNum = (666666 == i.AppEmailID) ? 0 : 666666,
                                                               OrderByTree = 2
                                                           }).ToList();
            var result = actionRequiredList.AsQueryable().Union(orderList.AsQueryable());
            return result.OrderByDescending(x => x.IsTreeTop);
        }

        public string GetEmailOrder()
        {
            return m_baseRepository.GetQuery<AppEmail>().OrderByDescending(a => a.EmailOrderby).Select(a => a.EmailOrderby).First().ToString();
        }

        public List<AppEmail> GetEmail(int appMailID)
        {
            var appMail = m_baseRepository.GetQuery<AppEmail>().Where(x => x.AppEmailID == appMailID).ToList();
            return appMail;
        }

        public void DeleteEmail(int appMailID)
        {
            var deleteEmail = GetEmail(appMailID);
            m_baseRepository.Delete<AppEmail>(deleteEmail);
            m_baseRepository.SaveChanges();
        }

        public string PutEmail(AppEmail appEmail, int userNum)
        {
            var message = "";
            if (appEmail.AppEmailID == 0)
            {
                try
                {
                    appEmail.UpdatedBy = userNum;
                    appEmail.UpdatedDate = DateTime.Now;
                    appEmail.UserTemplate = appEmail.EmailSubject.Contains("Action Required") ? false : true;
                    m_baseRepository.Add<AppEmail>(appEmail);
                    m_baseRepository.SaveChanges();
                }
                catch (Exception ex)
                {
                    message = ex.InnerException.InnerException.Message;
                }
            }
            else
            {
                appEmail.UpdatedBy = userNum;
                appEmail.UpdatedDate = DateTime.Now;
                appEmail.UserTemplate = appEmail.EmailSubject.Contains("Action Required") ? false : true;
                m_baseRepository.Edit<AppEmail>(appEmail);
                m_baseRepository.SaveChanges();
            }
            return message;
        }


        public void PutUserAppURlTracker(string ClientUserID, string SecretKey, string RequestTimeStamp, string EncryptedMagicKey, string SiteUrl, bool IsEncrypted)
        {
            UserAppUrlTracker userdata = new UserAppUrlTracker();
            userdata.ClientUserID = ClientUserID;
            userdata.SecretKey = SecretKey;
            userdata.RequestTimeStamp = RequestTimeStamp;
            userdata.EncryptedMagicKey = EncryptedMagicKey;
            userdata.SiteUrl = SiteUrl;
            userdata.IsEncrypted = IsEncrypted;
            m_baseRepository.Add<UserAppUrlTracker>(userdata);
            m_baseRepository.SaveChanges();            
        }

        // Author       : Shaheena Shaik
        //Creation Date :18/April-2017
        /// <summary>
        /// Getting DropdownValues of Messages from database
        /// </summary>
        /// <param name="messageId">Id of a message of a dropdown</param>
        /// <returns>Returning a query of Message details</returns>
        public List<AppMessage> GetDashboardMessage(int? messageId)
        {
            if (messageId != null)
                return m_baseRepository.GetQuery<AppMessage>().Where(x => x.Active == true && x.AppMessageID == messageId).ToList();
            return m_baseRepository.GetQuery<AppMessage>().Where(x => x.Active == true).ToList();
        }

        // Author       : Shaheena Shaik
        //Creation Date :18/April-2017
        /// <summary>
        /// Checking whether the role is Admin or not
        /// </summary>
        /// <param name="userNum">UserNum of a user in a Tree</param>
        /// <returns>Returning a boolesan result</returns>
        protected bool IsRoleAdmin(int userNum)
        {
            return m_baseRepository.GetQuery<AppUser>(new string[]{ "AppUserRole" }, x => x.UserNum == userNum && x.AppUserRole.KeyName== "SuperAdmin").Count() > 0;            
        }

        // Author       : Shaheena Shaik
        //Creation Date :18/April-2017
        /// <summary>
        /// userNumn's access userlist
        /// </summary>
        /// <param name="userNum">Usernum of an employee in a treeview</param>
        /// <returns>Returning a  query of users</returns>
        protected IQueryable<AppUser> GetAssignedAppUsers(int userNum)
        {
            return m_baseRepository.GetQuery<AppUser>().Where(x => x.MarkAsDelete != true && x.UserID != "laserbeam");            
        }

        // Author       : Shaheena Shaik
        //Creation Date :18/April-2017
        /// <summary>
        /// Checking a Role of a user
        /// </summary>
        /// <param name="userNum">UserNum of a user in a Role tree view</param>
        /// <returns>Returninga  boolean result</returns>
        protected bool IsRoleUser(int userNum)
        {
            return m_baseRepository.GetQuery<AppUser>(new string[] { "AppUserRole" }, x => x.UserNum == userNum && x.AppUserRole.KeyName == "SuperAdmin").Count() > 0;
        }

        // Author       : Shaheena Shaik
        //Creation Date :18/April-2017
        /// <summary>
        /// Getting user data as they assined to UserNum's role
        /// </summary>
        /// <param name="userNum"></param>
        /// <returns></returns>
        protected IQueryable<int> GetAssignedEmployeeNums(int userNum)
        {            
            return m_baseRepository.GetQuery<EmployeeGroup>().Where(x => m_baseRepository.GetQuery<ProxyPopulationAccess>().Any(l => l.UserNum == userNum && l.OrgGroupID == x.OrgGroupID)).Select(x => x.EmployeeNum);
        }

        // Author       : Shaheena Shaik
        //Creation Date :18/April-2017
        /// <summary>
        /// Getting Users based on their role
        /// </summary>
        /// <param name="roleId">RoleId of a User of Tree View</param>
        /// <param name="userNum">UserNum of a user of Treeview</param>
        /// <returns>Returning a userData as a Query</returns>
        public IQueryable<AppUser> GetUserByRole(int roleId, int userNum)
        {
            IQueryable<AppUser> appUsers = GetAssignedAppUsers(userNum);
            var users = (from AppUser in appUsers
                         where AppUser.UserRoleNum == roleId && AppUser.UserNum!= userNum
                         select new
                         {
                             UserNum = AppUser.UserNum,
                             UserName = AppUser.UserName,
                             UserID = AppUser.UserID
                         }).OrderBy(m => m.UserName).ToList();


            IQueryable<AppUser> dta2 = users.Select(user => new AppUser
            {
                UserNum = user.UserNum,
                UserName = user.UserName + '_' + user.UserID,
            }).OrderBy(m => m.UserName).AsQueryable();

            return dta2;
        }

        public AppConfigModel GetAppSetting()
        {
            AppConfigModel appConfig = new AppConfigModel();
            //var appSetting = m_baseRepository.GetQuery<AppSetting>().ToList();
            var appSetting = m_tenantCacheProvider.GetApplicationSetting();
            var baseCurrency = appSetting.BaseCurrency;
            appConfig.CurrentYear = appSetting.CurrentYear.ToString();
            appConfig.MeritCycleYear = appSetting.MeritCycleYear.ToString();
            appConfig.SMTPServer = appSetting.SMTPServer;
            appConfig.IsSAMLLogin = appSetting.IsSAMLLogin;
            appConfig.AdminEmailID = appSetting.AdminEmailID;
            appConfig.SMTPPort = appSetting.SMTPPort;
            appConfig.LoginAttempt = appSetting.LoginAttempts;
            appConfig.AdminPassword = appSetting.AdminPassword;
            appConfig.ToolName = appSetting.ToolName;
            appConfig.ToolVersion = appSetting.ToolVersion;
            appConfig.Instance = appSetting.Instance;
            appConfig.EnableNotification = appSetting.EnableNotification;
            appConfig.EnableFeedBack = appSetting.EnableFeedBack;
            appConfig.TriggerEmailSameServer = appSetting.TriggerEmailSameServer;
            appConfig.Version = appSetting.Version;
            appConfig.PasswordDisableDuration = appSetting.PasswordDisableDuration;
            appConfig.RangeExceed = appSetting.RangeExceed;
            //appConfig.EnableHelpLink = (appSetting.SingleOrDefault(m => m.AppSettingID == AppUserConstants.EnableHelpLink).AppSettingValue.ToLower() == "true");
            appConfig.BaseCurrency = appSetting.BaseCurrency;
            appConfig.CurrencyApiAccessKey = appSetting.CurrencyApiAccessKey;
            appConfig.BaseCurrencyNum = m_baseRepository.GetQuery<Currency>(x => x.CurrencyCode == baseCurrency.Trim()).Select(x => x.CurrencyCodeNum).FirstOrDefault();
            appConfig.AdminUserID = appSetting.AdminUserID;
            return appConfig;

        }

        // Author       : Shaheena Shaik
        //Creation Date :18/April-2017
        /// <summary>
        /// Getting Tree View Data of UserRole
        /// </summary>
        /// <param name="userNum">UserNum Of a User</param>
        /// <returns>Returning a query of data for Tree View</returns>
        public IQueryable<DashboardSearchCriteriaTree> GetSendDashboardMessageSearchCriteriaTree(int userNum)
        {
            var appUserRoles = m_baseRepository.GetQuery<AppUserRole>().Select(x => new { x.KeyName, x.UserRoleNum, x.UserRole, x.Active }).ToList();
            List<AppUserRole> list = new List<AppUserRole>();
            bool isAdmin = IsRoleAdmin(userNum);

            if (isAdmin)
            {
                appUserRoles = appUserRoles.Where(x => x.Active == true).ToList();
            }
            else
            {
                appUserRoles = appUserRoles.Where(x => x.KeyName != "SuperAdmin" && x.KeyName != "Employee").ToList();
            }
            list = appUserRoles.Select(x => new AppUserRole { Active = x.Active, KeyName = x.KeyName, UserRole = x.UserRole, UserRoleNum = x.UserRoleNum }).ToList();
            if (list.Count > 0)
            {
                list.Add(new AppUserRole { UserRole = "User Role", UserRoleNum = 88888888 });
            }

            IQueryable<DashboardSearchCriteriaTree> roleList = list.Select(i => new DashboardSearchCriteriaTree
            {
                UserRoleName = (i.UserRole != "User Role") ? i.UserRole + "(" + GetUserByRole(i.UserRoleNum, userNum).Count().ToString() + ")" : i.UserRole,
                UserRoleNum = i.UserRoleNum,
                ReportingManagerNum = (88888888 == i.UserRoleNum) ? 0 : 88888888,
                ReporteeCount = GetUserByRole(i.UserRoleNum, userNum).Count(),
                IsTreeTop = (88888888 == i.UserRoleNum),
                OrderByTree = 1,
                SelectedType = 1
            }).AsQueryable();
            return roleList.OrderByDescending(m => m.IsTreeTop);
        }

        // Author       : Shaheena Shaik
        //Creation Date :18/April-2017
        /// <summary>
        /// Getting User Grid Data from database
        /// </summary>
        /// <param name="checkRole">Checking whether the Role is present or not</param>
        /// <param name="userRoleNum">UserRoleNum of a user</param>
        /// <param name="userNum">userNum of a user who is LoggedIn</param>
        /// <returns>Returning a query of data which is required to bind to the user grid</returns>
        public IQueryable<MessageModel> GetGridValues(bool checkRole, int userRoleNum, int userNum)
        {
            IQueryable<MessageModel> userGrid;            
            IQueryable<AppUser> appUsers = m_baseRepository.GetQuery<AppUser>(new string[] { "AppUserRole", "AppUserStatu" }, x=>x.MarkAsDelete == false && x.UserNum != userNum && x.AppUserStatu.UserStatus!= "Lock" && x.AppUserStatu.UserStatus!="In-Active");
            if(checkRole)
            {
                if(userRoleNum == 88888888){
                    userGrid = (from usr in appUsers
                                select new MessageModel
                                {
                                    UserRoleNum = usr.UserRoleNum,
                                    UserNum = usr.UserNum,
                                    UserID = usr.UserID,
                                    UserName = usr.UserName,
                                    EmailID = usr.EmailID,
                                    successStatus = usr.MailDeliveryStatus,
                                    SecretKey=usr.SecretKey,
                                    Status = usr.MailDeliveryStatus == true ? "Success" : "Failed"
                                });
                }
                else
                {
                    userGrid = (from usr in appUsers.Where(x => x.UserRoleNum == userRoleNum)
                                select new MessageModel
                                {
                                    UserRoleNum = usr.UserRoleNum,
                                    UserNum = usr.UserNum,
                                    UserID = usr.UserID,
                                    UserName = usr.UserName,
                                    EmailID = usr.EmailID,
                                    successStatus = usr.MailDeliveryStatus,
                                    SecretKey = usr.SecretKey,
                                    Status = usr.MailDeliveryStatus == true ? "Success" : "Failed"
                                });
                }
            }
            else
            {
                userGrid = (from usr in appUsers
                            select new MessageModel
                            {
                                UserRoleNum = usr.UserRoleNum,
                                UserNum = usr.UserNum,
                                UserID = usr.UserID,
                                UserName = usr.UserName,
                                EmailID = usr.EmailID,
                                successStatus = usr.MailDeliveryStatus,
                                SecretKey = usr.SecretKey,
                                Status = (usr.MailDeliveryStatus == null) ? "" : (usr.MailDeliveryStatus == true ? "Success" : "Failed")
                            });
            }
            return userGrid;
            
        }

        // Author       : Shaheena Shaik
        //Creation Date :18/April-2017
        /// <summary>
        /// Updating or Creating a Dashboard Message
        /// </summary>
        /// <param name="messageSubject">Subject of a message</param>
        /// <param name="messageBody">Body of the message</param>
        /// <param name="messageNum">ID of a message</param>
        /// <param name="userNum">Usernum of user who is Loggedin</param>
        /// <returns>Returning a string of message</returns>
        public string UpdateOrCreateMessage(string messageSubject, string messageBody, int messageNum, int userNum)
        {
            var message = "";
            AppMessage appMessage = new AppMessage();
            appMessage.MessageSubject = messageSubject;
            appMessage.MessageBody = messageBody;
            appMessage.UpdatedBy = userNum;
            appMessage.UpdatedDate = DateTime.Now;
            appMessage.AppMessageID = messageNum;
            appMessage.Active = true;
            if (appMessage.AppMessageID == 0)
            {
                    m_baseRepository.Add<AppMessage>(appMessage);
                    m_baseRepository.SaveChanges();
            }
            else
            {
                AppMessage appMessages = m_baseRepository.GetQuery<AppMessage>().Where(x => x.AppMessageID == messageNum).FirstOrDefault();
                appMessages.MessageSubject = messageSubject;
                appMessages.MessageBody = messageBody;
                m_baseRepository.Edit<AppMessage>(appMessages);
                m_baseRepository.SaveChanges();
            }
            return message;
        }

        // Author       : Shaheena Shaik
        //Creation Date :18/April-2017
        /// <summary>
        /// Deleting a dashboard message from database
        /// </summary>
        /// <param name="AppMessageID">messageId to delete</param>
        public void DeleteDashboardMessage(int AppMessageID)
        {
            var appMessageDetails = m_baseRepository.GetQuery<AppMessage>().Where(x => x.AppMessageID == AppMessageID).FirstOrDefault();
            UserMessage userMessageDetails = m_baseRepository.GetQuery<UserMessage>().Where(x => x.AppMessageID == AppMessageID).FirstOrDefault();
       
                if (appMessageDetails != null && userMessageDetails !=null)
                {
                    m_baseRepository.Delete<UserMessage>(userMessageDetails);
                    m_baseRepository.Delete<AppMessage>(appMessageDetails);
                    m_baseRepository.SaveChanges();
                }
            else
            {
                m_baseRepository.Delete<AppMessage>(appMessageDetails);
                m_baseRepository.SaveChanges();
            }
            
        }
        // Author       : Shaheena Shaik
        //Creation Date :18/April-2017
        /// <summary>
        /// Sending a dashboard message to get updated into the database
        /// </summary>
        /// <param name="userMessage">message details</param>
        /// <param name="userNum">usernum of who is sending the message</param>
        /// <returns>returning a string message</returns>
        public string SendDashboardMessage(List<UserMessageModel> userMessage, int userNum)
        {
            var message = "";
            int appMessageID = userMessage.Select(x => x.AppMessageID).FirstOrDefault();
            List<int> userNumList = m_baseRepository.GetQuery<UserMessage>().Where(x => x.AppMessageID == appMessageID).Select(num => num.UserNum).ToList();
            List<UserMessage> msgList = userMessage.Where(userList => !userNumList.Contains(userList.UserNum)).Select(x => new UserMessage
            {
                AppMessageID = x.AppMessageID,
                UserNum = x.UserNum,
                UpdatedBy = userNum,
                UpdatedDate = DateTime.Now

            }).ToList();
            m_baseRepository.Add<UserMessage>(msgList);
            m_baseRepository.SaveChanges();

            return message;
        }

        // Author       : Shaheena Shaik
        //Creation Date :22/April-2017
        /// <summary>
        /// Exporting the user details 
        /// </summary>
        /// <param name="appEmailID">app Email id which is passing as a parameter to the stored procedure</param>
        /// <param name="userNum">logged in user</param>
        /// <returns>Returning the export details</returns>
        public async Task<DataTable> GetEmailNotificationExportData(int appEmailID, int userNum)
        {
            SqlParameter[] parameters = {
                                  new SqlParameter("@AppEmailID", appEmailID),
                                  new SqlParameter("@userNum", userNum)
                            };
            var emailNotificationExport = await m_baseRepository.GetDataTableFromStoredProcedure("[Common].[USP_Communication_GET_SendEmailExport]", parameters);
            return emailNotificationExport;
        }

        public async Task<DataTable> GetDashBoardMessageExportData(int appMessageId, int userNum)
        {
            SqlParameter[] parameters = {
                                  new SqlParameter("@AppMessageID", appMessageId),
                                  new SqlParameter("@userNum", userNum)
                            };
            var dashBoardMessageExport = await m_baseRepository.GetDataTableFromStoredProcedure("[Common].[USP_Communication_GET_SendMessageExport]", parameters);
            return dashBoardMessageExport;
        }

        public AppEmail GetEmailTemplate(int emailID)
        {
            return m_baseRepository.GetQuery<AppEmail>(m => m.AppEmailID == emailID).FirstOrDefault();
        }

        public void emailTracker(List<EmailTracker> emailTrackerObj)
        {
            for(int i=0;i<emailTrackerObj.Count();i++)
            {
                m_baseRepository.Add<EmailTracker>(emailTrackerObj[i]);
                m_baseRepository.SaveChanges();
            }
            
        }

        public void PutSuccessStatus(bool SuccessStatus, string userId)
        {
            AppUser updateStatus = m_baseRepository.GetQuery<AppUser>().Where(x => x.UserID == userId).FirstOrDefault();
            if(SuccessStatus == true){
                updateStatus.MailDeliveryStatus = SuccessStatus;
            }
            else{
                updateStatus.MailDeliveryStatus = null;
            }
            m_baseRepository.Edit<AppUser>(updateStatus);
            m_baseRepository.SaveChanges();
        }


        #endregion
    }
}
