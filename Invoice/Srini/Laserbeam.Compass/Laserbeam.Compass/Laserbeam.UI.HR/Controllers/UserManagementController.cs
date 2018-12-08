// Copyright (c) 2015 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.
// Component Name  :    UserManagement Controller
// Description     : 	UserManagement Methods
// Author          :	HARIHARASUBRAMANIYAN CHANDRASEKARAN		
// Creation Date   : 	FEB-01-2017
using Laserbeam.UI.HR.Common;
using System.Collections.Generic;
using Laserbeam.ProcessManager.Interfaces.Common;
using System.Linq;
using System.Web.Mvc;
using Laserbeam.BusinessObject.Common;
using Laserbeam.Constant.HR;
using Kendo.Mvc.UI;
using System;
using Kendo.Mvc.Extensions;
using Laserbeam.UI.HR.Models;
using System.Threading.Tasks;
using System.Web;
using System.IO;
using System.Data;

namespace Laserbeam.UI.HR.Controllers
{
    [AuthorizeUser(UserRoleConstants.AllAdminRoles)]
    public class UserManagementController : Controller
    {

        #region Fields
        private SessionManager m_sessionManager;
        private IUserManagementProcessManager m_usermanagementProcessManager;
        private readonly IAccountProcessManager m_accountProcessManager;
        #endregion
        #region Constructor
        // Author        :  HARIHARASUBRAMANIYAN CHANDRASEKARAN	
        // Creation Date :  09-Feb-2017            
        /// <summary>
        /// Initializes objects used in this class
        /// </summary>                
        /// <param name="compensationProcessManager">usermanagementProcessManager objects</param>        
        public UserManagementController(IUserManagementProcessManager usermanagementProcessManager, SessionManager sessionManager, IAccountProcessManager accountProcessManager)
        {
            m_usermanagementProcessManager = usermanagementProcessManager;
            m_sessionManager = sessionManager;
            m_accountProcessManager = accountProcessManager;
        }
        #endregion
        #region UserList Implementation 
        #region Page Implementation and CURD Operations 
        // Author        :  HARIHARASUBRAMANIYAN CHANDRASEKARAN		
        // Creation Date :  27-Feb-2017 
        /// <summary>
        /// It Gets the User count Details to show on the tile and  Gets usernum from user session
        /// </summary>        
        /// <returns>returns the view</returns>  
        // GET: UserManagement
        [HttpGet]
        public ActionResult Home()
        {
            UserModel userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
            // var users =await GetUserCount(userModel.UserNum).Result);
            // Task.Run(() => m_workForceProcessManager.GetEmployeeDataErrorCount()).Result;
            return View();
        }

        [HttpGet]
        public PartialViewResult _UserDetails()
        {
            ViewBag.AccessControlErrorCount = Task.Run(() => m_usermanagementProcessManager.GetErrorRecordCount()).Result;
            return PartialView();
        }

        // Author        :  HARIHARASUBRAMANIYAN CHANDRASEKARAN		
        // Creation Date :  27-Feb-2017 
        /// <summary>
        /// It Gets the User Add Popup
        /// </summary>        
        /// <returns>returns the view</returns>  
        // GET: UserManagement
        [HttpGet]
        public PartialViewResult _UserAddPopUp()
        {
            AddUserModel model = new AddUserModel();
            model.UserStatus = "Active";
            return PartialView(model);
        }

        // Author        :  HARIHARASUBRAMANIYAN CHANDRASEKARAN
        // Creation Date :  27-Mar-2017 
        /// <summary>
        /// To Gets the User Details 
        /// </summary>        
        /// <returns>returns the view</returns>
        [HttpGet]
        public PartialViewResult _UserModifyPopUp(AppUserModel data)
        {
            AppUserModel user = new AppUserModel();
            user.UserNum = data.UserNum;
            var userData = m_usermanagementProcessManager.GetUserInfo(user.UserNum);
            AddUserModel model = new AddUserModel();
            model.UserNum = userData.UserNum;
            model.UserID = userData.UserID;
            model.EmployeeID = userData.EmployeeID;
            model.FirstName = userData.FirstName;
            model.LastName = userData.LastName;
            model.PreferredName = userData.PreferredName ?? "";
            //model.EmailID = userData.EmailID;
            model.UserRoleNum = userData.UserRoleNum;
            model.UserStatus = userData.UserStatus;
            model.UserRole = userData.UserRole;
            model.UserRoleType = userData.UserRole;
            model.IsAdminAccess = userData.IsAdminAccess;
            return PartialView("_UserAddPopUp", model);
        }

        // Author        :  HARIHARASUBRAMANIYAN CHANDRASEKARAN
        // Creation Date :  27-Mar-2017 
        /// <summary>
        /// To Gets the User Details 
        /// </summary>        
        /// <returns>returns to Grid</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxChildActionOnly]
        public async Task<JsonResult> GetDataSourceForGrid([DataSourceRequest]DataSourceRequest request)
        {
            UserModel userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
            int employeeNum = userModel.EmployeeNum;
            var result = await m_usermanagementProcessManager.GetUserGridData(userModel.UserNum);
            return Json(result.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        //TO DO
        // Author        :  HARIHARASUBRAMANIYAN CHANDRASEKARAN
        // Creation Date :  27-Mar-2017 
        /// <summary>
        /// Gets the User Tile Information
        /// </summary>        
        /// <returns>returns to Grid</returns>
        public async Task<JsonResult> GetUserTileData()
        {
            UserModel userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
            int employeeNum = userModel.EmployeeNum;
            var users = await m_usermanagementProcessManager.GetUserCount(userModel.UserNum);
            return Json(users, JsonRequestBehavior.AllowGet);
        }

        // Author        :  HARIHARASUBRAMANIYAN CHANDRASEKARAN
        // Creation Date :  16-Mar-2017 
        /// <summary>
        /// Delete the User Details from the grid 
        /// </summary>        
        /// <returns>returns Json Result </returns>     
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxChildActionOnly]
        public async Task<JsonResult> DeleteUser(AppUserModel user, int userNum)
        {
             await m_usermanagementProcessManager.DeleteUser(user.UserID, user.UserNum, this.GetTenant());
            string message = "Deleted Successfully";
            return Json(message, JsonRequestBehavior.AllowGet);
        }



        // Author        :  HARIHARASUBRAMANIYAN CHANDRASEKARAN
        // Creation Date :  21-Mar-2017 
        /// <summary>
        /// It returns the columns of the grid to be filtered.
        /// </summary>
        /// <returns>returns PartialViewResult</returns>
        [HttpGet]
        public PartialViewResult _FilterSortPopup()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem { Text = "First Name", Value = "FirstName" });
            list.Add(new SelectListItem { Text = "Last Name", Value = "LastName" });
            list.Add(new SelectListItem { Text = "User Role", Value = "UserRole" });
            list.Add(new SelectListItem { Text = "User Status", Value = "UserStatus" });
            list.Add(new SelectListItem { Text = "Email", Value = "EmailID" });
            list.Add(new SelectListItem { Text = "Employee ID", Value = "EmployeeID" });

            return PartialView(list);
        }

        // Author        :  Karthikeyan Shanmugam
        // Creation Date :  27-Feb-2017 
        /// <summary>
        /// It Gets the list of User Details to bind to User Roles Dropdown
        /// </summary>
        /// <returns>returns Json Result to bind User Roles Dropdown</returns>        
        // Comment        :   Gets usernum from user session
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxChildActionOnly]
        public JsonResult GetDropdownUserRoles()
        {
            UserModel userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);

            var ddlData = m_usermanagementProcessManager.GetDropdownUserRoles(userModel.UserNum).Select(x => new { UserRoleNum = x.UserRoleNum, UserRole = x.UserRole });
            return Json(ddlData, JsonRequestBehavior.AllowGet);
        }



        // Author        :  HARIHARASUBRAMANIYAN CHANDRASEKARAN
        // Creation Date :  27-Feb-2017 
        /// <summary>
        /// Handles Add User functionality from ManageUser Screen
        /// </summary>
        /// <param name="user">Collection object contains User details</param>
        /// <returns>returns ActionResult</returns>             
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxChildActionOnly]
        public async Task<JavaScriptResult> _UserAddPopup(AddUserModel data, string isEmail)
        {
            UserModel userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
            AppUserModel user = new AppUserModel();
            user.UserID = data.UserID;
            user.EmployeeID = data.EmployeeID;
            user.FirstName = data.FirstName;
            user.LastName = data.LastName;
            user.PreferredName = data.PreferredName;
            user.EmailID = data.UserID;
            user.UserRoleNum = data.UserRoleNum ?? 0;
            user.UserStatus = data.UserStatus;
            user.UserNum = data.UserNum;
            user.isEmail = isEmail == "true" ? true : false;
            user.IsAdminAccess = data.IsAdminAccess;
            var addUser = await m_usermanagementProcessManager.AddUser(user, userModel.UserNum, this.GetTenant());
            //if ((addUser == 2 && isEmail == "true") || (addUser == 1 && isEmail == "true"))
            if (addUser !=0 && isEmail == "true")
              await SendWelcomeEmailToUser(data.UserNum, data.UserID);
            return JavaScript("closeAfterUserAdded('" + addUser + "');"); 
        }

        #endregion

        #region Remote Vaidation Methods
        // Author        :  HARIHARASUBRAMANIYAN CHANDRASEKARAN
        // Creation Date :  10-Mar-2017 
        /// <summary>
        /// To check userID is already Exist Or Not
        /// </summary>
        ///  <returns>returns Json Result </returns>
        [HttpPost]
        public JsonResult UserValidation(AddUserModel model)
        {
            if (model.UserNum == 0)
            {
                return Json(m_usermanagementProcessManager.UserValidation(model.UserID));
                
            }
            else
            {
                return Json(m_usermanagementProcessManager.UserDuplicateValidation(model.UserID, model.UserNum));
                
            }
        }

        // Author        :  HARIHARASUBRAMANIYAN CHANDRASEKARAN
        // Creation Date :  11-Mar-2017 
        /// <summary>
        /// To check EmployeeID is Valid Or Not
        /// </summary>
        ///  <returns>returns Json Result </returns>
        [HttpPost]
        public JsonResult EmployeeIdValidation(AddUserModel model)
        {
            bool data = m_usermanagementProcessManager.EmployeeIdValidation(model.EmployeeID);
            if (data == true)
            {

                return Json(true);
            }

            else
                return Json(false);
        }
        #endregion

        #region Export and Email
        // Author        :  HARIHARASUBRAMANIYAN CHANDRASEKARAN
        // Creation Date :  27-Feb-2017 
        /// <summary>
        /// AppUser Export 
        /// </summary>
        /// <param name="user">Collection object contains User details</param>
        /// <returns>returns ActionResult</returns> 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task AppUserExport()
        {
            UserModel userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
            string exportName = "Manage Users - User Extract - " + DateTime.Now.ToShortDateString().FormatWith("{0: MM-dd-yyyy}");
            string sheetName = "User Information";
            var exportData = await m_usermanagementProcessManager.GetExportUserData(userModel.UserNum);
            ExportExcel.ToExcel(exportData, exportName, sheetName);
        }

        // Author        :  HARIHARASUBRAMANIYAN CHANDRASEKARAN
        // Creation Date :  27-Feb-2017 
        /// <summary>
        /// AppUser Export 
        /// </summary>
        /// <param name="user">Collection object contains User details</param>
        /// <returns>returns ActionResult</returns> 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> SendMailToUser()
        {
            string requestURL = this.GetTenantUrl();
            AppConfigModel appConfigModel = m_accountProcessManager.GetAppSetting();
            bool count = await m_usermanagementProcessManager.SendEmailToUser(requestURL, appConfigModel);
            return Json((count) ? 1 : 0);

        }
        
        // Author        :  HARIHARASUBRAMANIYAN CHANDRASEKARAN
        // Creation Date :  27-Feb-2017 
        /// <summary>
        /// SendEmail
        /// </summary>
        /// <returns>returns ActionResult</returns> 
        [HttpGet]
        public PartialViewResult _SendEmail()
        {
            UserManagementEmailDetails emailDetails = new UserManagementEmailDetails();
            emailDetails = m_usermanagementProcessManager.GetMailDetails();
            return PartialView(emailDetails);
        }
        // Author        :  HARIHARASUBRAMANIYAN CHANDRASEKARAN
        // Creation Date :  27-Feb-2017 
        /// <summary>
        /// Get Yet To Login Users Count
        /// </summary>
        /// <param name="user">Collection object contains User details</param>
        /// <returns>returns ActionResult</returns>       
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxChildActionOnly]
        public JsonResult GetYetToLoginUsersList([DataSourceRequest]DataSourceRequest request)
        {
            UserModel userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
            IQueryable<SendReminderNotificationModel> yetToLoginUsersList = m_usermanagementProcessManager.GetYetToLoginUsersList(userModel.UserNum);
            return Json(yetToLoginUsersList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        //NOTE: Boobalan: Completed passing Anti-ForgeryToken
        [HttpPost]
        [AjaxChildActionOnly]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> SendEmailReminderToUser(List<SendReminderNotificationModel> sendReminderMail, string emailBody, string emailSubject)
        {
            string requestURL = this.GetTenantUrl();
            var emailContent = HttpUtility.HtmlDecode(Convert.ToString(emailBody));
            AppConfigModel appConfigModel = m_accountProcessManager.GetAppSetting();
            bool count = await m_usermanagementProcessManager.SendEmailReminderToUser(appConfigModel, sendReminderMail, emailContent, emailSubject, requestURL);
            return Json((count) ? 1 : 0);

        }
        #endregion

        #endregion
        #region Upload Users Implementation

        [HttpGet]
        public PartialViewResult _GetUserDataErrorList()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxChildActionOnly]
        public async Task<JsonResult> GetUserTemplateData([DataSourceRequest]  DataSourceRequest request)
        {
            UserModel userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
            var result = await m_usermanagementProcessManager.GetUserTemplateData(userModel.UserNum);
            return Json(result.ToDataSourceResult(request));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxChildActionOnly]
        public async Task<JsonResult> DeleteUserTemplateData(int xmlProcessNum)
        {
            var message = "";
            int status = await m_usermanagementProcessManager.DeleteUserTemplateData(xmlProcessNum);
            if (status > 0)
            {
                message = "Deleted Successfully";
            }
            return Json(message, JsonRequestBehavior.AllowGet);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task GetUserDataTemplate()
        {
            var exportData = await m_usermanagementProcessManager.GetUserDataTemplate();
            ExportExcel.ToExcel(exportData, "User Data Template");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxChildActionOnly]
        public async Task<JsonResult> GetUserDataErrorList([DataSourceRequest]  DataSourceRequest request)
        {
            var result = await m_usermanagementProcessManager.GetUserDataErrorList();
            return Json(result.ToDataSourceResult(request));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task GetUserDataErrorExport()
        {
            var exportData = await m_usermanagementProcessManager.GetUserDataErrorExport();
            ExportExcel.ToExcel(exportData, "User Data - Error");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Upload()
        {
            UserModel userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
            string message = "";
            string[] sheetname = new string[100];
            DataTable sheetData = new DataTable();
            string fileName = "";
            string isLastFile = HttpContext.Request.Form["isLastFile"];
            HttpPostedFileBase fileContent = null;
            if (Request.Files.Count > 0)
                fileContent = Request.Files[0];
            try
            {
                if (fileContent != null)
                {
                    fileName = Server.MapPath("~/UserData/" + Path.GetFileName(fileContent.FileName));
                    fileContent.SaveAs(fileName);
                    m_usermanagementProcessManager.InitializeConnection(fileName);
                    sheetname = m_usermanagementProcessManager.GetExcelSheetNames();
                }
            }
            catch (Exception)
            {
                message = "Invalid format or Password protected";
            }
            if (message != "Invalid format or Password protected" && sheetname.Length > 0)
            {

                sheetData = m_usermanagementProcessManager.GetDataTable(sheetname[0]);
                if (sheetData.Rows.Count > 0)
                {
                    List<string> columns = sheetData.Columns.Cast<DataColumn>().Where(x => x.ColumnName != "Error" && x.ColumnName != "UploadedBy" && x.ColumnName != "SheetName").Select(x => x.ColumnName).ToList();
                    List<string> udTemplateColumns = GetUserDataTemplateColumns();
                    var templateColumns = (from userDataColumn in udTemplateColumns
                                           from sheetColumns in columns
                                           where string.Equals(userDataColumn, sheetColumns, StringComparison.InvariantCultureIgnoreCase)
                                           select userDataColumn).ToList();

                    if (columns.Count() == templateColumns.Count())
                    {
                        int xmlProcessNum = m_usermanagementProcessManager.UpdateXmlProcess(Path.GetFileName(fileContent.FileName), fileName, sheetData.Rows.Count, userModel.UserNum);
                        bool result = await m_usermanagementProcessManager.InsertStagingTableData(sheetData, "Talent.StagingUserData", columns, xmlProcessNum);
                        if (result)
                            message = "Uploaded successfully";
                    }
                    else
                    {
                        List<string> templatemisMatchColumns = columns.Except(templateColumns).ToList();
                        message = String.Join(", ", templatemisMatchColumns);
                    }

                }
            }
            if (isLastFile != null && isLastFile != "")
            {
                if (await m_usermanagementProcessManager.ValidateUserData() > 0)
                {
                    await m_usermanagementProcessManager.ProcessUserData(this.GetTenant());
                    message = (message != "Invalid format or Password protected") ? "Processed successfully" : message;
                }
            }
            return Json(new { Message = message }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxChildActionOnly]
        public async Task<JsonResult> GetUserDataErrorCount()
        {
            return Json(await m_usermanagementProcessManager.GetErrorRecordCount(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task GetExportXmlFile(int xmlProcessNum)
        {
            var exportData = await m_usermanagementProcessManager.GetExportXmlFile(xmlProcessNum);
            ExportExcel.ToExcel(exportData, "User Data");
        }

        [HttpGet]
        public PartialViewResult _SendEmail_FilterSortPopup()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem { Text = "Email Address", Value = "EmailAddress" });
            list.Add(new SelectListItem { Text = "Role", Value = "Role" });
            return PartialView(list);
        }

        #endregion
        #region Private Methods Implementation 
        // Author        :  HARIHARASUBRAMANIYAN CHANDRASEKARAN
        // Creation Date :  27-Feb-2017 
        /// <summary>
        /// AppUser Export 
        /// </summary>
        /// <returns>returns ActionResult</returns> 
        private async Task SendWelcomeEmailToUser(int userNum, string userID)
        {
            string requestURL = this.GetTenantUrl();
            AppConfigModel appConfigModel = m_accountProcessManager.GetAppSetting();
            bool count = await m_usermanagementProcessManager.SendWelcomeEmailToUser(requestURL, userNum, userID, appConfigModel);
        }


        private List<string> GetUserDataTemplateColumns()
        {
            List<string> columns = new List<string>();
            columns.Add("EmployeeID");
            columns.Add("FirstName");
            columns.Add("LastName");
            columns.Add("UserID");
            columns.Add("UserRole");
            columns.Add("PreferredName");
            columns.Add("Email");
            return columns;
        }
        #endregion
    }
}
