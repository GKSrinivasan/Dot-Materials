// Copyright (c) 2016 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.
// Component Name  :  SessionRepository
// Description     :  This page is used to retrieve the Session data from database.
// Author         :  Raja Ganapathy
// Creation Date  :  05-Jul-2016   

using Laserbeam.BusinessObject.Common;
using Laserbeam.Constant.HR;
using Laserbeam.DataManager.Interfaces.Common;
using Laserbeam.DataManager.Interfaces.Core;
using Laserbeam.EntityManager.Common;
using Laserbeam.Libraries.Core.Interfaces.Common;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Laserbeam.DataManager.Common
{
    public class SessionRepository :  ISessionRepository
    {
       // Author         :  Raja Ganapathy
        // Creation Date  :  05-Jul-2016        
        /// <summary>
        /// Object of Base Repository
        /// </summary>
        private IBaseRepository m_baseRepository;
        private ITenantCacheProvider m_tenantCacheProvider;
        #region Constructors
        // Author         :  Raja Ganapathy
        // Creation Date  :  05-Jul-2016  
        /// <summary>
        /// Initializes objects used in this class
        /// </summary>
        /// <param name="baseRepository">Base Repository Object</param>
        public SessionRepository(IBaseRepository baseRepository, ITenantCacheProvider tenantCacheProvider)           
        {
            m_baseRepository = baseRepository;
            m_tenantCacheProvider = tenantCacheProvider;
        }
        #endregion


        #region Implements
        // Author         :  Raja Ganapathy
        // Creation Date  :  05-Jul-2016       
        /// <summary>
        /// To get user session data from database, 
        /// </summary>
        /// <param name="sessionId">EmployeeID or UserID for which the user session data is needed</param>
        /// <param name="isEmployeeId">Default is false, UserID should be provided as sessionId. True if EmployeeID is provided as sessionId</param>
        /// <returns>Returns UserModel object</returns>
        public UserModel GetUserSession(string sessionId, int year, bool isEmployeeId = false)
        {
            var isSampleData = m_tenantCacheProvider.GetBusinessSetting().GeneralConfiguration.IsSampleData;
            UserModel userModel = new UserModel();
            if (isEmployeeId)
            {
                var ee = m_baseRepository.GetQuery<Employee>().FirstOrDefault(e => e.EmployeeID == sessionId);
                var appUser = m_baseRepository.GetQuery<AppUser>(new string[] { "AppUserRole" }).FirstOrDefault(a => a.EmployeeNum == ee.EmployeeNum);
                    userModel.AppUserStatusID = appUser.AppUserStatusID;
                    userModel.DefaultPassword = appUser.DefaultPassword;
                    userModel.KeyName = appUser.AppUserRole.KeyName;
                    userModel.EmailID = appUser.EmailID;
                    userModel.EmployeeID = ee.EmployeeID;
                    userModel.EmployeeNum = appUser.EmployeeNum ?? 0;
                    userModel.EmployeeName = ee.EmployeeName;
                    userModel.FailedLoginAttempts = appUser.FailedLoginAttempts;                    
                    userModel.LastLoginDt = appUser.LastLoginDt;
                    userModel.UserID = appUser.UserID;
                    userModel.PreferredName = appUser.PreferredName;
                    userModel.UserName = appUser.UserName;
                    userModel.UserNum = appUser.UserNum;
                    userModel.UserPassword = appUser.UserPassword;
                    userModel.UserRole = appUser.AppUserRole.UserRole;
                    userModel.UserRoleNum = appUser.UserRoleNum;
                    userModel.IsDefaultUser = appUser.IsDefaultUser;
                    userModel.IsAdminAccess = appUser.IsAdminAccess ?? false;
                    userModel.AdminEmpNum = appUser.AdminEmpNum;
                    userModel.AdminUserNum = appUser.AdminUserNum;
                    userModel.enableSwitchtoAdmin = false;
                    userModel.IsSampleData = isSampleData;
            }
            else
            {
                var appUser = m_baseRepository.GetQuery<AppUser>(new string[] { "AppUserRole", "Employee" }).FirstOrDefault(a => a.UserID.ToLower() == sessionId.ToLower());               
                    userModel.AppUserStatusID = appUser.AppUserStatusID;
                    userModel.DefaultPassword = appUser.DefaultPassword;
                    userModel.KeyName = appUser.AppUserRole.KeyName;
                    userModel.EmailID = appUser.EmailID;
                    userModel.EmployeeID =appUser.Employee.EmployeeID;
                    userModel.EmployeeNum = appUser.EmployeeNum ?? 0;
                    userModel.EmployeeName = appUser.Employee.EmployeeName;
                    userModel.FailedLoginAttempts = appUser.FailedLoginAttempts;                    
                    userModel.LastLoginDt = appUser.LastLoginDt;
                    userModel.UserID = appUser.UserID;
                    userModel.PreferredName = appUser.PreferredName;
                    userModel.UserName = appUser.UserName;
                    userModel.UserNum = appUser.UserNum;
                    userModel.UserPassword = appUser.UserPassword;
                    userModel.UserRole = appUser.AppUserRole.UserRole;
                    userModel.UserRoleNum = appUser.UserRoleNum;
                    userModel.IsDefaultUser = appUser.IsDefaultUser;
                    userModel.IsAdminAccess = appUser.IsAdminAccess ?? false;
                    userModel.AdminEmpNum = appUser.AdminEmpNum;
                    userModel.AdminUserNum = appUser.AdminUserNum;
                    userModel.enableSwitchtoAdmin = false;
                    userModel.IsSampleData = isSampleData;
            }
         return userModel;
         //return GetGradeUserModel(userModel, year); 
        }
        public UserCredentialStatus GetUserSSOLoginSession(string emailAddress, out UserModel userModel)
        {
            List<AppUser> selectedAppUser = new List<AppUser>();
              if (emailAddress != null)
                selectedAppUser = m_baseRepository.GetQuery<AppUser>(new string[] { "AppUserRole", "Employee", "AppUserStatu" }, x => x.UserID == emailAddress).ToList();

            AppUser appUser = new AppUser();
            if (selectedAppUser.Count(x => x.MarkAsDelete == false && x.AppUserStatu.UserStatus.ToLower() == "active") > 0)
                appUser = selectedAppUser.Where(x => x.MarkAsDelete == false && x.AppUserStatu.UserStatus.ToLower() == "active").OrderByDescending(x => x.AppUserRole).FirstOrDefault();
            else
                appUser = selectedAppUser.OrderByDescending(x => x.AppUserRole).FirstOrDefault();
            userModel = new UserModel();
            if (appUser == null || appUser.MarkAsDelete == true)
            {
                return UserCredentialStatus.InvalidUser;
            }
            else if (appUser.AppUserStatu.UserStatus.ToLower().Trim() == "in-active")
            {
                return UserCredentialStatus.InActive;
            }
            else if (appUser.AppUserStatu.UserStatus.ToLower().Trim() == "lock")
            {
                return UserCredentialStatus.Locked;
            }
            userModel.AppUserStatusID = appUser.AppUserStatusID;
            userModel.DefaultPassword = appUser.DefaultPassword;
            userModel.EmailID = appUser.EmailID;
            userModel.EmployeeID = appUser.Employee.EmployeeID;
            userModel.EmployeeNum = appUser.EmployeeNum ?? 0;
            userModel.EmployeeName = appUser.Employee.EmployeeName;
            userModel.FailedLoginAttempts = appUser.FailedLoginAttempts;
            userModel.LastLoginDt = appUser.LastLoginDt;
            userModel.UserID = appUser.UserID;
            userModel.UserName = appUser.UserName;
            userModel.UserNum = appUser.UserNum;
            userModel.UserPassword = appUser.UserPassword;
            userModel.KeyName = appUser.AppUserRole.KeyName;
            userModel.UserRole = appUser.AppUserRole.UserRole;
            userModel.UserRoleNum = appUser.UserRoleNum;
            return UserCredentialStatus.Valid;
        }

        // Author         :  Raja Ganapathy
        // Creation Date  :  05-Jul-2016  
        /// <summary>
        /// To get user session data from database
        /// </summary>
        /// <param name="userNum">User tool number for which the user session data is needed</param>
        /// <returns>Returns UserModel object</returns>
        public UserModel GetUserSession(int userNum, int year)
        {
            var isSampleData = m_tenantCacheProvider.GetBusinessSetting().GeneralConfiguration.IsSampleData;
            var appUser = m_baseRepository.GetQuery<AppUser>(new string[] { "AppUserRole", "Employee" }).FirstOrDefault(a => a.UserNum == userNum);            
            UserModel userModel = new UserModel();
            userModel.AppUserStatusID = appUser.AppUserStatusID;
            userModel.DefaultPassword = appUser.DefaultPassword;
            userModel.EmailID = appUser.EmailID;
            userModel.EmployeeID = appUser.Employee.EmployeeID;
            userModel.EmployeeNum = appUser.EmployeeNum ?? 0;
            userModel.EmployeeName = appUser.Employee.EmployeeName;
            userModel.FailedLoginAttempts = appUser.FailedLoginAttempts;            
            userModel.LastLoginDt = appUser.LastLoginDt;
            userModel.UserID = appUser.UserID;
            userModel.UserName = appUser.UserName;
            userModel.UserNum = appUser.UserNum;
            userModel.UserPassword = appUser.UserPassword;
            userModel.KeyName = appUser.AppUserRole.KeyName;
            userModel.UserRole = appUser.AppUserRole.UserRole;
            userModel.UserRoleNum = appUser.UserRoleNum;
            userModel.IsDefaultUser = appUser.IsDefaultUser;
            userModel.IsAdminAccess = appUser.IsAdminAccess ?? false;
            userModel.AdminEmpNum = appUser.AdminEmpNum;
            userModel.AdminUserNum = appUser.AdminUserNum;
            userModel.IsSampleData = isSampleData;
            return userModel;
            //return GetGradeUserModel(userModel, year); 
        }

        // Author        :  Boobalan		
        // Creation Date :  03-01-2015      
        /// <summary>
        /// To get employee session from database
        /// </summary>
        /// <param name="employeeNum">Employee tool number</param>
        /// <param name="year">Employee job year</param>
        /// <returns>Returs EmployeeModel object</returns>
        public EmployeeModel GetEmployeeSession(int employeeNum, int year)
        {
            var empJob = m_baseRepository.GetQuery<EmployeeJob>(new string[] { "BonusType", "Location", "Grade", "Currency", "Job" }, i => i.EmployeeNum == employeeNum && i.JobYear == year).FirstOrDefault();            
            EmployeeModel employeeModel = new EmployeeModel();
            if (empJob != null)
            {
                employeeModel.BonusType = (empJob.BonusType!=null) ? empJob.BonusType.BonusTypeCode : null;
                employeeModel.EmployeeJobNum = empJob.EmpJobNum;
                employeeModel.EmployeeNum = empJob.EmployeeNum;
                employeeModel.Location = (empJob.Location!=null) ? empJob.Location.LocationCode.ToString() : null;
                employeeModel.ManagerNum = empJob.ManagerNum ?? 0;
                employeeModel.Currency = (empJob.Currency!= null) ? empJob.Currency.CurrencyCode.ToString() : "";
                employeeModel.Grade = empJob.Grade != null ? empJob.Grade.GradeCode : null;
                employeeModel.JobDescr = empJob.Job != null ? empJob.Job.JobDescr : null;
                var employee = empJob.Employee;
                if (employee != null)
                {
                    employeeModel.EmpEmailID = employee.EmailAddress;
                    employeeModel.EmployeeFirstName = employee.FirstName;
                    employeeModel.EmployeeID = employee.EmployeeID;
                    employeeModel.EmployeeLastName = employee.LastName;
                    employeeModel.EmployeeName = employee.EmployeeName;
                }
                var manager = empJob.Employee1;
                if (manager != null)
                {
                    employeeModel.ManagerEmailID = manager.EmailAddress;
                    employeeModel.ManagerFirstName = manager.FirstName;
                    employeeModel.ManagerJobNum = manager.EmployeeJobs.FirstOrDefault(i => i.JobYear == empJob.JobYear).EmpJobNum;
                    employeeModel.ManagerLastName = manager.LastName;
                    employeeModel.ManagerName = manager.EmployeeName;
                }                
            }
            return employeeModel;
        }

        // Author         :  Raja Ganapathy
        // Creation Date  :  05-Jul-2016  
        /// <summary>
        /// To get the user access menus
        /// </summary>
        /// <param name="userNum">Denotes the logged in employee num</param>
        /// <returns>Returns the user rights</returns>
        // Modified By   : VijayBabu.B.H
        /// Modified Date : 06-10-2016
        /// Reviewed By   : Raja Ganapathy
        //  Reviewed Date : 06-10-2016
        //  Comment       : Remove unwanted parentmenuid

        public List<UserRights> GetUserAccess(int userNum)
        {
            var userRole = m_baseRepository.GetQuery<AppUser>(new string[] { "AppUserRole" }, i => i.UserNum == userNum).Select(x => x.AppUserRole.KeyName).FirstOrDefault();                        
            var appmenu = m_baseRepository.GetQuery <AppMenu>(i=>i.Active).ToList();
            List<UserRights> userAccess;
            if (userRole.ToLower() == "superadmin")
            {
                userAccess = (from am in appmenu
                              join pam in appmenu on am.MenuParentNum equals pam.AppMenuID into p
                              from pam in p.DefaultIfEmpty(new AppMenu())
                              where am.MenuLinks != null
                 select new UserRights
                 {
                     MenuID = am.menuid.Trim(),
                     MenuLinks = am.MenuLinks,
                     MenuName = am.MenuName,
                     ParentMenuID = pam.menuid != null ? pam.menuid.ToString().Trim() : null,
                     ReadOnly = false,
                     isAdminLinks = am.AdminLinks
                 }).ToList();
            }
            else
            {
                var user = m_baseRepository.GetQuery<UserMenu>(i => i.UserID == userNum).ToList();
                var appMenuIDs = user.Select(i => i.AppMenuID);
                var otherApp = appmenu.Where(i => appMenuIDs.Contains(i.AppMenuID));
                userAccess = (from um in user
                                  join am in otherApp on um.AppMenuID equals am.AppMenuID
                              join pam in appmenu on am.MenuParentNum equals pam.AppMenuID into p
                              from pam in p.DefaultIfEmpty(new AppMenu())
                              where am.MenuLinks != null
                                  select new UserRights
                                  {
                                      MenuID = am.menuid.Trim(),
                                      MenuLinks = am.MenuLinks,
                                      MenuName = am.MenuName,
                                      ParentMenuID = pam.menuid != null ? pam.menuid.ToString().Trim() : null,
                                      ReadOnly = um.ReadOnly ?? false,
                                      isAdminLinks=am.AdminLinks
                                  }).ToList(); 
            }
            return userAccess;
        }

        //// Author         :  Raja Ganapathy
        //// Creation Date  :  05-Jul-2016  
        //// Modified By    :  N Thuishithaa
        //// Modified Date  :  04-August-2016  
        //// Comment        :  Get PasswordDisableDuration value from appsetting table and set value to appconfig model
        //// Reviewed By    :  Praveen Kumar
        //// Reviewed Date  :  20-August-2016 
        ///// <summary>
        ///// Get App setting values
        ///// </summary>
        ///// <returns>Returns the app setting values</returns>
        //public AppConfigModel GetAppSetting()
        //{
        //    AppConfigModel appConfig = new AppConfigModel();
        //    var appSetting = m_baseRepository.GetQuery<AppSetting>().ToList();
        //    var baseCurrency = appSetting.SingleOrDefault(m => m.AppSettingID == AppUserConstants.BaseCurrency).AppSettingValue;
        //    appConfig.CurrentYear = appSetting.SingleOrDefault(m => m.AppSettingID == AppUserConstants.CurrentYear).AppSettingValue;
        //    appConfig.MeritCycleYear = appSetting.SingleOrDefault(m => m.AppSettingID == AppUserConstants.MeritCycleYear).AppSettingValue;
        //    appConfig.SMTPServer = appSetting.SingleOrDefault(m => m.AppSettingID == AppUserConstants.SMTPServer).AppSettingValue;
        //    appConfig.IsSAMLLogin = appSetting.SingleOrDefault(m => m.AppSettingID == AppUserConstants.IsSAMLLogin).AppSettingValue;
        //    appConfig.AdminEmailID = appSetting.SingleOrDefault(m => m.AppSettingID == AppUserConstants.AdminEmailID).AppSettingValue;
        //    appConfig.SMTPPort = Convert.ToInt16(appSetting.SingleOrDefault(m => m.AppSettingID == AppUserConstants.SMTPPort).AppSettingValue);
        //    appConfig.LoginAttempt = Convert.ToInt16(appSetting.SingleOrDefault(m => m.AppSettingID == AppUserConstants.LoginAttempt).AppSettingValue);
        //    appConfig.AdminPassword = appSetting.SingleOrDefault(m => m.AppSettingID == AppUserConstants.AdminPassword).AppSettingValue;
        //    appConfig.ToolName = appSetting.SingleOrDefault(m => m.AppSettingID == AppUserConstants.ToolName).AppSettingValue;
        //    appConfig.ToolVersion = appSetting.SingleOrDefault(m => m.AppSettingID == AppUserConstants.ToolVersion).AppSettingValue;
        //    appConfig.Instance = appSetting.SingleOrDefault(m => m.AppSettingID == AppUserConstants.Instance).AppSettingValue;
        //    appConfig.EnableNotification = (appSetting.SingleOrDefault(m => m.AppSettingID == AppUserConstants.EnableNotification).AppSettingValue.ToLower() == "true");
        //    appConfig.EnableFeedBack = (appSetting.SingleOrDefault(m => m.AppSettingID == AppUserConstants.EnableFeedBack).AppSettingValue.ToLower() == "true");
        //    appConfig.Version = appSetting.SingleOrDefault(m => m.AppSettingID == AppUserConstants.Version).AppSettingValue;
        //    appConfig.PasswordDisableDuration = Convert.ToInt32(appSetting.SingleOrDefault(m => m.AppSettingID == AppUserConstants.PasswordDisableDuration).AppSettingValue);
        //    appConfig.RangeExceed = Convert.ToInt32(appSetting.SingleOrDefault(m => m.AppSettingID == AppUserConstants.RangeExceed).AppSettingValue);
        //    appConfig.BaseCurrency = appSetting.SingleOrDefault(m => m.AppSettingID == AppUserConstants.BaseCurrency).AppSettingValue;
        //    appConfig.CurrencyApiAccessKey = appSetting.SingleOrDefault(m => m.AppSettingID == AppUserConstants.CurrencyApiAccessKey).AppSettingValue;
        //    appConfig.BaseCurrencyNum = m_baseRepository.GetQuery<Currency>(x => x.CurrencyCode == baseCurrency.Trim()).Select(x => x.CurrencyCodeNum).FirstOrDefault();
        //    appConfig.TokBoxApiKey = Convert.ToInt32(appSetting.SingleOrDefault(m => m.AppSettingID == AppUserConstants.TokBoxApiKey).AppSettingValue);
        //    appConfig.TokBoxSecretKey = appSetting.SingleOrDefault(m => m.AppSettingID == AppUserConstants.TokBoxSecretKey).AppSettingValue;
        //    appConfig.TenantExpireDate = Convert.ToDateTime(appSetting.SingleOrDefault(m => m.AppSettingID == AppUserConstants.TenantExpireDate).AppSettingValue);
        //    appConfig.AdminUserID = appSetting.SingleOrDefault(m => m.AppSettingID == AppUserConstants.AdminUserID).AppSettingValue;
        //    appConfig.PayChangeURL = appSetting.SingleOrDefault(m => m.AppSettingID == AppUserConstants.PayChangeURL).AppSettingValue;
        //    return appConfig;
        //}



        #endregion


    }
}
