// Copyright (c) 2015 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.
// Component Name  :    Workflow Controller
// Description     : 	Workflow Methods
// Author          :	Muthuvel Sabarish M
// Creation Date   : 	27-Mar-2017 
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using Laserbeam.BusinessObject.Common;
using Laserbeam.Constant.HR;
using Laserbeam.ProcessManager.Interfaces.Common;
using Laserbeam.UI.HR.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Laserbeam.UI.HR.Models;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Data;
using System.IO;

namespace Laserbeam.UI.HR.Controllers
{
    [AuthorizeUser(UserRoleConstants.AllAdminRoles)]
    public class WorkflowController : Controller
    {
        #region Fields
        // Author         :   Muthuvel Sabarish M	
        // Creation Date  :   27-Mar-2017 
        /// <summary>
        /// Object of IWorkFlowProcessManager
        /// </summary>
        private IWorkFlowProcessManager m_workFlowProcessManager;

        // Author         :   Muthuvel Sabarish M	
        // Creation Date  :   27-Mar-2017 
        /// <summary>
        /// Object of session manager
        /// </summary>
        private SessionManager m_sessionManager;
        private readonly IAccountProcessManager m_accountProcessManager;
        #endregion

        #region Constructor
        // Author         :   Muthuvel Sabarish M	
        // Creation Date  :   27-Mar-2017 
        /// <summary>
        /// Initializes the objects in this class
        /// </summary>
        /// <param name="meritProcessManager">Object of IWorkFlowProcessManager</param>
        public WorkflowController(IWorkFlowProcessManager workFlowProcessManager, IAccountProcessManager accountProcessManager)
        {
            m_workFlowProcessManager = workFlowProcessManager;
            m_sessionManager = new SessionManager();
            m_accountProcessManager = accountProcessManager;
        }
        #endregion

        #region Public Method

        // Author         :   Muthuvel Sabarish M		
        // Creation Date  :   27-Mar-2017 
        /// <summary>
        /// It returns the main view for manage user page
        /// </summary>
        /// <returns>It returns the main view</returns>
        [HttpGet]
        public ActionResult Home()
        {
            WorkflowModel model = new WorkflowModel();
            model.ProcessNum = m_workFlowProcessManager.GetDefaultProcess();
            model.IsCustomized = m_workFlowProcessManager.GetWorkFlowDataIsCustomized();
            model.WorkFlowLevel = m_workFlowProcessManager.GetWorkFlowLevel();
            ViewBag.ApprovalErrorDataCount = Task.Run(() => m_workFlowProcessManager.GetWorkFlowDataErrorCount()).Result;
            return View(model);
        }

        // Author         :   Muthuvel Sabarish M	
        // Creation Date  :   28-Mar-2017 
        /// <summary>
        /// Gets the data to bind to process DropDown
        /// </summary>
        /// <returns>Returns processList</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxChildActionOnly]
        public JsonResult GetProcess()
        {
            var processList = m_workFlowProcessManager.GetProcessList();
            return Json(processList, JsonRequestBehavior.AllowGet);
        }


        // Author         :   Muthuvel Sabarish M	
        // Creation Date  :   28-Mar-2017 
        /// <summary>
        /// Gets the data to bind to workflow grid
        /// </summary>
        /// <returns>Returns workflow data</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxChildActionOnly]
        public async Task<JsonResult> GetGridData([DataSourceRequest]DataSourceRequest request, int ProcessNum = 0)
        {
            AppConfigModel appConfig = m_accountProcessManager.GetAppSetting();
            int meritCycleYear = Convert.ToInt32(appConfig.MeritCycleYear);
            var result = await m_workFlowProcessManager.GetGridData(ProcessNum, meritCycleYear);
            return Json(result.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxChildActionOnly]
        public async Task<JsonResult> GetApprovalTemplateData([DataSourceRequest]  DataSourceRequest request)
        {
            UserModel userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
            var result = await m_workFlowProcessManager.GetApprovalGridData(userModel.UserNum);
            return Json(result.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }


        // Author         :   Muthuvel Sabarish M	
        // Creation Date  :   31-Mar-2017 
        /// <summary>
        /// Get the employeeid 
        /// </summary>
        /// <returns>Returns employeeid</returns>
        [HttpGet]
        public JsonResult GetEmpId()
        {
            AppConfigModel appConfig = m_accountProcessManager.GetAppSetting();
            int year = Convert.ToInt32(appConfig.MeritCycleYear);
            var EmpIdList = m_workFlowProcessManager.GetEmployeeIdList(year);
            var serializer = new JavaScriptSerializer();
            WorkflowModel work = new WorkflowModel();
            work.EmpIds = serializer.Serialize(EmpIdList);
            return Json(work.EmpIds, JsonRequestBehavior.AllowGet);

        }

        //NOTE: Boobalan: Completed passing Anti-ForgeryToken
        // Author         :   Muthuvel Sabarish M	
        // Creation Date  :   30-Mar-2017 
        /// <summary>
        /// Update the data in Workflow grid
        /// </summary>
        /// <returns>Update workflow data</returns>
        [HttpPost]
        [AjaxChildActionOnly]
        [ValidateAntiForgeryToken]
        public async  Task<JsonResult> UpdateGridData(List<WorkFlowGrid> workFlow, int processNum, string module)
        {
            UserModel userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
            if (workFlow != null && workFlow.Count > 0)
            {

                AppConfigModel appConfig = m_accountProcessManager.GetAppSetting();
                int year = Convert.ToInt32(appConfig.MeritCycleYear);
                bool status = await m_workFlowProcessManager.UpdateWorkFlowGrid(workFlow, processNum, year, userModel.UserNum);
                if (status)
                {
                    return Json("Updated successfully");
                }
                else
                {
                    return Json("An error occurred while saving record,");
                }
            }
            else
            {
                return null;
            }
        }

        //NOTE: Boobalan: Completed passing Anti-ForgeryToken
        // Author         :   Muthuvel Sabarish M	
        // Creation Date  :   31-Mar-2017 
        /// <summary>
        /// Complete Update/Delete data in workflow grid based on action status 
        /// </summary>
        /// <returns>Update/Delete data in workflow grid </returns>
        [HttpPost]
        [AjaxChildActionOnly]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> CopyInWorkFlowGrid([DataSourceRequest]DataSourceRequest request, int processNum, string headerID, string headerValue, string actionStatus)
        {
            UserModel userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
            if (headerValue == null && actionStatus == "Delete")
            {
                headerValue = "";
            }
            if ((headerID != "" && headerValue != "") || (actionStatus == "Delete"))
            {
                string FilteredEmployee = "";
                AppConfigModel appConfig = m_accountProcessManager.GetAppSetting();
                int meritCycleYear = Convert.ToInt32(appConfig.MeritCycleYear);
                if (request.Filters != null)
                {
                    if (request.Filters.Count() != 0)
                    {
                        var getGridData = await m_workFlowProcessManager.GetGridData(processNum, meritCycleYear);
                        request.Page = 0;
                        request.PageSize = 0;
                        var obj = getGridData.ToDataSourceResult(request);
                        foreach (var item in obj.Data)
                            FilteredEmployee = FilteredEmployee + "," + ((Laserbeam.BusinessObject.Common.WorkFlowGrid)(item)).EmployeeID;

                    }
                }
                bool status = await m_workFlowProcessManager.CopyInWorkFlowGrid(meritCycleYear, headerID.TrimStart(','), headerValue.TrimStart(','), (FilteredEmployee != "" ? FilteredEmployee.TrimStart(',') : FilteredEmployee), processNum, actionStatus, userModel.UserNum);
                return Json(status);
                //if (status)
                //{
                //    return Json("Updated successfully");
                //}
                //else
                //{
                //    return Json("An error occurred while saving record,");
                //}
            }
            else
            {
                return null;
            }
        }


        // Author         :   Muthuvel Sabarish M	
        // Creation Date  :   3-Mar-2017 
        /// <summary>
        /// Get the Workflow data based on org structure 
        /// </summary>
        /// <returns>Returns Workflow data based on org structure </returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxChildActionOnly]
        public async Task<JsonResult> ReloadGrid([DataSourceRequest] DataSourceRequest request, int processNum,int Level, bool LevelBasedWFData)
        {
            UserModel userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
            int result = await m_workFlowProcessManager.ReloadGridData(processNum, userModel.UserNum, Level, LevelBasedWFData);
            if (result > 0)
            { return Json("Data reloaded successfully"); }
            else
            { return Json("An error occurred while reloading data,"); }
        }

        [HttpGet]
        public PartialViewResult _FilterSort(string type)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem { Text = "Employee ID", Value = "EmployeeID" });
            list.Add(new SelectListItem { Text = "Employee Name", Value = "EmployeeName" });
            if (type == "EmployeeID")
            {
                list.Add(new SelectListItem { Text = "Level 1", Value = "EmpID_1" });
                list.Add(new SelectListItem { Text = "Level 2", Value = "EmpID_2" });
                list.Add(new SelectListItem { Text = "Level 3", Value = "EmpID_3" });
                list.Add(new SelectListItem { Text = "Level 4", Value = "EmpID_4" });
                list.Add(new SelectListItem { Text = "Level 5", Value = "EmpID_5" });
                list.Add(new SelectListItem { Text = "Level 6", Value = "EmpID_6" });
                list.Add(new SelectListItem { Text = "Level 7", Value = "EmpID_7" });
                list.Add(new SelectListItem { Text = "Level 8", Value = "EmpID_8" });
                list.Add(new SelectListItem { Text = "Level 9", Value = "EmpID_9" });
                list.Add(new SelectListItem { Text = "Level 10", Value = "EmpID_10" });
            }
            else
            {
                list.Add(new SelectListItem { Text = "Level 1", Value = "EmpName_1" });
                list.Add(new SelectListItem { Text = "Level 2", Value = "EmpName_2" });
                list.Add(new SelectListItem { Text = "Level 3", Value = "EmpName_3" });
                list.Add(new SelectListItem { Text = "Level 4", Value = "EmpName_4" });
                list.Add(new SelectListItem { Text = "Level 5", Value = "EmpName_5" });
                list.Add(new SelectListItem { Text = "Level 6", Value = "EmpName_6" });
                list.Add(new SelectListItem { Text = "Level 7", Value = "EmpName_7" });
                list.Add(new SelectListItem { Text = "Level 8", Value = "EmpName_8" });
                list.Add(new SelectListItem { Text = "Level 9", Value = "EmpName_9" });
                list.Add(new SelectListItem { Text = "Level 10", Value = "EmpName_10" });
            }
            return PartialView(list);
        }

        [HttpGet]
        public PartialViewResult _GetApprovalDataErrorList()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxChildActionOnly]
        public async Task<JsonResult> GetApprovalDataErrorList([DataSourceRequest]  DataSourceRequest request)
        {
            var result = await m_workFlowProcessManager.GetApprovalDataErrorList();
            return Json(result.ToDataSourceResult(request));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task GetApprovalDataErrorExport()
        {
            var exportData = await m_workFlowProcessManager.GetApprovalDataErrorExport();
            ExportExcel.ToExcel(exportData, "Approval Data - Error");
        }

        // Author       : Shaheena Shaik
        // Creation Date:6-april-2017
        // Reviewed By  :Raja Ganapathy
        // Reviewed Date: 6-April-2017
        /// <summary>
        /// Getting Work Flow Export data and converting it to Excel
        /// </summary>
        /// <param name="moduleNum">ModelNum of a type Compensation</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task GetExportGridData(int moduleNum)
        {
            var exportWorkFlowData = await m_workFlowProcessManager.GetWorkFlowExportDetails(moduleNum);
            ExportExcel.ToExcel(exportWorkFlowData, "WorkFlow Excel Sheet Data");
        }      

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxChildActionOnly]
        public async Task<JsonResult> DeleteApprovalTemplateData(int xmlProcessNum)
        {
            var message = "";
            int status = await m_workFlowProcessManager.DeleteApprovalTemplateData(xmlProcessNum);
            if (status > 0)
            {
                message = "Deleted Successfully";
            }
            return Json(message, JsonRequestBehavior.AllowGet);

        }


        // Author         :   Muthuvel Sabarish.M
        // Creation Date  :   07-Apr-2017 
        /// <summary>
        /// Get the approval data template
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task GetApprovalDataTemplate()
        {
            var exportData = await m_workFlowProcessManager.GetApprovalDataTemplate();
            ExportExcel.ToExcel(exportData, "Approval Data Template");
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
                    fileName = Server.MapPath("~/ApprovalData/" + Path.GetFileName(fileContent.FileName));
                    fileContent.SaveAs(fileName);
                    m_workFlowProcessManager.InitializeConnection(fileName);
                    sheetname = m_workFlowProcessManager.GetExcelSheetNames();
                }
            }
            catch (Exception)
            {
                message = "Invalid format or Password protected";
            }
            if (message != "Invalid format or Password protected" && sheetname.Length > 0)
            {
                sheetData = m_workFlowProcessManager.GetDataTable(sheetname[0]);
                if (sheetData.Rows.Count > 0)
                {
                    List<string> columns = sheetData.Columns.Cast<DataColumn>().Where(x => x.ColumnName != "Error" && x.ColumnName != "UploadedBy" && x.ColumnName != "SheetName").Select(x => x.ColumnName).ToList();
                    List<string> apTemplateColumns = GetApprovalDataTemplateColumns();
                    var templateColumns = (from approvalDataColumn in apTemplateColumns
                                           from sheetColumns in columns
                                           where string.Equals(approvalDataColumn, sheetColumns, StringComparison.InvariantCultureIgnoreCase)
                                           select approvalDataColumn).ToList();

                    if (columns.Count() == templateColumns.Count())
                    {
                        int xmlProcessNum = m_workFlowProcessManager.UpdateXmlProcess(Path.GetFileName(fileContent.FileName), fileName, sheetData.Rows.Count, userModel.UserNum);
                        bool result = await m_workFlowProcessManager.InsertStagingTableData(sheetData, "Talent.StagingApprovalData", columns, xmlProcessNum);
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
                if (await m_workFlowProcessManager.ValidateApprovalData() > 0)
                {
                    if (await m_workFlowProcessManager.ProcessApprovalData() > 0)
                        message = (message != "Invalid format or Password protected") ? "Processed successfully" : message;
                }
            }
            return Json(new { Message = message, uploadComplete = uploadComplete }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxChildActionOnly]
        public async Task<JsonResult> GetWorkFlowDataErrorCount()
        {
            return Json(await m_workFlowProcessManager.GetWorkFlowDataErrorCount(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task GetExportXmlFile(int xmlProcessNum)
        {
            var exportData = await m_workFlowProcessManager.GetExportXmlFile(xmlProcessNum);
            ExportExcel.ToExcel(exportData, "Approval Data");
        }


        #endregion

        #region Private Method
        private List<string> GetApprovalDataTemplateColumns()
        {
            List<string> columns = new List<string>();
            columns.Add("EmployeeID");
            columns.Add("LevelOneManager");
            columns.Add("LevelTwoManager");
            columns.Add("LevelThreeManager");
            columns.Add("LevelFourManager");
            columns.Add("LevelFiveManager");
            columns.Add("LevelSixManager");
            columns.Add("LevelSevenManager");
            columns.Add("LevelEightManager");
            columns.Add("LevelNineManager");
            columns.Add("LevelTenManager");
            columns.Add("Module");
            return columns;
        }
        #endregion

        #region Chart
        public async Task<JsonResult> GetTreantOrgChart()
        {
            List<WorkFlowOrgChart> g = new List<WorkFlowOrgChart>();
            AppConfigModel appConfig = m_sessionManager.GetSession<AppConfigModel>(SessionConstants.AppConfigModel);
            int meritCycleYear = Convert.ToInt32(appConfig.MeritCycleYear);
            var exportWorkFlowData = await m_workFlowProcessManager.GetTreantOrgChart(meritCycleYear);
            foreach (DataRow row in exportWorkFlowData.Rows)
            {
                g.Add(new WorkFlowOrgChart
                { Employee = row["EmployeeName"].ToString(), mgrID = row["ParentNode"].ToString(), empID = row["Node"].ToString() });
            }
            return Json(g, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetGoogleOrgChart()
        {
            List<WorkFlowOrgChart> g = new List<WorkFlowOrgChart>();
            AppConfigModel appConfig = m_sessionManager.GetSession<AppConfigModel>(SessionConstants.AppConfigModel);
            int meritCycleYear = Convert.ToInt32(appConfig.MeritCycleYear);
            var exportWorkFlowData = await m_workFlowProcessManager.GetGoogleOrgChart(meritCycleYear);
            foreach (DataRow row in exportWorkFlowData.Rows)
            {
                g.Add(new WorkFlowOrgChart
                { Employee = row["EmpName"].ToString(), mgrID = row["MgrID"].ToString(), empID = row["EmpID"].ToString(),designation=row["Designation"].ToString() });
            }
            return Json(g, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetKendoOrgChart()
        {
            AppConfigModel appConfig = m_sessionManager.GetSession<AppConfigModel>(SessionConstants.AppConfigModel);
            int meritCycleYear = Convert.ToInt32(appConfig.MeritCycleYear);
            EmployeeModel employeeModel = m_sessionManager.GetSession<EmployeeModel>(SessionConstants.EmployeeModel);
            var result = await m_workFlowProcessManager.GetKendoOrgChart(meritCycleYear);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}