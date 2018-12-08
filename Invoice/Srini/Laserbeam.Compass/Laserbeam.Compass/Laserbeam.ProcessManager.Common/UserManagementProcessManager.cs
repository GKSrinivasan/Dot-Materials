// Copyright (c) 2017 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.
// Component Name  : 	UserManagement Process Manager
// Description     : 	UserManagement related business logics
// Author          :    Karthikeyan Shanmugam
// Creation Date   :    09-Feb-2017
using Laserbeam.BusinessObject.Common;
using Laserbeam.DataManager.Interfaces.Common;
using Laserbeam.ProcessManager.Interfaces.Common;
using System;
using System.Collections.Generic;
using System.Data;
using Laserbeam.Libraries.Common;
using System.Web;
using System.Threading.Tasks;
using Laserbeam.Constant.HR;
using System.Linq;
using Laserbeam.Libraries.Interfaces.Common;
using System.Data.OleDb;
using Laserbeam.EntityManager.Common;

namespace Laserbeam.ProcessManager.Common
{
    public class UserManagementProcessManager : IUserManagementProcessManager
    {
        #region Fields
        IUserManagementRepository m_usermanagementRepository;
        private readonly IEmail m_email;
        #endregion

        #region Constructors
        // Author        :  Karthikeyan Shanmugam
        // Creation Date :  09-Feb-2017
        /// <summary>
        /// Initializes objects used in this class
        /// </summary>
        /// <param name="budgetRepository">compensationRepository objcets</param>        
        public UserManagementProcessManager(IUserManagementRepository usermanagementRepository, IEmail email)
        {
            m_usermanagementRepository = usermanagementRepository;
            m_email = email;

        }
        #endregion

        // Author        :  Karthikeyan Shanmugam
        // Creation Date :  09-Feb-2017
        /// <summary>
        /// This Method is used to get the list user roles to bind to dropdown.
        /// and  Added usernum as parameter
        /// </summary>
        /// <returns>returns user role collection list</returns>        
        public IQueryable<AppUserRoleModel> GetDropdownUserRoles(int userNum)
        {
            return m_usermanagementRepository.GetDropdownUserRoles(userNum);
        }

        // Author        :  Karthikeyan Shanmugam
        // Creation Date :  09-Feb-2017
        /// <summary>
        /// It invokes deleting the user
        /// </summary>
        /// <param name="id">The ID belongs to the user to be deleted</param>
        /// <returns>returns deleted status</returns>
        public async Task<bool> DeleteUser(string id, int userNum, string tenantName)
        {
            return await m_usermanagementRepository.DeleteUser(id, userNum,tenantName);
        }


       
        public async Task<IEnumerable<AppUserDataModel>> GetUserGridData(int userNum)
        {
            return await m_usermanagementRepository.GetUserGridData(userNum);
        }

        // Author        :  Karthikeyan Shanmugam
        // Creation Date :  07-Mar-2017
        /// <summary>
        /// This Method is used to get the user count details 
        /// </summary>
        /// <returns>returns user count</returns>        
      public async  Task<IEnumerable<AppUsers>> GetUserCount(int userNum)
        {
            return await m_usermanagementRepository.GetUserCount(userNum);
        }

        //// Author        :  Karthikeyan Shanmugam
        //// Creation Date :  11-Mar-2017 
        ///// <summary>
        ///// This Method is used to get the list user status to bind to dropdown.
        ///// </summary>
        ///// <returns>returns user status collection list</returns>
        //public IEnumerable<AppUserStatusModel> GetUserStatus()
        //{
        //    return m_usermanagementRepository.GetUserStatus();
        //}
        // Author        :  Karthikeyan Shanmugam
        // Creation Date :  10-Mar-2017
        /// <summary>
        /// This Method is used to validate the user ID is alredy exist or not 
        /// </summary>
        /// <returns>returns validated result</returns>   
        public bool UserValidation(string userId)
        {
            var user = m_usermanagementRepository.GetUser(userId);
            return (user == null) ? true : false;
        }

       public  bool UserDuplicateValidation(string userId, int userNum)
        {
            var user = m_usermanagementRepository.UserDuplicateValidation(userId,userNum);
            return user;
        }

        // Author        :  Karthikeyan Shanmugam
        // Creation Date :  11-Mar-2017
        /// <summary>
        /// This Method is used to validate the EmployeeID 
        /// </summary>
        /// <returns>returns validated result</returns>   
        public bool EmployeeIdValidation(string employeeId)
        {
            var employee = m_usermanagementRepository.GetEmployee(employeeId);
            return (employee == null) ? false : true;
        }
        // Author        :  Karthikeyan Shanmugam
        // Creation Date :  28-Feb-2017
        /// <summary>
        /// Invokes Inserting User details to Database.
        /// </summary>
        /// /// <param name="user">Added userNum as parameter</param>  
        /// <returns>App user object which contains the user details to be inserted</returns>                
        public async Task<int> AddUser(AppUserModel user, int userNum, string tenantName)
        {
            return await m_usermanagementRepository.AddUser(user, userNum, tenantName);
        }

        // Author        :  Karthikeyan Shanmugam
        // Creation Date :  28-Feb-2017
        /// <summary>
        /// Get User details from Database
        /// </summary>
        /// /// <param name="user">Added userNum as parameter</param>  
        /// <returns>App user object which contains the user details </returns>
        public AppUserModel GetUserInfo(int userNum)
        {
            return m_usermanagementRepository.GetUserInfo(userNum);
        }
        public async Task<DataTable> GetExportUserData(int userNum)
        {
            return await m_usermanagementRepository.GetExportUserData(userNum);
        }

        public async Task<bool> SendEmailToUser(string requestURL, AppConfigModel appConfigModel)
        {
            int mailSentStatus = 0;
            //AppConfigModel appConfigModel = m_sessionRepository.GetAppSetting();
            string templateName = "Welcome to Compass";
            AppEmail newUserEmailTemplateNumber = m_usermanagementRepository.GetEmailTemplate(templateName);
            List<AppUser> users = new List<AppUser>();
            users = m_usermanagementRepository.GetYettoLoginUsers();
            List<EmailDetails> emailDetails = new List<EmailDetails>();
            string siteUrl = requestURL;
            bool isEncrypted = false;
            bool isSingleUse = (templateName == "Welcome to Compass");
            string encryptedMagicKey = string.Empty;
            string referenceUrl = isEncrypted ? siteUrl + "?mayaLink=" + HttpUtility.UrlEncode(encryptedMagicKey) : siteUrl;


            if (newUserEmailTemplateNumber != null)
            {
                foreach (var item in users)
                {
                    emailDetails = (from n in users
                                    select new EmailDetails
                                    {
                                        ToEmailID = n.EmailID,
                                        EmailSubject = newUserEmailTemplateNumber.EmailSubject,
                                        EmailBody =
                              Convert.ToString(newUserEmailTemplateNumber.EmailBody).Replace(AppEmailConstants.AtUsername, item.UserName).
                              Replace("@BeginUrlButton", "<a href='" + referenceUrl + "' style='font-family:Calibri;font-size:20px;background-color: #337ab7;color: white;border: 1px solid #2e6da4;text-decoration:none;padding:5px 20px;border-radius:5px;'>")
                              .Replace("@EndUrlButton", "</a>"),
                                        FromEmailID = appConfigModel.AdminEmailID
                                    }).ToList();
                    mailSentStatus = await m_email.SendEmailAsync(appConfigModel, emailDetails, true);
                }
            }
            return mailSentStatus>=0;

        }

        // Author       : Shaheena Shaik
        // Creation Date: 10-April-2017
        /// <summary>
        /// 
        /// </summary>
        /// <param name="appConfigModel"></param>
        /// <param name="sendReminderMail"></param>
        /// <param name="emailBody"></param>
        /// <param name="emailSubject"></param>
        /// <returns></returns>
        public async Task<bool> SendEmailReminderToUser(AppConfigModel appConfigModel, List<SendReminderNotificationModel> sendReminderMail, string emailBody, string emailSubject, string requestURL)
        {

            int mailSentStatus = 0;
            string templateName = "Welcome to Compass";
            emailBody = emailBody.Replace("display:none;", "display:inline;");
            m_usermanagementRepository.UpdateAppEmailSubjectandBody(emailBody, emailSubject, templateName);
            AppEmail newUserEmailTemplateNumber =  m_usermanagementRepository.GetEmailTemplate(templateName);
            EmailDetails emailDetails = new EmailDetails();
            string siteUrl = requestURL;
            bool isEncrypted = false;
            bool isSingleUse = (templateName == "Welcome to Compass");
            string encryptedMagicKey = string.Empty;
            foreach (var item in sendReminderMail)
            {
                if (!(string.IsNullOrWhiteSpace(item.UserId) || string.IsNullOrWhiteSpace(item.SecretKey)))
                {
                    DateTime epochStart = new DateTime(1970, 01, 01, 0, 0, 0, 0, DateTimeKind.Utc);
                    TimeSpan timeSpan = DateTime.UtcNow - epochStart;
                    string requestTimeStamp = (isSingleUse) ? "" : Convert.ToUInt64(timeSpan.TotalSeconds).ToString();
                    isEncrypted = MayaLink.TryEncrypt(item.UserId + '|' + item.SecretKey + '|' + requestTimeStamp, out encryptedMagicKey);
                }

                string referenceUrl = isEncrypted ? siteUrl + "?mayaLink=" + HttpUtility.UrlEncode(encryptedMagicKey) : siteUrl;



                emailDetails.ToEmailID = item.EmailAddress;
                emailDetails.EmailSubject = newUserEmailTemplateNumber.EmailSubject;
                emailDetails.EmailBody =
        Convert.ToString(newUserEmailTemplateNumber.EmailBody).Replace(AppEmailConstants.AtUsername, item.UserName).Replace(AppEmailConstants.AtUserID,item.UserId)
        .Replace("@BeginUrlButton", "<a href='" + referenceUrl + "' style='font-family:Calibri;font-size:20px;background-color: #337ab7;color: white;border: 1px solid #2e6da4;text-decoration:none;padding:5px 20px;border-radius:5px;'>")
        .Replace("@EndUrlButton", "</a>");
                emailDetails.FromEmailID = appConfigModel.AdminEmailID;
                mailSentStatus = await m_email.SendEmailAsync(appConfigModel, emailDetails, true);
            }
            return mailSentStatus>=0;
        }

        public UserManagementEmailDetails GetMailDetails()
        {
            UserManagementEmailDetails emailDetails = new UserManagementEmailDetails();
            string templateName = "Welcome to Compass";
            AppEmail emailTemplate = m_usermanagementRepository.GetEmailTemplate(templateName);
            emailDetails.EmailBody = emailTemplate.EmailBody.Replace("display:inline;", "display:none;");
            emailDetails.EmailSubject = emailTemplate.EmailSubject;
            return emailDetails;


        }
        public async Task<bool> SendWelcomeEmailToUser(string requestURL, int userNum, string userID, AppConfigModel appConfigModel)
        {
            int mailSentStatus = 0;
            string templateName = "Welcome to Compass";
            AppEmail newUserEmailTemplateNumber = m_usermanagementRepository.GetEmailTemplate(templateName);
            AppUser manageUser = m_usermanagementRepository.GetUserDetails(userNum, userID);
            string userName = (manageUser.PreferredName == null) ? manageUser.UserName : manageUser.PreferredName;
            EmailDetails emailDetails = new EmailDetails();
            string siteUrl = requestURL;
            bool isEncrypted = false;
            bool isSingleUse = (templateName == "Welcome to Compass");
            string encryptedMagicKey = string.Empty;
            if (!(string.IsNullOrWhiteSpace(manageUser.UserID) || string.IsNullOrWhiteSpace(manageUser.SecretKey)))
            {
                DateTime epochStart = new DateTime(1970, 01, 01, 0, 0, 0, 0, DateTimeKind.Utc);
                TimeSpan timeSpan = DateTime.UtcNow - epochStart;
                string requestTimeStamp = (isSingleUse) ? "" : Convert.ToUInt64(timeSpan.TotalSeconds).ToString();
                isEncrypted = MayaLink.TryEncrypt(manageUser.UserID + '|' + manageUser.SecretKey + '|' + requestTimeStamp, out encryptedMagicKey);
            }

            string referenceUrl = isEncrypted ? siteUrl + "?mayaLink=" + HttpUtility.UrlEncode(encryptedMagicKey) : siteUrl;
            if (newUserEmailTemplateNumber != null)
            {
                string Password = string.Empty;
                emailDetails.ToEmailID = manageUser.EmailID;
                emailDetails.EmailSubject = newUserEmailTemplateNumber.EmailSubject;
                emailDetails.EmailBody =
                    Convert.ToString(newUserEmailTemplateNumber.EmailBody).Replace(AppEmailConstants.AtUsername, userName).
                    Replace(AppEmailConstants.AtUserID, userID).
                    Replace("@BeginUrlButton", "<a href='" + referenceUrl + "' style='font-family:Calibri;font-size:20px;background-color: #337ab7;color: white;border: 1px solid #2e6da4;text-decoration:none;padding:5px 20px;border-radius:5px;'>")
                    .Replace("@EndUrlButton", "</a>");

                emailDetails.FromEmailID = appConfigModel.AdminEmailID;
                mailSentStatus = await m_email.SendEmailAsync(appConfigModel, emailDetails, true);
            }
            return mailSentStatus == 1 ? true : false;

        }

        public async Task<IEnumerable<TemplateDataModel>> GetUserTemplateData(int userNum)
        {
            return await m_usermanagementRepository.GetUserTemplateData(userNum);
        }

        public async Task<int> DeleteUserTemplateData(int xmlProcessNum)
        {
            return await m_usermanagementRepository.DeleteUserTemplateData(xmlProcessNum);
        }


        public async Task<DataTable> GetUserDataTemplate()
        {
            return await m_usermanagementRepository.GetUserDataTemplate();
        }



        public int UpdateXmlProcess(string sheetName, string fileSourcePath, int recordCount, int userNum)
        {
            return m_usermanagementRepository.UpdateXmlProcess(sheetName, fileSourcePath, recordCount, userNum);
        }

        public async Task<bool> InsertStagingTableData(DataTable userData, string DbName, List<string> columnsName, int xmlProcessNum)
        {
            return await m_usermanagementRepository.InsertStagingTableData(userData, DbName, columnsName, xmlProcessNum);
        }

        public async Task<int> ValidateUserData()
        {
            return await m_usermanagementRepository.ValidateUserData();
        }

        public async Task<int> ProcessUserData(string tenantName)
        {
          return await  m_usermanagementRepository.ProcessUserData(tenantName);
        }


        public async Task<DataTable> GetUserDataErrorExport()
        {
            return await m_usermanagementRepository.GetUserDataErrorExport();
        }

        public async Task<List<TemplateErrorListModel>> GetUserDataErrorList()
        {
            var result = await m_usermanagementRepository.GetUserDataErrorList();
            return (from x in result
                    select new TemplateErrorListModel
                    {
                        Error = x.Error,
                        AffectedData = x.AffectedData,
                        HowToFix = x.HowToFix
                    }).ToList();

        }

        public async Task<int> GetErrorRecordCount()
        {
            return await m_usermanagementRepository.GetErrorRecordCount();
        }

        public async Task<DataTable> GetExportXmlFile(int xmlProcessNum)
        {
            return await m_usermanagementRepository.GetExportXmlFile(xmlProcessNum);
        }

        #region Excel

        #region Variable Declaration

        private OleDbConnection cn;
        private OleDbDataAdapter daAdapter;

        //private string ExcelCon = @"Provider=Microsoft.Jet.OLEDB.4.0;";
        private string strConnectionString;

        //private string strParseError = "";
        private string SheetName, Range;

        #endregion

        // Author         :   Raja Ganapathy
        // Creation Date  :   30-Mar-2017 
        /// <summary>
        /// Initialize ExcelXML component using the sepecifed File name, By default HDR property will be false.
        /// </summary>
        /// <param name="strFileName"></param>
        public void InitializeConnection(string filePath)
        {
            strConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + (filePath) + ";Extended Properties=\"Excel 12.0;HDR=YES;\"";
            cn = new OleDbConnection(strConnectionString);
        }

        #region Excel File Info

        public String[] GetExcelSheetNames()
        {
            System.Data.DataTable dt = null;
            try
            {
                cn.Open();
                // Get the data table containing the schema
                dt = cn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                if (dt == null)
                {
                    return null;
                }

                String[] excelSheets = new String[dt.Rows.Count];
                int i = 0;
                // Add the sheet name to the string array.
                foreach (DataRow row in dt.Rows)
                {
                    string strSheetTableName = row["TABLE_NAME"].ToString();
                    excelSheets[i] = strSheetTableName.Replace("'", "").Split('$')[0];
                    break;
                }
                return excelSheets;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                // Clean up.
                cn.Close();
            }
        }

        public IQueryable<SendReminderNotificationModel> GetYetToLoginUsersList(int userNum)
        {
            return m_usermanagementRepository.GetYetToLoginUsersList(userNum);
        }


        public DataTable GetDataTable(string strSheetName)
        {
            try
            {
                string strComand;
                if (strSheetName.IndexOf("|") > 0)
                {
                    SheetName = strSheetName.Substring(0, strSheetName.IndexOf("|"));
                    Range = strSheetName.Substring(strSheetName.IndexOf("|") + 1);
                    strComand = "select * from [" + SheetName + "$" + Range + "]";
                }
                else
                {
                    strComand = "select * from [" + strSheetName + "$]";
                }
                daAdapter = new OleDbDataAdapter(strComand, cn);
                DataTable dt = new DataTable("Table");
                daAdapter.FillSchema(dt, SchemaType.Source);
                daAdapter.Fill(dt);
                cn.Close();
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


    }
        #endregion
        #endregion
}


