
// Copyright (c) 2015 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.

// Component Name :   ExchangeRate 
// Description    :   Actions of Exchange Rate
// Author         :   Hariharasubramaniyan Chandrasekaran		
// Creation Date  :   27-Mar-2017 

using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using Laserbeam.ProcessManager.Interfaces.Common;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Laserbeam.BusinessObject.Common;
using Laserbeam.UI.HR.Common;
using System.Net.Http;
using Newtonsoft.Json;
using System.Data;
using System.Web.Helpers;
using System.ComponentModel.DataAnnotations;
using System.Web.Script.Serialization;
using System.Collections.Generic;
using Laserbeam.Constant.HR;
using System;

namespace Laserbeam.UI.HR.Controllers
{
    [AuthorizeUser(UserRoleConstants.AllAdminRoles)]
    public class ExchangeRateController : Controller
    {
        #region Fields
        // Author         :   Hariharasubramaniyan Chandrasekaran	
        // Creation Date  :   27-Mar-2017 
        /// <summary>
        /// Instance of  ExchangeRateRepository
        /// </summary>
        private IExchangeRateProcessManager m_exchangeRateProcessManager;// { get; set; }
        private SessionManager m_sessionManager;
        private readonly IAccountProcessManager m_accountProcessManager;
        public object CreateDataTable { get; private set; }

        #endregion

        #region Constructor
        // Author         :   Hariharasubramaniyan Chandrasekaran	
        // Creation Date  :   27-Mar-2017 
        /// <summary>
        /// Constructor of  ExchangeRateRepository
        /// </summary>
        public ExchangeRateController(IExchangeRateProcessManager exchangeRateProcessManager, SessionManager sessionManager, IAccountProcessManager accountProcessManager)
        {
            m_exchangeRateProcessManager = exchangeRateProcessManager;
            m_sessionManager = sessionManager;
            m_accountProcessManager = accountProcessManager;
        }

        #endregion

        #region Public Methods

        [HttpGet]
        public ActionResult Home()
        {
            ExchangeRateData model = new ExchangeRateData();
            model.CurrencyCode = m_exchangeRateProcessManager.GetBaseCurrency().Trim();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxChildActionOnly]
        public async Task<JsonResult> GetExchangeRateGridData([DataSourceRequest]DataSourceRequest request)
        {

            var result = await m_exchangeRateProcessManager.GetExchangeRateData();
            return Json(result.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public PartialViewResult _ExchangeRatePopUp()
        {
            AppConfigModel app = m_accountProcessManager.GetAppSetting();
            ViewBag.BaseCurrency = app.BaseCurrency;
            ExchangeRateData model = new ExchangeRateData();

            return PartialView(model);
        }

        [HttpGet]
        public PartialViewResult _BaseCurrencyChangePopUp()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxChildActionOnly]
        public PartialViewResult _ExchangeRatePopUpModify(int exchangeRateNum, string CurrencyCode, decimal ExchangeRate,string cultureCode)
        {
            ExchangeRateData model = new ExchangeRateData();
            model.CurrencyCode = CurrencyCode;
            model.ExchangeRate = ExchangeRate;
            model.CurrencyCodeNum = exchangeRateNum;
            model.CultureCode = cultureCode;
            AppConfigModel app = m_accountProcessManager.GetAppSetting();
            ViewBag.BaseCurrency = app.BaseCurrency;
            return PartialView("_ExchangeRatePopUp", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxChildActionOnly]
        public JsonResult GetCultureCode()
        {
            var ddlData = m_exchangeRateProcessManager.GetCultureCode().Select(x => new { CultureCodeNum = x.CultureCode, CultureCode = x.CultureCode });
            return Json(ddlData, JsonRequestBehavior.AllowGet);
        }

        // Author       : Shaheena Shaik
        // Creation Date: 24-April-2017
        /// <summary>
        /// Getting existing Currency Codes from database
        /// </summary>
        /// <returns>Returning a query of currency codes</returns>
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxChildActionOnly]
        public async Task<JsonResult> GetCurrencyCode()
        {
            var currencyCodeData = await m_exchangeRateProcessManager.GetCurrencyCode();
                //.Select(x => new { CurrencyCodeNum = x.CurrencyCodeNum, CurrencyCode = x.CurrencyCode });
            return Json(currencyCodeData, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxChildActionOnly]
        public JavaScriptResult _ExchangeRatePopUp(ExchangeRateData data)
        {
          
            var a = m_exchangeRateProcessManager.AddExchangeRate(data);
            return JavaScript("closeAfterExchangeRateAdded();");
        }


        [HttpGet]
        public async Task<JavaScriptResult> ExchangeRate()
        {
            AppConfigModel app = m_accountProcessManager.GetAppSetting();
            var AccessKey = app.CurrencyApiAccessKey;
            var BaseCurrency = app.BaseCurrency;
         //   string apiUrl = "https://v3.exchangerate-api.com/bulk/b6e1b7650c6ac113af41c296/USD";
            string apiUrl = "https://v3.exchangerate-api.com/bulk/" + AccessKey +"/"+ BaseCurrency;
            using (HttpClient client = new HttpClient())
            {
               
                client.BaseAddress = new System.Uri(apiUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    
                    var data = await response.Content.ReadAsStringAsync();
                  //  System.IO.File.WriteAllText(@"D:\textfile.txt", data);
                   // YahooExchangeApi test = JsonConvert.DeserializeObject<YahooExchangeApi>(data);
                    ExchangeRateAPIResults res= JsonConvert.DeserializeObject<ExchangeRateAPIResults>(data);
                    Countries countries= JsonConvert.DeserializeObject<ExchangeRateAPIResults>(data).rates;
                    m_exchangeRateProcessManager.UpdateExchangeRate(countries,app.BaseCurrency);
                  

                }
            }
            return JavaScript("closeAfterUpdate();");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxChildActionOnly]
        public async Task<int> UpdateBaseCurrency(string currencyCode,int currencyCodeNum)
        {

            CultureCodeData currencyData = new CultureCodeData();
            currencyData.CurrencyCode = currencyCode.Trim();
            currencyData.CurrencyCodeNum = currencyCodeNum;
            var a = m_exchangeRateProcessManager.UpdateBaseCurrency(currencyData);
            AppConfigModel app = m_accountProcessManager.GetAppSetting();
            app.BaseCurrency= currencyCode.Trim();
            app.BaseCurrencyNum = currencyCodeNum;
            m_sessionManager.SetSession<AppConfigModel>(SessionConstants.AppConfigModel, app);
            var AccessKey = app.CurrencyApiAccessKey;
            var BaseCurrency = currencyCode.Trim();
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
                    //System.IO.File.WriteAllText(@"D:\textfile.txt", data);
                    ExchangeRateAPIResults res = JsonConvert.DeserializeObject<ExchangeRateAPIResults>(data);
                    Countries countries = JsonConvert.DeserializeObject<ExchangeRateAPIResults>(data).rates;
                                       m_exchangeRateProcessManager.UpdateExchangeRate(countries, currencyCode.Trim());


                }
            }
            return 1;
          
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxChildActionOnly]
        public JsonResult ValidateCurrencyCode(string currencyCodeValue)
        {
            bool isCurrencyCodeExists = m_exchangeRateProcessManager.ValidateCurrencyCode(currencyCodeValue);            
            if (isCurrencyCodeExists == true)
                return Json(true);
            else
                return Json(false);
        }

        // Author           : Shaheena Shaik
        // Creation Date    :30-June-2017
        /// <summary>
        /// Creation FilterSort PartialView for Exchange Rate grid
        /// </summary>
        /// <returns>Returning a list of columns to be displayed for applying filter in a Filter/Sort popup</returns>
        [HttpGet]
        public PartialViewResult _ExchangeRateFilterSort()
        {
            List<SelectListItem> filterList = new List<SelectListItem>();
            filterList.Add(new SelectListItem { Text = "CurrencyCode", Value = "CurrencyCode" });
            filterList.Add(new SelectListItem { Text = "CultureCode", Value = "CultureCode" });
            filterList.Add(new SelectListItem { Text = "ExchangeRate", Value = "ExchangeRate" });
            return PartialView(filterList);
        }

        // Author       : Shaheena Shaik
        // Creation date: 30-June-2017
        /// <summary>
        /// Getting ExchangeRate Export data from database
        /// </summary>
        /// <returns>Returning ExchangeRate Export</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task ExchangeRateExport()
        {
            string exportName = "ExchangeRate - Extract - " + DateTime.Now.ToShortDateString().FormatWith("{0: MM-dd-yyyy}");
            string sheetName = "ExchangeRate Information";
            var exportData = await m_exchangeRateProcessManager.GetExchangeRateExportData();
            ExportExcel.ToExcel(exportData, exportName, sheetName);
        }

        #endregion

    }
}