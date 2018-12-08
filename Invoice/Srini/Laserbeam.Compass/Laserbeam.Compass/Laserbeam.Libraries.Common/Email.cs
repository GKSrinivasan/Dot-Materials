
// Copyright (c) 2015 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.

// Component Name  : 	Praveenkumar Selvaraj
// Description     : 	Email utility class
// Author          :	Praveenkumar Selvaraj
// Creation Date   : 	APR-13-2015
using System;
using System.Collections.Generic;
using Laserbeam.Libraries.Interfaces.Common;
using System.Net.Mail;
using Laserbeam.BusinessObject.Common;
using System.Threading.Tasks;
using Laserbeam.EntityManager.Common;

namespace Laserbeam.Libraries.Common
{
    public class Email : IEmail
    {
        #region Public Fields

        public string[] mailContent;

        #endregion

        #region Private Fields

        private readonly SmtpClient emailClient = new SmtpClient();

        #endregion

        #region Public Methods

        public Int16 SendEmail(AppConfigModel appConfigModel, EmailDetails emailDetailList, bool isBodyHtml)
        {
            try
            {
                var eMailMessage = new MailMessage();
                var fromAddressEmailID = new MailAddress(appConfigModel.AdminEmailID);
                eMailMessage.From = fromAddressEmailID;
                configureEmailSettings(appConfigModel);
                eMailMessage.To.Add(emailDetailList.ToEmailID);
                if (emailDetailList.EmailCC != null)
                {
                    string[] cc = emailDetailList.EmailCC.Split(',');
                    foreach (var s in cc)
                    {
                        if (s != "")
                            eMailMessage.CC.Add(s);
                    }
                }
                eMailMessage.Subject = emailDetailList.EmailSubject;
                eMailMessage.Body = emailDetailList.EmailBody;
                eMailMessage.IsBodyHtml = isBodyHtml;
                eMailMessage.Priority = MailPriority.Normal;
                emailClient.Send(eMailMessage);
                eMailMessage.Dispose();
            }
            catch
            {
                return 2;
            }
            return 1;
        }

        public Int16 SendEmail(AppConfigModel appConfigModel, List<EmailDetails> emailDetailList, bool isBodyHtml)
        {
            try
            {
                var eMailMessage = new MailMessage();
                var fromAddressEmailID = new MailAddress(appConfigModel.AdminEmailID);
                eMailMessage.From = fromAddressEmailID;
                configureEmailSettings(appConfigModel);
                foreach (var email in emailDetailList)
                {
                    eMailMessage.To.Add(email.ToEmailID);
                    if (email.EmailCC != null)
                    {
                        string[] cc = email.EmailCC.Split(',');
                        foreach (var s in cc)
                        {
                            if (s != "")
                                eMailMessage.CC.Add(s);
                        }
                    }
                    eMailMessage.Subject = email.EmailSubject;
                    eMailMessage.Body = email.EmailBody;
                    eMailMessage.IsBodyHtml = isBodyHtml;
                    eMailMessage.Priority = MailPriority.Normal;
                }
                emailClient.Send(eMailMessage);
                eMailMessage.Dispose();
            }
            catch
            {
                return 2;
            }
            return 1;
        }

        public List<EmailTracker> SendEmail(List<EmailDetails> emailContentDetails, int templateID, AppConfigModel appConfigModel, int userNum, bool isBodyHtml)
        {
            List<EmailTracker> emailTrack = new List<EmailTracker>();
            configureEmailSettings(appConfigModel);
            foreach (var email in emailContentDetails)
            {

                EmailTracker emailTrackerObj = new EmailTracker();
                try
                {
                    var eMailMessage = new MailMessage();
                    eMailMessage.From = new MailAddress(email.FromEmailID);
                    eMailMessage.To.Add(email.ToEmailID);
                    eMailMessage.Subject = email.EmailSubject;
                    eMailMessage.Body = email.EmailBody;
                    eMailMessage.IsBodyHtml = isBodyHtml;
                    eMailMessage.Priority = MailPriority.Normal;
                    emailClient.Send(eMailMessage);
                    emailTrackerObj.ErrorDescription = null;
                    emailTrackerObj.Success = true;

                }
                catch (Exception ex)
                {
                    emailTrackerObj.ErrorDescription = ex.ToString();
                    emailTrackerObj.Success = false;
                }
                emailTrackerObj.AppEmailID = templateID;
                emailTrackerObj.ClientUserID = email.ClientUserID;
                emailTrackerObj.FromEmail = email.FromEmailID;
                emailTrackerObj.UpdatedDate = DateTime.Now;
                emailTrackerObj.ToEmail = email.ToEmailID;
                emailTrackerObj.UpdatedBy = userNum;
                emailTrack.Add(emailTrackerObj);
            }
            return emailTrack;
        }

        public async Task<Int16> SendEmailAsync(AppConfigModel appConfigModel, EmailDetails emailDetailList, bool isBodyHtml)
        {
            try
            {
                var eMailMessage = new MailMessage();
                var fromAddressEmailID = new MailAddress(appConfigModel.AdminEmailID);
                eMailMessage.From = fromAddressEmailID;
                configureEmailSettings(appConfigModel);
                eMailMessage.To.Add(emailDetailList.ToEmailID);
                if (emailDetailList.EmailCC != null)
                {
                    string[] cc = emailDetailList.EmailCC.Split(',');
                    foreach (var s in cc)
                    {
                        if (s != "")
                            eMailMessage.CC.Add(s);
                    }
                }
                eMailMessage.Subject = emailDetailList.EmailSubject;
                eMailMessage.Body = emailDetailList.EmailBody;
                eMailMessage.IsBodyHtml = isBodyHtml;
                eMailMessage.Priority = MailPriority.Normal;
                //emailClient.Send(eMailMessage);
                await emailClient.SendMailAsync(eMailMessage);
                eMailMessage.Dispose();
            }
            catch
            {
                return 2;
            }
            return 1;
        }

        public async Task<Int16> SendEmailAsync(AppConfigModel appConfigModel, List<EmailDetails> emailDetailList, bool isBodyHtml)
        {
            try
            {
                var eMailMessage = new MailMessage();
                var fromAddressEmailID = new MailAddress(appConfigModel.AdminEmailID);
                eMailMessage.From = fromAddressEmailID;
                configureEmailSettings(appConfigModel);
                foreach (var email in emailDetailList)
                {
                    eMailMessage.To.Add(email.ToEmailID);
                    if (email.EmailCC != null)
                    {
                        string[] cc = email.EmailCC.Split(',');
                        foreach (var s in cc)
                        {
                            if (s != "")
                                eMailMessage.CC.Add(s);
                        }
                    }
                    eMailMessage.Subject = email.EmailSubject;
                    eMailMessage.Body = email.EmailBody;
                    eMailMessage.IsBodyHtml = isBodyHtml;
                    eMailMessage.Priority = MailPriority.Normal;
                }
                await emailClient.SendMailAsync(eMailMessage);
                eMailMessage.Dispose();
            }
            catch
            {
                return 2;
            }
            return 1;
        }

        #endregion

        #region Private Methods

        private void configureEmailSettings(AppConfigModel appConfigModel)
        {
            System.Net.NetworkCredential myCredential = new System.Net.NetworkCredential(appConfigModel.AdminUserID, appConfigModel.AdminPassword);
            emailClient.Host = appConfigModel.SMTPServer;
            emailClient.Port = Convert.ToInt32(appConfigModel.SMTPPort);
            emailClient.EnableSsl = true;
            emailClient.UseDefaultCredentials = false;
            emailClient.Credentials = myCredential;
            emailClient.ServicePoint.MaxIdleTime = 1;
        }

        #endregion
    }
}
