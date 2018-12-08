// Copyright (c) 2017 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.
// Component Name :   WorkForce
// Description    :   Allows to user select the template and upload the employee data and correction the data.
// Author         :   Raja Ganapathy
// Creation Date  :   30-Mar-2017 

using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using Laserbeam.ProcessManager.Interfaces.Common;
using Laserbeam.UI.HR.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Laserbeam.BusinessObject.Common;
using Laserbeam.Constant.HR;
using System.IO;
using System.Data;
using Laserbeam.UI.HR.Models;

namespace Laserbeam.UI.HR.Controllers
{
    [AuthorizeUser(UserRoleConstants.AllAdminRoles)]
    public class WorkforceController : Controller
    {
        #region Fields
        // Author         :   Raja Ganapathy
        // Creation Date  :   30-Mar-2017 
        /// <summary>
        /// Object of IWorkForceProcessManager
        /// </summary>
        private IWorkForceProcessManager m_workForceProcessManager;
        private SessionManager m_sessionManager;
        private ISessionProcessManager m_sessionProcessManager;
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
        public WorkforceController(IWorkForceProcessManager workForceProcessManager, SessionManager sessionManager, ISessionProcessManager sessionProcessManager, IAccountProcessManager accountProcessManager)
        {
            m_workForceProcessManager = workForceProcessManager;
            m_sessionManager = sessionManager;
            m_sessionProcessManager = sessionProcessManager;
            m_accountProcessManager = accountProcessManager;
        }
        #endregion

        // Author         :   Raja Ganapathy
        // Creation Date  :   30-Mar-2017 
        /// <summary>
        /// To get the workforce landing page
        /// </summary>
        /// <returns>Returns the tile data model to view</returns>  
        [HttpGet]
        public async Task<ActionResult> Home(bool clearData=false)
        {                    
            WorkforceModel model = new WorkforceModel();
            model.RuleConfiguration = m_workForceProcessManager.GetMeritConfiguration();
            model.RatingRange = m_workForceProcessManager.GetRatings().OrderBy(x => x.RatingOrder).Select(i => new SelectListItem { Text = i.RatingDescr, Value = i.RatingRange.ToString() }).ToList();
            ViewBag.EmployeeErrorDataCount = Task.Run(() => m_workForceProcessManager.GetEmployeeDataErrorCount()).Result;
            ViewBag.ClearData = clearData;
            if (clearData)
            {
                bool sucess = await m_workForceProcessManager.ClearAllData(this.GetTenant());
                AppConfigModel appConfig = m_accountProcessManager.GetAppSetting();
                UserModel user = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
                var userModel = m_sessionProcessManager.GetUserSession(user.UserID, Convert.ToInt32(appConfig.CurrentYear));
                m_sessionManager.SetSession<UserModel>(SessionConstants.UserModel, userModel);
                //user.IsSampleData = false;
            }
            return View(model);
        }

        // Author         :   Raja Ganapathy
        // Creation Date  :   30-Mar-2017 
        /// <summary>
        /// To get the choose fields view
        /// </summary>
        /// <returns>Returns the partial view</returns>
        [HttpGet]
        public PartialViewResult _ChooseFields(bool isWizard = false)
        {
            ViewBag.isWizard = isWizard;
            return PartialView();
        }

        // Author         :   Raja Ganapathy
        // Creation Date  :   30-Mar-2017 
        /// <summary>
        /// To get the Upload data view
        /// </summary>
        /// <returns>Returns the partial view</returns>
        [HttpGet]
        public PartialViewResult _UploadData()
        {
            return PartialView();
        }

        // Author         :   Raja Ganapathy
        // Creation Date  :   30-Mar-2017 
        /// <summary>
        /// To get the uploaded fields view
        /// </summary>
        /// <returns>Returns the partial view</returns>
        //[HttpGet]
        //public PartialViewResult _UploadedFileDetails()
        //{
        //    return PartialView();
        //}

        // Author         :   Raja Ganapathy
        // Creation Date  :   30-Mar-2017 
        /// <summary>
        /// To get the Datacorrection view
        /// </summary>
        /// <returns>Returns the columns list to partial view</returns>
        [HttpGet]
        public PartialViewResult _DataCorrection(string errorType = "")
        {
            ViewBag.ErrorType = errorType;
            List<TemplateMetaColumns> dataCorrection = new List<TemplateMetaColumns>();
            if (errorType == "")
                dataCorrection = m_workForceProcessManager.GetTemplateMetaColumnDetails().Where(x => x.IsTemplateDefaultColumn == true).ToList();
            return PartialView(dataCorrection);
        }

        // Author         :   Raja Ganapathy
        // Creation Date  :   30-Mar-2017 
        /// <summary>
        /// To get the Error list window
        /// </summary>
        /// <returns>Returns the partial view</returns>
        //[HttpGet]
        //public PartialViewResult _GetErrorList()
        //{
        //    return PartialView();
        //}

        // Author         :   Raja Ganapathy
        // Creation Date  :   30-Mar-2017 
        /// <summary>
        /// To get the Employee search
        /// </summary>
        /// <returns>Returns the partial view</returns>
        [HttpGet]
        public PartialViewResult _EmployeeSearch()
        {
            return PartialView();
        }

        // Author         :   Raja Ganapathy
        // Creation Date  :   30-Mar-2017 
        /// <summary>
        /// To get the Tile data view
        /// </summary>
        /// <returns>Returns the partial view</returns>
        [HttpGet]
        public async Task<PartialViewResult> _GetTileData()
        {
            UserModel userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
            var tileData = await m_workForceProcessManager.GetWorkForceTileData(userModel.UserNum);
            return PartialView(tileData);
        }

        // Author         :   Raja Ganapathy
        // Creation Date  :   30-Mar-2017 
        /// <summary>
        /// To get the selected fields partial view
        /// </summary>
        /// <returns>Returns the partial view</returns>
        [HttpGet]
        public PartialViewResult _GetTemplateSelectedFields()
        {
            return PartialView();
        }

        [HttpGet]
        public async Task<PartialViewResult> _GetErrorEmployeeDataCorrection(string errorType)
        {
            var templateData = m_workForceProcessManager.GetTemplateMetaColumnDetails().Where(x => x.IsTemplateDefaultColumn == true).ToList();
            var employeeData = await m_workForceProcessManager.GetEmployeeErrorRecordDetails(errorType);
            ViewBag.StagingNumber = employeeData.Where(x => x.FieldName == "StagingNumber").Select(x => x.FieldValue).FirstOrDefault();
            var templateGroup = GetTemplateWithValues(templateData, employeeData);
            return PartialView("~/Views/WorkForce/_EmployeeDetails.cshtml", templateGroup);
        }

        // Author         :   Raja Ganapathy
        // Creation Date  :   30-Mar-2017 
        /// <summary>
        /// To get the datacorrection page
        /// </summary>
        /// <returns>Returns the employee data and selected fields information to partial view</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxChildActionOnly]
        public async Task<PartialViewResult> _EmployeeDetails(string employeeID = "")
        {
            ViewBag.StagingNumber = 0;
            var templateData = m_workForceProcessManager.GetTemplateMetaColumnDetails().Where(x => x.IsTemplateDefaultColumn == true).ToList();
            var employeeData = await m_workForceProcessManager.GetSearchedEmployeeDetails(employeeID);
            var templateGroup = GetTemplateWithValues(templateData, employeeData);
            return PartialView(templateGroup);
        }


        // Author         :   Raja Ganapathy
        // Creation Date  :   30-Mar-2017 
        /// <summary>
        /// To get the employee data
        /// </summary>
        /// <param name="request">DataSourceRequest</param>
        /// <returns>Returns the json object</returns>
        [HttpGet]
        public JsonResult GetEmployeeSearchData([DataSourceRequest] DataSourceRequest request)
        {
            UserModel userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
            var employeesearch = m_workForceProcessManager.GetEmployeeList(userModel.UserNum);
            return Json(employeesearch.ToList(), JsonRequestBehavior.AllowGet);
        }

        // Author         :   Raja Ganapathy
        // Creation Date  :   30-Mar-2017 
        /// <summary>
        /// To update the changed fields
        /// </summary>
        /// <param name="selectedFields">Defines the selected fields</param>
        /// <returns>Returns the message</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxChildActionOnly]
        public async Task<ActionResult> UpdateChooseFields(List<int> selectedFields)
        {
           bool status = await m_workForceProcessManager.UpdateSelectedTemplateColumns(selectedFields);
            if (status == true)
                return Json("Updated successfully");
            return null;
        }

        // Author         :   Raja Ganapathy
        // Creation Date  :   30-Mar-2017 
        /// <summary>
        /// To get the template fields 
        /// </summary>
        /// <param name="request">DataSourceRequest</param>
        /// <returns>Returns the json object</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxChildActionOnly]
        public JsonResult GetTemplateColumnsList([DataSourceRequest] DataSourceRequest request)
        {
            var result = m_workForceProcessManager.GetTemplateMetaColumnDetails().Where(x => x.IsTemplateColumn == true).ToList();
            return Json(result.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        // Author         :   Raja Ganapathy
        // Creation Date  :   30-Mar-2017 
        /// <summary>
        /// To get the selected template fields 
        /// </summary>
        /// <param name="request">DataSourceRequest</param>
        /// <returns>Returns the json object</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxChildActionOnly]
        public JsonResult GetSelectedTemplateColumnsList([DataSourceRequest] DataSourceRequest request)
        {
            var result = m_workForceProcessManager.GetTemplateMetaColumnDetails().Where(x => x.IsEnabled == true && x.IsTemplateColumn == true).ToList();
            return Json(result.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        // Author         :   Raja Ganapathy
        // Creation Date  :   30-Mar-2017 
        /// <summary>
        /// Get the employee data template
        /// </summary>
        [HttpPost]
        public async Task GetEmployeeDataTemplate()
        {
            var exportData = await m_workForceProcessManager.GetEmployeeDataTemplate();
            ExportExcel.ToExcel(exportData, "Employee Data Template");
        }

        // Author         :   Raja Ganapathy
        // Creation Date  :   30-Mar-2017 
        /// <summary>
        /// Exports the uploaded data
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task GetWorkForceUploadedData(string isMeritEligible)
        {
            UserModel userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
            var exportData = await m_workForceProcessManager.GetUploadedEmployeeDetails(userModel.UserNum, isMeritEligible);
            ExportExcel.ToExcel(exportData, ((isMeritEligible == "NO") ? "All Employees Data" : "Merit Eligible Employees Data"));
        }

        // Author         :   Raja Ganapathy
        // Creation Date  :   30-Mar-2017 
        /// <summary>
        /// To get the Dropdown or Combobox values
        /// </summary>
        /// <param name="request">request</param>
        /// <param name="ColumnName">ColumnName</param>
        /// <returns>List of selected values</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxChildActionOnly]
        public async Task<JsonResult> GetDropDownValue([DataSourceRequest] DataSourceRequest request, string ColumnName = "")
        {
            var result = await m_workForceProcessManager.GetSelectedDropDownDetails(ColumnName);
            return Json(result.ToList(), JsonRequestBehavior.AllowGet);
        }

        // Author         :   Raja Ganapathy
        // Creation Date  :   30-Mar-2017 
        /// <summary>
        /// To upload the selected sheets
        /// </summary>
        /// <returns>Returns the file info</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Upload()
        {
            UserModel userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
            string message = "";
            string uploadComplete = "";
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
                    fileName = Server.MapPath("~/EmployeeData/" + Path.GetFileName(fileContent.FileName));
                    fileContent.SaveAs(fileName);
                    m_workForceProcessManager.InitializeConnection(fileName);
                    sheetname = m_workForceProcessManager.GetExcelSheetNames();
                }
            }
            catch (Exception)
            {
                message = "Invalid format or Password protected";
            }
            if (message != "Invalid format or Password protected" && sheetname.Length > 0)
            {

                sheetData = m_workForceProcessManager.GetDataTable(sheetname[0]);
                if (sheetData.Rows.Count > 0)
                {
                    List<string> columns = sheetData.Columns.Cast<DataColumn>().Where(x => x.ColumnName != "Error" && x.ColumnName != "UploadedBy" && x.ColumnName != "SheetName").Select(x => x.ColumnName).ToList();
                    var templateActualColumns = m_workForceProcessManager.GetTemplateMetaColumnDetails().Where(x => x.IsEnabled == true && x.IsTemplateColumn == true).ToList();
                    List<string> templatemisMatchColumns = templateActualColumns.Where(x => !columns.Contains(x.FieldName)).Select(x=>x.FieldName).ToList();
                    var templateColumns = (m_workForceProcessManager.GetTemplateMetaColumnDetails().Where(x => x.IsTemplateColumn == true && x.IsEnabled == true).Select(x => x.FieldName).ToList());
                    List<string> sheetMissMatchColumns = columns.Except(templateColumns).ToList();

                    if (columns.Count() == templateActualColumns.Count() && templatemisMatchColumns.Count()==0 && sheetMissMatchColumns.Count()==0)
                    {
                        int xmlProcessNum = m_workForceProcessManager.UpdateXmlProcess(Path.GetFileName(fileContent.FileName), fileName, sheetData.Rows.Count, userModel.UserNum);
                        bool result = await m_workForceProcessManager.InsertStagingTableData(sheetData, "Talent.StagingEmployeeData", columns, xmlProcessNum);
                        if (result)
                            message = "Uploaded successfully";
                    }
                    else
                    {
                        message = (templatemisMatchColumns.Count() != 0) ?  String.Join(", ", templatemisMatchColumns) + " Columns mismatch with sheet" : String.Join(", ", sheetMissMatchColumns) + " Columns mismatch with template";
                    }
                }
            }
            if (isLastFile != null && isLastFile != "")
            {
                if (await m_workForceProcessManager.ValidateEmployeeData(userModel.UserNum) > 0)
                {
                    if (await m_workForceProcessManager.ProcessEmployeeData(userModel.UserNum) > 0)
                        message = (message != "Invalid format or Password protected") ? "Processed successfully" : message;
                }
            }
            return Json(new { Message = message, uploadComplete = uploadComplete }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxChildActionOnly]
        public async Task<JsonResult> ClearAllData()
        {

            bool sucess = await  m_workForceProcessManager.ClearAllData(this.GetTenant());
            UserModel user = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
            user.IsSampleData = false;
            return Json(sucess, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxChildActionOnly]
        public async Task<JsonResult> GetEmployeeDataErrorList([DataSourceRequest]  DataSourceRequest request)
        {
            var result = await m_workForceProcessManager.GetEmployeeDataErrorList();
            return Json(result.ToDataSourceResult(request));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task GetEmployeeDataErrorExport()
        {
            var exportData = await m_workForceProcessManager.GetEmployeeDataErrorExport();
            ExportExcel.ToExcel(exportData, "Employee Data - Error");
        }

        // Author         :   Raja Ganapathy
        // Creation Date  :   30-Mar-2017 
        /// <summary>
        /// To get the Template columns with their values
        /// </summary>
        /// <param name="templateData">Defines the templateData</param>
        /// <param name="employeeData">Defines the employeeData</param>
        /// <returns>Returns list of templatecolumn model with value</returns>
        private List<TemplateMetaColumns> GetTemplateWithValues(List<TemplateMetaColumns> templateData, IEnumerable<EmployeeErrorData> employeeData)
        {
            return (from x in templateData
                    join y in employeeData on x.FieldName equals y.FieldName into temp
                    from y in temp.DefaultIfEmpty(new EmployeeErrorData())
                    select new TemplateMetaColumns
                    {
                        TemplateMetaColumnID = x.TemplateMetaColumnID,
                        DataFormat = x.DataFormat,
                        SampleData = x.SampleData,
                        DataLength = x.DataLength,
                        FieldName = x.FieldName,
                        AliasName = x.AliasName,
                        FunctionalGroup = x.FunctionalGroup,
                        FieldDescription = x.FieldDescription,
                        IsEnabled = x.IsEnabled,
                        IsTemplateColumn = x.IsTemplateColumn,
                        IsMandate = x.IsMandate,
                        ControlType = x.ControlType,
                        PlaceHolder = x.PlaceHolder,
                        FieldValue = y.FieldValue,
                        ErrorMessage = y.Error,
                        FieldInformation = x.FieldInformation,
                        ControlFormat = x.ControlFormat
                    }).ToList();
        }


        // Author       : Shaheena Shaik
        // Creation Date: 25-April-2017
        /// <summary>
        /// Getting Upload WorkFlow Grid data from database
        /// </summary>
        /// <param name="userNum">ID of LoggedInUserNum</param>
        /// <returns>Returning a view with grid details</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxChildActionOnly]
        public async Task<JsonResult> GetEmployeeLoadedDataDetails([DataSourceRequest] DataSourceRequest request)
        {
            UserModel userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
            var uploadWorkFLowGridDetails = await m_workForceProcessManager.GetEmployeeDataGridDetails(userModel.UserNum);
            return Json(uploadWorkFLowGridDetails.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxChildActionOnly]
        public async Task<JsonResult> DeleteEmployeeTemplateData(int xmlProcessNum)
        {
            var message = "";
            int status = await m_workForceProcessManager.DeleteEmployeeTemplateData(xmlProcessNum);
            if (status > 0)
            {
                message = "Deleted Successfully";
            }
            return Json(message, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxChildActionOnly]
        public JsonResult GetEmployeeIDIsDuplicate(string employeeID)
        {            
            bool result = m_workForceProcessManager.GetEmployeeCount(employeeID);
            return Json(result, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxChildActionOnly]
        public async Task<JsonResult> AddorUpdateEmployeeDetails(DataCorrectionModel data)
        {
            UserModel userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
            AppConfigModel appConfig = m_accountProcessManager.GetAppSetting();
            bool result = await m_workForceProcessManager.UpdateEmployeeData(data, userModel.UserNum, appConfig.CurrentYear);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public PartialViewResult _GetOrphanEmployeeDetails()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxChildActionOnly]
        public JsonResult GetOrphanEmployeeDetails([DataSourceRequest] DataSourceRequest request)
        {
            var orphanData = m_workForceProcessManager.GetOrphanEmployeeDetails();
            return Json(orphanData.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxChildActionOnly]
        public async Task<JsonResult> GetEmployeeDataErrorCount()
        {
            return Json(await m_workForceProcessManager.GetEmployeeDataErrorCount(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxChildActionOnly]
        public async Task<JsonResult> AssignEmployeeToCorporate(List<OrphanManagerDetails> EmployeeJobNum)
        {
            List<int> list = new List<int>();
            if (EmployeeJobNum != null)
                list = EmployeeJobNum.Select(x => x.EmployeeJobNum).ToList();
            int result = await m_workForceProcessManager.AssignEmployeeToCorporate(list);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxChildActionOnly]
        public async Task<JsonResult> GetCirCularReference(string EmployeeID, string SupervisorID, string PayrollStatus)
        {
            string status = await m_workForceProcessManager.GetCirCularReference(EmployeeID, SupervisorID, PayrollStatus);
            return Json(((status!=null) ? status : ""), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task GetExportXmlFile(int xmlProcessNum)
        {
            var exportData = await m_workForceProcessManager.GetExportXmlFile(xmlProcessNum);
            ExportExcel.ToExcel(exportData, "Employee Data");
        }
        public async Task GetExportTrainingData()
        {
            var exportData = await m_workForceProcessManager.GetExportTrainingData();
            ExportExcel.ToExcel(exportData, "Employee Training Data");
        }
        // Author       : Shaheena Shaik
        // Creation date:4-July-2017
        /// <summary>
        /// Validating PayCurrency whether it is already exists in database or not
        /// </summary>
        /// <param name="payCurrencyCode">The currencyCode which is newly added</param>
        /// <returns></returns>

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxChildActionOnly]
        public async Task<JsonResult> PayCurrencyValidation(string payCurrencyCode)
        {
            string isCurrencyCodeExists = await m_workForceProcessManager.payCurrencyValidation(payCurrencyCode);
            return Json(((isCurrencyCodeExists != null) ? isCurrencyCodeExists : ""), JsonRequestBehavior.AllowGet);
        }
    }
}

