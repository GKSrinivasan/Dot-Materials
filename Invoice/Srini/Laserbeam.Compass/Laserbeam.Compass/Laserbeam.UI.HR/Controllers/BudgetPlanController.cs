
// Copyright (c) 2015 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.

// Component Name :   BudgetPlan 
// Description    :   Allows to create Salary Plans and Manage Salary Plans	
// Author         :   Hariharasubramaniyan Chandrasekaran	
// Creation Date  :   10-Feb-2017 

using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using Laserbeam.BusinessObject.Common;
using Laserbeam.Constant.HR;
using Laserbeam.ProcessManager.Interfaces.Common;
using Laserbeam.UI.HR.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Laserbeam.UI.HR.Models;

namespace Laserbeam.UI.HR.Controllers
{
    [AuthorizeUser(UserRoleConstants.AllAdminRoles)]
    public class BudgetPlanController : Controller
    {
        #region Fields
        // Author         :   Hariharasubramaniyan Chandrasekaran		
        // Creation Date  :   10-Feb-2017 
        /// <summary>
        /// Object of IBudgetPlanProcessManager
        /// </summary>
        private IBudgetPlanProcessManager m_budgetPlanProcessManager;
        private readonly IAccountProcessManager m_accountProcessManager;
        // Author         :   Hariharasubramaniyan Chandrasekaran		
        // Creation Date  :   10-Feb-2017 
        /// <summary>
        /// Object of session manager
        /// </summary>
        private SessionManager m_sessionManager;
        #endregion
        #region Constructor
        // Author         :   Hariharasubramaniyan Chandrasekaran		
        // Creation Date  :   10-Feb-2017 
        /// <summary>
        /// Initializes the objects in this class
        /// </summary>
        /// <param name="meritProcessManager">Object of IBudgetPlanProcessManager</param>
        public BudgetPlanController(IBudgetPlanProcessManager budgetPlanProcessManager,SessionManager sessionManager, IAccountProcessManager accountProcessManager)
        {
            m_budgetPlanProcessManager = budgetPlanProcessManager;
            m_sessionManager = sessionManager;
            m_accountProcessManager = accountProcessManager;
        }
        #endregion
        #region BudgetPlan Implementation

        // Author         :   Hariharasubramaniyan Chandrasekaran	
        // Creation Date  :   11-Feb-2017 
        /// <summary>
        /// It returns the main view for manage user page
        /// </summary>
        /// <returns>It returns the main view</returns>
       [HttpGet]
        public async Task<ActionResult> Home()
        {
            List<SelectListItem> menu = new List<SelectListItem>();
            AppConfigModel app = m_accountProcessManager.GetAppSetting();
            EmployeeModel employeeModel = m_sessionManager.GetSession<EmployeeModel>(SessionConstants.EmployeeModel);
            //var currencyCountryData = m_budgetPlanProcessManager.GetBaseCountryDesc();
            var getCurrenyExchange = await m_budgetPlanProcessManager.GetCurrencies();
            var budgetPlanConfiguration = m_budgetPlanProcessManager.GetBudgetPlanConfiguration();
            var baseCurrencyCode = ((budgetPlanConfiguration.BudgetCurrencyFormat == "UserCurrency") ? employeeModel.Currency : getCurrenyExchange.Where(x => x.CurrencyNum == app.BaseCurrencyNum).Select(x => x.CurrencyCode).FirstOrDefault());
            var currencyCountryData = m_budgetPlanProcessManager.GetBaseCountryDesc(baseCurrencyCode);
            var model = new BudgetPlanModel
                {
                    BudgetPlanConfiguration = budgetPlanConfiguration,
                    ExchangeCurrencies = getCurrenyExchange.ToList(),
                    Menu = menu,
                BaseCultureCode =  currencyCountryData.CultureCode ,
                BaseCurrencyCode =currencyCountryData.CurrencyCode,
                BaseCurrencyNum = currencyCountryData.CurrencyNum,
                BaseExchangeRate = currencyCountryData.BaseExchangeRate
                //BaseCultureCode = (currencyCountryData != null) ? currencyCountryData.CultureCode : "",
                //BaseCurrencyCode = ((budgetPlanConfiguration.BudgetCurrencyFormat == "UserCurrency") ? employeeModel.Currency : getCurrenyExchange.Where(x => x.CurrencyNum == app.BaseCurrencyNum).Select(x => x.CurrencyCode).FirstOrDefault()),
                //BaseCurrencyNum = ((budgetPlanConfiguration.BudgetCurrencyFormat == "UserCurrency") ? getCurrenyExchange.Where(x => x.CurrencyCode == employeeModel.Currency).Select(x => x.CurrencyNum).FirstOrDefault() : getCurrenyExchange.Where(x => x.CurrencyNum == app.BaseCurrencyNum).Select(x => x.CurrencyNum).FirstOrDefault()),
                //BaseExchangeRate = (currencyCountryData != null) ? currencyCountryData.BaseExchangeRate : 0,
            };
          var isEmpty=  m_budgetPlanProcessManager.IsBudgetDataEmpty();
            if(isEmpty)
                return View("NoData");
            else
            return View(model);
        }

        // Modified By    :   Hari
        // Modified Date  :   11-21-2017
        // Comment        :   Put the budget based on multiCountry 
        /// <summary>
        /// Update budget percentage
        /// </summary>
        /// <param name="BudgetPercent">New budget percentage to be updated</param>
        /// <returns></returns>
       [HttpPost]
       [ValidateAntiForgeryToken]
       [AjaxChildActionOnly]
        public async Task<JsonResult> PutBudgetPct([DataSourceRequest]DataSourceRequest request, string BudgetPercent,bool isProration,string filterEmployee)
        {
            UserModel userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
            string filterEmp= (filterEmployee == null) ? filterEmployee = "" : filterEmployee;
            bool status = await m_budgetPlanProcessManager.PutBudgetPct(BudgetPercent, userModel.UserNum,isProration, ((filterEmp != "") ? filterEmp.TrimStart(',') : filterEmp));
            return Json((status) ? 1 : 0);

        }
        // Modified By    :   Hari
        // Modified Date  :   11-21-2017
        // Comment        :   Budget Plan Export
        [HttpPost,ValidateAntiForgeryToken]
        public async Task BudgetPlanExport(int selectedCurrencyNum)
        {
            UserModel userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
            int employeeNum = userModel.EmployeeNum;
            string exportName = "Budget Plan Extract-" + DateTime.Now.ToShortDateString();
            var exportData = await m_budgetPlanProcessManager.GetExportBudgetData(employeeNum, selectedCurrencyNum);
             ExportExcel.ToExcel(exportData, exportName);
        }


        // Modified By    :   Hari
        // Modified Date  :   11-21-2017
        // Comment        :   Clear the Proration Values
        /// <summary>
        /// Update budget percentage
        /// </summary>
        /// <param name="BudgetPercent">New budget percentage to be updated</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxChildActionOnly]
        public async Task<JsonResult> ClearProrationValues(string isProration)
        {
            UserModel userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
            bool status = await m_budgetPlanProcessManager.ClearProrationValues(isProration, userModel.UserNum);
            return Json((status) ? 1 : 0);

        }


        // Author         :   Hariharasubramaniyan Chandrasekaran	
        // Creation Date  :   11-Feb-2017 
        /// <summary>
        ///  Gets data for budget grid
        /// </summary>
        /// <returns>Returns json data for budget grid</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxChildActionOnly]
        public async Task<JsonResult> GetBudgetPlanGridData([DataSourceRequest]DataSourceRequest request, int selectedCurrencyNum)
        {
            UserModel userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel); 
            int employeeNum = userModel.EmployeeNum; //(userModel.UserRole == UserRoleConstants.WorkFlowAdmin || userModel.UserRole == UserRoleConstants.Support || userModel.UserRole == UserRoleConstants.Admin) ? m_budgetPlanProcessManager.GetTopLevelManager(userModel.EmployeeNum) : userModel.EmployeeNum;
            var result = await m_budgetPlanProcessManager.GetEmployeeBudget(employeeNum, selectedCurrencyNum);
            return Json(result.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        // Modified By    :   Hari
        // Modified Date  :   11-21-2017
        // Comment        :   Get Proration popup
        [HttpGet]
        public PartialViewResult _Proration( )
        {
            BudgetProration model = m_budgetPlanProcessManager.GetBudgetProration();
            return PartialView(model);
        }
        // Modified By    :   Hari
        // Modified Date  :   11-21-2017
        // Comment        :   Get Proration Popup 
        [HttpGet]
        public PartialViewResult _ProrationPopUp()
        {
            BudgetProration model = new BudgetProration();
            return PartialView("_Proration",model);
        }
        // Modified By    :   Hari
        // Modified Date  :   11-21-2017
        // Comment        :   Update Proration Values 
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxChildActionOnly]
        public async Task<JavaScriptResult> _Proration(BudgetProration budgetProration,string processStartDateValue,string processEndDateValue,string prorationType,string isMerit)
        {
            ActionResult result = View();
            UserModel userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
            BudgetProrationUpdateModel updateModel = new BudgetProrationUpdateModel();
            updateModel.BudgetProrationValue = true;
            updateModel.IsMerit = isMerit == "true" ? true : false;
            updateModel.ProrateStartDate =  processStartDateValue;
            updateModel.ProrateEndDate =  processEndDateValue;
            updateModel.ProrationDatesPerMonth = budgetProration.ProrationDatesPerMonth;
            updateModel.ProrationDuration = budgetProration.ProrationDuration;
            updateModel.ProrationType = prorationType;
            bool status = await m_budgetPlanProcessManager.PutBudgetProration(updateModel, userModel.UserNum);
            return JavaScript("closeAfterProration();");

        }
        
        // Author         :   Hariharasubramaniyan Chandrasekaran	
        // Creation Date  :   11-Feb-2017 
        /// <summary>
        ///  Updates the data from the grid
        /// </summary>
        /// <param name="dataSourceRequest">data source request object</param>
        /// <param name="budgetPlanGridData">data to update</param>
        /// <returns>Json result</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxChildActionOnly]
        public async Task<JsonResult> UpdateBudgetPlanGridData([DataSourceRequest]DataSourceRequest request, [Bind(Prefix = "models")] List<BudgetPlanGridData> BudgetPlanGridData)
        {
          
            var userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel); 
            var loginUserNum = userModel.UserNum;
            string message = string.Empty;
            bool updatesStatus = await m_budgetPlanProcessManager.UpdateEmployeeBudget(BudgetPlanGridData, loginUserNum);
            return Json(new
            {
                Message = "Updated Successfully"
            });
        }
        // Author         :   Hariharasubramaniyan Chandrasekaran	
        // Creation Date  :   11-Feb-2017 
        /// <summary>
        ///   To return filter and sort partial view
        /// </summary>
        /// <returns>Return partial view result</returns>
        [HttpGet]
        public PartialViewResult _FilterSort(bool IsProratedBudgetEnable, bool IsBudgetEnable)
        {

            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem { Text = "Manager Name", Value = "ManagerName" });
            if (IsBudgetEnable)
            {
                list.Add(new SelectListItem { Text = "Budget Pct", Value = "BudgetPercent" });
                list.Add(new SelectListItem { Text = "Budget Amount", Value = "Budget" });
            }
            if (IsProratedBudgetEnable)
            {
                list.Add(new SelectListItem { Text = "Prorated Pct", Value = "ProratedBudgetPct" });
                list.Add(new SelectListItem { Text = "Prorated Amt", Value = "ProratedBudget" });
            }
            list.Add(new SelectListItem { Text = "Adjusted Pct", Value = "AdjustedBudgetPct" });
            list.Add(new SelectListItem { Text = "Adjusted Amt", Value = "AdjustedBudget" });
            return PartialView(list);
        }
        #endregion
    }
}