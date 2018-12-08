// Copyright (c) 2015 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.
// Component Name  :    AccountProcessManager
// Description     : 	Login related process manager.	
// Author          :	Roopan		
// Creation Date   : 	APR-01-2015
using Laserbeam.BusinessObject.Common;
using Laserbeam.Constant.HR;
using Laserbeam.DataManager.Interfaces.Common;
using Laserbeam.EntityManager.Common;
using Laserbeam.Libraries.Common;
using Laserbeam.Libraries.Interfaces.Common;
using Laserbeam.ProcessManager.Interfaces.Common;
using System;
using System.Threading.Tasks;
using System.Web;

namespace Laserbeam.ProcessManager.Common
{
    public class AccountProcessManager : IAccountProcessManager
    {
        #region Fields
        private readonly IAppUserRepository m_appUserRepository;
        private readonly ISessionRepository m_sessionRepository;
        private readonly IKeyGenerator m_keyGenerator;
        private readonly IEmail m_email;
        private readonly IEmailProcessManager m_emailProcessManager;
        private readonly IPasswordEncryption m_passwordEncryption;
        #endregion
        #region Constructor
        // Author         :  Raja Ganapathy
        // Creation Date  :  05-Jul-2016  
        /// <summary>
        /// Initializes objects used in this class
        /// </summary>
        /// <param name="appUserRepository">Denotes appuser repository</param>
        /// <param name="KeyGenerator">Denotes KeyGenerator repository</param>
        /// <param name="email">Denotes email repository</param>
        /// <param name="sessionRepository">Denotes sessionRepository repository</param>
        /// <param name="emailProcessManager">Denotes emailProcessManager repository</param>
        public AccountProcessManager(IAppUserRepository appUserRepository, IKeyGenerator KeyGenerator, IEmail email, ISessionRepository sessionRepository, IEmailProcessManager emailProcessManager,IPasswordEncryption encryptionLibrary)
        {
            m_appUserRepository = appUserRepository;
            m_keyGenerator = KeyGenerator;
            m_email = email;
            m_sessionRepository = sessionRepository;
            m_emailProcessManager = emailProcessManager;
            m_passwordEncryption = encryptionLibrary;
        }
        #endregion
        #region Public Methods Implementation
        // Author        :  Roopan		
        // Creation Date :  APR-03-2015
        /// <summary>
        /// Get the user details based on user id
        /// </summary>
        /// <param name="userID">ID of user</param>
        /// <returns></returns>
        public UserModel GetUser(string userID)
        {
            var appUser = m_appUserRepository.GetUserDataModel(userID);
            return appUser;
        }

       
        // Modified By    :  N Thuishithaa
        // Modified Date  :  04-August-2016  
        // Reviewed By    :  Praveen Kumar
        // Reviewed Date  :  20-August-2016 
        // Comment        :  Removed resetting user credential concept before resetting user credential 
        public UserCredentialStatus ResetUserCredential(string userId, AppConfigModel appConfig)
        {
            int? loginAttempt = appConfig.LoginAttempt;//m_appUserRepository.GetloginAttempt();
            var user = m_appUserRepository.GetUser(userId);
            UserCredentialStatus userStatus;
            if (user == null || user.MarkAsDelete == true)
            {
               return userStatus = UserCredentialStatus.InvalidUser;
            }
            userStatus = validateUser(user, appConfig);
            if (userStatus == UserCredentialStatus.InActive && user.FailedLoginAttempts >= loginAttempt)
            {
                return userStatus = UserCredentialStatus.Locked;
            }
            return userStatus;
        }
        // Modified By    :  Hari
        // Modified Date  :  05-10-2017 
        // Comment        :  User Validation 
        public bool UserValidation(string userId)
        {
            var user = m_appUserRepository.GetUser(userId);
            return (user == null) ? false : true;
        }
        public AppConfigModel GetAppSetting()
        {
            return m_appUserRepository.GetAppSetting();
        }
        // Modified By    :  Pulithevan Stalin
        // Modified Date  :  05-10-2016 
        // Reviewed By    :  Sanjeeviram Kamalakannan
        // Reviewed Date  :  05-10-2016 
        // Comment        :  Put the inActive key 
        public UserCredentialStatus ValidateUserCredential(string userId, string password, AppConfigModel appConfig)
        {
            int? loginAttempt = appConfig.LoginAttempt; //m_appUserRepository.GetloginAttempt();
            var user = m_appUserRepository.GetUser(userId);
            UserCredentialStatus userStatus;
            if (user == null || user.MarkAsDelete == true)
            {
               return  userStatus = UserCredentialStatus.InvalidUser;
            }
            userStatus = validateUser(user, appConfig);
            if (userStatus == UserCredentialStatus.Valid)
            {
                return userStatus = validatePassword(user, password, appConfig.LoginAttempt);
            }
            if (userStatus == UserCredentialStatus.InActive || userStatus == UserCredentialStatus.Locked)
            {
                addFailedLoginAttempts(user, appConfig.LoginAttempt);
                return userStatus = (user.FailedLoginAttempts >= loginAttempt) ? UserCredentialStatus.Locked : UserCredentialStatus.InActive;
            }
            return userStatus;
        }
        // Modified By    :  N Thuishithaa
        // Modified Date  :  04-August-2016 
        // Reviewed By    :  Praveen Kumar
        // Reviewed Date  :  20-August-2016 
        // Comment        :  Get lockeddate of loggedin user
        public DateTime? GetLockedDate(string userId)
        {
            return m_appUserRepository.GetLockedDate(userId);
        }
        // Author         :  Raja Ganapathy
        // Creation Date  :  05-Jul-2016 
        /// <summary>
        /// Insert the user logged details in table
        /// </summary>
        /// <param name="userNum">Denotes the user num</param>
        /// <param name="isSSOLogin">Denotes the SSO Login</param>
        public void PushUserLogoutDetails(int userNum, bool isSSOLogin)
        {
            m_appUserRepository.PutUserLogoutDetails(userNum, isSSOLogin);
        }

        public void PutUserLogInDetails(int userNum)
        {
            m_appUserRepository.PutUserLogInDetails(userNum);
        }
        // Modified By    :  N Thuishithaa
        // Modified Date  :  04-August-2016 
        // Reviewed By    :  Praveen Kumar
        // Reviewed Date  :  20-August-2016 
        // Comment        :  Update failedloggedin attempts and appuserstatus after changing the password
        // Modified By    :  N Thuishithaa
        // Modified Date  :  12-Oct-2016  
        // Reviewed By    :  Hari
        // Reviewed Date  :  12-Oct-2016
        // Comment        :  Added condition to validateSecretKey if userStatus is inActive and FailedLoginAttempts greater than loginAttempt        
        public UserCredentialStatus ValidateUserSecretKey(string userId, string secretKey, string requestTimeStamp, AppConfigModel appConfig, string mayaLink = null)
        {
            DateTime epochStart = new DateTime(1970, 01, 01, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan currentTs = DateTime.UtcNow - epochStart;
            var serverTotalSeconds = Convert.ToUInt64(currentTs.TotalSeconds);
            UserCredentialStatus userStatus;
            int? loginAttempt = appConfig.LoginAttempt; //m_appUserRepository.GetloginAttempt();
            if (!string.IsNullOrWhiteSpace(requestTimeStamp) && (serverTotalSeconds - Convert.ToUInt64(requestTimeStamp)) > 86400)
            {
                userStatus = UserCredentialStatus.SecretKeyTimeExpired;
            }
            else
            {
                var user = m_appUserRepository.GetUser(userId);
                if (user == null && mayaLink != null)
                {
                    throw new ApplicationException("Hi, It looks like you have already created an account.");
                } 
                if (user==null)
                {
                    throw new ApplicationException("Tenant Not Found");
                }
                if(user.MarkAsDelete == true)
                {
                    userStatus = UserCredentialStatus.InvalidUser;
                }
                else
                {
                    userStatus = validateUser(user, appConfig);
                    if (userStatus == UserCredentialStatus.Valid)
                    {
                        userStatus = validateSecretKey(user, secretKey);
                    }
                    else if (userStatus == UserCredentialStatus.InActive && user.FailedLoginAttempts >= loginAttempt)
                    {
                        userStatus = validateSecretKey(user, secretKey);
                    }
                }
               
            }
            return userStatus;
        }
        // Modified By    : Karthikeyan Shanmugam
        // Modified Date  : 14-Feb-2017
        // Reviewed By    : Hariharasubramaniyan
        // Reviewed Date  : 14-Feb-2017
        // comments       : To show the cilent time based on UTC time it shows depend upon the client location or time zone
        public UserCredentialStatus ChangeUserCredential(string userId, string newPassword, AppConfigModel appConfig)
        {
            var user = m_appUserRepository.GetUser(userId);
            UserCredentialStatus userStatus = validateUser(user, appConfig);
            var localTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.Local);
            if (user == null || user.MarkAsDelete == true)
            {
                return userStatus = UserCredentialStatus.InvalidUser;
            }
            if (userStatus == UserCredentialStatus.InActive || userStatus == UserCredentialStatus.ValidFirstTime || userStatus == UserCredentialStatus.Valid)
            {
                    int appUserStatusID = m_appUserRepository.GetAppUserStatusId("Active");
                    user.AppUserStatusID = appUserStatusID;
                    user.SecretKey = Guid.NewGuid().ToString();
                    user.UserPassword = m_passwordEncryption.EncryptPassword(newPassword);
                    user.LastLoginDt = localTime;
                    user.FailedLoginAttempts = 0;
                    user.LockedDate = null;
                    if (m_appUserRepository.UpdateUser(user))
                    return userStatus = UserCredentialStatus.PasswordChanged;
            }
            return userStatus;
        }
        public string GetPasswordMode()
        {
            return m_appUserRepository.GetPasswordMode();
        }
        // Modified By    :   N Thuishithaa		
        // Modified Date  :   15-10-2016
        // Reviewed By    :   Hari		
        // Reviewed Date  :   15-10-2016
        // Comment        :   Changed color of login button in email
        public async Task<int> SendEmailToUser(UserModel manageUser, string templateName, string requestURL)
        {
            int mailSentStatus = 0;
            AppEmail newUserEmailTemplateNumber = m_appUserRepository.GetEmailTemplate(templateName);
            EmailDetails emailDetails = new EmailDetails();

            string siteUrl = requestURL;
            bool isEncrypted = false;
            bool isSingleUse = (templateName == "Forget Password Reset");
            string encryptedMagicKey = string.Empty;
            if (!(string.IsNullOrWhiteSpace(manageUser.UserID) || string.IsNullOrWhiteSpace(manageUser.SecretKey)))
            {
                DateTime epochStart = new DateTime(1970, 01, 01, 0, 0, 0, 0, DateTimeKind.Utc);
                TimeSpan timeSpan = DateTime.UtcNow - epochStart;
                string requestTimeStamp = (isSingleUse) ? "" : Convert.ToUInt64(timeSpan.TotalSeconds).ToString();
                isEncrypted = MayaLink.TryEncrypt(manageUser.UserID + '|' + manageUser.SecretKey + '|' + requestTimeStamp, out encryptedMagicKey);
            }
            
            string referenceUrl = isEncrypted ? siteUrl + "?mayaLink=" + HttpUtility.UrlEncode(encryptedMagicKey)  : siteUrl;
            if (newUserEmailTemplateNumber != null)
            {
                string Password = string.Empty;
                emailDetails.ToEmailID = manageUser.EmailID;
                emailDetails.EmailSubject = newUserEmailTemplateNumber.EmailSubject;
                emailDetails.EmailBody =
                    Convert.ToString(newUserEmailTemplateNumber.EmailBody).Replace(AppEmailConstants.AtUsername, manageUser.UserName).
                    Replace("@BeginUrlButton", "<a href='" + referenceUrl + "' style='font-family:Calibri;font-size:20px;background-color: #337ab7;color: white;border: 1px solid #2e6da4;text-decoration:none;padding:5px 20px;border-radius:5px;'>")
                    .Replace("@EndUrlButton", "</a>");
                AppConfigModel appConfigModel = m_appUserRepository.GetAppSetting();
                emailDetails.FromEmailID = appConfigModel.AdminEmailID;
                mailSentStatus =await m_email.SendEmailAsync(appConfigModel,emailDetails, true);
            }
            return mailSentStatus;
        }

        public int SendContactEmail(string tenant, string toEmail)
        {
            AppEmail TenantExpireInfoEmailTemplate = m_appUserRepository.GetEmailTemplate("TenantExpireInfo");
            int mailSentStatus = 0;
            EmailDetails emailDetails = new EmailDetails();
            string Password = string.Empty;
            emailDetails.ToEmailID = toEmail;
            emailDetails.EmailSubject = tenant + "- Requested to Contact";
            emailDetails.EmailBody = Convert.ToString(TenantExpireInfoEmailTemplate.EmailBody).Replace("@tenant", tenant);
               AppConfigModel appConfigModel = m_appUserRepository.GetAppSetting();
            emailDetails.FromEmailID = appConfigModel.AdminEmailID;
            mailSentStatus = m_email.SendEmail(appConfigModel, emailDetails, true);
            return mailSentStatus;
        }

        public async Task PutUserPopulation(int userNum)
        {
            await m_appUserRepository.PutUserPopulation(userNum);
        }
        // Author         :  Arunraj C
        // Creation Date  :  018-Jul-2017  
        /// <summary>
        /// To get the password length
        /// </summary>
        public int getPasswordLength()
        {
            return m_appUserRepository.defaultPasswordLength();
        }
        #endregion
        #region Private Methods Implementation
        private UserCredentialStatus validateSecretKey(AppUser user, string secretKey)
        {
            if (user.SecretKey == secretKey)
                return UserCredentialStatus.Valid;
            else
                return UserCredentialStatus.InValidSecretKey;
        }
        // Modified By    :  N Thuishithaa
        // Modified Date  :  04-August-2016  
        // Reviewed By    :  Praveen Kumar
        // Reviewed Date  :  20-August-2016 
        // Comment        :  Update failedloggedin attempts and appuserstatus if locked date is expired
        // Modified By    :  N Thuishithaa
        // Modified Date  :  12-Oct-2016  
        // Reviewed By    :  Hari
        // Reviewed Date  :  12-Oct-2016
        // Comment        :  Return status as Valid after updating failedloggedin attempts and appuserstatus if locked date is expired
        // Modified By    : Karthikeyan Shanmugam
        // Modified Date  : 14-Feb-2017
        // Reviewed By    : Hariharasubramaniyan
        // Reviewed Date  : 14-Feb-2017
        // comments       : To show the cilent time based on UTC time it shows depend upon the client location or time zone
        private UserCredentialStatus validateUser(AppUser user,AppConfigModel appConfig)
        {
            AppConfigModel appConfigModel = appConfig;
            if (appConfigModel == null || appConfigModel == default(AppConfigModel))
            {
                appConfigModel = m_appUserRepository.GetAppSetting();
            }
            //AppConfigModel appConfigModel = m_sessionRepository.GetAppSetting();
            var userStatus = m_appUserRepository.GetAppUserStatus(user.AppUserStatusID);
            user.LockedDate = m_appUserRepository.GetLockedDate(user.UserID);
            user.LockedDate = user.LockedDate != null ? user.LockedDate.Value.AddMinutes(appConfigModel.PasswordDisableDuration) : user.LockedDate;
            var localTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.Local);
            if (userStatus.TrimEnd() == "In-Active" && localTime >= user.LockedDate)
            {
                user.FailedLoginAttempts = 0;
                int appUserStatusID = m_appUserRepository.GetAppUserStatusId("Active");
                user.AppUserStatusID = appUserStatusID;
                user.LockedDate = null;
                user.UpdatedBy = user.UserNum;
                user.UpdatedDate = localTime;
                m_appUserRepository.UpdateUser(user);
                return UserCredentialStatus.Valid;
            }
            if (user == null) return UserCredentialStatus.InvalidUser;
            if (userStatus.TrimEnd() == "In-Active" && localTime >= user.LockedDate) return UserCredentialStatus.Valid;
            if (userStatus.TrimEnd() == "In-Active") return UserCredentialStatus.InActive;
            if (userStatus.TrimEnd() == "Lock") return UserCredentialStatus.Locked;
            return UserCredentialStatus.Valid;
        }
        private UserCredentialStatus validatePassword(AppUser user, string password, int? loginAttempt)
        {
            if (m_passwordEncryption.ValidatePassword(password, user.UserPassword))
            {
                revertFailedloginAttempts(user);
                return UserCredentialStatus.Valid;
            }

            addFailedLoginAttempts(user, loginAttempt);
            return (user.FailedLoginAttempts >= loginAttempt) ? UserCredentialStatus.Locked : UserCredentialStatus.InvalidPassword;
        }
        private void revertFailedloginAttempts(AppUser user)
        {
            user.FailedLoginAttempts = 0;            
            user.UpdatedDate = DateTime.Now;
            m_appUserRepository.UpdateUser(user);
        }
        // Modified By    :  N Thuishithaa
        // Modified Date  :  04-August-2016  
        // Comment        :  Added lockeddate to loggedin user
        // Modified By    : Karthikeyan Shanmugam
        // Modified Date  : 14-Feb-2017
        // Reviewed By    : Hariharasubramaniyan
        // Reviewed Date  : 14-Feb-2017
        // comments       : To show the cilent time based on UTC time it shows depend upon the client location or time zone
        private void addFailedLoginAttempts(AppUser user,int? loginAttempt)
        {
            int appUserStatusID = m_appUserRepository.GetAppUserStatusId("In-Active");
            byte failedLoginAttempts = Convert.ToByte(user.FailedLoginAttempts > 0 ? ++user.FailedLoginAttempts : 1);
            user.FailedLoginAttempts = failedLoginAttempts;
            user.AppUserStatusID = Convert.ToByte((loginAttempt.HasValue && failedLoginAttempts >= loginAttempt) ? appUserStatusID : user.AppUserStatusID);
            var localTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.Local);
            user.LockedDate = (failedLoginAttempts == loginAttempt) ? localTime : user.LockedDate;
            user.UpdatedBy = user.UserNum;
            user.UpdatedDate = localTime;
            m_appUserRepository.UpdateUser(user);
        }
        #endregion
       

        
    }
}
