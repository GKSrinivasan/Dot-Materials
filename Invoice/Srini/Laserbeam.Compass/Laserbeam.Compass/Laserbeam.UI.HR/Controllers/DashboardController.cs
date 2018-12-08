// Copyright (c) 2014 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.
// Component Name  :    Dashboard controller
// Description     : 	Actions of Dashboard are defined		
// Author          :	Hariharasubramaniyan	
// Creation Date   : 	05-06-2017
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Laserbeam.BusinessObject.Common;
using Laserbeam.Constant.HR;
using Laserbeam.ProcessManager.Interfaces.Common;
using Laserbeam.UI.HR.Common;
using Laserbeam.UI.HR.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;


namespace Laserbeam.UI.HR.Controllers
{
    [AuthorizeUser(UserRoleConstants.AllRoles)]
    public class DashboardController : Controller
    {
        // GET: Dashboard

        #region Fields
        private IDashboardProcessManager m_dashboardProcessManager;
        private ISessionProcessManager m_sessionProcessManager;
        private SessionManager m_sessionManager;// = new SessionManager(); 
        private IUserManagementProcessManager m_usermanagementProcessManager;
        private readonly IAccountProcessManager m_accountProcessManager;

        #endregion

        #region Constructors

        // Author         :  Hariharasubramaniyan 
        // Creation Date  :  05-Jul-2017  
        /// <summary>
        /// Initializes objects used in this class
        /// </summary>        
        /// <param name="sessionProcessManager">sessionProcessManager objects</param>
        /// <param name="dashboardBusinessManager">dashboardBusinessManager objects</param>
        /// <param name="sessionManager">sessionManager objects</param>
        public DashboardController(ISessionProcessManager sessionProcessManager, IDashboardProcessManager dashboardBusinessManager, SessionManager sessionManager, IUserManagementProcessManager usermanagementProcessManager, IAccountProcessManager accountProcessManager)
        {
            m_sessionProcessManager = sessionProcessManager;
            m_dashboardProcessManager = dashboardBusinessManager;
            m_sessionManager = sessionManager;
            m_usermanagementProcessManager = usermanagementProcessManager;
            m_accountProcessManager = accountProcessManager;
        }


        #endregion

        #region Actions

        // Author         :  Hariharasubramaniyan 
        // Creation Date  :  05-Jul-2017  
        // Reviewed By    :  Raja Ganapathy
        // Reviewed Date  :  31-08-2017  
        /// <summary>
        /// Get the dashboard budget chart
        /// </summary>
        /// <returns>Returns the view</returns>
        // Modify by     : Arunraj C
        // Creation Date : 04-May-2017
        /// <summary>
        /// Get the new dashboard details
        /// </summary>
        [HttpGet]
        public async Task<ActionResult> DashboardView()
        {
            AppConfigModel appConfig = new AppConfigModel();
            appConfig = m_accountProcessManager.GetAppSetting();
          //  m_sessionManager.SetSession<AppConfigModel>(SessionConstants.AppConfigModel, appConfig);
            UserModel userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
           // var userDetails = m_dashboardProcessManager.GetUserDetails(userModel.UserNum);//.Where(x => x.UserID.ToLower() == userModel.UserID.ToLower()).FirstOrDefault();
            var userDetails = m_dashboardProcessManager.GetUserInfo(userModel.UserID);
            //SetDashboardSession(userDetails.UserNum, userDetails.UserName);
            var model = new HomeDashboardDetails();
            int userNum = m_sessionManager.GetSession<int>(SessionConstants.LoggedSelectedUserNum);
            var result = m_dashboardProcessManager.GetEventsForLoggedInUser(userNum).OrderByDescending(x => x.UpdatedDate).ToList();
            AppConfigModel appConfigModel = m_accountProcessManager.GetAppSetting();
            EmployeeModel employeeModel = m_sessionManager.GetSession<EmployeeModel>(SessionConstants.EmployeeModel);
            int year = Convert.ToInt32(appConfigModel.CurrentYear);
            var getCurrency = m_dashboardProcessManager.GetCurrencies();
            var Currency = getCurrency.OrderBy(c => c.CurrencyNum).FirstOrDefault();
            model.isWorkFlowEnabled = m_dashboardProcessManager.isWorkFlowEnable();
            var selectedCurreny = getCurrency.Where(x => x.CurrencyCode == appConfigModel.BaseCurrency).Select(x => x.CultureCode).FirstOrDefault()?? "en-US";
            var loggedInCurrency= m_dashboardProcessManager.loggedInCultureCode(userNum, year);
            model.CultureCode = (loggedInCurrency == "") ? selectedCurreny : loggedInCurrency;
            var GetEmpSalaryDetails = await m_dashboardProcessManager.GetEmployeeSalaryDetails(userNum);
            List<EmployeeSalaryDetails> EmpSalaryDetailsModel = new List<EmployeeSalaryDetails>();                        
            model.WorkFlowBudgetSpendCount = await m_dashboardProcessManager.GetWorkFolwBudgetCount(userNum);
            model.EmployeeSalaryDetails = GetEmpSalaryDetails;                        
            if (Currency != null)            
                model.CurrencyCode = Currency.CultureCode;            
            else            
                model.CurrencyCode = "en-US";
                return View("MyDashboardView", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxChildActionOnly]
        public async Task<PartialViewResult> _DashboardWorkFlow(string type = "Yet")
        {
            int userNum = m_sessionManager.GetSession<int>(SessionConstants.LoggedSelectedUserNum);
            AppConfigModel appConfigModel = m_accountProcessManager.GetAppSetting();
            int year = Convert.ToInt32(appConfigModel.CurrentYear);
            var model = new DashboardWorkFlowModel();
            model.ManagerApproval = await m_dashboardProcessManager.GetApprovalData(userNum, type);
            model.TeamApproval = await m_dashboardProcessManager.GetTeamApprovalData(userNum, type);
            ViewBag.Type = type;            
            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxChildActionOnly]
        public async Task<JsonResult> GetApprovalEmployeeSearchData([DataSourceRequest] DataSourceRequest request, string type = "Yet")
        {
            int userNum = m_sessionManager.GetSession<int>(SessionConstants.LoggedSelectedUserNum);
            var reportee = await m_dashboardProcessManager.GetApprovalSearchData(userNum,type);
            return Json(reportee.ToList(), JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxChildActionOnly]
        public PartialViewResult _DashboardBudgetUtilization(string type = "yet")
        {
            UserModel user = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
            ViewBag.type = type;
            int userNum = 0;
            if (user.IsAdminAccess == true && user.AdminUserNum != null)
            {
                 userNum = user.AdminUserNum??0;
            }

            else
            {
                 userNum = m_sessionManager.GetSession<int>(SessionConstants.LoggedSelectedUserNum);
            }
            AppConfigModel appConfigModel = m_accountProcessManager.GetAppSetting();
            int year = Convert.ToInt32(appConfigModel.CurrentYear);
            EmployeeModel employeeModel = m_sessionManager.GetSession<EmployeeModel>(SessionConstants.EmployeeModel);
            var getCurrency = m_dashboardProcessManager.GetCurrencies();
            var Currency = getCurrency.OrderBy(c => c.CurrencyNum).FirstOrDefault();
            var budget = m_dashboardProcessManager.GetManagerBudgetDetails(userNum, year ).ToList();
            var selectedCurreny = getCurrency.Where(x => x.CurrencyCode == appConfigModel.BaseCurrency).Select(x => x.CultureCode).FirstOrDefault();
            var loggedInCurrency = m_dashboardProcessManager.loggedInCultureCode(userNum, year);
            var cultureCode = (loggedInCurrency == "") ? selectedCurreny : loggedInCurrency;
            ViewBag.selectedCultureCode = cultureCode;
            if (type == "over")
                budget = budget.Where(x => x.ManagerBalance < 0).OrderBy(x => x.ManagerBalance).ToList();
            else if (type == "within")
                budget = budget.Where(x => x.ManagerBalance >= 0 && x.ManagerSpent > 0).OrderBy(x => x.ManagerBalance).ToList();
            else if (type == "yet")
                budget = budget.Where(x => x.ManagerSpent == 0).OrderBy(x => x.ManagerBalance).ToList();
            else
                budget = budget.OrderBy(x => x.ManagerBalance).ToList();

            ViewBag.Budget =(budget.Count()>0)? string.Format(new CultureInfo(cultureCode), "{0:c0}", budget.Sum(x => x.ManagerBudget)).Replace(" ", "") :string.Format(new CultureInfo(cultureCode), "{0:c0}", 0).Replace(" ", "");
            ViewBag.Spent = (budget.Count() > 0) ? string.Format(new CultureInfo(cultureCode), "{0:c0}", budget.Sum(x => x.ManagerSpent)).Replace(" ", "") : string.Format(new CultureInfo(cultureCode), "{0:c0}", 0).Replace(" ", "");
            ViewBag.Balance = (budget.Count() > 0) ? budget.Sum(x => x.ManagerBalance)<0?
                "("+string.Format(new CultureInfo(cultureCode), "{0:c0}", Math.Abs(budget.Sum(x => x.ManagerBalance))).Replace(" ", "")+")"
                : string.Format(new CultureInfo(cultureCode), "{0:c0}", Math.Abs(budget.Sum(x => x.ManagerBalance))).Replace(" ", "")
                 : string.Format(new CultureInfo(cultureCode), "{0:c0}", 0).Replace(" ", "");
            ViewBag.BudgetPct = (budget.Count() > 0) ? budget.Sum(x => x.ManagerBudget) == 0 ? "0%" : Math.Round((Convert.ToDecimal(Convert.ToDecimal(budget.Sum(x => x.ManagerBudgetPct)) / budget.Select(x=>x.ManagerNum).Count())), 0).ToString() + "%" : "0%";
            ViewBag.SpentPct = (budget.Count() > 0) ? budget.Sum(x => x.ManagerBudget) == 0 ? "0%" : Math.Round((Convert.ToDecimal(Convert.ToDecimal(budget.Sum(x => x.ManagerSpent)) / Convert.ToDecimal(budget.Sum(x => x.ManagerBudget))) * budget[0].ManagerBudgetPct),0).ToString()+"%" : "0%";
            ViewBag.BalancePct = (budget.Count() > 0) ? budget.Sum(x => x.ManagerBudget) == 0 ? "0%" : budget.Sum(x => x.ManagerBudget) > budget.Sum(x => x.ManagerSpent) ?
                (Math.Round((Convert.ToDecimal(Convert.ToDecimal(budget.Sum(x => x.ManagerBalance)) / Convert.ToDecimal(budget.Sum(x => x.ManagerBudget))) * budget[0].ManagerBudgetPct),0).ToString()+"%"):
                ("("+ Math.Abs(Math.Round((Convert.ToDecimal(Convert.ToDecimal(budget.Sum(x => x.ManagerBalance)) / Convert.ToDecimal(budget.Sum(x => x.ManagerBudget))) * budget[0].ManagerBudgetPct), 0)).ToString() + "%"+")") : "0%";
            ViewBag.SpentPctLevel = (budget.Count() > 0) ? budget.Sum(x => x.ManagerBalance) < 0 ? 100 : budget.Sum(x => x.ManagerBudget) == 0 ? 0 : Math.Round((Convert.ToDecimal(Convert.ToDecimal(budget.Sum(x => x.ManagerSpent)) / Convert.ToDecimal(budget.Sum(x => x.ManagerBudget))) * 100), 0) : 0;
            ViewBag.BalancePctLevel = (budget.Count() > 0) ? budget.Sum(x => x.ManagerBalance) < 0 ? 100 : budget.Sum(x => x.ManagerBudget) == 0 ? 0 : Math.Round((Convert.ToDecimal(Convert.ToDecimal(budget.Sum(x => x.ManagerBalance)) / Convert.ToDecimal(budget.Sum(x => x.ManagerBudget))) * 100), 0) : 0;
            return PartialView(budget);
        }

        [HttpGet]
        public PartialViewResult _ManagerViewPopup()
        {
            EmployeeModel employeeModel = m_sessionManager.GetSession<EmployeeModel>(SessionConstants.EmployeeModel);
            ViewBag.EmployeeNum = employeeModel.EmployeeNum;
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxChildActionOnly]
        public JsonResult GetDashboardBudgetUtilization([DataSourceRequest]  DataSourceRequest request, List<ManagerBudgetDetails> budgetModel)
        {
            if (budgetModel != null)
                return Json(budgetModel.ToDataSourceResult(request));
            else
                return Json(false);
        }

        // Author         :  Hariharasubramaniyan 
        // Creation Date  :  05-Jul-2017  
        // Reviewed By    :  Raja Ganapathy
        // Reviewed Date  :  31-08-2017  
        /// <summary>
        /// Get the UserName droipdown text
        /// </summary>
        /// <returns>Returns the Json result</returns>
        [AjaxChildActionOnly]
        public async Task<JsonResult> GetUsers()
        {
            UserModel userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
            var userList = await m_dashboardProcessManager.GetAllUsers(userModel.UserNum);
            return Json(userList, JsonRequestBehavior.AllowGet);
        }


        // Author         :  Hariharasubramaniyan 
        // Creation Date  :  05-Jul-2017  
        // Reviewed By    :  Raja Ganapathy
        // Reviewed Date  :  31-08-2017  
        /// <summary>
        /// This method binds the active user dropdown list and also searches a user from the list with the text entered in the combobox
        /// </summary>
        /// <param name="text"> It is the string entered or the user selected in the dropdown list </param>
        /// <returns>User dropdownlist</returns>
        [HttpGet]
        [AjaxChildActionOnly]
        public void SetDashboardSession(int userNum, string userText)
        {
            AppConfigModel appConfigModel = m_accountProcessManager.GetAppSetting();
            int year = Convert.ToInt32(appConfigModel.CurrentYear);
            m_sessionManager.SetSession<int>(SessionConstants.LoggedSelectedUserNum, userNum);
            m_sessionManager.SetSession<string>(SessionConstants.LoggedSelectedUserText, userText);
            m_sessionManager.SetSession<List<UserRights>>(SessionConstants.UserAccess, m_sessionProcessManager.GetUserAccess(userNum));
            var userModel = m_sessionProcessManager.GetUserSession(userNum, year);
            m_sessionManager.SetSession<UserModel>(SessionConstants.UserModel, userModel);
            m_sessionManager.SetSession<int>(SessionConstants.LoggedSelectedUserRoleNum, userModel.UserRoleNum);
            var employeeModel = m_sessionProcessManager.GetEmployeeSession(userModel.EmployeeNum, year);
            m_sessionManager.SetSession<EmployeeModel>(SessionConstants.SelectedEmployeeModel, employeeModel);
            m_sessionManager.SetSession<EmployeeModel>(SessionConstants.EmployeeModel, employeeModel);
        }

        [HttpGet]
        [AjaxChildActionOnly]
        public void SetSwitchUserSession(bool isAdmin)
        {
            UserModel user = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
            if (isAdmin)
            {
                var userData = m_dashboardProcessManager.GetUserDetails(user.UserNum);
                user.UserRole = userData.UserRole;
                user.UserRoleNum = userData.UserRoleNum;
                user.enableSwitchtoAdmin = false;
            }
            else
            {
                user.UserRole = "Manager";
                user.UserRoleNum = 3;
                user.enableSwitchtoAdmin = true;
            }
            m_sessionManager.SetSession<UserModel>(SessionConstants.UserModel, user);
        }

        // Author         :  Hariharasubramaniyan 
        // Creation Date  :  05-Jul-2017  
        // Modified By    :  Revathy		
        // Modified Date  :  25-07-2017
        // Reviewed By    :  Raja Ganapathy
        // Reviewed Date  :  31-08-2017  
        // Modified By    :  Karthikeyan Shanmugam		
        // Modified Date  :  24-03-2017
        // Included records pending for an Employee's approval 
        /// <summary>
        /// Get the title bar data
        /// </summary>
        /// <param name="moduleName">Denotes the module name</param>
        /// <returns>Returns the title bar data</returns>
        //comment : added the Preferred Name
        public PartialViewResult GetTitleBarData(string moduleName)
        {
            EmployeeModel employeeModel = m_sessionManager.GetSession<EmployeeModel>(SessionConstants.EmployeeModel);
            UserModel userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
            var userName = m_dashboardProcessManager.GetUserDetails(userModel.UserNum).UserName;
            int userNumSelected = m_sessionManager.GetSession<int>(SessionConstants.LoggedSelectedUserNum);
            EmployeeTitleModel EmployeeTitle = new EmployeeTitleModel();
            if (employeeModel != null && moduleName == "Wizard")
            {
                EmployeeTitle.UserRole = userModel.UserRole;
                EmployeeTitle.EmployeeName = employeeModel.EmployeeName;
                EmployeeTitle.UserName = (userModel.PreferredName != null) ?( userModel.PreferredName != String.Empty? userModel.PreferredName: userModel.UserName) : userModel.UserName;
                var ShortNameSplit = EmployeeTitle.UserName.Trim().Split(' ');
                string ShortName = "";
                for (int s = 0; s < ShortNameSplit.Count(); s++)
                {
                    ShortName = ShortName + ShortNameSplit[s].Substring(0, 1);
                }
                //EmployeeTitle.UserNameShort = (userModel.PreferredName != null) ? (userModel.PreferredName != String.Empty? userModel.PreferredName.Substring(0, 1): userModel.UserName.Substring(0, 1) ): (userModel.UserName != String.Empty ? userModel.UserName.Substring(0, 1) : "");
                EmployeeTitle.UserNameShort = ShortName;
                return PartialView("_WizardTitleBar", EmployeeTitle);
            }
            else if (employeeModel != null && moduleName != "Wizard")
            {
                Tuple<int, List<MyApproval>> tuple = Task.Run(() => m_dashboardProcessManager.GetManagerApproval(employeeModel.EmployeeNum, userModel.UserNum, userNumSelected)).Result;
                EmployeeTitle.EmployeeName = employeeModel.EmployeeName;
                EmployeeTitle.UserName = (userModel.PreferredName != null) ? (userModel.PreferredName != String.Empty ? userModel.PreferredName : userModel.UserName) : userModel.UserName;
                var ShortNameSplit = EmployeeTitle.UserName.Trim().Split(' ');
                string ShortName = "";
                for(int s = 0; s < ShortNameSplit.Count();s++)
                {
                    ShortName = ShortName + ShortNameSplit[s].Substring(0, 1);
                }
                //EmployeeTitle.UserNameShort = (userModel.PreferredName != null) ? (userModel.PreferredName != String.Empty ? userModel.PreferredName.Substring(0, 1) : userModel.UserName.Substring(0, 1)) : (userModel.UserName != String.Empty ? userModel.UserName.Substring(0, 1) : "");
                EmployeeTitle.UserNameShort = ShortName;
                EmployeeTitle.UserRole = userModel.UserRole;
                string loggedSelectedUserText = m_sessionManager.GetSession<string>(SessionConstants.LoggedSelectedUserText);
                // EmployeeTitle.UserText = (loggedSelectedUserText.Split('-')[1]).Trim();
                EmployeeTitle.UserText = loggedSelectedUserText.Trim();
                EmployeeTitle.ApprovalCount = tuple.Item1;
                EmployeeTitle.ChatUnreadCount = m_dashboardProcessManager.GetChatUnReadMessageCount(userNumSelected);
                EmployeeTitle.MyApprovals = tuple.Item2.ToList();
                return PartialView("_UserTitleBar", EmployeeTitle);
            }
            else
            {
                return PartialView("_UserTitleBar", EmployeeTitle);
            }
        }       

        [HttpGet]
        public PartialViewResult _NotificationPanel()
        {
            EmployeeModel employeeModel = m_sessionManager.GetSession<EmployeeModel>(SessionConstants.EmployeeModel);
            UserModel userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
            int userNumSelected = m_sessionManager.GetSession<int>(SessionConstants.LoggedSelectedUserNum);
            EmployeeTitleModel EmployeeTitle = new EmployeeTitleModel();
            if (employeeModel != null)
            {
                List<MyApproval> tuple = new List<MyApproval>();
                EmployeeTitle.ApprovalCount = 0;
                EmployeeTitle.MyApprovals = tuple;
            }
            return PartialView(EmployeeTitle);
        }

        // Author         :  Arunraj C
        // Creation Date  :  04-May-2017  
        // Reviewed By    :  
        // Reviewed Date  :  
        /// <summary>
        /// To fetch the daily task based on usernum
        /// </summary>        
        [HttpGet]
        public PartialViewResult _DailyTask()
        {
            int userNum = m_sessionManager.GetSession<int>(SessionConstants.LoggedSelectedUserNum);
            var task = m_dashboardProcessManager.GetDailyTask(userNum).ToList();
            var GetTask = task.Where(x => x.TaskCompleted == false).ToList();
            var completedTask = task.Where(x => x.TaskCompleted == true).ToList();
            UserModel userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
            var UserLoginStatusCount = Task.Run(() => m_dashboardProcessManager.UserLoginStatusCount(userNum)).Result;
            ViewBag.UserRole = userModel.UserRole;
            ViewBag.UsersYettoLogin = UserLoginStatusCount;
            ViewBag.TaskCount = GetTask.Count();
            ViewBag.CompletedTaskCount = completedTask.Count();
            DailyTaskModel model = new DailyTaskModel();
            model.DailyTaskList = GetTask.Take(4).ToList();
            model.ApprovalList= Task.Run(() => m_dashboardProcessManager.GetMyTaskApprovalList(userNum)).Result;
            return PartialView(model);
        }

        // Author         :  Arunraj C
        // Creation Date  :  04-May-2017  
        // Reviewed By    :  
        // Reviewed Date  :  
        /// <summary>
        /// To insert & edit the daily task
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxChildActionOnly]
        public PartialViewResult _DailyTask(string TaskContent, string TaskAction = "", int TaskID = 0)
        {
            DailyTaskModel dailyTask = new DailyTaskModel();
            int userNum = m_sessionManager.GetSession<int>(SessionConstants.LoggedSelectedUserNum);
            dailyTask.UserNum = userNum;
            dailyTask.TaskDescr = TaskContent;            
            if (TaskAction == "Add")
            {
                dailyTask.Active = true;
                dailyTask.CreatedDate = DateTime.Now;
                m_dashboardProcessManager.AddDailyTask(dailyTask);
            }
            else if (TaskAction == "Edit")
            {
                dailyTask.UpdatedDate = DateTime.Now;
                dailyTask.TaskNum = TaskID;
                m_dashboardProcessManager.EditDailyTask(dailyTask);
            }
            var task = m_dashboardProcessManager.GetDailyTask(userNum).ToList();
            var GetTask = task.Where(x => x.TaskCompleted == false).ToList();
            var completedTask = task.Where(x => x.TaskCompleted == true).ToList();
            UserModel userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
            var UserLoginStatusCount = Task.Run(() => m_dashboardProcessManager.UserLoginStatusCount(userNum)).Result;
            ViewBag.UserRole = userModel.UserRole;
            ViewBag.UsersYettoLogin = UserLoginStatusCount;
            ViewBag.TaskCount = GetTask.Count();
            ViewBag.CompletedTaskCount = completedTask.Count();
            DailyTaskModel model = new DailyTaskModel();
            model.DailyTaskList = GetTask.Take(4).ToList();
            model.ApprovalList = Task.Run(() => m_dashboardProcessManager.GetMyTaskApprovalList(userNum)).Result;
            return PartialView(model);
        }

        // Author         :  Arunraj C
        // Creation Date  :  09-May-2017  
        // Reviewed By    :  
        // Reviewed Date  :  
        /// <summary>
        /// To edit the daily task
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxChildActionOnly]
        public JsonResult _GetEditDailyTask(int TaskID)
        {
            var GetTask = m_dashboardProcessManager.GetEditDailyTask(TaskID);
            return Json(GetTask.TaskDescr, JsonRequestBehavior.AllowGet);
        }

        // Author         :  Arunraj C
        // Creation Date  :  09-May-2017  
        // Reviewed By    :  
        // Reviewed Date  :  
        /// <summary>
        /// To edit the daily task
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxChildActionOnly]
        public ActionResult _DeleteDailyTask(int TaskID)
        {
            int userNum = m_sessionManager.GetSession<int>(SessionConstants.LoggedSelectedUserNum);
            var GetTask = m_dashboardProcessManager.DeleteDailyTask(TaskID);
            var task = m_dashboardProcessManager.GetDailyTask(userNum).ToList();
            var completedTask = task.Where(x => x.TaskCompleted == true).ToList();
            GetTask = completedTask.Count().ToString();
            return Json(GetTask, JsonRequestBehavior.AllowGet);
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

        // Author         :  Arunraj C
        // Creation Date  :  09-May-2017  
        // Reviewed By    :  
        // Reviewed Date  :  
        /// <summary>
        /// To get the manager budget utilization
        /// </summary>
        public JsonResult _ManagerBudgetDetails([DataSourceRequest]  DataSourceRequest request)
        {
            UserModel userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
            int userNum = m_sessionManager.GetSession<int>(SessionConstants.LoggedSelectedUserNum);
            AppConfigModel appConfigModel = m_accountProcessManager.GetAppSetting();
            int year = Convert.ToInt32(appConfigModel.CurrentYear);
            EmployeeModel employeeModel = m_sessionManager.GetSession<EmployeeModel>(SessionConstants.EmployeeModel);
            var Currency = m_dashboardProcessManager.GetCurrencies().OrderBy(c => c.CurrencyNum).FirstOrDefault();
            var GetManagerBudgetDetails = m_dashboardProcessManager.GetManagerBudgetDetails(userModel.IsAdminAccess?userModel.AdminUserNum??0:userNum, year).OrderBy(x => x.ManagerBalance);
            return Json(GetManagerBudgetDetails.ToDataSourceResult(request));
        }

        // Author         :  Arunraj C
        // Creation Date  :  09-May-2017  
        // Reviewed By    :  
        // Reviewed Date  :  
        /// <summary>
        /// To get the Announcements
        /// </summary>
        [HttpGet]
        public PartialViewResult _Announcements()
        {
            int userNum = m_sessionManager.GetSession<int>(SessionConstants.LoggedSelectedUserNum);
            var result = m_dashboardProcessManager.GetEventsForLoggedInUser(userNum).OrderByDescending(x => x.UpdatedDate).ToList();
            List<EventsandCommunication> EventModel = new List<EventsandCommunication>();
            if (result != null)
            {
                EventModel = (from e in result
                              select new EventsandCommunication
                              {
                                  EmailSubject = HttpUtility.HtmlDecode(e.EmailSubject),
                                  EmailBody = HttpUtility.HtmlDecode(e.EmailBody),
                                  UpdatedBy = e.UpdatedBy,
                                  UpdatedDate = (e.UpdatedDate).ToString(),
                                  MinutesMessage = getTimeAgo(e.UpdatedDate)
                              }).ToList();
            }
            ViewBag.AnnouncementCount = result.Count();
            return PartialView(EventModel.Take(4).ToList());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxChildActionOnly]
        public async Task<PartialViewResult> _LevelApprovalDetails(int managerNum,int level)
        {
            var result = await m_dashboardProcessManager.GetLevelApprovalData(managerNum, level);      
            return PartialView(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxChildActionOnly]
        public PartialViewResult _YetToSubmitSendRemainder(int managerNum)
        {
            ViewBag.YetToSubmitSelectedManager = managerNum;      
            return PartialView();
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxChildActionOnly]
        public async Task<JsonResult> SendYetToSubmitRemainder(string mailSubject,string mailContent,int managerNum)
        {
            AppConfigModel appConfig= m_accountProcessManager.GetAppSetting();
            var result = await m_dashboardProcessManager.GetYetToSubmitMailList(managerNum, mailSubject, mailContent, appConfig);           
            return Json(true,JsonRequestBehavior.AllowGet);
        }

        // Author         :  Arunraj C
        // Creation Date  :  11-May-2017  
        // Reviewed By    :  
        // Reviewed Date  :  
        /// <summary>
        /// To get all Announcements
        /// </summary>    
        [HttpGet]
        public ActionResult _ShowAllAnnouncements()
        {
            int userNum = m_sessionManager.GetSession<int>(SessionConstants.LoggedSelectedUserNum);
            var result = m_dashboardProcessManager.GetEventsForLoggedInUser(userNum).OrderByDescending(x => x.UpdatedDate).ToList();
            List<EventsandCommunication> EventModel = new List<EventsandCommunication>();
            if (result != null)
            {
                EventModel = (from e in result
                              select new EventsandCommunication
                              {
                                  EmailSubject = HttpUtility.HtmlDecode(e.EmailSubject),
                                  EmailBody = HttpUtility.HtmlDecode(e.EmailBody),
                                  UpdatedBy = e.UpdatedBy,
                                  UpdatedDate = (e != null) ? Convert.ToDateTime(e.UpdatedDate).ToString(e.DateFormat) : " ",
                                  MinutesMessage = getTimeAgo(e.UpdatedDate)
                              }).ToList();
            }
            ViewBag.AnnouncementCount = result.Count();
            return PartialView("_Announcements",EventModel);
        }
        // Author         :  Arunraj C
        // Creation Date  :  11-May-2017  
        // Reviewed By    :  
        // Reviewed Date  :  
        /// <summary>
        /// To set the tas is completed
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxChildActionOnly]
        public ActionResult _SetDailyTaskComplete(int TaskID)
        {
            var setTask = m_dashboardProcessManager.SetDailyTaskComplete(TaskID);
            return Json(setTask, JsonRequestBehavior.AllowGet);
        }

        // Author         :  Arunraj C
        // Creation Date  :  11-May-2017  
        // Reviewed By    :  
        // Reviewed Date  :  
        /// <summary>
        /// To show completed task
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxChildActionOnly]
        public ActionResult _CompletedDailyTask()
        {
            int userNum = m_sessionManager.GetSession<int>(SessionConstants.LoggedSelectedUserNum);
            var getAllTask = m_dashboardProcessManager.GetDailyTask(userNum).Where(x => x.TaskCompleted == true).OrderByDescending(x=>x.TaskCompletedDate).ToList();            
            ViewBag.CompletedTaskCount = getAllTask.Count();
            return PartialView(getAllTask);
        }

        // Author         :  Arunraj C
        // Creation Date  :  11-May-2017  
        // Reviewed By    :  
        // Reviewed Date  :  
        /// <summary>
        /// To show completed task
        /// </summary>
        [HttpGet]
        public ActionResult _ShowAllTask()
        {
            int userNum = m_sessionManager.GetSession<int>(SessionConstants.LoggedSelectedUserNum);
            var task = m_dashboardProcessManager.GetDailyTask(userNum).ToList();
            var GetTask = task.Where(x => x.TaskCompleted == false).ToList();
            var completedTask = task.Where(x => x.TaskCompleted == true).ToList();
            UserModel userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
            var UserLoginStatusCount = Task.Run(() => m_dashboardProcessManager.UserLoginStatusCount(userNum)).Result;
            ViewBag.UserRole = userModel.UserRole;
            ViewBag.UsersYettoLogin = UserLoginStatusCount;
            ViewBag.TaskCount = GetTask.Count();
            ViewBag.CompletedTaskCount = completedTask.Count();
            DailyTaskModel model = new DailyTaskModel();
            model.DailyTaskList = GetTask;
            model.ApprovalList = Task.Run(() => m_dashboardProcessManager.GetMyTaskApprovalList(userNum)).Result;
            return PartialView("_DailyTask", model);
        }

        // Author         :  Arunraj C
        // Creation Date  :  11-May-2017  
        // Reviewed By    :  
        // Reviewed Date  :  
        /// <summary>
        /// To Convert date into X Time ago format 
        /// </summary>
        private string getTimeAgo(DateTime? strDate)
        {
            string strTime = string.Empty;
            if (IsDate(Convert.ToString(strDate)))
            {
                TimeSpan t = DateTime.Now - Convert.ToDateTime(strDate);
                double deltaSeconds = t.TotalSeconds;

                double deltaMinutes = deltaSeconds / 60.0f;
                int minutes;

                if (deltaSeconds < 5)
                {
                    return "Just now";
                }
                else if (deltaSeconds < 60)
                {
                    return Math.Floor(deltaSeconds) + " seconds ago";
                }
                else if (deltaSeconds < 120)
                {
                    return "A minute ago";
                }
                else if (deltaMinutes < 60)
                {
                    return Math.Floor(deltaMinutes) + " minutes ago";
                }
                else if (deltaMinutes < 120)
                {
                    return "An hour ago";
                }
                else if (deltaMinutes < (24 * 60))
                {
                    minutes = (int)Math.Floor(deltaMinutes / 60);
                    return minutes + " hours ago";
                }
                else if (deltaMinutes < (24 * 60 * 2))
                {
                    return "Yesterday";
                }
                else if (deltaMinutes < (24 * 60 * 7))
                {
                    minutes = (int)Math.Floor(deltaMinutes / (60 * 24));
                    return minutes + " days ago";
                }
                else if (deltaMinutes < (24 * 60 * 14))
                {
                    return "Last week";
                }
                else if (deltaMinutes < (24 * 60 * 31))
                {
                    minutes = (int)Math.Floor(deltaMinutes / (60 * 24 * 7));
                    return minutes + " weeks ago";
                }
                else if (deltaMinutes < (24 * 60 * 61))
                {
                    return "Last month";
                }
                else if (deltaMinutes < (24 * 60 * 365.25))
                {
                    minutes = (int)Math.Floor(deltaMinutes / (60 * 24 * 30));
                    return minutes + " months ago";
                }
                else if (deltaMinutes < (24 * 60 * 731))
                {
                    return "Last year";
                }

                minutes = (int)Math.Floor(deltaMinutes / (60 * 24 * 365));
                return minutes + " years ago";
            }
            else
            {
                return "";
            }
        }


        public static bool IsDate(string o)
        {
            DateTime tmp;
            return DateTime.TryParse(o, out tmp);
        }

        // Author        :  Arunraj C
        // Creation Date :  12-May-2017 
        /// <summary>
        /// Merit Export 
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task MeritExport()
        {
            AppConfigModel appConfigModel = m_accountProcessManager.GetAppSetting();
            UserModel userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
            int year = Convert.ToInt32(appConfigModel.CurrentYear);
            int userNum = m_sessionManager.GetSession<int>(SessionConstants.LoggedSelectedUserNum);
            var employeeModel = m_sessionProcessManager.GetUserSession(userNum, year);
            string exportName = "Dashboard-Merit Extract - " + DateTime.Now.ToShortDateString().FormatWith("{0: MM-dd-yyyy}");
            string sheetName = "Merit Information";
            var exportData = await m_dashboardProcessManager.GetExportMeritData(employeeModel.EmployeeNum, userModel.IsAdminAccess ? userModel.AdminUserNum ?? 0 : userNum);
            ExportExcel.ToExcel(exportData, exportName, sheetName);
        }

        // Author        :  Arunraj C
        // Creation Date :  12-May-2017 
        /// <summary>
        /// Promotion Export 
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task PromotionExport()
        {
            AppConfigModel appConfigModel = m_accountProcessManager.GetAppSetting();
            UserModel userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
            int year = Convert.ToInt32(appConfigModel.CurrentYear);
            int userNum = m_sessionManager.GetSession<int>(SessionConstants.LoggedSelectedUserNum);
            var employeeModel = m_sessionProcessManager.GetUserSession(userNum, year);
            string exportName = "Dashboard-Promotion Extract - " + DateTime.Now.ToShortDateString().FormatWith("{0: MM-dd-yyyy}");
            string sheetName = "Merit Information";
            var exportData = await m_dashboardProcessManager.GetExportPromotionData(employeeModel.EmployeeNum, userModel.IsAdminAccess ? userModel.AdminUserNum ?? 0 : userNum);
            ExportExcel.ToExcel(exportData, exportName, sheetName);
        }
        // Author        :  Srinivasan
        // Creation Date :  12-May-2017 
        /// <summary>
        /// Adjustment Export 
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task WorkFlowExport()
        {
            UserModel userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
            int userNum = m_sessionManager.GetSession<int>(SessionConstants.LoggedSelectedUserNum);
            string exportName = "Dashboard-WorkFlow Extract - " + DateTime.Now.ToShortDateString().FormatWith("{0: MM-dd-yyyy}");
            string sheetName = "WorkFlow Information";
            var exportData = await m_dashboardProcessManager.GetExportWorkFlowData(userNum);
            ExportExcel.ToExcel(exportData, exportName, sheetName);
        }

        // Author        :  Arunraj C
        // Creation Date :  12-May-2017 
        /// <summary>
        /// Adjustment Export 
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task AdjustmentExport()
        {
            AppConfigModel appConfigModel = m_accountProcessManager.GetAppSetting();
            UserModel userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
            int year = Convert.ToInt32(appConfigModel.CurrentYear);
            int userNum = m_sessionManager.GetSession<int>(SessionConstants.LoggedSelectedUserNum);
            var employeeModel = m_sessionProcessManager.GetUserSession(userNum, year);
            string exportName = "Dashboard-Adjustment Extract - " + DateTime.Now.ToShortDateString().FormatWith("{0: MM-dd-yyyy}");
            string sheetName = "Adjustment Information";
            var exportData = await m_dashboardProcessManager.GetExportAdjustmentData(employeeModel.EmployeeNum, userModel.IsAdminAccess ? userModel.AdminUserNum ?? 0 : userNum);
            ExportExcel.ToExcel(exportData, exportName, sheetName);
        }
        #endregion

        public ActionResult TimeoutPopup()
        {
            return View();
        }
        // Author        :  Balamurugan
        // Creation Date :  28-Jul-2017 
        /// <summary>
        /// BudgetUtilization Export
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task BudgetUtilizationExport()
        {
            UserModel userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
            int userNum = m_sessionManager.GetSession<int>(SessionConstants.LoggedSelectedUserNum);
            string exportName = "Dashboard-Budget Utilization Export - " + DateTime.Now.ToShortDateString().FormatWith("{0: MM-dd-yyyy}");
            string sheetName = "Budget Utilization Information";
            var exportData = await m_dashboardProcessManager.GetExportBudgetUtilizData(userNum);
            ExportExcel.ToExcel(exportData, exportName, sheetName);
        }

    }

}








