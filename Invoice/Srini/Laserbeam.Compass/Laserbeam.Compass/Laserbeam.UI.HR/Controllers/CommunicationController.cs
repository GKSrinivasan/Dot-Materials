
// Copyright (c) 2015 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.

// Component Name :   Communication 
// Description    :   Sending Email and Sending Dashboard Messages
// Author         :   Shaheena Shaik	
// Creation Date  :   12-April-2017

using Laserbeam.ProcessManager.Interfaces.Common;
using Laserbeam.UI.HR.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Laserbeam.BusinessObject.Common;
using Laserbeam.Constant.HR;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System.Threading.Tasks;

namespace Laserbeam.UI.HR.Controllers
{
    [AuthorizeUser(UserRoleConstants.AllAdminRoles)]
    public class CommunicationController : Controller
    {
        // GET: Communication
        #region Fields
        // Author         :   Shaheena Shaik	
        // Creation Date  :   12-April-2017  
        /// <summary>
        /// Object of ICommunicationProcessManager
        /// </summary>
        private SessionManager m_sessionManager;// = new SessionManager();
        private ICommunicationProcessManager m_communicationProcessManager;
        #endregion

        #region Constructors
        // Author         :  Shaheena Shaik
        // Creation Date  :  12-April-2017  
        /// <summary>
        /// Object of session manager
        /// </summary>
        public CommunicationController(ICommunicationProcessManager communicationProcessManager, SessionManager sessionManager)
        {
            m_communicationProcessManager = communicationProcessManager;
            m_sessionManager = sessionManager;
        }
        #endregion



        // Author         :  Shaheena Shaik
        // Creation Date  :  13-April-2017 
        /// <summary>
        /// Getting app email details from database
        /// </summary>
        /// <returns>Returning a view with a model of appEmail details</returns>
        [HttpGet]
        public ActionResult Home()
        {
            ViewBag.Division = "Communication";
            ViewBag.Title = "Manage Communication Template";
            var model = m_communicationProcessManager.GetAppEmailInfo();

            return View(model);
        }

        // Author         :  Shaheena Shaik
        // Creation Date  :  13-April-2017 
        /// <summary>
        /// Creating a partial view for _ManagerCommunicationTemplate section
        /// </summary>
        /// <returns>Returning a PartialView</returns>
        [HttpGet]
        public PartialViewResult _ManagerCommunicationTemplate()
        {
            return PartialView();
        }

        // Author         :  Shaheena Shaik
        // Creation Date  :  13-April-2017 
        /// <summary>
        /// Creating a partial view for EmailCommunication section
        /// </summary>
        /// <returns>Returning a PartialView</returns>
        [HttpGet]
        public ActionResult EmailCommunication()
        {
            return PartialView();
        }

        // Author         :  Shaheena Shaik
        // Creation Date  :  13-April-2017 
        /// <summary>
        /// Creating a partial view for dashboard message section
        /// </summary>
        /// <returns>Returning a PartialView</returns>
        [HttpGet]
        public ActionResult DashboardMessage()
        {
            return PartialView();
        }


        // Author         :  Shaheena Shaik
        // Creation Date  :  13-April-2017 
        /// <summary>
        /// Updating and Deleting the message from database
        /// </summary>
        /// <param name="EmailSubject">email Subject of the message</param>
        /// <param name="EmailBody">email Body of the message</param>
        /// <param name="AppEmailID">appEmail Id </param>
        /// <returns>Returning a partial view with a model</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxChildActionOnly]
        public PartialViewResult SaveAndDeleteCommunicationEmail(string EmailSubject, string EmailBody, int AppEmailID = 0)
        {
            UserModel userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
            if (EmailSubject == null || EmailBody == null)
            {
                m_communicationProcessManager.DeleteEmail(AppEmailID);
            }
            else
            {
                var message = m_communicationProcessManager.PutCommunicationEmail(EmailSubject, EmailBody, AppEmailID, userModel.UserNum);
            }
            var model = m_communicationProcessManager.GetAppEmailInfo();
            return PartialView(model);
        }

        // Author         :  Shaheena Shaik
        // Creation Date  :  13-April-2017 
        /// <summary>
        /// Getting values for binding manage communication tre view from database
        /// </summary>
        /// <returns>returning the json object with tree view details</returns>
        [HttpPost, ValidateAntiForgeryToken, AjaxChildActionOnly]
        public JsonResult BindManageCommunicationTreeView()
        {
            var model = m_communicationProcessManager.GetBindManageCommunicationTreeView().ToList();
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        // Author         :  Shaheena Shaik
        // Creation Date  :  17-April-2017
        /// <summary>
        /// Getting Dropdown values in Dashboard Messages section
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxChildActionOnly]
        public JsonResult GetDropdownValues()
        {
            var detail = m_communicationProcessManager.GetDashboardMessage(null).Select(x => new SelectMessageItem
            {
                Text = x.MessageSubject,
                Value = x.AppMessageID.ToString()
            }).ToList();
            return Json(detail, JsonRequestBehavior.AllowGet);
        }

        // Author         :  Shaheena Shaik
        // Creation Date  :  17-April-2017
        /// <summary>
        /// Getting UserRole TreeView Values from database
        /// </summary>
        /// <returns>Returning a json object of Roles </returns>
        [HttpPost, ValidateAntiForgeryToken, AjaxChildActionOnly]
        public JsonResult SendDashboardMessageSearchCriteriaTree()
        {
            UserModel userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
            var result = m_communicationProcessManager.GetSendDashboardMessageSearchCriteriaTree(userModel.UserNum);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        // Author         :  Shaheena Shaik
        // Creation Date  :  17-April-2017
        /// <summary>
        /// Getting Message Subject and Message Body of related Message Id from database
        /// </summary>
        /// <param name="messageId">Id of a message</param>
        /// <returns>Returning json object of Message subject and Message Body</returns>
        [HttpPost, ValidateAntiForgeryToken, AjaxChildActionOnly]
        public JsonResult GetDropdown(int? messageId)
        {
            var detail = m_communicationProcessManager.GetDashboardMessage(messageId);
            var ddlEmailSubject = detail.FirstOrDefault().MessageSubject;
            var divEditorbody = HttpUtility.HtmlDecode(Convert.ToString(detail.FirstOrDefault().MessageBody));
            return Json(new { MessageSubject = ddlEmailSubject, MessageBody = divEditorbody }, JsonRequestBehavior.AllowGet);
        }

        // Author         :  Shaheena Shaik
        // Creation Date  :  17-April-2017
        /// <summary>
        /// Binding the values of Manage Communication Template tree view
        /// </summary>
        /// <returns>Returning the json object of the data for Manage Communication Template tree view</returns>
        [HttpPost, ValidateAntiForgeryToken, AjaxChildActionOnly]
        public JsonResult BindManageCommunicationTemplateTreeView()
        {
            var model = m_communicationProcessManager.GetBindManageCommunicationTreeView().ToList();
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        // Author         :  Shaheena Shaik
        // Creation Date  :  17-April-2017
        // NOTE : Boobalan: Completed passing Anti-ForgeryToken
        /// <summary>
        /// Update or Deleting email from database
        /// </summary>
        /// <param name="EmailSubject">EmailSubject of the mail</param>
        /// <param name="EmailBody">Body</param>
        /// <param name="AppEmailID">AppEmailID</param>
        /// <returns>Returning the partial view with a model object</returns>
        [HttpPost]
        [AjaxChildActionOnly]
        [ValidateAntiForgeryToken]
        public PartialViewResult _ManagerCommunicationTemplate(string EmailSubject, string EmailBody, int AppEmailID = 0)
        {
            EmailBody = HttpUtility.HtmlDecode(Convert.ToString(EmailBody));
            UserModel userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
            if (EmailSubject == null || EmailBody == null)
            {
                m_communicationProcessManager.DeleteEmail(AppEmailID);
            }
            else
            {
                var message = m_communicationProcessManager.PutCommunicationEmail(EmailSubject, EmailBody, AppEmailID, userModel.UserNum);
            }
            var model = m_communicationProcessManager.GetAppEmailInfo();
            return PartialView(model);
        }


        // Author         :  Shaheena Shaik
        // Creation Date  :  18-April-2017
        /// <summary>
        /// Getting UserGrid Values based on User Role from database
        /// </summary>
        /// <param name="request"></param>
        /// <param name="checkRole">Checking whether role exists or not</param>
        /// <param name="userRoleNum">User Role Num of a Selected role</param>
        /// <returns>Returning a json object of User Grid values</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxChildActionOnly]
      public JsonResult GetUserGridValues([DataSourceRequest] DataSourceRequest request, string checkRole, int userRoleNum)
      {
          UserModel userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
          var gridData = m_communicationProcessManager.GetGridValues(checkRole == "true" ? true : false, userRoleNum, userModel.UserNum);
          return Json(gridData.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        // Author         :  Shaheena Shaik
        // Creation Date  :  18-April-2017
        /// <summary>
        /// Updating or Creating a DashBoard Message into the database
        /// </summary>
        /// <param name="messageSubject">Subject of the message</param>
        /// <param name="messageBody">Body of the message</param>
        /// <param name="messageNum">messageNum</param>
        /// <returns>Returning the messsage</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxChildActionOnly]
      public string UpdateOrCreateMessage(string messageSubject, string messageBody, int messageNum)
      {
          UserModel userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
          var message = m_communicationProcessManager.UpdateOrCreateMessage(messageSubject, messageBody, messageNum, userModel.UserNum);
          return message;
      }
        // Author         :  Shaheena Shaik
        // Creation Date  :  18-April-2017
        /// <summary>
        /// Deleting dashboard message from database
        /// </summary>
        /// <param name="AppMessageID">AppMessageID</param>
        /// <param name="messageId">Id of the messsage</param>
        /// <returns>Returning the json object</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxChildActionOnly]
        public JsonResult DeleteDashboardMessage(int AppMessageID, int? messageId)
      {
          m_communicationProcessManager.DeleteDashboardMessage(AppMessageID);
          var result = "success";
          return Json(result, JsonRequestBehavior.AllowGet);
      }


        // Author         :  Shaheena Shaik
        // Creation Date  :  18-April-2017
        /// <summary>
        /// updating a Dashboard message into the database
        /// </summary>
        /// <param name="userMessage">message details</param>
        /// <returns>Returning comfirmation message</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxChildActionOnly]
        public JsonResult SendDashboardMessage(List<UserMessageModel> userMessage)
        {
            UserModel userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);

            string message = m_communicationProcessManager.SendDashboardMessage(userMessage, userModel.UserNum);
            if (message.Trim() == "")
                message = "Message Sent successfully";
            else
                message = "Message updation failed";
            return Json(message);
        }

        // Author         :  Shaheena Shaik
        // Creation Date  :  18-April-2017
        /// <summary>
        /// Getting DashboardMessage Filter/Sort data from database
        /// </summary>
        /// <returns>Returning a list</returns>
        [HttpGet]
      public PartialViewResult _DashboardMessageFilterSort()
      {
          List<SelectListItem> list = new List<SelectListItem>();
          //list.Add(new SelectListItem { Text = "User ID", Value = "UserID" });
          list.Add(new SelectListItem { Text = "User Name", Value = "UserName" });
          list.Add(new SelectListItem { Text = "Email Address", Value = "EmailID" });
            return PartialView(list);
      }
        
        /// <summary>
        /// Getting data for binding Email Notification Roles Tree view
        /// </summary>
        /// <returns>Returning a Json object of data fro tree view</returns>
        [HttpPost,ValidateAntiForgeryToken,AjaxChildActionOnly]
      public JsonResult _SendEmailNotificationTree()
      {
          UserModel userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
          var result = m_communicationProcessManager.GetSendDashboardMessageSearchCriteriaTree(userModel.UserNum).ToList();
          return Json(result, JsonRequestBehavior.AllowGet);
      }

        // Author         :  Shaheena Shaik
        // Creation Date  :  18-April-2017
        //NOTE: Boobalan: Completed passing Anti-ForgeryToken
        /// <summary>
        /// Sending Emailnotification to users
        /// </summary>
        /// <param name="request"></param>
        /// <param name="emailInfoList">Model of Email Details</param>
        /// <param name="template">Body of the mail</param>
        /// <param name="templateID">id of the email</param>
        /// <param name="subject">Email Subject</param>
        /// <returns></returns>
        [HttpPost]
        [AjaxChildActionOnly]
        [ValidateAntiForgeryToken]
        public JsonResult GetEmailDetails([DataSourceRequest] DataSourceRequest request, List<EmailDetails> emailInfoList, int? templateID, string subject)
        {
            AppConfigModel appConfigModel = m_communicationProcessManager.GetAppSetting();
            UserModel userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
            string requestURL = this.GetTenantUrl();
            List<EmailDetails> sentUpdate = m_communicationProcessManager.EmailConfiguration(emailInfoList, templateID ?? 0, appConfigModel, userModel.UserNum, requestURL);
            var successCount = sentUpdate.Where(x => x.Success == true).Count();
            var totalCount = emailInfoList.Count();
            var msg = "Success";

            if (successCount == 0)
            { msg = "Email sending Failed"; }
            else
            {
                msg = "Email Sent Successfully for " + successCount + "/" + totalCount;
            }
            return Json(new { Data = sentUpdate, Message = msg },JsonRequestBehavior.AllowGet);
        }

        // Author         :  Shaheena Shaik
        // Creation Date  :  18-April-2017
        /// <summary>
        /// Getting EmailNotification Filter/Sort data from database
        /// </summary>
        /// <returns>Returning a list</returns>
        [HttpGet]
      public PartialViewResult _EmailNotificationFilterSort()
      {
          List<SelectListItem> list = new List<SelectListItem>();
          list.Add(new SelectListItem { Text = "User Name", Value = "UserName" });
          list.Add(new SelectListItem { Text = "Email Address", Value = "EmailID" });
          return PartialView(list);
      }

        // Author       : Shaheena Shaik
        //Creation Date :22/April-2017
        /// <summary>
        /// Exporting the user details 
        /// </summary>
        /// <param name="appEmailID">app Email id which is passing as a parameter to the stored procedure</param>
        /// <param name="userNum">logged in user</param>
        /// <returns>Returning the export details</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public async Task GetEmailNotificationExportData(int appEmailID)
        {
            UserModel userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
            var exportData = await m_communicationProcessManager.GetEmailNotificationExportData(appEmailID, userModel.UserNum);
            ExportExcel.ToExcel(exportData, " Email Notification-Export");
        }
        // Author       : Shaheena Shaik
        //Creation Date :23/April-2017
        /// <summary>
        /// Exporting the user details in Dashboard Message page 
        /// </summary>
        /// <param name="appEmailID">app Email id which is passing as a parameter to the stored procedure</param>
        /// <param name="userNum">logged in user</param>
        /// <returns>Returning the export details</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task GetDashboardMessageExportData(int appMessageId)
        {
            UserModel userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
            var exportData = await m_communicationProcessManager.GetDashBoardMessageExportData(appMessageId, userModel.UserNum);
            ExportExcel.ToExcel(exportData, " Dashboard Messages-Export");
        }
        
    }
}