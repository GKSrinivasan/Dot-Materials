// Copyright (c) 2016 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.
// Component Name  :  UserManagementRepository
// Description     :  Repository for UserManagement
// Author         :  HARIHARASUBRAMANIYAN CHANDRASEKARAN
// Creation Date  :  10-Feb-2017

using Laserbeam.BusinessObject.Common;
using Laserbeam.DataManager.Interfaces.Common;
using Laserbeam.DataManager.Interfaces.Core;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Laserbeam.EntityManager.Common;
using Master = Laserbeam.EntityManager.TenantMaster;
namespace Laserbeam.DataManager.Common
{
    public class UserManagementRepository : IUserManagementRepository
    {
        #region Fields
        // Author         :  HARIHARASUBRAMANIYAN CHANDRASEKARAN
        // Creation Date  :  09-Feb-2017
        /// <summary>
        /// Object of Base Repository
        /// </summary>
        private IBaseRepository m_baseRepository;

        private IMasterBaseRepository m_masterRepository;
        #endregion

        #region Constructors
        // Author        :  HARIHARASUBRAMANIYAN CHANDRASEKARAN
        // Creation Date :  09-Feb-2017
        /// <summary>
        ///Initializes objects used in this class 
        ///</summary>        
        /// <param name="baseRepository">Base Repository Object</param>  

        public UserManagementRepository(IBaseRepository baseRepository, IMasterBaseRepository masterRepository)
        {
            m_baseRepository = baseRepository;
            m_masterRepository = masterRepository;
        }
        #endregion

        #region User List Implementation
        #region CURD Operations
        // Author        :  HARIHARASUBRAMANIYAN CHANDRASEKARAN
        // Creation Date :  10-Feb-2017
        /// <summary>
        /// This Method is used to get the list user roles to bind to dropdown.
        /// Active user roles only display, Added user role key name (userrole.KeyName) and user role admin all text changes in this repository 
        /// </summary>
        /// <param name="userNum">usernum as a parameter        
        /// <returns>returns user role collection list</returns>              
        public IQueryable<AppUserRoleModel> GetDropdownUserRoles(int userNum)
        {
            var userrole = m_baseRepository.GetQuery<AppUser>(new string[] { "AppUserRole" }, m => m.UserNum == userNum).Select(x => x.AppUserRole.KeyName).FirstOrDefault();
            bool isadmin = userrole == "SuperAdmin" ? true : false;
            if (isadmin == true)
            {
                return m_baseRepository.GetQuery<AppUserRole>(x => x.Active == true && x.KeyName != "SuperAdmin").Select(m => new AppUserRoleModel
                {
                    UserRoleNum = m.UserRoleNum,
                    UserRole = m.UserRole,
                    Active = m.Active,
                    KeyName = m.KeyName
                });
            }
            else
            {
                return m_baseRepository.GetQuery<AppUserRole>(x => x.Active == true && x.KeyName != "SuperAdmin").Select(m => new AppUserRoleModel
                {
                    UserRoleNum = m.UserRoleNum,
                    UserRole = m.UserRole,
                    Active = m.Active,
                    KeyName = m.KeyName
                });
            }


        }

        // Author        :  HARIHARASUBRAMANIYAN CHANDRASEKARAN
        // Creation Date :  11-Feb-2017
        /// <summary>
        /// It is used to hide the user from app
        /// </summary>
        /// <param name="id">The ID belongs to the user to be deleted</param>
        /// <param name="userStatus">Userstatus of the user to be deleted</param>
        /// <returns>returns deleted status</returns>      
        public async Task<bool> DeleteUser(string id, int userNum, string tenantName)
        {
            var tenantNum = m_masterRepository.GetQuery<Master.Tenant>(m => m.TenantURLName == tenantName).Single().TenantNum;
            SqlParameter[] parameters = { new SqlParameter("@clearAll", false),
                                        new SqlParameter("@userID", id),
                                        new SqlParameter("@tenantNum", tenantNum) };
           await  m_masterRepository.ExecuteStoredProcedure("[dbo].[USP_DEL_MasterUserList]", parameters);

            AppUser userData = m_baseRepository.GetQuery<AppUser>(s => s.UserNum == userNum).FirstOrDefault();
            userData.MarkAsDelete = true;
            m_baseRepository.Edit(userData);
            m_baseRepository.SaveChanges();            
            return true;
        }
        public async Task<IEnumerable<AppUserDataModel>> GetUserGridData(int userNum)
        {
            SqlParameter[] sqlParameter = { new SqlParameter("@userNum", userNum) };
            var userData = await m_baseRepository.GetData<AppUserDataModel>("[Common].[USP_UA_GET_UserAccessGridData] @userNum", sqlParameter);
            return userData;
        }
        // Author        :  HARIHARASUBRAMANIYAN CHANDRASEKARAN
        // Creation Date :  24-Feb-2017       
        /// <summary>
        /// This Method is used to get the show the user count details 
        /// </summary>
        /// <returns>returns user count details</returns>   
        public async Task<IEnumerable<AppUsers>> GetUserCount(int userNum)
        {
            SqlParameter[] sqlParameter = { new SqlParameter("@UserNum", userNum) };
            var userData = await m_baseRepository.GetData<AppUsers>("[Common].[USP_UA_GET_UserCountInfo] @UserNum", sqlParameter);
            return userData;
        }
        
        // Author        :  HARIHARASUBRAMANIYAN CHANDRASEKARAN
        // Creation Date :  24-Feb-2017 
        // Comment        :  Added LoginID in appuser        
        /// <summary>
        /// Inserts User details to Database.
        /// </summary>
        /// <param name="user">App user object which contains the user details to be inserted</param>       
        /// <returns>returns the result model</returns>
        public async Task<int> AddUser(AppUserModel user, int userNum, string tenantName)
        {
            int model = 0;
            var appUser= m_baseRepository.GetQuery<AppUser>().ToList();
            if (user.UserNum > 0)
            {
                Employee employee = m_baseRepository.GetQuery<Employee>().Where(s => s.EmployeeID == user.EmployeeID).FirstOrDefault();
                int appUserStatus = m_baseRepository.GetQuery<AppUserStatu>().Where(s => s.UserStatus == user.UserStatus).Select(x => x.AppUserStatusID).FirstOrDefault();
                AppUser userdata = appUser.Where(m => m.UserNum == user.UserNum).FirstOrDefault();
                var tenantUser = m_masterRepository.GetQuery<Master.TenantUser>(m => m.UserId == userdata.UserID).FirstOrDefault();
                var tenantUserNew = m_masterRepository.GetQuery<Master.TenantUser>(m => m.UserId == user.UserID).FirstOrDefault();
                if (tenantUser == null)
                {
                    if (tenantUserNew == null)
                    {
                        var tenantNum = m_masterRepository.GetQuery<Master.Tenant>(m => m.TenantURLName == tenantName).Single().TenantNum;
                        Master.TenantUser userData = new Master.TenantUser();
                        userData.UserId = user.UserID;
                        userData.TenantNum = tenantNum;
                        m_masterRepository.Add<Master.TenantUser>(userData);
                    }
                }
                else
                {
                    tenantUser.UserId = user.UserID;
                    m_masterRepository.Edit<Master.TenantUser>(tenantUser);
                }
                m_masterRepository.SaveChanges();

                userdata.EmployeeNum = employee.EmployeeNum;
                userdata.EmployeeID = user.EmployeeID;
                userdata.UserID = user.UserID;
                userdata.PreferredName = user.PreferredName;
                userdata.FirstName = user.FirstName;
                userdata.LastName = user.LastName;
                userdata.EmailID = user.EmailID;
                userdata.SecretKey = Guid.NewGuid().ToString();
                userdata.LoginID = user.UserID;
                userdata.UserName = user.FirstName;
                userdata.UserRoleNum = user.UserRoleNum;
                userdata.AppUserStatusID = appUserStatus;
                userdata.UpdatedBy = userNum;
                userdata.UpdatedDate = DateTime.Now;
                if(user.UserStatus == "Lock")
                userdata.LockedDate = DateTime.Now;
                userdata.IsAdminAccess = user.IsAdminAccess;
                if(user.IsAdminAccess)
                {
                    var adminEmp = m_baseRepository.GetQuery<Employee>(x => x.EmployeeID == "999999999").FirstOrDefault();
                    var admindata = appUser.Where(m => m.EmployeeNum == adminEmp.EmployeeNum).FirstOrDefault();
                    userdata.AdminEmpNum = adminEmp.EmployeeNum;
                    userdata.AdminUserNum = admindata.UserNum;
                }
                else
                {
                    userdata.AdminEmpNum = null;
                    userdata.AdminUserNum = null;
                }
                userdata.LoginID = user.LoginID != null ? user.LoginID : m_baseRepository.GetQuery<AppUser>().Where(w => w.UserNum == user.UserNum).Select(x => x.LoginID).FirstOrDefault();
                if (userdata.MarkAsDelete == true) userdata.MarkAsDelete = false;
                if (user.isEmail == true)
                {
                    userdata.MailDeliveryStatus = true;
                    userdata.MailDeliveryDate = DateTime.Now;
                }
                                          
                m_baseRepository.Edit<AppUser>(userdata);
                m_baseRepository.SaveChanges();
                model = 1;
            }
            else
            {
                Employee employee = m_baseRepository.GetQuery<Employee>().Where(s => s.EmployeeID == user.EmployeeID).FirstOrDefault();
                var appUserStatus = m_baseRepository.GetQuery<AppUserStatu>().Where(s => s.UserStatus == user.UserStatus).FirstOrDefault();
                if (employee != null && employee.EmployeeNum != 0)
                {
                    var tenantNum = m_masterRepository.GetQuery<Master.Tenant>(m => m.TenantURLName == tenantName).Single().TenantNum;
                    DataTable dataTable = new DataTable();
                    dataTable.Columns.Add("UserId");
                    dataTable.Columns.Add("TenantNum");
                    var dataRow = dataTable.NewRow();
                    dataRow["UserId"] = user.UserID;
                    dataRow["TenantNum"] = tenantNum;
                    dataTable.Rows.Add(dataRow);
                    SqlParameter param = new SqlParameter("@UserList", dataTable);
                    param.SqlDbType = SqlDbType.Structured;
                    param.Direction = ParameterDirection.Input;
                    try
                    {
                       await m_masterRepository.ExecuteStoredProcedure("USP_PUT_MasterUserList", new SqlParameter[] { param });
                    }
                    catch//(Exception e)
                    {

                    }

                    AppUser userdata = new AppUser();
                    userdata.EmployeeNum = employee.EmployeeNum;
                    userdata.UserID = user.UserID;
                    userdata.PreferredName = user.PreferredName;
                    userdata.FirstName = user.FirstName;
                    userdata.LastName = user.LastName;
                    userdata.EmailID = user.EmailID;
                    userdata.EmployeeID = user.EmployeeID;
                    userdata.SecretKey = Guid.NewGuid().ToString();
                    userdata.LoginID = user.UserID;
                    userdata.MarkAsDelete = false;
                    userdata.UserName = user.FirstName;
                    userdata.UserRoleNum = user.UserRoleNum;
                    userdata.AppUserStatusID = appUserStatus.AppUserStatusID;
                    userdata.IsAdminAccess = user.IsAdminAccess;
                    if (user.IsAdminAccess)
                    {
                        var admindata = appUser.Where(m => m.EmployeeID == "999999999").FirstOrDefault();
                        userdata.AdminEmpNum = admindata.EmployeeNum;
                        userdata.AdminUserNum = admindata.UserNum;
                    }
                    else
                    {
                        userdata.AdminEmpNum = null;
                        userdata.AdminUserNum = null;
                    }
                    if (user.UserStatus == "Lock")
                        userdata.LockedDate = DateTime.Now;
                    if (user.isEmail == true)
                    {
                        userdata.MailDeliveryStatus = true;
                        userdata.MailDeliveryDate = DateTime.Now;
                    }

                    m_baseRepository.Add<AppUser>(userdata);
                    m_baseRepository.SaveChanges();
                    model = 2;                   
                }
            }
            return model;
        }


        // Author        :  HARIHARASUBRAMANIYAN CHANDRASEKARAN
        // Creation Date :  27-Mar-2017 
        /// <summary>
        /// Get User details from Database.
        /// </summary>
        /// <param name="user">Added loggedinusernum as parameter </param>      
        /// <returns>App user object which contains the user details to be displayed</returns> 
        public AppUserModel GetUserInfo(int userNum)
        {
            AppUserModel model = new AppUserModel();
            var appUser = m_baseRepository.GetQuery<AppUser>(new string[] { "AppUserRole", "AppUserStatu", "Employee" }, m => m.UserNum == userNum).FirstOrDefault();
            model.UserID = appUser.UserID;
            model.UserNum = appUser.UserNum;
            model.EmployeeID = appUser.Employee.EmployeeID;
            model.FirstName = appUser.FirstName;
            model.LastName = appUser.LastName;
            model.PreferredName = appUser.PreferredName;
            model.EmailID = appUser.EmailID;
            model.UserRoleNum = appUser.UserRoleNum;
            model.UserRole = appUser.AppUserRole.KeyName;
            model.AppUserStatusID = appUser.AppUserStatu.AppUserStatusID;
            model.UserStatus = appUser.AppUserStatu.UserStatus.Trim();
            model.EmployeeNum = appUser.Employee.EmployeeNum;
            model.UserName = appUser.FirstName;
            model.IsAdminAccess = appUser.IsAdminAccess??false;
            return model;
        }


        #endregion
        #region Remote Validations
        // Author        :  HARIHARASUBRAMANIYAN CHANDRASEKARAN
        // Creation Date :  10-Mar-2017
        /// <summary>
        /// This method is used to get the user based specified UserID
        /// </summary>
        /// <param name="UserID">UserID to get the User</param>
        /// <returns>returns the appuser object containing the specified UserID</returns>
        public AppUser GetUser(string userId)
        {
            return m_baseRepository.GetQuery<AppUser>(user => user.UserID == userId || user.UserID == null).FirstOrDefault();
        }
        public bool UserDuplicateValidation(string userId, int userNum)
        {
            string  oldUserId = m_baseRepository.GetQuery<AppUser>(user => user.UserNum == userNum).Select(x=>x.UserID).FirstOrDefault();
            if(oldUserId==userId)
            {
                return true;
            }
            else
            return m_baseRepository.GetQuery<AppUser>(user => user.UserID == userId || user.UserID == null).FirstOrDefault() ==null ? true :false;
        }
       

        // Author        :  HARIHARASUBRAMANIYAN CHANDRASEKARAN
        // Creation Date :  11-Mar-2017
        /// <summary>
        /// This method is used to get the user based specified EmployeeID
        /// </summary>
        /// <param name="UserID">EmployeeID to get the User</param>
        /// <returns>returns the appuser object containing the specified EmployeeID</returns>
        public Employee GetEmployee(string employeeId)
        {
            return m_baseRepository.GetQuery<Employee>(m => m.EmployeeID == employeeId || employeeId.Length < 0).FirstOrDefault();

        }


        #endregion
        #region Export and Email

        public async Task<DataTable> GetExportUserData(int userNum)
        {
            SqlParameter[] parameters = { new SqlParameter("@UserNum", userNum)};
            return await m_baseRepository.GetDataTableFromStoredProcedure("[Common].[USP_UA_GET_UserExport]", parameters);
        }
        // Author        :  HARIHARASUBRAMANIYAN CHANDRASEKARAN
        // Creation Date :  28-Feb-2017
        /// <summary>
        /// This method is used to get the Email Template to send email.
        /// </summary>
        /// <param name="templateName">Name of the template inorder to get that template</param>
        /// <returns>returns the AppEmail object which contains the template</returns>
        public AppEmail GetEmailTemplate(string templateName)
        {
            return m_baseRepository.GetQuery<AppEmail>(m => m.EmailKey == templateName).FirstOrDefault();
        }
        public AppUser GetUserDetails(int userNum, string userID)
        {
            if (userNum == 0)
                return m_baseRepository.GetQuery<AppUser>(m => m.UserID == userID).FirstOrDefault();
            else
                return m_baseRepository.GetQuery<AppUser>(m => m.UserNum == userNum).FirstOrDefault();

        }

        // Modified By        :  Muthuvel Sabarish
        // Modified Date      :  27-Mar-2017
        // Comment            :  include and check the userStatus
        public List<AppUser> GetYettoLoginUsers()
        {
            int userStatus = m_baseRepository.GetQuery<AppUserStatu>().Where(s => s.UserStatus == "Active").Select(m => m.AppUserStatusID).FirstOrDefault();
            return m_baseRepository.GetQuery<AppUser>().Where(m => m.LastLoginDt == null && m.AppUserStatusID == userStatus).ToList();
        }
        public void UpdateAppEmailSubjectandBody(string emailBody, string emailSubject, string emailTemplate)
        {
            AppEmail updateAppEmail = new AppEmail();
            updateAppEmail = m_baseRepository.GetQuery<AppEmail>().Where(x => x.EmailKey == emailTemplate).FirstOrDefault();
            updateAppEmail.EmailBody = emailBody;
            updateAppEmail.EmailSubject = emailSubject;
            m_baseRepository.Edit<AppEmail>(updateAppEmail);
            m_baseRepository.SaveChanges();
        }


        public IQueryable<SendReminderNotificationModel> GetYetToLoginUsersList(int userNum)
        {                                                   
            IQueryable<SendReminderNotificationModel> yetToLoginUserList = (from userList in m_baseRepository.GetQuery<AppUser>(new string[] { "AppUserRole" }, x => x.AppUserStatu.UserStatus == "Active" && x.LastLoginDt == null && x.UserNum != userNum && x.MarkAsDelete == false && x.UserPassword == null)
                                                                            select new SendReminderNotificationModel
                                                                            {
                                                                                UserId = userList.UserID,
                                                                                UserName = userList.UserName,
                                                                                EmailAddress = userList.EmailID,
                                                                                Role = userList.AppUserRole.UserRole,
                                                                                SecretKey = userList.SecretKey
                                                                            });
            return yetToLoginUserList;
        }


        #endregion
        #endregion
        #region Upload Users Implementation
        // Author        :  Muthuvelsabarish.M
        // Creation Date :  26-April-2017 
        /// <summary>
        //To get user template data
        // </summary>
        // <param name="userNum"> Added usernum as parameter</param>
        // <returns></returns>   
        public async Task<IEnumerable<TemplateDataModel>> GetUserTemplateData(int userNum)
        {
            SqlParameter[] parameters = { new SqlParameter("@UserNum", userNum), new SqlParameter("@TemplateName", "UserData") };
            return await m_baseRepository.GetData<TemplateDataModel>("[Talent].[USP_SLP_GET_LoadedSheetDetails] @UserNum,@TemplateName", parameters);
        }

        // Author        :  Muthuvelsabarish.M
        // Creation Date :  26-April-2017 
        /// <summary>
        //delete UserTemplate Data
        // </summary>
        // <param name="userNum"> Added xmlProcessNum as parameter</param>
        // <returns></returns> 
        public async Task<int> DeleteUserTemplateData(int xmlProcessNum)
        {
            SqlParameter[] parameters = { new SqlParameter("@XmlProcessNum", xmlProcessNum), new SqlParameter("@TemplateName", "UserData") };
            return await m_baseRepository.ExecuteStoredProcedure("[Talent].[USP_SLP_DeleteLoadedSheetDetails]", parameters);
        }

        public async Task<DataTable> GetUserDataTemplate()
        {
            return await m_baseRepository.GetDataTableFromStoredProcedure("[Talent].[USP_SLP_GET_UserDataTemplate]");
        }

        public int UpdateXmlProcess(string sheetName, string fileSourcePath, int recordCount, int userNum)
        {
            XmlProcess xmlprocess = new XmlProcess();
            xmlprocess.ProcessDate = DateTime.Now;
            xmlprocess.RecordCount = recordCount;
            xmlprocess.XmlFilename = sheetName;
            xmlprocess.SheetName = sheetName;
            xmlprocess.FileSourcePath = fileSourcePath;
            xmlprocess.ValidateStatus = 0;
            xmlprocess.ProcessStatus = 0;
            xmlprocess.UserNum = userNum;
            xmlprocess.MetaXmlTemplateNum = m_baseRepository.GetQuery<MetaXmlTemplate>(x => x.TemplateName == "UserData").Select(x => x.MetaXmlTemplateNum).FirstOrDefault();
            m_baseRepository.Add<XmlProcess>(xmlprocess);
            m_baseRepository.SaveChanges();
            return xmlprocess.XmlProcessNum;
        }
        public async Task<bool> InsertStagingTableData(DataTable userData, string DbName, List<string> columnsName, int xmlProcessNum)
        {
            bool result = await m_baseRepository.SqlBulkInsert(userData, DbName, columnsName) != "" ? false : true;
            SqlParameter[] parameters = { new SqlParameter("@XmlProcessNum", xmlProcessNum),
                                        new SqlParameter("@TableName", "Talent.StagingUserData") };
            await m_baseRepository.ExecuteStoredProcedure("[Talent].[USP_SLP_PUT_XMLProcessNum]", parameters);
            return result;
        }

        public async Task<int> ValidateUserData()
        {
            var result = await m_baseRepository.GetData<int>("[Talent].[USP_SLP_PUT_UserDataValidation]", new SqlParameter[] { });
            return result.FirstOrDefault();
        }
        public async Task<int> ProcessUserData(string tenantName)
        {
            var tenantNum = m_masterRepository.GetQuery<Master.Tenant>(m => m.TenantURLName == tenantName).Single().TenantNum;
            SqlParameter[] sqlParameters = new SqlParameter[] { new SqlParameter("@tenantNum", tenantNum) };
            var dataTable = await m_baseRepository.GetDataTableFromStoredProcedure("[Talent].[USP_GET_UnProcessedUsers]", sqlParameters);
            
            SqlParameter param = new SqlParameter("@UserList", dataTable);
            param.SqlDbType = SqlDbType.Structured;
            param.Direction = ParameterDirection.Input;
            await m_masterRepository.ExecuteStoredProcedure("USP_PUT_MasterUserList", new SqlParameter[] { param });
            return await m_baseRepository.ExecuteStoredProcedure("[Talent].[USP_SLP_PUT_ImportUserData]");
        }

        public async Task<IEnumerable<TemplateErrorListModel>> GetUserDataErrorList()
        {
            return await m_baseRepository.GetData<TemplateErrorListModel>("[Talent].[USP_SLP_GET_UserErrorData]", new SqlParameter[] { });
        }

        public async Task<DataTable> GetUserDataErrorExport()
        {
            return await m_baseRepository.GetDataTableFromStoredProcedure("[Talent].[USP_SLP_GET_UserDataErrorExport]");
        }

        public async Task<int> GetErrorRecordCount()
        {
            SqlParameter[] parameter = { new SqlParameter("@TableName", "Talent.StagingUserData") };
            var result = await m_baseRepository.GetData<int>("[Talent].[USP_SLP_GET_ErrorRecordCount] @TableName", parameter);
            return result.FirstOrDefault();
        }

        public async Task<DataTable> GetExportXmlFile(int xmlProcessNum)
        {
            SqlParameter[] parameters = { new SqlParameter("@XmlProcessNum", xmlProcessNum),
                                        new SqlParameter("@TableName", "Talent.StagingUserData") };
            return await m_baseRepository.GetDataTableFromStoredProcedure("[Talent].[USP_SLP_GET_LoadedDataExport]", parameters);
        }
        #endregion
















    }
}
