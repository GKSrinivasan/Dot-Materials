using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Laserbeam.BusinessObject.Common;
using Laserbeam.Constant.HR;
using Laserbeam.ProcessManager.Interfaces.Common;
using Laserbeam.UI.HR.Common;
using Laserbeam.UI.HR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Laserbeam.UI.HR.Controllers
{
    [AuthorizeUser(UserRoleConstants.AllRoles)]
    public class MarketPayRangeController : Controller
    {
        #region Fields
        // Author         :   Arunraj C
        // Creation Date  :   11-OCT-2017 
        /// <summary>
        /// Object of IMarketPayRangeProcessManager
        /// </summary>
        private IMarketPayRangeProcessManager m_marketPayRangeProcessManager;
        private SessionManager m_sessionManager;
        #endregion

        #region Constructor
        // Author         :   Arunraj C
        // Creation Date  :   11-OCT-2017 
        /// <summary>
        /// Initialize the processManager and session 
        /// </summary>
        /// <param name="marketPayRangeProcessManager">Object of IMarketPayRangeProcessManager</param>
        /// <param name="sessionManager">Object of SessionManager</param>
        public MarketPayRangeController(IMarketPayRangeProcessManager marketPayRangeProcessManager, SessionManager sessionManager)
        {
            m_marketPayRangeProcessManager = marketPayRangeProcessManager;
            m_sessionManager = sessionManager;
        }
        #endregion


        // GET: MarketPayRange
        [HttpGet]
        public ActionResult Home()
        {
            var busSettingData = m_marketPayRangeProcessManager.GetBusSetting();
            var selectedMarketPayRange = busSettingData.MarketPayRange;
            MarketPayRangeModel model = new MarketPayRangeModel();
            model.SelectedMarketPayRange = selectedMarketPayRange;
            return View(model);
        }

        //[HttpGet]
        //public PartialViewResult _Reportees(string selectedMarketPayRange)
        //{           
        //    MarketPayRangeModel model = new MarketPayRangeModel();
        //    model.SelectedMarketPayRange = selectedMarketPayRange;
        //    return PartialView(model);
        //}
        [HttpPost]
        public async Task<JsonResult> _Reportees(string selectedMarketPayRange, bool isToggleSwitch = false)
        {
            if (isToggleSwitch)
            {
                BusSettingModel busSetting = new BusSettingModel();
                busSetting.MarketPayRange = selectedMarketPayRange;
                await m_marketPayRangeProcessManager.SetBusSetting(busSetting);
            }            
            return Json("Success",JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> _GetMarketPayRangeReportees([DataSourceRequest]DataSourceRequest request, string selectedMarketPayRange)
        {
            var userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
            var loginUserNum = userModel.UserNum;
            var marketPayRangeGridData = await m_marketPayRangeProcessManager.GetMarketPayRangeData(loginUserNum, selectedMarketPayRange);
            return Json(marketPayRangeGridData.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public PartialViewResult _AddMarketPayRange(string selectedValue = "", int marketPayRangeNum = 0,bool isEdit = false)
        {
            var busSettingData = m_marketPayRangeProcessManager.GetBusSetting();
            AddMarketPayRange model = new AddMarketPayRange();
            ViewBag.SelectedMarketPayRange = busSettingData.MarketPayRange;
            model.MarketPayRangeNum = marketPayRangeNum;
            model.JobCode = selectedValue;
            model.Grade = selectedValue;
            model.EmployeeID = selectedValue;
            model.IsEdit = isEdit;
            if (marketPayRangeNum > 0)
            {
                var getData = m_marketPayRangeProcessManager.GetSelectedMarketPayRange(marketPayRangeNum, busSettingData.MarketPayRange);
                model.MarketPayRangeNum = getData.MarketPayRangeNum;
                model.JobNum = getData.JobNum;
                model.JobCode = getData.JobCode;
                model.GradeNum = getData.GradeNum;
                model.Grade = getData.Grade;
                model.EmployeeNum = getData.EmployeeNum;
                model.EmployeeName = getData.EmployeeName;
                model.EmployeeID = getData.EmployeeID;
                model.CurrentMin = getData.CurrentMin;
                model.CurrentMid = getData.CurrentMid;
                model.CurrentMax = getData.CurrentMax;
                model.HourlyCurrentMin = getData.HourlyCurrentMin;
                model.HourlyCurrentMid = getData.HourlyCurrentMid;
                model.HourlyCurrentMax = getData.HourlyCurrentMax;
                model.FutureMin = getData.FutureMin;
                model.FutureMid = getData.FutureMid;
                model.FutureMax = getData.FutureMax;
                model.HourlyFutureMin = getData.HourlyFutureMin;
                model.HourlyFutureMid = getData.HourlyFutureMid;
                model.HourlyFutureMax = getData.HourlyFutureMax;               
            }
            ViewBag.SelectedValue = selectedValue;
            return PartialView(model);
        }

        [HttpPost]
        public async Task<ActionResult> _AddMarketPayRange(AddMarketPayRange model)
        {
            var busSettingData = m_marketPayRangeProcessManager.GetBusSetting();
            string updateDataValue = "";
            if (model.JobCode != null)
                updateDataValue = model.JobCode;
            else if(model.Grade != null)
                updateDataValue = model.Grade;
            else
                updateDataValue = model.EmployeeID;
            bool IsExisting = false;
            if(!model.IsEdit)
                IsExisting = m_marketPayRangeProcessManager.IsMarketDataExist(busSettingData.MarketPayRange, updateDataValue);
            if(!IsExisting)
            {
                var userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
                var loginUserNum = userModel.UserNum;
                var mUpadteModel = new MarketPayRangeModel
                {
                    MarketPayRangeNum = model.MarketPayRangeNum,
                    JobNum = model.JobNum,
                    JobCode = model.JobCode,
                    GradeNum = model.GradeNum,
                    Grade = model.Grade,
                    EmployeeNum = model.EmployeeNum,
                    EmployeeName = model.EmployeeName,
                    EmployeeID = model.EmployeeID,
                    CurrentMin = model.CurrentMin,
                    CurrentMid = model.CurrentMid,
                    CurrentMax = model.CurrentMax,
                    HourlyCurrentMin = model.HourlyCurrentMin,
                    HourlyCurrentMid = model.HourlyCurrentMid,
                    HourlyCurrentMax = model.HourlyCurrentMax,
                    FutureMin = model.FutureMin,
                    FutureMid = model.FutureMid,
                    FutureMax = model.FutureMax,
                    HourlyFutureMin = model.HourlyFutureMin,
                    HourlyFutureMid = model.HourlyFutureMid,
                    HourlyFutureMax = model.HourlyFutureMax
                };
                
               await  m_marketPayRangeProcessManager.AddUpdateMarketPayRange(mUpadteModel, loginUserNum);
            }
            else
            {
                return Json("already exists", JsonRequestBehavior.AllowGet);
            }
            return Json("Success",JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public PartialViewResult _FilterSort(string selectedMarketPayRange)
        {
            List<SelectListItem> list = new List<SelectListItem>();            
            if (selectedMarketPayRange.ToLower() == "jobcode")
                list.Add(new SelectListItem { Text = "Job Code", Value = "JobCode" });                
            else if (selectedMarketPayRange.ToLower() =="grade")
                list.Add(new SelectListItem { Text = "Grade", Value = "GradeCode" });
            else if (selectedMarketPayRange.ToLower() == "byemployee")
            {
                list.Add(new SelectListItem { Text = "EmployeeID", Value = "EmployeeID" });
                list.Add(new SelectListItem { Text = "Employee Name", Value = "EmployeeName" });
            }
            list.Add(new SelectListItem { Text = "Current Min", Value = "CurrentMin" });
            list.Add(new SelectListItem { Text = "Current Mid", Value = "CurrentMid" });
            list.Add(new SelectListItem { Text = "Current Max", Value = "CurrentMax" });
            list.Add(new SelectListItem { Text = "Hourly Current Min", Value = "HourlyCurrentMin" });
            list.Add(new SelectListItem { Text = "Hourly Current Mid", Value = "HourlyCurrentMid" });
            list.Add(new SelectListItem { Text = "Hourly Current Max", Value = "HourlyCurrentMax" });
            list.Add(new SelectListItem { Text = "New Min", Value = "FutureMin" });
            list.Add(new SelectListItem { Text = "New Mid", Value = "FutureMid" });
            list.Add(new SelectListItem { Text = "New Max", Value = "FutureMax" });
            list.Add(new SelectListItem { Text = "Hourly New Min", Value = "HourlyFutureMin" });
            list.Add(new SelectListItem { Text = "Hourly New Mid", Value = "HourlyFutureMid" });
            list.Add(new SelectListItem { Text = "Hourly New Max", Value = "HourlyFutureMax" });
            return PartialView(list);
        }
    }
}