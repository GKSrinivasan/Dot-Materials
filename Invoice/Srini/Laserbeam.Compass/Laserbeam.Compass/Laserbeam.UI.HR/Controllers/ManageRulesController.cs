// Copyright (c) 2015 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.

// Component Name :   ManageRules 
// Description    :   Actions of ManageRules
// Author         :   Raja Ganapathy		
// Creation Date  :   30-Mar-2017 

using Laserbeam.BusinessObject.Common;
using Laserbeam.Constant.HR;
using Laserbeam.ProcessManager.Interfaces.Common;
using Laserbeam.UI.HR.Common;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Laserbeam.UI.HR.Controllers
{
    [AuthorizeUser(UserRoleConstants.AllAdminRoles)]
    public class ManageRulesController : Controller
    {
        #region Fields
        // Author         :   Raja Ganapathy
        // Creation Date  :   30-Mar-2017 
        /// <summary>
        /// Object of IWorkForceProcessManager
        /// </summary>
        private IRuleConfigurationProcessManager m_ruleConfigurationProcessManager;
        private SessionManager m_sessionManager;
        private readonly IAccountProcessManager m_accountProcessManager;
        #endregion


        #region Constructor
        // Author         :   Raja Ganapathy
        // Creation Date  :   30-Mar-2017 
        /// <summary>
        /// Initialize the processManager and session 
        /// </summary>
        /// <param name="workForceProcessManager">Object of IWorkForceProcessManager</param>
        /// <param name="sessionManager">Object of SessionManager</param>
        public ManageRulesController(IRuleConfigurationProcessManager ruleConfigurationProcessManager, SessionManager sessionManager, IAccountProcessManager accountProcessManager)
        {
            m_ruleConfigurationProcessManager = ruleConfigurationProcessManager;
            m_sessionManager = sessionManager;
            m_accountProcessManager = accountProcessManager;
        }
        #endregion

        #region Public Methods

        [HttpGet]
        public ActionResult Home()
        {
                    
            return View();
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxChildActionOnly]
        public async Task<ActionResult> UpdateBusSetting(BusSettingModel BusSettingDetails)
        {
            UserModel userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
            var multiCurrencyValue = BusSettingDetails.MultiCurrency;
            if (m_ruleConfigurationProcessManager.CheckMultiCurrencyChanged("MultiCurrency", multiCurrencyValue) && multiCurrencyValue.ToLower()==("YES").ToLower())
            {
                await PutExchangeRateData();
            }
            if (BusSettingDetails.CurrentYear == "" || BusSettingDetails.CurrentYear == null)
                BusSettingDetails.CurrentYear = BusSettingDetails.oldYear;
            BusSettingDetails.IDPEndPoint = BusSettingDetails.IDPEndPoint == null ? "" : BusSettingDetails.IDPEndPoint;
            await m_ruleConfigurationProcessManager.PutBusSetting(BusSettingDetails, userModel.UserNum);

            if (BusSettingDetails.CurrentYear != BusSettingDetails.oldYear)
            {
                AppConfigModel appConfigModel = m_accountProcessManager.GetAppSetting();
                appConfigModel.CurrentYear = BusSettingDetails.CurrentYear;
                appConfigModel.MeritCycleYear = BusSettingDetails.CurrentYear;
                m_sessionManager.SetSession<AppConfigModel>(SessionConstants.AppConfigModel, appConfigModel);
                await m_ruleConfigurationProcessManager.RunBuildManagerTree();
            }

            if (BusSettingDetails != null)
            {
                return Json("Updated successfully");
            }
            return Json("Updated successfully");

        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxChildActionOnly]
        public JsonResult GetBusSetting()
        {
            BusSettingModel busSettingModel = m_ruleConfigurationProcessManager.GetBusSetting();
            return Json(busSettingModel, JsonRequestBehavior.AllowGet);

        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxChildActionOnly]
        public async Task<JsonResult> PutApplyRules()
        {
            bool status = await m_ruleConfigurationProcessManager.PutApplyRules();
            return Json((status) ? 1 : 0);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxChildActionOnly]
        public PartialViewResult _SetRules(bool isWizard)
        {
            string tenantCreationBaseUrl = this.GetTenantUrl()+"/sso";
            UserModel userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
            ViewBag.UserRole = userModel.UserRole;
            RuleConfigModel ruleConfigModel = new RuleConfigModel();
            List<string> regionalSetting = new List<string>() { "en-us", "en" };
            ruleConfigModel.RegionalSetting = regionalSetting;
            ruleConfigModel.isWizard = isWizard;
            List<RoundingTypeModel> roundingTypeList = m_ruleConfigurationProcessManager.GetRoundingType();
            ruleConfigModel.Rounding = roundingTypeList.Select(x => x.RoundTypeKey).ToList();
            List<string> pwdLength = new List<string>();           
            pwdLength.Add("4");pwdLength.Add("5");pwdLength.Add("6");pwdLength.Add("7");pwdLength.Add("8");
            pwdLength.Add("9");pwdLength.Add("10");pwdLength.Add("11");pwdLength.Add("12");pwdLength.Add("13");
            pwdLength.Add("14");pwdLength.Add("15");pwdLength.Add("16");pwdLength.Add("17");pwdLength.Add("18");
            pwdLength.Add("19");pwdLength.Add("20");pwdLength.Add("21");pwdLength.Add("22");pwdLength.Add("22");
            pwdLength.Add("23");pwdLength.Add("24");pwdLength.Add("25");
            ruleConfigModel.PasswordLength = pwdLength;
            List<DecimalTypeModel> decimalTypeList = m_ruleConfigurationProcessManager.GetDecimalType().ToList();
            ruleConfigModel.DecimalVal = decimalTypeList.Where(x => x.Type.ToLower() == "annual").Select(x => x.DecimalTypeKey).ToList();
            ruleConfigModel.HourlyDecimalVal = decimalTypeList.Where(x => x.Type.ToLower() == "hourly").Select(x => x.DecimalTypeKey).ToList();
            ruleConfigModel.PercentageDecimalVal = decimalTypeList.Where(x => x.Type.ToLower() == "percentage").Select(x => x.DecimalTypeKey).ToList();
            ruleConfigModel.DateFormat = m_ruleConfigurationProcessManager.GetDateFormats();
            ruleConfigModel.SSOUrl = tenantCreationBaseUrl;
            return PartialView(ruleConfigModel);
        }

        [ValidateAntiForgeryToken]
        [AjaxChildActionOnly]
        [HttpPost]
        public async Task<JsonResult> clearApprovalDetails()
        {
            bool status = await m_ruleConfigurationProcessManager.clearApprovalDetails();
            return Json((status) ? 1 : 0);
        }

        #endregion

        #region Private Methods

        private async Task<int> PutExchangeRateData()
        {
            AppConfigModel app = m_accountProcessManager.GetAppSetting();
            var AccessKey = app.CurrencyApiAccessKey;
            var BaseCurrency = "USD";            
            string apiUrl = "https://v3.exchangerate-api.com/bulk/" + AccessKey + "/" + BaseCurrency;
            using (HttpClient client = new HttpClient())
            {

                client.BaseAddress = new System.Uri(apiUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {

                    var data = await response.Content.ReadAsStringAsync();                    
                    ExchangeRateAPIResults res = JsonConvert.DeserializeObject<ExchangeRateAPIResults>(data);
                    Countries countries = JsonConvert.DeserializeObject<ExchangeRateAPIResults>(data).rates;
                    m_ruleConfigurationProcessManager.UpdateExchangeRate(countries, BaseCurrency);
                }
            }
            return 1;
        }

        #endregion
    }
}