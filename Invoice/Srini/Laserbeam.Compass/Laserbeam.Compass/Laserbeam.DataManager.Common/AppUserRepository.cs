// Copyright (c) 2015 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.

// Component Name  :    AppUserRepository
// Description     : 	Repository for AppUser
// Author          :	Raja Ganapathy		
// Creation Date   : 	05-Jul-2016

using Laserbeam.DataManager.Interfaces.Common;
using System;
using System.Linq;
using System.Data.SqlClient;
using System.Collections.Generic;
using Laserbeam.BusinessObject.Common;
using Laserbeam.DataManager.Interfaces.Core;
using Laserbeam.EntityManager.Common;
using System.Threading.Tasks;
using Laserbeam.Libraries.Core.Interfaces.Common;

namespace Laserbeam.DataManager.Common
{
    public class AppUserRepository : IAppUserRepository
    {
        #region Fields
        // Author         :  Raja Ganapathy
        // Creation Date  :  05-Jul-2016        
        /// <summary>
        /// Object of Base Repository
        /// </summary>
        private IBaseRepository m_baseRepository;
        private ITenantCacheProvider m_tenantCacheProvider;
        #endregion
        #region Constructors
        // Author         :  Raja Ganapathy
        // Creation Date  :  05-Jul-2016  
        /// <summary>
        /// Initializes objects used in this class
        /// </summary>
        /// <param name="baseRepository">Base Repository Object</param>
        public AppUserRepository(IBaseRepository baseRepository, ITenantCacheProvider tenantCacheProvider)            
        {
            m_baseRepository = baseRepository;
            m_tenantCacheProvider = tenantCacheProvider;
        }
        #endregion
        #region Public Methods Implementation
        public UserModel GetUserDataModel(string userID)
        {
            var appUsers = m_baseRepository.GetQuery<AppUser>(new string[] { "AppUserRole", "AppUserStatu" }, i => i.UserID == userID)
                .Select(m => new UserModel
                {
                    AppUserStatusID = m.AppUserStatusID,
                    DefaultPassword = m.DefaultPassword,
                    EmailID = m.EmailID,
                    EmployeeID = m.Employee.EmployeeID,
                    EmployeeName = m.Employee.EmployeeName,
                    EmployeeNum = m.EmployeeNum ?? 0,
                    FailedLoginAttempts = m.FailedLoginAttempts,
                    KeyName = m.AppUserRole.KeyName,
                    LastLoginDt = m.LastLoginDt,
                    UserID = m.UserID,
                    UserName = m.UserName,
                    UserNum = m.UserNum,
                    UserPassword = m.UserPassword,
                    UserRole = m.AppUserRole.UserRole,
                    UserRoleNum = m.UserRoleNum,
                    SecretKey = m.SecretKey,
                    WizardStepsCompleted = m.WizardStepsCompleted ?? 0,
                    IsDefaultUser = m.IsDefaultUser
                }).FirstOrDefault();
            return appUsers;
        }
        // Author         :  Raja Ganapathy
        // Creation Date  :  05-Jul-2016  
        /// <summary>
        /// To get user details
        /// </summary>
        /// <param name="userID">ID of user</param>
        /// <returns></returns>
        public AppUser GetUser(string userID)
        {
            var appUsers = m_baseRepository.GetQuery<AppUser>(i => i.UserID == userID).FirstOrDefault();
            return appUsers;
        }
        public string GetAppUserStatus(int? appUserStatusId)
        {
            return m_baseRepository.GetQuery<AppUserStatu>(x => x.AppUserStatusID == appUserStatusId).Select(x => x.UserStatus).FirstOrDefault();
        }
        public int GetAppUserStatusId(string appUserStatus)
        {
            return m_baseRepository.GetQuery<AppUserStatu>(x => x.UserStatus.TrimEnd() == appUserStatus).Select(x => x.AppUserStatusID).FirstOrDefault();
        }
        public AppConfigModel GetAppSetting()
        {
            AppConfigModel appConfig = new AppConfigModel();
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
            appConfig.TenantExpireDate = Convert.ToDateTime(appSetting.TenantExpireDate);
            return appConfig;

        }
        // Author         :  Raja Ganapathy
        // Creation Date  :  05-Jul-2016  
        /// <summary>
        /// To get the all users
        /// </summary>
        /// <param name="userNum">Defines the usernum for filter</param>
        /// <returns>Returns the List</returns>
        public IQueryable<AppUser> GetUsersByUserNum(List<int> userNum)
        {
            return m_baseRepository.GetQuery<AppUser>(i => userNum.Contains(i.UserNum));          
        }

        // Author         :  Raja Ganapathy
        // Creation Date  :  05-Jul-2016  
        /// <summary>
        /// To get the particular user data
        /// </summary>
        /// <param name="userNum">Defines the user num</param>
        /// <returns>Returns the user details</returns>
        public AppUser GetUserData(int userNum)
        {
            return m_baseRepository.GetQuery<AppUser>(i => i.UserNum == userNum).FirstOrDefault();
            
        }

        // Author         :  Raja Ganapathy
        // Creation Date  :  05-Jul-2016  
        /// <summary>
        /// To get user details with status
        /// </summary>
        /// <param name="userID">ID of user</param>
        /// <returns></returns>
        public AppUser GetUserWithStatus(string userID)
        {
            var user = m_baseRepository.GetQuery<AppUser>(new string[] { "AppUserRole", "AppUserStatu" }, m => m.UserID == userID);
            return user.FirstOrDefault();
        }
       
        // Author         :  N Thuishithaa
        // Creation Date  :  04-August-2016  
        // Reviewed By    :  Praveen Kumar
        // Reviewed Date  :  20-August-2016  
        /// <summary>
        /// To get LockedDate of login user
        /// </summary>
        /// <param name="userID">ID of loggedin user</param>
        /// <returns></returns>
        // Modified By    :  Pulithevan Stalin
        // Modified Date  :  06-10-2016 
        // Reviewed By    :  Vijay Babu
        // Reviewed Date  :  06-10-2016 
        // Comment        :  Add ToLower key word 
        public DateTime? GetLockedDate(string userId)
        {

            var datedata = m_baseRepository.GetQuery<AppUser>(X => X.UserID.ToLower() == userId.ToLower()).Select(xx => xx.LockedDate).FirstOrDefault();
            return datedata;
        }
        // Author         :  Raja Ganapathy
        // Creation Date  :  05-Jul-2016  
        /// <summary>
        /// Update user details
        /// </summary>
        /// <param name="appUser">user object</param>
        public bool UpdateUser(AppUser appUser)
        {
            m_baseRepository.Edit<AppUser>(appUser);
            m_baseRepository.SaveChanges();
            return true;
        }
        public string GetPasswordMode()
        {
            return m_tenantCacheProvider.GetApplicationSetting().PasswordMode.ToLower();
         }
        // Author         :  Raja Ganapathy
        // Creation Date  :  05-Jul-2016  
        /// <summary>
        /// To get the user roles
        /// </summary>
        /// <returns>Returns the user roles</returns>
        public IQueryable<AppUserRole> GetUserRoles()
        {
            return m_baseRepository.GetQuery<AppUserRole>();            
        }
        public AppEmail GetEmailTemplate(string templateName)
        {
            return m_baseRepository.GetQuery<AppEmail>(m => m.EmailKey == templateName).FirstOrDefault();
        }
        // Author         :  Raja Ganapathy
        // Creation Date  :  05-Jul-2016  
        /// <summary>
        /// Update the last login date in database
        /// </summary>
        /// <param name="userNum">Denotes the user num</param>
        /// <param name="isSSOLogin">Denotes it is single sign on</param>
        // Modified By    : Karthikeyan Shanmugam
        // Modified Date  : 14-Feb-2017
        // Reviewed By    : Hariharasubramaniyan
        // Reviewed Date  : 14-Feb-2017
        // comments       : To show the cilent time based on UTC time it shows depend upon the client location or time zone

        public void PutUserLogoutDetails(int userNum, bool isSSOLogin)
        {
            var appUser = GetUserData(userNum);
            var localTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.Local);
            if (isSSOLogin)
                appUser.SSOLastLogOutDate = localTime;
            else
                appUser.LastLogOutDt = localTime;
            appUser.IsLoggedIn = false;           
            m_baseRepository.Edit<AppUser>(appUser);
            m_baseRepository.SaveChanges();
        }

        public void PutUserLogInDetails(int userNum)
        {
            var appUser = GetUserData(userNum);
            appUser.IsLoggedIn = true;
            m_baseRepository.Edit<AppUser>(appUser);
            m_baseRepository.SaveChanges();
        }

        public async Task PutUserPopulation(int userNum)
        {
           await m_baseRepository.ExecuteStoredProcedure("[Talent].[USP_PUT_UserPopulation]", new SqlParameter[] { new SqlParameter("@LoggedUserNum", userNum) });
        }

        // Author         :  Arunraj C
        // Creation Date  :  018-Jul-2017  
        /// <summary>
        /// To get the password length
        /// </summary>
        public int defaultPasswordLength()
        {
            var passwordLength = m_tenantCacheProvider.GetApplicationSetting().PasswordLength;
            return Convert.ToInt32(passwordLength);
        }
        #endregion
    }
}

