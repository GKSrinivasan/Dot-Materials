using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Laserbeam.BusinessObject.Common;
using Laserbeam.BusinessObject.Common.AnalyticsBusinessObjects;
using Laserbeam.Constant.HR;
using Laserbeam.EntityManager.TenantMaster;
using Laserbeam.Libraries.Common;
using Laserbeam.ProcessManager.Interfaces.Common;
using Laserbeam.UI.HR.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Laserbeam.UI.HR.Controllers
{
    [AuthorizeUser(UserRoleConstants.AllRoles)]
    public class AnalyticsController : Controller
    {
        #region Fields
        private SessionManager m_sessionManager = new SessionManager();
        private IAnalyticsProcessManager m_analyticsProcessManager;
        private ICompensationProcessManager m_compensationProcessManager;
        private readonly IAccountProcessManager m_accountProcessManager;
        #endregion
        #region Constructor
        public AnalyticsController(IAnalyticsProcessManager analyticsProcessManager, IAccountProcessManager accountProcessManager, ICompensationProcessManager compensationProcessManager)
        {
            m_analyticsProcessManager = analyticsProcessManager;
            m_accountProcessManager = accountProcessManager;
            m_compensationProcessManager = compensationProcessManager;
        }
        #endregion
        #region Public Methods Implementation
        [HttpGet]
        public async Task<ActionResult> Home()
        {
            UserModel userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
            ViewBag.EmployeeNum = userModel.IsAdminAccess ? userModel.AdminEmpNum ?? 0 : userModel.EmployeeNum;
            EmployeeModel employeeModel = m_sessionManager.GetSession<EmployeeModel>(SessionConstants.EmployeeModel);
            int loggedInEmpNum = m_sessionManager.GetSession<EmployeeModel>(SessionConstants.EmployeeModel).EmployeeNum;
            ViewBag.IsBonusEnable = m_analyticsProcessManager.isBonusEnable();
            int baseCurrencyNum = Convert.ToInt32(m_accountProcessManager.GetAppSetting().BaseCurrencyNum);
            var compensationTypeConfiguration = m_compensationProcessManager.GetCompensationTypeConfiguration();
            var getCurrenyExchange = await m_compensationProcessManager.GetCurrencies();
            int DefaultCurrencyNum = ((compensationTypeConfiguration.BudgetCurrencyFormat == "UserCurrency") ? getCurrenyExchange.Where(x => x.CurrencyCode == employeeModel.Currency).Select(x => x.CurrencyNum).FirstOrDefault() : getCurrenyExchange.Where(x => x.CurrencyNum == baseCurrencyNum).Select(x => x.CurrencyNum).FirstOrDefault());
            var proration = await m_analyticsProcessManager.GetProrationChartData(loggedInEmpNum, employeeModel.EmployeeNum, true, false, userModel.UserNum, DefaultCurrencyNum);
            var avgCostEmployee = await m_analyticsProcessManager.GetAvgCostChartData(loggedInEmpNum, employeeModel.EmployeeNum, true, false, userModel.UserNum, DefaultCurrencyNum);
            var chgSalaryEmployee = await m_analyticsProcessManager.GetChgSalaryChartData(loggedInEmpNum, employeeModel.EmployeeNum, true, false, userModel.UserNum, DefaultCurrencyNum);
            var increaseCountEmp = await m_analyticsProcessManager.GetIncreaseCountChartData(loggedInEmpNum, employeeModel.EmployeeNum, true, false, userModel.UserNum);
            var marketMidCompRatio = await m_analyticsProcessManager.GetCompRatioChartData(loggedInEmpNum, employeeModel.EmployeeNum, true, false, userModel.UserNum, DefaultCurrencyNum);
            var populationCount = await m_analyticsProcessManager.GetPopulationCountChartData(loggedInEmpNum, employeeModel.EmployeeNum, true, false, userModel.UserNum);
            var outlierCount = await m_analyticsProcessManager.GetOutlierCountChartData(loggedInEmpNum, employeeModel.EmployeeNum, true, false, userModel.UserNum, DefaultCurrencyNum);
            var increaseDistributionEmp = await m_analyticsProcessManager.GetIncraeseDistribututionEmpChartData(loggedInEmpNum, employeeModel.EmployeeNum, true, false, userModel.UserNum);
            var increaseManager = await m_analyticsProcessManager.GetIncraeseDistribututionMgrChartData(loggedInEmpNum, employeeModel.EmployeeNum, true, false, userModel.UserNum);
            var CompRevenue = await m_analyticsProcessManager.GetCompRevenueChartData(loggedInEmpNum, employeeModel.EmployeeNum, true, false, userModel.UserNum);
            var BonusTarget = await m_analyticsProcessManager.GetBonusTargetChartData(loggedInEmpNum, employeeModel.EmployeeNum, true, false, userModel.UserNum);
            var BonusEligibility = await m_analyticsProcessManager.GetBonusEligibilityChartData(loggedInEmpNum, employeeModel.EmployeeNum, true, false, userModel.UserNum);
            var BonusCompRatio = await m_analyticsProcessManager.GetBonusCompRatioChartData(loggedInEmpNum, employeeModel.EmployeeNum, true, false, userModel.UserNum);
            ViewBag.MultiCurrencyEnabled = compensationTypeConfiguration.FeatureConfigurationMultiCurrencyDisplay;
            var model = new ChartData
            {
                ProRationEECount = proration.ProRationEECount,
                ProRationTotalIncrease = proration.ProRationTotalIncrease,
                ProRationTotalSalary = proration.ProRationTotalSalary,
                AvgCostTotalEligibleEEs = avgCostEmployee.AvgCostTotalEligibleEEs,
                AvgCostTotalCompSalary = avgCostEmployee.AvgCostTotalCompSalary,
                AvgCostPerEEcost = avgCostEmployee.AvgCostPerEEcost,
                ChnSalaryCurrent = chgSalaryEmployee.ChnSalaryCurrent,
                ChnSalaryNew = chgSalaryEmployee.ChnSalaryNew,
                Diffence = chgSalaryEmployee.Diffence,
                Merit = increaseCountEmp.Merit,
                LumpSum = increaseCountEmp.LumpSum,
                Promotion = increaseCountEmp.Promotion,
                Adjustment = increaseCountEmp.Adjustment,
                CRNewSalary = marketMidCompRatio.CRNewSalary,
                CRMarketMid = marketMidCompRatio.CRMarketMid,
                CRratio = marketMidCompRatio.CRratio,
                Active = populationCount.Active,
                Termed = populationCount.Termed,
                Total = populationCount.Total,
                OutliersCount = outlierCount.OutliersCount,
                OutlierTotalIncrease = outlierCount.OutlierTotalIncrease,
                OutlierTotalSalary = outlierCount.OutlierTotalSalary,
                TotalRevenue = CompRevenue.TotalRevenue,
                RevenuePct = CompRevenue.RevenuePct,
                MeritAmt = CompRevenue.MeritAmt,
                MeritPct = CompRevenue.MeritPct,
                LumpSumAmt = CompRevenue.LumpSumAmt,
                LumpSumPct = CompRevenue.LumpSumPct,
                PromotionAmt = CompRevenue.PromotionAmt,
                PromotionAmtPct = CompRevenue.PromotionAmtPct,
                AdjustmentAmt = CompRevenue.AdjustmentAmt,
                AdjustmentPct = CompRevenue.AdjustmentPct,
                EmpCount = increaseDistributionEmp.EmpCount,
                TotalIncrease = increaseDistributionEmp.TotalIncrease,
                UtilizationPct = increaseDistributionEmp.UtilizationPct,
                MgrCount = increaseManager.MgrCount,
                MgrTotalIncrease = increaseManager.MgrTotalIncrease,
                MgrUtilizationPct = increaseManager.MgrUtilizationPct,
                TargetAmt = BonusTarget.TargetAmt,
                PayoutAmt = BonusTarget.PayoutAmt,
                TargetRatio = BonusTarget.TargetRatio,
                TargetAmtValue = BonusTarget.TargetAmtValue,
                TotalCount = BonusEligibility.TotalCount,
                EligibleCount = BonusEligibility.EligibleCount,
                IncrBonusCount = BonusEligibility.IncrBonusCount,
                ReceivedPct = BonusEligibility.ReceivedPct,
                TotalCash = BonusCompRatio.TotalCash,
                TotalBonusAmt = BonusCompRatio.TotalBonusAmt,
                BonusCompensationRatio = BonusCompRatio.BonusCompensationRatio,
                ExchangeCurrencies = getCurrenyExchange.ToList(),

            };
            return View(model);
        }
        [HttpPost,ValidateAntiForgeryToken]
        public async Task<JsonResult> GetAnalyticsManagerTree(int managerNum)
        {
            int userNumSelected = m_sessionManager.GetSession<int>(SessionConstants.LoggedSelectedUserNum);
            UserModel userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
            EmployeeModel employeeModel = m_sessionManager.GetSession<EmployeeModel>(SessionConstants.EmployeeModel);
            var result = await m_analyticsProcessManager.GetAnalyticsManagerTree(employeeModel.EmployeeNum, employeeModel.EmployeeNum, userModel.UserNum, userNumSelected);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<JsonResult> GetMeritIncreaseByDepartment([DataSourceRequest] DataSourceRequest request, int employeeNum, string groupBy, int currencyCodeNum=0, bool isRollup = true, bool isSelectedRollup = true)
        {
            int loggedInEmpNum = m_sessionManager.GetSession<EmployeeModel>(SessionConstants.EmployeeModel).EmployeeNum;
            UserModel userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
            EmployeeModel employeeModel = m_sessionManager.GetSession<EmployeeModel>(SessionConstants.EmployeeModel);
            var compensationTypeConfiguration = m_compensationProcessManager.GetCompensationTypeConfiguration();
            int baseCurrencyNum = Convert.ToInt32(m_accountProcessManager.GetAppSetting().BaseCurrencyNum);
            var getCurrenyExchange = await m_compensationProcessManager.GetCurrencies();
            int DefaultCurrencyNum = ((compensationTypeConfiguration.BudgetCurrencyFormat == "UserCurrency") ? getCurrenyExchange.Where(x => x.CurrencyCode == employeeModel.Currency).Select(x => x.CurrencyNum).FirstOrDefault() : getCurrenyExchange.Where(x => x.CurrencyNum == baseCurrencyNum).Select(x => x.CurrencyNum).FirstOrDefault());
            var model = await m_analyticsProcessManager.GetMeritIncreaseByDepartment(loggedInEmpNum, employeeNum, groupBy, isRollup, isSelectedRollup, userModel.UserNum, currencyCodeNum == 0 ? DefaultCurrencyNum : currencyCodeNum);
            return Json(model.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        [HttpPost, ValidateAntiForgeryToken, AjaxChildActionOnly]
        public async Task<JsonResult> GetMeritIncreaseByGender([DataSourceRequest] DataSourceRequest request, int employeeNum, int currencyCodeNum=0, string groupBy = "Gender", bool isRollup = true, bool isSelectedRollup = true)
        {
            int loggedInEmpNum = m_sessionManager.GetSession<EmployeeModel>(SessionConstants.EmployeeModel).EmployeeNum;
            UserModel userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
            EmployeeModel employeeModel = m_sessionManager.GetSession<EmployeeModel>(SessionConstants.EmployeeModel);
            var compensationTypeConfiguration = m_compensationProcessManager.GetCompensationTypeConfiguration();
            int baseCurrencyNum = Convert.ToInt32(m_accountProcessManager.GetAppSetting().BaseCurrencyNum);
            var getCurrenyExchange = await m_compensationProcessManager.GetCurrencies();
            int DefaultCurrencyNum = ((compensationTypeConfiguration.BudgetCurrencyFormat == "UserCurrency") ? getCurrenyExchange.Where(x => x.CurrencyCode == employeeModel.Currency).Select(x => x.CurrencyNum).FirstOrDefault() : getCurrenyExchange.Where(x => x.CurrencyNum == baseCurrencyNum).Select(x => x.CurrencyNum).FirstOrDefault());
            var model = await m_analyticsProcessManager.GetMeritIncreaseByGender(loggedInEmpNum, employeeNum, groupBy, isRollup, isSelectedRollup, userModel.UserNum ,currencyCodeNum == 0 ? DefaultCurrencyNum : currencyCodeNum);
            return Json(model.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        [HttpPost, ValidateAntiForgeryToken, AjaxChildActionOnly]
        public async Task<JsonResult> GetMeritIncreasebyGrade([DataSourceRequest] DataSourceRequest request, int employeeNum, int currencyCodeNum=0, string groupBy = "Gender", bool isRollup = true, bool isSelectedRollup = true)
        {
            int loggedInEmpNum = m_sessionManager.GetSession<EmployeeModel>(SessionConstants.EmployeeModel).EmployeeNum;
            UserModel userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
            EmployeeModel employeeModel = m_sessionManager.GetSession<EmployeeModel>(SessionConstants.EmployeeModel);
            var compensationTypeConfiguration = m_compensationProcessManager.GetCompensationTypeConfiguration();
            int baseCurrencyNum = Convert.ToInt32(m_accountProcessManager.GetAppSetting().BaseCurrencyNum);
            var getCurrenyExchange = await m_compensationProcessManager.GetCurrencies();
            int DefaultCurrencyNum = ((compensationTypeConfiguration.BudgetCurrencyFormat == "UserCurrency") ? getCurrenyExchange.Where(x => x.CurrencyCode == employeeModel.Currency).Select(x => x.CurrencyNum).FirstOrDefault() : getCurrenyExchange.Where(x => x.CurrencyNum == baseCurrencyNum).Select(x => x.CurrencyNum).FirstOrDefault());
            var model = await m_analyticsProcessManager.GetMeritIncreasebyGrade(loggedInEmpNum, employeeNum, groupBy, isRollup, isSelectedRollup, userModel.UserNum ,currencyCodeNum == 0 ? DefaultCurrencyNum : currencyCodeNum);
            return Json(model.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public PartialViewResult _MeritIncreaseByDepartment()
        {
            return PartialView();
        }

        [HttpPost,ValidateAntiForgeryToken]
        public async Task<PartialViewResult> _ChartPage(int employeeNum,int selectedCurrencyNum, bool isRollup = false, bool isSelectedRollup = false)
        {
            int loggedInEmpNum = m_sessionManager.GetSession<EmployeeModel>(SessionConstants.EmployeeModel).EmployeeNum;
            UserModel userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
            EmployeeModel employeeModel = m_sessionManager.GetSession<EmployeeModel>(SessionConstants.EmployeeModel);
            ViewBag.IsBonusEnable = m_analyticsProcessManager.isBonusEnable();
            var proration = await m_analyticsProcessManager.GetProrationChartData(loggedInEmpNum, employeeNum, isRollup, isSelectedRollup, userModel.UserNum, selectedCurrencyNum);
            var avgCostEmployee = await m_analyticsProcessManager.GetAvgCostChartData(loggedInEmpNum, employeeNum, isRollup, isSelectedRollup, userModel.UserNum, selectedCurrencyNum);
            var chgSalaryEmployee = await m_analyticsProcessManager.GetChgSalaryChartData(loggedInEmpNum, employeeNum, isRollup, isSelectedRollup, userModel.UserNum, selectedCurrencyNum);
            var increaseCountEmp = await m_analyticsProcessManager.GetIncreaseCountChartData(loggedInEmpNum, employeeNum, isRollup, isSelectedRollup, userModel.UserNum);
            var marketMidCompRatio = await m_analyticsProcessManager.GetCompRatioChartData(loggedInEmpNum, employeeNum, isRollup, isSelectedRollup, userModel.UserNum, selectedCurrencyNum);
            var populationCount = await m_analyticsProcessManager.GetPopulationCountChartData(loggedInEmpNum, employeeNum, isRollup, isSelectedRollup, userModel.UserNum);
            var outlierCount = await m_analyticsProcessManager.GetOutlierCountChartData(loggedInEmpNum, employeeNum, isRollup, isSelectedRollup, userModel.UserNum, selectedCurrencyNum);
            var increaseDistributionEmp = await m_analyticsProcessManager.GetIncraeseDistribututionEmpChartData(loggedInEmpNum, employeeNum, isRollup, isSelectedRollup, userModel.UserNum);
            var increaseManager = await m_analyticsProcessManager.GetIncraeseDistribututionMgrChartData(loggedInEmpNum, employeeNum, isRollup, isSelectedRollup, userModel.UserNum);
            var CompRevenue = await m_analyticsProcessManager.GetCompRevenueChartData(loggedInEmpNum, employeeNum, isRollup, isSelectedRollup, userModel.UserNum);
            var BonusTarget = await m_analyticsProcessManager.GetBonusTargetChartData(loggedInEmpNum, employeeNum, isRollup, isSelectedRollup, userModel.UserNum);
            var BonusEligibility = await m_analyticsProcessManager.GetBonusEligibilityChartData(loggedInEmpNum, employeeNum, isRollup, isSelectedRollup, userModel.UserNum);
            var BonusCompRatio = await m_analyticsProcessManager.GetBonusCompRatioChartData(loggedInEmpNum, employeeNum, isRollup, isSelectedRollup, userModel.UserNum);
            var getCurrenyExchange = await m_compensationProcessManager.GetCurrencies();
            var model = new ChartData
            {
                ProRationEECount = proration.ProRationEECount,
                ProRationTotalIncrease = proration.ProRationTotalIncrease,
                ProRationTotalSalary = proration.ProRationTotalSalary,
                AvgCostTotalEligibleEEs = avgCostEmployee.AvgCostTotalEligibleEEs,
                AvgCostTotalCompSalary = avgCostEmployee.AvgCostTotalCompSalary,
                AvgCostPerEEcost = avgCostEmployee.AvgCostPerEEcost,
                ChnSalaryCurrent = chgSalaryEmployee.ChnSalaryCurrent,
                ChnSalaryNew = chgSalaryEmployee.ChnSalaryNew,
                Diffence = chgSalaryEmployee.Diffence,
                Merit = increaseCountEmp.Merit,
                LumpSum = increaseCountEmp.LumpSum,
                Promotion = increaseCountEmp.Promotion,
                Adjustment = increaseCountEmp.Adjustment,
                CRNewSalary = marketMidCompRatio.CRNewSalary,
                CRMarketMid = marketMidCompRatio.CRMarketMid,
                CRratio = marketMidCompRatio.CRratio,
                Active = populationCount.Active,
                Termed = populationCount.Termed,
                Total = populationCount.Total,
                OutliersCount = outlierCount.OutliersCount,
                OutlierTotalIncrease = outlierCount.OutlierTotalIncrease,
                OutlierTotalSalary = outlierCount.OutlierTotalSalary,
                TotalRevenue = CompRevenue.TotalRevenue,
                RevenuePct = CompRevenue.RevenuePct,
                MeritAmt = CompRevenue.MeritAmt,
                MeritPct = CompRevenue.MeritPct,
                LumpSumAmt = CompRevenue.LumpSumAmt,
                LumpSumPct = CompRevenue.LumpSumPct,
                PromotionAmt = CompRevenue.PromotionAmt,
                PromotionAmtPct = CompRevenue.PromotionAmtPct,
                AdjustmentAmt = CompRevenue.AdjustmentAmt,
                AdjustmentPct = CompRevenue.AdjustmentPct,
                EmpCount = increaseDistributionEmp.EmpCount,
                TotalIncrease = increaseDistributionEmp.TotalIncrease,
                UtilizationPct = increaseDistributionEmp.UtilizationPct,
                MgrCount  = increaseManager.MgrCount,
                MgrTotalIncrease = increaseManager.MgrTotalIncrease,
                MgrUtilizationPct = increaseManager.MgrUtilizationPct,
                TargetAmt = BonusTarget.TargetAmt,
                PayoutAmt = BonusTarget.PayoutAmt,
                TargetRatio = BonusTarget.TargetRatio,
                TargetAmtValue = BonusTarget.TargetAmtValue,
                TotalCount = BonusEligibility.TotalCount,
                EligibleCount = BonusEligibility.EligibleCount,
                IncrBonusCount = BonusEligibility.IncrBonusCount,
                ReceivedPct = BonusEligibility.ReceivedPct,
                TotalCash = BonusCompRatio.TotalCash,
                TotalBonusAmt = BonusCompRatio.TotalBonusAmt,
                BonusCompensationRatio = BonusCompRatio.BonusCompensationRatio,
                ExchangeCurrencies = getCurrenyExchange.ToList(),

            };
            return PartialView(model);
        }
        [HttpGet]
        public PartialViewResult _MeritIncreaseByGrade()
        {
            return PartialView();
        }
        [HttpGet]
        public PartialViewResult _MeritIncreaseByGender()
        {
            return PartialView();
        }
        [HttpPost, ValidateAntiForgeryToken,AjaxChildActionOnly]
        public async Task<ActionResult> _PayRangeDistribution(int employeeNum, bool isRollup = false, bool isSelectedRollup = false)
        {
            int loggedInEmpNum = m_sessionManager.GetSession<EmployeeModel>(SessionConstants.EmployeeModel).EmployeeNum;
            UserModel userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
            var payRangeDistributions = await m_analyticsProcessManager.GetPayRangeData(loggedInEmpNum, employeeNum, isRollup, isSelectedRollup, userModel.UserNum);
            var model = new RatingDistributionModel
            {
                PayRangeDistribution = payRangeDistribution(payRangeDistributions),
                PayRangeDistributionChart = payRangeDistributionCurve(payRangeDistributions)
            };
            return PartialView(model);
        }
        #region Reports
        [HttpPost, ValidateAntiForgeryToken]
        public void CountryBudgetReport(int selectedEmployeeNum, bool isRollup = false, bool isSelectedRollup = false)
        {
            string updatedConnectionString = UpdateDataSource();
            int loggedInEmpNum = m_sessionManager.GetSession<EmployeeModel>(SessionConstants.EmployeeModel).EmployeeNum;
            UserModel userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
            DateTime currentDateTime = DateTime.Now;
            var currentDate = (currentDateTime.ToString("dd-MMM-yyyy"));
            Reports.ManagerReports.BudgetUtilizationByCountry BudgetCountry = new Reports.ManagerReports.BudgetUtilizationByCountry(loggedInEmpNum, selectedEmployeeNum, isRollup, isSelectedRollup, userModel.UserNum, updatedConnectionString);
            MemoryStream pdfContent = new MemoryStream();
            BudgetCountry.ExportToPdf(pdfContent);
            pdfContent.Position = 0;
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=Budget Utilization by Country-" + currentDate + ".pdf");
            Response.Buffer = true;
            pdfContent.WriteTo(Response.OutputStream);
            Response.Flush();
            Response.End();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public void MeritExceptionReport(int selectedEmployeeNum, bool isRollup = false, bool isSelectedRollup = false)
        {
            string updatedConnectionString = UpdateDataSource();
            int loggedInEmpNum = m_sessionManager.GetSession<EmployeeModel>(SessionConstants.EmployeeModel).EmployeeNum;
            UserModel userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
            DateTime currentDateTime = DateTime.Now;
            var currentDate = (currentDateTime.ToString("dd-MMM-yyyy"));
            Reports.ManagerReports.MeritExceptions MeritExceptions = new Reports.ManagerReports.MeritExceptions(loggedInEmpNum, selectedEmployeeNum, isRollup, isSelectedRollup, userModel.UserNum, updatedConnectionString);
            MemoryStream pdfContent = new MemoryStream();
            MeritExceptions.ExportToPdf(pdfContent);
            pdfContent.Position = 0;
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=Merit Exception-" + currentDate + ".pdf");
            Response.Buffer = true;
            pdfContent.WriteTo(Response.OutputStream);
            Response.Flush();
            Response.End();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public void BudgetStatusByDepartement(int selectedEmployeeNum, bool isRollup = false, bool isSelectedRollup = false)
        {
            string updatedConnectionString = UpdateDataSource();
            int loggedInEmpNum = m_sessionManager.GetSession<EmployeeModel>(SessionConstants.EmployeeModel).EmployeeNum;
            UserModel userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
            DateTime currentDateTime = DateTime.Now;
            var currentDate = (currentDateTime.ToString("dd-MMM-yyyy"));
            Reports.ManagerReports.ManagerBudgetByFunction ManagerBudget = new Reports.ManagerReports.ManagerBudgetByFunction(loggedInEmpNum, selectedEmployeeNum, isRollup, isSelectedRollup, userModel.UserNum, updatedConnectionString);
            MemoryStream pdfContent = new MemoryStream();
            ManagerBudget.ExportToPdf(pdfContent);
            pdfContent.Position = 0;
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=Review and Budget Status-" + currentDate + ".pdf");
            Response.Buffer = true;
            pdfContent.WriteTo(Response.OutputStream);
            Response.Flush();
            Response.End();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public void IncreaseDistributionByManager(int selectedEmployeeNum, bool isRollup = false, bool isSelectedRollup = false)
        {
            string updatedConnectionString = UpdateDataSource();
            int loggedInEmpNum = m_sessionManager.GetSession<EmployeeModel>(SessionConstants.EmployeeModel).EmployeeNum;
            UserModel userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
            DateTime currentDateTime = DateTime.Now;
            var currentDate = (currentDateTime.ToString("dd-MMM-yyyy"));
            Reports.ManagerReports.IncraeaseDistributionManager IncreaseDistribution = new Reports.ManagerReports.IncraeaseDistributionManager(loggedInEmpNum, selectedEmployeeNum, isRollup, isSelectedRollup, userModel.UserNum, updatedConnectionString);
            MemoryStream pdfContent = new MemoryStream();
            IncreaseDistribution.ExportToPdf(pdfContent);
            pdfContent.Position = 0;
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=Increase Distribution by Manager-" + currentDate + ".pdf");
            Response.Buffer = true;
            pdfContent.WriteTo(Response.OutputStream);
            Response.Flush();
            Response.End();
        }
        #endregion
        #region Exports
        [HttpPost,ValidateAntiForgeryToken]
        public async Task IncreaseExport(int employeeNum, string groupBy, bool isRollup = true, bool isSelectedRollup = true)
        {
            int loggedInEmpNum = m_sessionManager.GetSession<EmployeeModel>(SessionConstants.EmployeeModel).EmployeeNum;
            UserModel userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
            string exportName = "Increase Export";
            string sheetName = "Increase Export";
            var exportData = await m_analyticsProcessManager.IncreaseExport(loggedInEmpNum, employeeNum, groupBy,isRollup, isSelectedRollup, userModel.UserNum);
            ExportExcel.ToExcel(exportData, exportName, sheetName);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task PayRangeExport(int employeeNum, string groupBy, bool isRollup = true, bool isSelectedRollup = true)
        {
            int loggedInEmpNum = m_sessionManager.GetSession<EmployeeModel>(SessionConstants.EmployeeModel).EmployeeNum;
            UserModel userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
            string exportName = "PayRange Export";
            string sheetName = "PayRange Export";
            var exportData = await m_analyticsProcessManager.PayRangeExport(loggedInEmpNum, employeeNum, groupBy, isRollup, isSelectedRollup, userModel.UserNum);
            ExportExcel.ToExcel(exportData, exportName, sheetName);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task CompExport(int employeeNum, string groupBy, bool isRollup = true, bool isSelectedRollup = true)
        {
            int loggedInEmpNum = m_sessionManager.GetSession<EmployeeModel>(SessionConstants.EmployeeModel).EmployeeNum;
            UserModel userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
            string exportName = "CompExport";
            string sheetName = "CompExport";
            var exportData = await m_analyticsProcessManager.CompExport(loggedInEmpNum, employeeNum, groupBy, isRollup, isSelectedRollup, userModel.UserNum);
            ExportExcel.ToExcel(exportData, exportName, sheetName);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task ProRateExport(int employeeNum, string groupBy, bool isRollup = true, bool isSelectedRollup = true)
        {
            int loggedInEmpNum = m_sessionManager.GetSession<EmployeeModel>(SessionConstants.EmployeeModel).EmployeeNum;
            UserModel userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
            string exportName = "ProRateExport";
            string sheetName = "ProRateExport";
            var exportData = await m_analyticsProcessManager.ProRateExport(loggedInEmpNum, employeeNum, groupBy, isRollup, isSelectedRollup, userModel.UserNum);
            ExportExcel.ToExcel(exportData, exportName, sheetName);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task AvgCostExport(int employeeNum, string groupBy, bool isRollup = true, bool isSelectedRollup = true)
        {
            int loggedInEmpNum = m_sessionManager.GetSession<EmployeeModel>(SessionConstants.EmployeeModel).EmployeeNum;
            UserModel userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
            string exportName = "AvgCostExport";
            string sheetName = "AvgCostExport";
            var exportData = await m_analyticsProcessManager.AvgCostExport(loggedInEmpNum, employeeNum, groupBy, isRollup, isSelectedRollup, userModel.UserNum);
            ExportExcel.ToExcel(exportData, exportName, sheetName);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task IncreaseChartExport(int employeeNum, string groupBy, bool isRollup = true, bool isSelectedRollup = true)
        {
            int loggedInEmpNum = m_sessionManager.GetSession<EmployeeModel>(SessionConstants.EmployeeModel).EmployeeNum;
            UserModel userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
            string exportName = "IncreaseChartExport";
            string sheetName = "IncreaseChartExport";
            var exportData = await m_analyticsProcessManager.IncreaseChartExport(loggedInEmpNum, employeeNum, groupBy, isRollup, isSelectedRollup, userModel.UserNum);
            ExportExcel.ToExcel(exportData, exportName, sheetName);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task IncreaseEmpCountExport(int employeeNum, string groupBy, bool isRollup = true, bool isSelectedRollup = true)
        {
            int loggedInEmpNum = m_sessionManager.GetSession<EmployeeModel>(SessionConstants.EmployeeModel).EmployeeNum;
            UserModel userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
            string exportName = "IncreaseChartExport";
            string sheetName = "IncreaseChartExport";
            var exportData = await m_analyticsProcessManager.IncreaseEmpCountExport(loggedInEmpNum, employeeNum, groupBy, isRollup, isSelectedRollup, userModel.UserNum);
            ExportExcel.ToExcel(exportData, exportName, sheetName);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task MarketDataExport(int employeeNum, string groupBy, bool isRollup = true, bool isSelectedRollup = true)
        {
            int loggedInEmpNum = m_sessionManager.GetSession<EmployeeModel>(SessionConstants.EmployeeModel).EmployeeNum;
            UserModel userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
            string exportName = "MarketDataExport";
            string sheetName = "MarketDataExport";
            var exportData = await m_analyticsProcessManager.MarketDataExport(loggedInEmpNum, employeeNum, groupBy, isRollup, isSelectedRollup, userModel.UserNum);
            ExportExcel.ToExcel(exportData, exportName, sheetName);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task PopulationDataExport(int employeeNum, string groupBy, bool isRollup = true, bool isSelectedRollup = true)
        {
            int loggedInEmpNum = m_sessionManager.GetSession<EmployeeModel>(SessionConstants.EmployeeModel).EmployeeNum;
            UserModel userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
            string exportName = "PopulationDataExport";
            string sheetName = "PopulationDataExport";
            var exportData = await m_analyticsProcessManager.PopulationDataExport(loggedInEmpNum, employeeNum, groupBy, isRollup, isSelectedRollup, userModel.UserNum);
            ExportExcel.ToExcel(exportData, exportName, sheetName);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task OutlierExport(int employeeNum, string groupBy, bool isRollup = true, bool isSelectedRollup = true)
        {
            int loggedInEmpNum = m_sessionManager.GetSession<EmployeeModel>(SessionConstants.EmployeeModel).EmployeeNum;
            UserModel userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
            string exportName = "OutlierExport";
            string sheetName = "OutlierExport";
            var exportData = await m_analyticsProcessManager.OutlierExport(loggedInEmpNum, employeeNum, groupBy, isRollup, isSelectedRollup, userModel.UserNum);
            ExportExcel.ToExcel(exportData, exportName, sheetName);
        }
        #endregion
        #endregion
        #region Private Method Implementation
        private List<PayRangeDistribution> payRangeDistribution(PayRangeDistribution payRangeDistributions)
        {
            List<PayRangeDistribution> payRangeDistribution = new List<PayRangeDistribution>
         {
        new PayRangeDistribution { Typestr = payRangeDistributions.TypeBefore,
            Belowminstr= payRangeDistributions.BelowminBefore +"<br />"+ string.Format("{0:0.0}",payRangeDistributions.BelowminBeforePct)+"%",
            Lowerstr=payRangeDistributions.LowerBefore +"<br />"+ string.Format("{0:0.0}",payRangeDistributions.LowerBeforePct)+"%",
             Upperstr= payRangeDistributions.UpperBefore +"<br />"+ string.Format("{0:0.0}",payRangeDistributions.UpperBeforePct)+"%",
             Overmaxstr= payRangeDistributions.OvermaxBefore+"<br />"+ string.Format("{0:0.0}",payRangeDistributions.OvermaxBeforePct)+"%"},
        new PayRangeDistribution { Typestr = payRangeDistributions.TypeAfter, Belowminstr= payRangeDistributions.BelowminAfter +"<br />"+ string.Format("{0:0.0}",payRangeDistributions.BelowminAfterPct)+"%", Lowerstr=payRangeDistributions.LowerAfter +"<br />"+ string.Format("{0:0.0}",payRangeDistributions.LowerAfterPct)+"%",
            Upperstr= payRangeDistributions.UpperAfter +"<br />"+ string.Format("{0:0.0}",payRangeDistributions.UpperAfterPct)+"%",Overmaxstr= payRangeDistributions.OvermaxAfter+"<br />"+ string.Format("{0:0.0}",payRangeDistributions.OvermaxAfterPct)+"%"}
        };
            return payRangeDistribution;
        }
        private List<PayRangeDistribution> payRangeDistributionCurve(PayRangeDistribution payRangeDistribution)
        {
            int AfterBelowmin = (int)payRangeDistribution.BelowminAfter;
            int AfterLower = (int)payRangeDistribution.LowerAfter;
            int AfterUpper = (int)payRangeDistribution.UpperAfter;
            int AfterOvermax = (int)payRangeDistribution.OvermaxAfter;

            int BelowBelowmin = (int)payRangeDistribution.BelowminBefore;
            int BelowLower = (int)payRangeDistribution.LowerBefore;
            int BelowUpper = (int)payRangeDistribution.UpperBefore;
            int BelowOvermax = (int)payRangeDistribution.OvermaxBefore;
            List<PayRangeDistribution> grid = new List<PayRangeDistribution>();
            grid.Add(new PayRangeDistribution { Title = "BELOW MIN", After = AfterBelowmin, Before = BelowBelowmin });
            grid.Add(new PayRangeDistribution { Title = "BELOW MID", After = AfterLower, Before = BelowLower });
            grid.Add(new PayRangeDistribution { Title = "ABOVE MID", After = AfterUpper, Before = BelowUpper });
            grid.Add(new PayRangeDistribution { Title = "ABOVE MAX", After = AfterOvermax, Before = BelowOvermax });
            return grid;
        }
        private string UpdateDataSource()
        {
            string updatedConnectionString;

            string tenant = this.GetTenant();
            TenantConnection tenantData;
            using (LaserbeamCompassMasterEntities master = LaserbeamCompassMasterEntities.Create())
            {
                tenantData = master.TenantConnections.SingleOrDefault(m => m.Tenant.TenantURLName == tenant);
                string dbServer = "";
                string dbName = "";
                string dbUserId = "";
                string dbPassword = "";
                MayaLink.TryDecrypt(tenantData.DatabaseServer, out dbServer);
                MayaLink.TryDecrypt(tenantData.DatabaseName, out dbName);
                MayaLink.TryDecrypt(tenantData.DatabaseUserId, out dbUserId);
                MayaLink.TryDecrypt(tenantData.DatabasePassword, out dbPassword);
                updatedConnectionString = "XpoProvider=MSSqlServer;Data source=" + dbServer + ";Initial catalog=" + dbName + ";User ID=" + dbUserId + ";Password=" + dbPassword + "; MultipleActiveResultSets = True; Application Name = EntityFramework";
            }
            return updatedConnectionString;
        }
        #endregion

    }
}