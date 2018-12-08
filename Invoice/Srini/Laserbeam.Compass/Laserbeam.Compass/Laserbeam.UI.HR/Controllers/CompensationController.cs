// Copyright (c) 2016 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.
// Component Name  :    Compensation Controller
// Description     : 	Actions of Manager Compensation are defined	
// Author          :	Raja		
// Creation Date   : 	22-06-2016

using Kendo.Mvc.UI;
using Laserbeam.BusinessObject.Common;
using Laserbeam.Constant.HR;
using Laserbeam.ProcessManager.Interfaces.Common;
using Laserbeam.UI.HR.Common;
using Laserbeam.UI.HR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Laserbeam.BusinessObject.Common.MeritBusinussObjects;
using System.Threading.Tasks;
using System.Web;
namespace Laserbeam.UI.HR.Controllers
{
    [AuthorizeUser(UserRoleConstants.AllRoles)]
    public class CompensationController : Controller
    {
        #region Fields
        private SessionManager m_sessionManager;// = new SessionManager();
        private ICompensationProcessManager m_compensationProcessManager;
        private readonly IAccountProcessManager m_accountProcessManager;
        #endregion

        #region Constructors
        // Author         :  Raja Ganapathy
        // Creation Date  :  05-Jul-2016  
        // Reviewed By   :  Hari		
        // Reviewed Date :  13-10-2016
        /// <summary>
        /// Initializes objects used in this class
        /// </summary>                
        /// <param name="compensationProcessManager">compensationProcessManager objects</param>
        /// <param name="emailProcessManger">emailProcessManger objects</param>
        public CompensationController(ICompensationProcessManager compensationProcessManager,SessionManager sessionManager, IAccountProcessManager accountProcessManager)
        {            
            m_compensationProcessManager = compensationProcessManager;
            m_sessionManager = sessionManager;
            m_accountProcessManager = accountProcessManager;
        }
        #endregion
        #region Public Methods Implementation

        // Author        :  Raja Ganapathy		
        // Creation Date :  22-06-2016
        // Reviewed By   :  Hari		
        // Reviewed Date :  13-10-2016
        /// <summary>
        /// Get compensation home
        /// </summary>
        /// <param name="notifiedEmployeeNum">Approval manager num</param>
        /// <param name="isMyApproval">Denotes the my approval</param>
        /// <returns>Returns compensation home</returns>
       [HttpGet]
        public async Task<ActionResult> Home()
        {
            UserModel userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
            EmployeeModel employeeModel = m_sessionManager.GetSession<EmployeeModel>(SessionConstants.EmployeeModel);
            int notifiedEmployeeNum = 0;
            int isMyApproval = 0;
            var isEmpty = m_compensationProcessManager.IsEmployeeDataEmpty();
          var IsInDirects = m_compensationProcessManager.IsInDirects(userModel.IsAdminAccess == true ? userModel.AdminEmpNum ?? 0 : employeeModel.EmployeeNum);
            if (isEmpty)
                return View("NoData");
            else
                return View(await CompensationView(notifiedEmployeeNum, isMyApproval));
        }

        // Author        :  Raja Ganapathy		
        // Creation Date :  22-06-2016
        // Reviewed By   :  Hari		
        // Reviewed Date :  13-10-2016
        /// <summary>
        /// Get compensation home
        /// </summary>
        /// <param name="notifiedEmployeeNum">Approval manager num</param>
        /// <param name="isMyApproval">Denotes the my approval</param>
        /// <returns>Returns compensation home</returns>
        //[NoDirectAccess]
        [HttpPost,ValidateAntiForgeryToken]
        public async Task<ActionResult> Home(int notifiedEmployeeNum = 0, int isMyApproval = 0)
        {            
            var isEmpty = m_compensationProcessManager.IsEmployeeDataEmpty();
            if (isEmpty)
                return View("NoData");
            else
                return View(await CompensationView(notifiedEmployeeNum, isMyApproval));
        }
        [HttpGet]
        public PartialViewResult _SubmitPopUp()
        {
            
            return PartialView();
        }
        [HttpGet]
        public PartialViewResult _ApprovePopUp()
        {

            return PartialView();
        }
        [HttpGet]
        public PartialViewResult _ReopenPopUp()
        {

            return PartialView();
        }
        [HttpPost,ValidateAntiForgeryToken,AjaxChildActionOnly]
        public async Task<JsonResult> _GetSubmitReportees([DataSourceRequest] DataSourceRequest request, int selectedManagerNum, MenuType compMenuType, bool isRollup = false)
        {
            UserModel userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
            EmployeeModel employeeModel = m_sessionManager.GetSession<EmployeeModel>(SessionConstants.EmployeeModel);
            int loggedSelectedUserNum = m_sessionManager.GetSession<int>(SessionConstants.LoggedSelectedUserNum);
            var compensationSubmitReportee = await m_compensationProcessManager.GetCompSubmitReportees(selectedManagerNum, employeeModel.EmployeeNum, userModel.UserNum, compMenuType, isRollup, loggedSelectedUserNum);
            return Json(compensationSubmitReportee.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        [HttpPost,ValidateAntiForgeryToken,AjaxChildActionOnly]
        public async Task<JsonResult> _GetApprovalReportees([DataSourceRequest] DataSourceRequest request, int selectedManagerNum, MenuType compMenuType, bool isRollup = false)
        {
            UserModel userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
            EmployeeModel employeeModel = m_sessionManager.GetSession<EmployeeModel>(SessionConstants.EmployeeModel);
            int loggedSelectedUserNum = m_sessionManager.GetSession<int>(SessionConstants.LoggedSelectedUserNum);
            var compensationSubmitReportee = await m_compensationProcessManager.GetCompApprovalReportees(selectedManagerNum, employeeModel.EmployeeNum, userModel.UserNum, compMenuType, isRollup, loggedSelectedUserNum);
            return Json(compensationSubmitReportee.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        [HttpPost,ValidateAntiForgeryToken,AjaxChildActionOnly]
        public async Task<JsonResult> _GetReopenReportees([DataSourceRequest] DataSourceRequest request, int selectedManagerNum, MenuType compMenuType, bool isRollup = false)
        {
            UserModel userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
            EmployeeModel employeeModel = m_sessionManager.GetSession<EmployeeModel>(SessionConstants.EmployeeModel);
            int loggedSelectedUserNum = m_sessionManager.GetSession<int>(SessionConstants.LoggedSelectedUserNum);
            var compensationSubmitReportee = await m_compensationProcessManager.GetCompReopenReportees(selectedManagerNum, employeeModel.EmployeeNum, userModel.UserNum, compMenuType, isRollup, loggedSelectedUserNum);
            return Json(compensationSubmitReportee.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }


        // Author        :  Raja Ganapathy		
        // Creation Date :  22-06-2016
        // Reviewed By   :  Hari		
        // Reviewed Date :  13-10-2016
        /// <summary>
        /// Returns the employeename
        /// </summary>
        /// <param name="selectedManagerNum">Denotes the selected manager num</param>
        /// <returns>Returns the  manager Name</returns>
        [HttpPost,AjaxChildActionOnly,ValidateAntiForgeryToken]
        public async Task<JsonResult>  GetManagerName(int selectedManagerNum)
        {            
            var mgrName = await m_compensationProcessManager.GetManagerName(selectedManagerNum);
            return Json(mgrName, JsonRequestBehavior.AllowGet);
        }
        
        [HttpPost,ValidateAntiForgeryToken,AjaxChildActionOnly]
        public async Task<JsonResult> GetSubmitEmployeeSearchData([DataSourceRequest] DataSourceRequest request, int selectedManagerNum, MenuType compMenuType, bool isRollup = false)
        {
            UserModel userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
            EmployeeModel employeeModel = m_sessionManager.GetSession<EmployeeModel>(SessionConstants.EmployeeModel);                        
            var employeesearch = await m_compensationProcessManager.GetCompApprovalReporteesSearch(selectedManagerNum, userModel.UserNum, compMenuType, isRollup,"Submit");
            return Json(employeesearch.ToList(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost,ValidateAntiForgeryToken,AjaxChildActionOnly]
        public async Task<JsonResult> GetApproveReopenEmployeeSearchData([DataSourceRequest] DataSourceRequest request, int selectedManagerNum, MenuType compMenuType, bool isRollup = false)            
        {
            UserModel userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
            EmployeeModel employeeModel = m_sessionManager.GetSession<EmployeeModel>(SessionConstants.EmployeeModel);
            var employeesearch = await m_compensationProcessManager.GetCompApprovalReporteesSearch(selectedManagerNum, userModel.UserNum, compMenuType, isRollup, "Approve");
            return Json(employeesearch.ToList(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost, ValidateAntiForgeryToken, AjaxChildActionOnly]
        public async Task<JsonResult> GetReopenEmployeeSearchData([DataSourceRequest] DataSourceRequest request, int selectedManagerNum, MenuType compMenuType, bool isRollup = false)
        {
            UserModel userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
            EmployeeModel employeeModel = m_sessionManager.GetSession<EmployeeModel>(SessionConstants.EmployeeModel);
            var employeesearch = await m_compensationProcessManager.GetCompApprovalReporteesSearch(selectedManagerNum, userModel.UserNum, compMenuType, isRollup, "Reopen");
            return Json(employeesearch.ToList(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost,ValidateAntiForgeryToken,AjaxChildActionOnly]
        public async Task<JsonResult> GetApproveReopenStatus(int selectedManagerNum, MenuType compMenuType, bool isRollup = false)
        {
            UserModel userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
            EmployeeModel employeeModel = m_sessionManager.GetSession<EmployeeModel>(SessionConstants.EmployeeModel);
            var approvalCount = await m_compensationProcessManager.GetCompApprovalReporteesSearch(selectedManagerNum, userModel.UserNum, compMenuType, isRollup, "Approve");
            var reopenCount = await m_compensationProcessManager.GetCompApprovalReporteesSearch(selectedManagerNum, userModel.UserNum, compMenuType, isRollup, "Reopen");
            bool result = (approvalCount.Count() > 0 || reopenCount.Count() > 0) ? true : false;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        // Author        :  Raja Ganapathy		
        // Creation Date :  22-06-2016
        // Reviewed By   :  Hari		
        // Reviewed Date :  13-10-2016
        /// <summary>
        /// To get Budget details
        /// </summary>        
        /// <param name="employeeNum">Denotes selected employee num</param>
        /// <param name="compensationTypeConfiguration">Denotes compensation type configuration</param>
        /// <param name="compMenuType">Denotes the comp menu type</param>
        /// <returns>Returns the json result to view</returns>
        [AjaxChildActionOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> GetBudgetSpentData(int employeeNum, MenuType compMenuType, string currencyCulture, int currencyCodeNum, bool isRollup = false, bool isSelectedRollup = false)
        {
            List<BudgetModel> budgetDetails = new List<BudgetModel>();
            int loggedInEmpNum = m_sessionManager.GetSession<EmployeeModel>(SessionConstants.EmployeeModel).EmployeeNum;
            UserModel userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
            var budgetModel =await m_compensationProcessManager.GetBudgetData(loggedInEmpNum, employeeNum, userModel.UserNum, compMenuType, currencyCulture, currencyCodeNum, isRollup, isSelectedRollup);
            var indirects = m_compensationProcessManager.IsInDirects(employeeNum);
            budgetDetails.Add(budgetModel);
            budgetDetails[0].isInDirects = indirects;
            return Json(budgetDetails, JsonRequestBehavior.AllowGet);
        }

        // Author        :  Raja Ganapathy		
        // Creation Date :  22-06-2016
        // Reviewed By   :  Hari		
        // Reviewed Date :  13-10-2016
        /// <summary>
        /// Get the direct reportees of manager or group
        /// </summary>
        /// <param name="request">stores the page no and page size of the view</param>
        /// <param name="selectedManagerNum">selected manager num</param>        
        /// <param name="compMenuType">Org type(My team, assigned group)</param>
        /// <param name="isRollup">Rollup selected or not</param>        
        /// <returns>partial view of direct reportees</returns>
        [AjaxChildActionOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> _GetReportees([DataSourceRequest] DataSourceRequest request, int selectedManagerNum, MenuType compMenuType, bool isRollup = false)
        {
            UserModel userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);    
            EmployeeModel employeeModel = m_sessionManager.GetSession<EmployeeModel>(SessionConstants.EmployeeModel);
            int loggedSelectedUserNum = m_sessionManager.GetSession<int>(SessionConstants.LoggedSelectedUserNum);
            var compensationReportee = await m_compensationProcessManager.GetCompReportees(selectedManagerNum, employeeModel.EmployeeNum, userModel.UserNum, compMenuType, isRollup, loggedSelectedUserNum);          
            return Json(compensationReportee.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        // Author        :  Raja Ganapathy		
        // Creation Date :  22-06-2016
        // Reviewed By   :  Hari		
        // Reviewed Date :  13-10-2016
        /// <summary>
        /// To get the merit grid data
        /// </summary>
        /// <param name="request">DataSourceRequest</param>
        /// <param name="model">MeritGridModel model</param>        
        [AjaxChildActionOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JavaScriptResult UpdateCompReportees([DataSourceRequest]DataSourceRequest request, [Bind(Prefix = "models")]List<MeritGridModel> model)
        {
            UserModel userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
            int userNumSelected = m_sessionManager.GetSession<int>(SessionConstants.LoggedSelectedUserNum);
            AppConfigModel appConfig = m_accountProcessManager.GetAppSetting();
            int year = Convert.ToInt32(appConfig.MeritCycleYear);
            m_compensationProcessManager.UpdateCompReportees(model, year, userModel.UserNum);
            return JavaScript("getApprovalStatus();");
        }


        // Author        :  Raja Ganapathy		
        // Creation Date :  22-06-2016
        // Reviewed By   :  Hari		
        // Reviewed Date :  13-10-2016
        /// <summary>
        /// Action to bind the text in menu
        /// </summary>
        /// <param name="employeeNum">employee num of the manager</param>
        /// <param name="year">current year</param>
        /// <param name="compMenuType">Org type(My team, assigned team, assigned group)</param>
        /// <returns></returns>
        //[HttpPost]
      //  [ValidateAntiForgeryToken]
        public PartialViewResult _ManagerAction(int employeeNum, CompensationTypeConfiguration compensationTypeConfiguration, MenuType compMenuType, string employeeName,List<ExchangeCurrencies> currencies,bool isInDirects)
        {
            List<SelectListItem> menu = new List<SelectListItem>();
            menu.Add(new SelectListItem { Text = "Filter & Sort", Value = ManagerActionType.Filter.ToString() });
            menu.Add(new SelectListItem { Text = "Clear Filter", Value = ManagerActionType.ClearFilter.ToString() });
            menu.Add(new SelectListItem { Text = "Export", Value = ManagerActionType.Export.ToString() });
            menu.Add(new SelectListItem { Text = "Show in USD", Value = ManagerActionType.USD.ToString() });
            menu.Add(new SelectListItem { Text = "Show in Local", Value = ManagerActionType.Local.ToString() });            
            var model = new ManagerActionModel
            {
                CompMenuType = compMenuType,
                EmployeeNum = employeeNum,
                ExchangeCurrencies = currencies,
                CompensationTypeConfiguration = compensationTypeConfiguration,
                Menu = menu,
                EmployeeName = employeeName
            };
            ViewBag.IsInDirect = isInDirects;
            return PartialView(model);
        }

        // Author        :  Raja Ganapathy		
        // Creation Date :  22-06-2016
        // Reviewed By   :  Hari		
        // Reviewed Date :  13-10-2016
        /// <summary>
        /// Filter and sort the employee names
        /// </summary>
        /// <returns>compensation filter model</returns>
        [HttpGet]
        public async Task<PartialViewResult> _FilterSort()
        {
            var filterSortList = (await m_compensationProcessManager.GetEmpSort()).Select(m => new SelectListItem { Text = m.Text, Value = m.Value }).ToList();
            return PartialView(filterSortList);
        }

        // Author        :  Raja Ganapathy		
        // Creation Date :  22-06-2016
        // Reviewed By   :  Hari		
        // Reviewed Date :  13-10-2016
        /// <summary>
        /// Filter and sort the employee names
        /// </summary>
        /// <returns>Returns action names</returns>
        [AjaxChildActionOnly]
        [HttpGet]
        public List<SelectListItem> GetAction()
        {
            List<SelectListItem> Action = new List<SelectListItem>();
            Action.Add(new SelectListItem { Text = "Starts with", Value = CompFilterType.StartsWith.ToString() });
            Action.Add(new SelectListItem { Text = "Contains", Value = CompFilterType.Contains.ToString() });
            return Action;
        }


        // Author        :  Raja Ganapathy		
        // Creation Date :  22-06-2016
        // Reviewed By   :  Hari		
        // Reviewed Date :  13-10-2016
        /// <summary>
        /// To display employee's informations in the popup
        /// </summary>
        /// <param name="employee">Employeenum for which the informations is needed</param>
        /// <returns>_EmployeeInfo partial view</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<PartialViewResult> _EmployeeInfo(int employeeNum)
        {
            UserModel userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
            IEnumerable<EmployeeInfoDetails> employeeinfo = await m_compensationProcessManager.GetEmployeeInfo(employeeNum, userModel.UserNum);
            var compensationTypeConfiguration = m_compensationProcessManager.GetCompensationTypeConfiguration();
            bool enableLumpsum = compensationTypeConfiguration.FeatureConfigurationLumpSum;
            var EmployeeBasicDetails=await m_compensationProcessManager.GetEmployeeBasicInfo(employeeNum);
            ViewBag.Title = EmployeeBasicDetails != null ? EmployeeBasicDetails.ElementAt(0).JobTitle : "";
            ViewBag.CurrentSalary= (EmployeeBasicDetails != null && EmployeeBasicDetails.ElementAt(0).CurrentAnnualSalaryLocal!=null) ? EmployeeBasicDetails.ElementAt(0).CurrentAnnualSalaryLocal : 0;
            ViewBag.CurrentHourlySalary = (EmployeeBasicDetails != null && EmployeeBasicDetails.ElementAt(0).CurrentHourlySalaryLocal != null) ? EmployeeBasicDetails.ElementAt(0).CurrentHourlySalaryLocal : 0;
            ViewBag.TotalIncrease = (EmployeeBasicDetails != null && EmployeeBasicDetails.ElementAt(0).TotalIncrease!=null) ? EmployeeBasicDetails.ElementAt(0).TotalIncrease : 0;
            ViewBag.TotalIncreasePct = (EmployeeBasicDetails != null && EmployeeBasicDetails.ElementAt(0).TotalIncreasePCT!=null) ? EmployeeBasicDetails.ElementAt(0).TotalIncreasePCT : 0;
            ViewBag.NewSalary = (EmployeeBasicDetails != null && EmployeeBasicDetails.ElementAt(0).NewSalaryLocal!=null) ? EmployeeBasicDetails.ElementAt(0).NewSalaryLocal : 0;
            ViewBag.NewHourlySalary = (EmployeeBasicDetails != null && EmployeeBasicDetails.ElementAt(0).NewHourlySalaryLocal != null) ? EmployeeBasicDetails.ElementAt(0).NewHourlySalaryLocal : 0;
            ViewBag.EmployeeStatus = (EmployeeBasicDetails != null && EmployeeBasicDetails.ElementAt(0).EmployeeStatus != null) ? EmployeeBasicDetails.ElementAt(0).EmployeeStatus : "";
            if (enableLumpsum == true)
                ViewBag.LumpSum = (EmployeeBasicDetails != null && EmployeeBasicDetails.ElementAt(0).LumpSumAmountLocal != null) ? EmployeeBasicDetails.ElementAt(0).LumpSumAmountLocal : 0;
            else
                ViewBag.LumpSum = 0;
           ViewBag.CultureCode= EmployeeBasicDetails != null ? EmployeeBasicDetails.ElementAt(0).CultureCode : "";
            ViewBag.LumpSumEnable = enableLumpsum == true ? "true" :"false";

            return PartialView(employeeinfo);
        }


        // Author        :  Raja Ganapathy
        // Creation Date :  22-06-2016
        // Reviewed By   :  Hari		
        // Reviewed Date :  13-10-2016
        /// <summary>
        /// Gets the data to bind to the promotion list pop-up
        /// </summary>
        /// <param name="empJobNum">Denotes the empjobnum selected in the grid</param>
        /// <param name="promotionComment">Denotes the promotion selected in the grid</param>            
        /// <returns>Pop-up of the promotion dropdownlist</returns>     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public PartialViewResult _Promotion(string newGradeTitle, int empJobNum)
        {            
            ViewBag.LoggedUserNumOrEmpNum = m_sessionManager.GetSession<int>(SessionConstants.LoggedSelectedUserNum);
            var model = new PromotionModel
            {   
                newTitle = newGradeTitle,
                promotionComments = m_compensationProcessManager.GetPromotionComment(empJobNum).ToList(),                
            };
            return PartialView(model);
        }


      
        // Author        :  Raja Ganapathy		
        // Creation Date :  22-06-2016
        // Reviewed By   :  Hari		
        // Reviewed Date :  13-10-2016
        /// <summary>
        /// Displays the manager tree
        /// </summary>        
        /// <param name="managerNum">Denotes logged in employee num</param>
        /// <param name="MenuType">Denotes the menu type(My team or Assigned group)</param>
        /// <returns>Returns manager tree data</returns>
        [HttpPost]
        [AjaxChildActionOnly]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> GetCompManagerTreeData(int managerNum, ViewPageType pageType)
        {
            int userNumSelected = m_sessionManager.GetSession<int>(SessionConstants.LoggedSelectedUserNum);
            UserModel userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);   
            EmployeeModel employeeModel = m_sessionManager.GetSession<EmployeeModel>(SessionConstants.EmployeeModel);
            var result =await m_compensationProcessManager.GetCompManagerTree(employeeModel.EmployeeNum, employeeModel.EmployeeNum, userModel.UserNum, userNumSelected, pageType);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        // Author        :  Hari		
        // Creation Date :  25-07-2016
        // Reviewed By   :  Raja Ganapathy
        // Reviewed Date :  25-07-2016
        /// <summary>
        /// Prormotion comment save function
        /// </summary>        
        /// <param name="empJobNum">employee job num</param>
        /// <param name="employeeCompCommentNum">employeeCompensation Comment</param>
        /// <param name="comments">Comments</param>
        /// <param name="grade">grade</param>
        [HttpPost]
        [AjaxChildActionOnly]
        [ValidateAntiForgeryToken]
        public void PromotionComment(int empJobNum,int employeeCompCommentNum,string comments,string grade)
        {
            UserModel userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
            int userNumSelected = m_sessionManager.GetSession<int>(SessionConstants.LoggedSelectedUserNum);
            m_compensationProcessManager.UpdateEmployeeComment(empJobNum, userModel.UserNum, employeeCompCommentNum, comments, grade);
        }

        // Author        :  Raja Ganapathy		
        // Creation Date :  22-08-2016
        // Reviewed By   :  Hari		
        // Reviewed Date :  13-10-2016
        /// <summary>
        /// To update the employee comp approval status
        /// </summary>
        /// <param name="approvalStatus">Approval status</param>
        /// <param name="selectedManagerNum">Selected manager num</param>
        /// <returns>Returns the json result</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxChildActionOnly]
        public async Task<JsonResult> CompApproval(List<SubmitReporteesModel> selectedRows, int selectedManagerNum, MenuType MenuType, bool isRollup, ApprovalStatus approvalStatus, string Comment)
        {
            UserModel userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
            AppConfigModel appConfig  = m_accountProcessManager.GetAppSetting();
            EmployeeModel employeeModel = m_sessionManager.GetSession<EmployeeModel>(SessionConstants.EmployeeModel);
            int userNumSelected = m_sessionManager.GetSession<int>(SessionConstants.LoggedSelectedUserNum);
            int value = await m_compensationProcessManager.UpdateApprovalStatus(selectedRows, employeeModel.EmployeeNum, selectedManagerNum, MenuType, isRollup, approvalStatus, appConfig, userModel.UserNum, Comment);
            var result = 0;// (value > 0) ? await m_compensationProcessManager.GetApprovalStatus(selectedManagerNum,employeeModel.EmployeeNum)  : 0 ;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

     

        // Author        :  Raja Ganapathy		
        // Creation Date :  22-08-2016
        // Reviewed By   :  Hari		
        // Reviewed Date :  13-10-2016
        /// <summary>
        /// To get the comp completed count
        /// </summary>
        /// <param name="selectedManagerNum">Denotes the selected manager num</param>
        /// <returns>Returns the comp completed count</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxChildActionOnly]
        public async Task<JsonResult> GetCompCompletedCount(int selectedManagerNum)
        {
            UserModel userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
            //int year = Convert.ToInt32(m_sessionManager.GetSession<AppConfigModel>(SessionConstants.AppConfigModel).MeritCycleYear);
            int year=Convert.ToInt32(m_accountProcessManager.GetAppSetting().MeritCycleYear);
            return Json(await m_compensationProcessManager.GetCompCompletedCount(selectedManagerNum, year, userModel.UserNum), JsonRequestBehavior.AllowGet);
        }
        // Author        :  Raja Ganapathy		
        // Creation Date :  22-08-2016
        // Reviewed By   :  Hari		
        // Reviewed Date :  13-10-2016
        /// <summary>
        /// To revert the promotion
        /// </summary>
        /// <param name="empJobNum">Denotes the employee job num</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxChildActionOnly]
        public async Task RevertPromotion(int empJobNum, decimal newSalaryLocal,decimal newHrlyRate,decimal newCompRatio,decimal TCC)
        {
            await m_compensationProcessManager.RevertPromotion(empJobNum, newSalaryLocal,newHrlyRate,newCompRatio,TCC);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxChildActionOnly]
        public PartialViewResult _MeritExceedComment(int empJobNum, CommentType commentType, int rowIndex = 0)
        {
            ViewBag.rowIndex = rowIndex;
            ViewBag.EmpJobNum = empJobNum;
            ViewBag.CommentType = commentType;
            UserModel userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
            IEnumerable<CommentModel> commentModel = m_compensationProcessManager.GetComments(empJobNum, commentType, userModel.UserNum);
            ViewBag.RuleConfiguration = m_compensationProcessManager.GetRuleConfiguration();
            return PartialView(commentModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JavaScriptResult _MeritExceedCommentUpdate(int commentKey, string meritcomment, bool IsMeritEditItem = false, int MeritEmpCommentNum = 0)
        {
            if (!string.IsNullOrEmpty(meritcomment))
            {
                UserModel userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
                EmployeeModel employeeModel = m_sessionManager.GetSession<EmployeeModel>(SessionConstants.EmployeeModel);
                CommentInputModel commentInput = new CommentInputModel();
                commentInput.Comment = HttpUtility.HtmlDecode(meritcomment);
                commentInput.CommentKey = commentKey;
                commentInput.EmpCommentNum = MeritEmpCommentNum;
                commentInput.CompensationCommentTypeNum = 1;
                commentInput.CommentedEmployeeNum = userModel.UserNum;

                m_compensationProcessManager.PutUpdateComments(commentInput, CommentType.CompensationMeritMandit, IsMeritEditItem);
            }
            return JavaScript("closeAfterMeritSlide();");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxChildActionOnly]
        public async Task<JsonResult> DeleteComment(int commentKey)
        {
            await m_compensationProcessManager.DeleteComments(commentKey);
            string message = "Deleted Successfully";
            return Json(message, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult checkWorkFlowStatus()
       {
            var status = m_compensationProcessManager.GetworkflowStatus();
            return Json(status, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PayChange()
        {
            var appconfig = m_accountProcessManager.GetAppSetting();
            var userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
            userModel.TenantName = Convert.ToString(System.Security.Principal.GenericPrincipal.Current.Claims.Where(x => x.Type == "tenant").Select(c => c.Value).FirstOrDefault());
            userModel.UrlAuthority = Convert.ToString(Request.Url.Authority);
            return Redirect(appconfig.PayChangeURL);
        }
        #endregion
            #region Private Methods Implementation
        private async Task<CompensationViewModel> CompensationView(int notifiedEmployeeNum , int isMyApproval )
        {
            
            var getCurrenyExchange = await m_compensationProcessManager.GetCurrencies();
            string notifiedEmployeeName = await getManagerNameForTree(notifiedEmployeeNum);
            UserModel userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
            AppConfigModel app = m_accountProcessManager.GetAppSetting();
            int userNumSelected = m_sessionManager.GetSession<int>(SessionConstants.LoggedSelectedUserNum);
            EmployeeModel employeeModel = m_sessionManager.GetSession<EmployeeModel>(SessionConstants.EmployeeModel);
            var compensationTypeConfiguration = m_compensationProcessManager.GetCompensationTypeConfiguration();
           compensationTypeConfiguration.Year = Convert.ToInt32(m_accountProcessManager.GetAppSetting().MeritCycleYear); 
            //compensationTypeConfiguration.Year = Convert.ToInt32(m_sessionManager.GetSession<AppConfigModel>(SessionConstants.AppConfigModel).MeritCycleYear);
            compensationTypeConfiguration.UserNum = userNumSelected;
            compensationTypeConfiguration.LoggedInEmployeeNum = employeeModel.EmployeeNum;
            CompensationConfiguration compensationConfiguration = m_compensationProcessManager.GetReporteeConfiguration();

            List<SelectListItem> menu = new List<SelectListItem>();
            var model = new CompensationViewModel
            {
                EmployeeNum = (notifiedEmployeeNum == 0) ? userModel.IsAdminAccess ? userModel.AdminEmpNum ?? 0 : employeeModel.EmployeeNum : notifiedEmployeeNum,
                EmployeeName = (notifiedEmployeeNum == 0) ? employeeModel.EmployeeName : notifiedEmployeeName,
                CompMenuType = MenuType.MyTeam,
                CompensationTypeConfiguration = compensationTypeConfiguration,
                aliasName = compensationConfiguration.aliasName,
                compensationGridDisplay = compensationConfiguration.compensationGridDisplay,
                rating = m_compensationProcessManager.GetRatings().OrderBy(x => x.RatingOrder).Select(i => new SelectListItem { Text = i.RatingDescr, Value = i.RatingNum.ToString() }).ToList(),
                RatingRange = m_compensationProcessManager.GetRatings().OrderBy(x => x.RatingOrder).Select(i => new SelectListItem { Text = i.RatingRange, Value = i.RatingNum.ToString() }).ToList(),
                IsMyApproval = (isMyApproval == 0) ? false : true,
                RangeExceedPCT = app.RangeExceed,
                ExchangeCurrencies = getCurrenyExchange.ToList(),
                Menu = menu,
            };
            ViewBag.FeatureEnabled = ((model.CompensationTypeConfiguration.FeatureConfigurationAdjustment ? 1 : 0) + (model.CompensationTypeConfiguration.FeatureConfigurationLumpSum ? 1 : 0) + (model.CompensationTypeConfiguration.FeatureConfigurationMerit ? 1 : 0) + (model.CompensationTypeConfiguration.FeatureConfigurationPromotion ? 1 : 0));
            ViewBag.DefaultCurrencyNum = ((model.CompensationTypeConfiguration.BudgetCurrencyFormat == "UserCurrency") ? model.ExchangeCurrencies.Where(x => x.CurrencyCode == employeeModel.Currency).Select(x => x.CurrencyNum).FirstOrDefault() : model.ExchangeCurrencies.Where(x => x.CurrencyNum == app.BaseCurrencyNum).Select(x => x.CurrencyNum).FirstOrDefault());
            ViewBag.DefaultCurrencyCode = ((model.CompensationTypeConfiguration.BudgetCurrencyFormat == "UserCurrency") ? employeeModel.Currency : model.ExchangeCurrencies.Where(x => x.CurrencyNum == app.BaseCurrencyNum).Select(x => x.CurrencyCode).FirstOrDefault());
            ViewBag.IsInDirects = m_compensationProcessManager.IsInDirects(userModel.IsAdminAccess==true? userModel.AdminEmpNum??0:employeeModel.EmployeeNum);
            return model;
        }
        private async Task<string> getManagerNameForTree(int selectedManagerNum)
        {
            return await m_compensationProcessManager.GetManagerName(selectedManagerNum);
        }
        #endregion
    }
}
