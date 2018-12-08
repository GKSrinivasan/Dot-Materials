// Copyright (c) 2016 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.
// Component Name  : 	Communication Business Manager
// Description     : 	Allows to perform various operation on SendEmail and DashBoard Messages. 	
// Author          :  Shaheena Shaik
// Creation Date   :  18-April-2017

using Laserbeam.BusinessObject.Common;
using Laserbeam.DataManager.Interfaces.Common;
using Laserbeam.ProcessManager.Interfaces.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Laserbeam.Libraries.Interfaces.Common;
using Laserbeam.Constant.HR;
using System.Data;
using Laserbeam.EntityManager.Common;
using Laserbeam.Libraries.Common;
using System.Web;

namespace Laserbeam.ProcessManager.Common
{
    public class CommunicationProcessManager : ICommunicationProcessManager
    {
        #region Fields
        // Author         :   Shaheena Shaik	
        // Creation Date  :   18-April-2017 
        /// <summary>
        /// Instance of CommunicationRepository
        /// </summary>
        ICommunicationRepository m_communicationRepository;
        private readonly IEmail m_email;
        #endregion


        #region Constructors
        // Author         :   Shaheena Shaik	
        // Creation Date  :   18-April-2017 
        /// <summary>
        /// Constructor for CommunicationProcessManager
        /// </summary>
        /// <param name="groupRepository">Instance of CommunicationRepository</param>
        public CommunicationProcessManager(ICommunicationRepository communicationRepository, IEmail email)
        {
            m_communicationRepository = communicationRepository;
            m_email = email;
        }
        #endregion

        // Author       : Shaheena Shaik
        //Creation Date :18/April-2017
        /// <summary>
        /// Getting a data for binding ManageCommunication Tree view
        /// </summary>
        /// <returns>Returning a tree view data as a query</returns>
        public IQueryable<ManageCommunicationTemplate> GetBindManageCommunicationTreeView()
        {
            return m_communicationRepository.GetBindManageCommunicationTreeView();
        }

        // Author       : Shaheena Shaik
        //Creation Date :18/April-2017
        /// <summary>
        /// Getting appEmailDetails from database to display onto the tree view 
        /// </summary>
        /// <returns>Returning a list of AppEmail data</returns>
        public List<AppEmailDetails> GetAppEmailInfo()
        {
            var appEmailModel = m_communicationRepository.GetAppEmailInfo().OrderBy(m => m.EmailOrderby);
           List<AppEmailDetails> appEmailInfo = appEmailModel.Select(x => new AppEmailDetails
            {
                AppEmailID = x.AppEmailID,
                EmailBody = x.EmailBody,
                EmailDescr = x.EmailDescr,
                EmailKey = x.EmailKey,
                EmailOrderby = x.EmailOrderby,
                EmailSubject = x.EmailSubject,
                EmailScript = x.EmailScript,
                UserTemplate = x.UserTemplate,
                Active = x.Active
            }).ToList();
           return appEmailInfo;

        }

        // Author       : Shaheena Shaik
        //Creation Date :18/April-2017
        /// <summary>
        /// Getting Email from the database to get Email data
        /// </summary>
        /// <param name="appMailID">appEmail id of the Email which we choosen from tree</param>
        /// <returns></returns>
        public List<AppEmail> GetEmail(int appMailID)
        {
            return m_communicationRepository.GetEmail(appMailID);
        }

        // Author       : Shaheena Shaik
        //Creation Date :18/April-2017
        /// <summary>
        /// deleting an Email from database
        /// </summary>
        /// <param name="appMailID">Id of the mail which we are going to delete</param>
        public void DeleteEmail(int appMailID)
        {
            m_communicationRepository.DeleteEmail(appMailID);
        }

        // Author       : Shaheena Shaik
        //Creation Date :18/April-2017
        /// <summary>
        /// Updating or Adding Communication mail into the database
        /// </summary>
        /// <param name="ddlSubject">Subject of the mail</param>
        /// <param name="editor">Body of the mail</param>
        /// <param name="appMailID">Id of the mail</param>
        /// <param name="userNum">UserNum of who is updating or creating email</param>
        /// <returns>Returning a string message</returns>
        public string PutCommunicationEmail(string ddlSubject, string editor, int appMailID, int userNum)
        {
            var msg = "";
            if (appMailID == 0)
            {
                string order1 = m_communicationRepository.GetEmailOrder();                
                string order2 = (order1 == "") ? "0" : order1;
                byte order = byte.Parse(order2);
                order += 1;
                AppEmail app = new AppEmail();
                app.EmailSubject = ddlSubject;
                app.EmailBody = editor;
                app.AppEmailID = appMailID;
                app.Active = true;
                app.Active = true;
                app.EmailOrderby = order;
                app.EmailKey = ddlSubject;
                msg = m_communicationRepository.PutEmail(app, userNum);
            }
            else
            {
                var list = GetEmail(appMailID).FirstOrDefault();
                list.EmailSubject = ddlSubject;
                list.EmailBody = editor;
                list.AppEmailID = appMailID;
                msg = m_communicationRepository.PutEmail(list, userNum);
            }
            return msg;
        }

        // Author       : Shaheena Shaik
        //Creation Date :18/April-2017
        /// <summary>
        /// Getting list of message Data from the databse to display onto the tree view
        /// </summary>
        /// <param name="messageId">Id of the message to get the required data </param>
        /// <returns>returning a list of message details </returns>
        public List<AppMessageModel> GetDashboardMessage(int? messageId)
        {
            var DashboardMessageDetails = m_communicationRepository.GetDashboardMessage(messageId);
            List<AppMessageModel> appMessageDetails = DashboardMessageDetails.Select(x => new AppMessageModel
                {
                    Active = x.Active,
                    AppMessageID = x.AppMessageID,
                    MessageBody = x.MessageBody,
                    MessageDescr = x.MessageDescr,
                    MessageOrderby = x.MessageOrderby,
                    MessageScript = x.MessageScript,
                    MessageSubject = x.MessageSubject,
                    MessageType = x.MessageType,
                    UpdatedBy = x.UpdatedBy,
                    UpdatedDate = DateTime.Now
                }).ToList();
            return appMessageDetails;

        }

        // Author       : Shaheena Shaik
        //Creation Date :18/April-2017
        /// <summary>
        /// Getting data for tree view from database
        /// </summary>
        /// <param name="userNum">userNum of user who is loggedIn</param>
        /// <returns>Returning Tree view data as a query</returns>
        public IQueryable<DashboardSearchCriteriaTree> GetSendDashboardMessageSearchCriteriaTree(int userNum)
        {
            return m_communicationRepository.GetSendDashboardMessageSearchCriteriaTree(userNum);
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
            return m_communicationRepository.GetGridValues(checkRole,userRoleNum,userNum);
        }

        // Author       : Shaheena Shaik
        //Creation Date :18/April-2017
        /// <summary>
        ///  Updating or Creating a Dashboard Message
        /// </summary>
        /// <param name="messageSubject">Subject of a message</param>
        /// <param name="messageBody">Body of the message</param>
        /// <param name="messageNum">ID of a message</param>
        /// <param name="userNum">Usernum of user who is Loggedin</param>
        /// <returns>Returning a string of message</returns>
        public string UpdateOrCreateMessage(string messageSubject, string messageBody, int messageNum, int userNum)
        {
            return m_communicationRepository.UpdateOrCreateMessage(messageSubject, messageBody, messageNum, userNum);
        }

        // Author       : Shaheena Shaik
        //Creation Date :18/April-2017
        /// <summary>
        /// Deleting a dashboard message from database
        /// </summary>
        /// <param name="AppMessageID">messageId to delete</param>
        public void DeleteDashboardMessage(int AppMessageID)
        {
            m_communicationRepository.DeleteDashboardMessage(AppMessageID);
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
            return m_communicationRepository.SendDashboardMessage(userMessage, userNum);
        }

        public AppConfigModel GetAppSetting()
        {
            return m_communicationRepository.GetAppSetting();
        }

        // Author       : Shaheena Shaik
        // Creation date: 18/April-2017
        /// <summary>
        /// adding or updadting email data into the database
        /// </summary>
        /// <param name="emailInfoList">model ofemail details </param>
        /// <param name="emailContent">email body</param>
        /// <param name="templateID">id of a mail</param>
        /// <param name="subject">email subject</param>
        /// <param name="appConfig">appconfig model object </param>
        /// <returns>returnig the email details which is updated into database</returns>
        public List<EmailDetails> EmailConfiguration(List<EmailDetails> emailInfoList, int templateID, AppConfigModel appConfigModel, int userNum, string requestURL)
        {
            Exception exceptionMessage = null;
            List<EmailTracker> sendStatuss = new List<EmailTracker>();
            var template = m_communicationRepository.GetEmailTemplate(templateID);
            List<EmailDetails> emailDataList = emailInfoList.Select(x => new EmailDetails
            {
                AdminEmailID = appConfigModel.AdminEmailID,                
                EmailSubject = template.EmailSubject,
                EmailBody = template.EmailBody,
                ClientUserID = x.UserID,
                DefaultPassword = x.DefaultPassword,
                UserName = x.UserName,
                EmailID = x.EmailID,                
                SecretKey = x.SecretKey,
                UserNum = x.UserNum
            }).ToList();

            List<EmailDetails> emailContentDetails = mailContentDetails(emailDataList, appConfigModel, requestURL, template);
            List<EmailDetails> emailDataList2 = new List<EmailDetails>();
            try
            {
                if (emailDataList != null)
                {
                    sendStatuss = m_email.SendEmail(emailContentDetails, templateID, appConfigModel, userNum, true);
                    var emailObject = emailDataList.Select(x => new EmailDetails
                    {
                        ClientUserID = x.ClientUserID,
                        UserName = x.UserName,
                        EmailID = x.EmailID,
                        DefaultPassword = x.DefaultPassword,
                        Success = sendStatuss.Where(y => y.ClientUserID == x.ClientUserID).Select(y => y.Success).FirstOrDefault(),
                        ManagerName = x.ManagerName

                    }).ToList();

                    emailDataList2 = emailObject.Select(e => new EmailDetails
                    {
                        ClientUserID = e.ClientUserID,
                        UserName = e.UserName,
                        EmailID = e.EmailID,
                        DefaultPassword = e.DefaultPassword,
                        Success = e.Success,
                        SuccessStatus = e.Success == true ? "Success" : "Failed",
                        UpdatedDateForEmail = DateTime.Now.ToShortDateString(),
                        ManagerName = e.ManagerName
                    }).ToList();
                    var successStatus = emailDataList2.Select(x => x.SuccessStatus).FirstOrDefault();
                    string userId = emailInfoList.Select(x => x.UserID).FirstOrDefault();

                    if (successStatus == "Success")
                    {
                        bool SuccessStatus = true;
                        m_communicationRepository.PutSuccessStatus(SuccessStatus, userId);
                    }
                        
                    m_communicationRepository.emailTracker(sendStatuss);
                    return emailDataList2;
                }

            }
            catch (Exception ex)
            {
                exceptionMessage = ex;
            }
            return emailDataList2;
        }
        
        private List<EmailDetails> mailContentDetails(List<EmailDetails> emailInfoList, AppConfigModel appConfigModel, string requestURL, AppEmail template)
        {
            List<EmailDetails> emailDataList2 = new List<EmailDetails>();
            string siteUrl = requestURL;
            bool isEncrypted = false;
            bool isSingleUse = (template.EmailKey == "Welcome to TMS" || template.EmailKey == "Forget Password Reset");
            string encryptedMagicKey = string.Empty;
            foreach (var item in emailInfoList)
            {
                if ((item.UserNum != 0) && !string.IsNullOrWhiteSpace(item.SecretKey))
                {
                    DateTime epochStart = new DateTime(1970, 01, 01, 0, 0, 0, 0, DateTimeKind.Utc);
                    TimeSpan timeSpan = DateTime.UtcNow - epochStart;
                    string requestTimeStamp = (isSingleUse) ? "" : Convert.ToUInt64(timeSpan.TotalSeconds).ToString();
                    isEncrypted = MayaLink.TryEncrypt(item.ClientUserID + '|' + item.SecretKey + '|' + requestTimeStamp, out encryptedMagicKey);
                    item.referenceUrl = isEncrypted ? siteUrl + "?mayaLink=" + HttpUtility.UrlEncode(encryptedMagicKey) : siteUrl;
                    //The generated mayaLink will be stored in the database for reference.
                    m_communicationRepository.PutUserAppURlTracker(item.ClientUserID, item.SecretKey, requestTimeStamp, encryptedMagicKey, siteUrl + "?mayaLink=" + HttpUtility.UrlEncode(encryptedMagicKey), isEncrypted);
                }
            }

            if (template != null)
            {
                if (emailInfoList != null)
                {                    
                    foreach (var item in emailInfoList)
                    {
                        EmailDetails email = new EmailDetails();
                        email.FromEmailID = item.AdminEmailID;
                        email.UserName = item.UserName;
                        email.EmailSubject = template.EmailSubject;
                        email.ToEmailID = item.EmailID;
                        email.ClientUserID = item.ClientUserID;
                        email.EmailBody = Convert.ToString(template.EmailBody)
                                    .Replace(AppEmailConstants.AtUsername, item.UserName)
                                    .Replace(AppEmailConstants.AtUserID, item.ClientUserID)
                                    .Replace(AppEmailConstants.AtManagerName, item.UserName)
                                    .Replace(AppEmailConstants.ApprovalManager, item.UserName)
                                    .Replace("@BeginUrlButton", "<a href='" + item.referenceUrl + "' style='font-family:Calibri;font-size:20px;background-color: #337ab7;color: white;border: 1px solid #2e6da4;text-decoration:none;padding:5px 20px;border-radius:5px;'>")
                                    .Replace("@EndUrlButton", "</a>");


                        emailDataList2.Add(email);
                    }
                }
                return emailDataList2;
            }   
            else
            {
                return emailInfoList;
            }
                
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
            return await m_communicationRepository.GetEmailNotificationExportData(appEmailID,userNum);
        }
        public async Task<DataTable> GetDashBoardMessageExportData(int appMessageId, int userNum)
        {
            return await m_communicationRepository.GetDashBoardMessageExportData(appMessageId, userNum);
        }

    }
}
