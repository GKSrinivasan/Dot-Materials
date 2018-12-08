// Copyright (c) 2015 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.

// Component Name :   ICommunicationRepository
// Description    :   Interface signature for CommunicationRepository
// Author         :   Shaheena Shaik
// Creation Date  :   18-April-2017

using Laserbeam.BusinessObject.Common;
using Laserbeam.EntityManager.Common;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Laserbeam.DataManager.Interfaces.Common
{
    public interface ICommunicationRepository
    {
        // Author       : Shaheena Shaik
        //Creation Date : 18-April-2017
        /// <summary>
        /// Getting a data for binding ManageCommunication Tree view
        /// </summary>
        /// <returns>Returning a tree view data as a query</returns>
        IQueryable<ManageCommunicationTemplate> GetBindManageCommunicationTreeView();

        // Author       : Shaheena Shaik
        //Creation Date :18/April-2017
        /// <summary>
        /// Getting appEmailDetails from database to display onto the tree view 
        /// </summary>
        /// <returns>Returning a list of AppEmail data</returns>
        IQueryable<AppEmail> GetAppEmailInfo();

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
        string GetEmailOrder();
        string PutEmail(AppEmail appEmail, int userNum);

        // Author       : Shaheena Shaik
        //Creation Date :18/April-2017
        /// <summary>
        /// Getting Email from the database to get Email data
        /// </summary>
        /// <param name="appMailID">appEmail id of the Email which we choosen from tree</param>
        /// <returns></returns>
        List<AppEmail> GetEmail(int appMailID);

        // Author       : Shaheena Shaik
        //Creation Date :18/April-2017
        /// <summary>
        /// deleting an Email from database
        /// </summary>
        /// <param name="appMailID">Id of the mail which we are going to delete</param>
        void DeleteEmail(int appMailID);

        // Author       : Shaheena Shaik
        //Creation Date :18/April-2017
        /// <summary>
        /// Getting list of message Data from the databse to display onto the tree view
        /// </summary>
        /// <param name="messageId">Id of the message to get the required data </param>
        /// <returns>returning a list of message details </returns>
        List<AppMessage> GetDashboardMessage(int? messageId);

        // Author       : Shaheena Shaik
        //Creation Date :18/April-2017
        /// <summary>
        /// Getting data for tree view from database
        /// </summary>
        /// <param name="userNum">userNum of user who is loggedIn</param>
        /// <returns>Returning Tree view data as a query</returns>
        IQueryable<DashboardSearchCriteriaTree> GetSendDashboardMessageSearchCriteriaTree(int userNum);

        // Author       : Shaheena Shaik
        //Creation Date :18/April-2017
        /// <summary>
        /// Getting User Grid Data from database
        /// </summary>
        /// <param name="checkRole">Checking whether the Role is present or not</param>
        /// <param name="userRoleNum">UserRoleNum of a user</param>
        /// <param name="userNum">userNum of a user who is LoggedIn</param>
        /// <returns>Returning a query of data which is required to bind to the user grid</returns>
        IQueryable<MessageModel> GetGridValues(bool checkRole, int userRoleNum, int userNum);

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
        string UpdateOrCreateMessage(string messageSubject, string messageBody, int messageNum, int userNum);

        // Author       : Shaheena Shaik
        //Creation Date :18/April-2017
        /// <summary>
        /// Deleting a dashboard message from database
        /// </summary>
        /// <param name="AppMessageID">messageId to delete</param>
        void DeleteDashboardMessage(int AppMessageID);

        // Author       : Shaheena Shaik
        //Creation Date :18/April-2017
        /// <summary>
        /// Sending a dashboard message to get updated into the database
        /// </summary>
        /// <param name="userMessage">message details</param>
        /// <param name="userNum">usernum of who is sending the message</param>
        /// <returns>returning a string message</returns>
        string SendDashboardMessage(List<UserMessageModel> userMessage, int userNum);
        AppConfigModel GetAppSetting();

        // Author       : Shaheena Shaik
        //Creation Date :22/April-2017
        /// <summary>
        /// Exporting the user details 
        /// </summary>
        /// <param name="appEmailID">app Email id which is passing as a parameter to the stored procedure</param>
        /// <param name="userNum">logged in user</param>
        /// <returns>Returning the export details</returns>
        Task<DataTable> GetEmailNotificationExportData(int appEmailID, int userNum);
        AppEmail GetEmailTemplate(int emailID);
        
        Task<DataTable> GetDashBoardMessageExportData(int appMessageId, int userNum);
        void emailTracker(List<EmailTracker> emailTrackerObj);
        void PutUserAppURlTracker(string ClientUserID, string SecretKey, string RequestTimeStamp, string EncryptedMagicKey, string SiteUrl, bool IsEncrypted);

        void PutSuccessStatus(bool SuccessStatus, string userId);

    }
}
