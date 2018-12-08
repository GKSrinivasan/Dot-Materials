// Copyright (c) 2014 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.

// Component Name :   Email
// Description    :   All Business logic related to Email is placed here 	
// Author         :   Thiyagu
// Creation Date  :   04-11-2014

using Laserbeam.BusinessObject.Common;
using Laserbeam.Constant.HR;
using Laserbeam.DataManager.Interfaces.Common;
using Laserbeam.EntityManager.Common;
using Laserbeam.Libraries.Interfaces.Common;
using Laserbeam.ProcessManager.Interfaces.Common;

namespace Laserbeam.ProcessManager.HR.Common
{
    public class EmailProcessManager : IEmailProcessManager
    {
        #region Fields
        IEmailDetailsRepository m_emailDetailsRepository;
        IAppUserRepository m_accountRepository;
        ISessionRepository m_sessionRepository;
        IEmail m_email;
        #endregion

        #region Constructors
        public EmailProcessManager(IEmailDetailsRepository emailDetailsRepository, IEmail email, IAppUserRepository accountRepository, ISessionRepository sessionRepository)
        {
            m_emailDetailsRepository = emailDetailsRepository;
            m_email = email;
            m_accountRepository = accountRepository;
            m_sessionRepository = sessionRepository;
        }
        #endregion

        #region Implements
        
        
        
        
        // Author        :  Priya		
        // Creation Date :  12-2-2014        
        /// <summary>
        /// To send email to the user on click of forgot password link and update userdetails 
        /// </summary>
        /// <param name="appConfigModel">appConfigModel object in session</param>
        /// <param name="user">Usermodel object in session</param>
        /// <param name="userId">UserID for which the mail is to be sent</param>
        /// <returns>sendStatus flag</returns>
        public int ForgotPassword(string toEmailID, string userName, string defaultPassword)
        {
            int sendStatus = 0;
            AppConfigModel appConfigModel = m_accountRepository.GetAppSetting();
            string sMTPServer = appConfigModel.SMTPServer;
            EmailDetails emailDetails = mailDetailsBasedOnForgotPassword(toEmailID, userName, appConfigModel, defaultPassword);
            if (emailDetails != null)
            {                
                sendStatus = m_email.SendEmail(appConfigModel, emailDetails, true);
            }
            return sendStatus;
        }
        #endregion

        #region Private Methods

        
        
        // Author        :  Priya		
        // Creation Date :  12-2-2014       
        /// <summary>
        /// Gets the email details to send email to user 
        /// </summary>
        /// <param name="user">Usermodel object in session</param>
        /// <returns>EmailDetails</returns>
        private EmailDetails mailDetailsBasedOnForgotPassword(string toEmailID, string userName, AppConfigModel appConfigModel, string defaultPassword)
        {
            AppEmail appEmailInfo = m_emailDetailsRepository.GetAppEmailDetails(AppEmailConstants.ForgetPasswordReset);
            if (appEmailInfo != null)
            {
                EmailDetails emailDetails = new EmailDetails();                
                emailDetails.FromEmailID = appConfigModel.AdminEmailID;
                emailDetails.ToEmailID = toEmailID;
                emailDetails.UserName = userName;
                emailDetails.DefaultPassword = defaultPassword;
                emailDetails.EmailSubject = appEmailInfo.EmailSubject.Replace(AppEmailConstants.AtManagerName, emailDetails.ManagerName);
                emailDetails.EmailBody = appEmailInfo.EmailBody.Replace(AppEmailConstants.AtUsername, emailDetails.UserName).Replace(AppEmailConstants.AtDefaultPassword, defaultPassword);
                return emailDetails;
            }
            return null;
        }
        

        
        #endregion

    }
}
