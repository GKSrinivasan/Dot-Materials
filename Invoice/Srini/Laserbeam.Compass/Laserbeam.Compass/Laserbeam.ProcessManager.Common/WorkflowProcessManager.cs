// Copyright (c) 2017 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.
// Component Name  : 	Workflow Process Manager
// Description     : 	Workflow related business logics
// Author          :    Muthuvel Sabarish M
// Creation Date   :    27-Mar-2017 

using Laserbeam.DataManager.Interfaces.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Laserbeam.BusinessObject.Common;
using Laserbeam.ProcessManager.Interfaces.Common;
using System.Data;
using System.Data.OleDb;

namespace Laserbeam.ProcessManager.Common
{
    public class WorkflowProcessManager : IWorkFlowProcessManager
    {
        #region Fields
        // Author         :   Muthuvel Sabarish M	
        // Creation Date  :   27-Mar-2017 
        /// <summary>
        /// Instance of WorkflowRepository
        /// </summary>
        private IWorkFlowRepository m_workflowRepository;

        #endregion

        #region Constructor

        // Author         :   Muthuvel Sabarish M	
        // Creation Date  :   27-Mar-2017 
        /// <summary>
        /// Constructor for WorkflowProcessManager
        /// </summary>
        /// <param name="groupRepository">Instance of ConfigurationRepository</param>
        public WorkflowProcessManager(IWorkFlowRepository workflowRepository)
        {
            m_workflowRepository = workflowRepository;
        }

        #endregion

        // Author         :   Muthuvel Sabarish M	
        // Creation Date  :   28-Mar-2017 
        /// <summary>
        /// Gets the data to bind to process DropDown
        /// </summary>
        /// <returns>Returns processList</returns>
        public IEnumerable<DropDownListModel> GetProcessList()
        {
            return m_workflowRepository.GetProcessList().Select(x => new DropDownListModel
            {
                Text = x.ModuleName,
                Value = x.ModuleNum.ToString(),
            });
        }

        // Author         :   Muthuvel Sabarish M	
        // Creation Date  :   28-Mar-2017 
        /// <summary>
        /// Gets the data to bind to workflow grid
        /// </summary>
        /// <returns>Returns workflow data</returns>
        public async Task<IEnumerable<WorkFlowGrid>> GetGridData(int processTypeNum, int meritCycleYear)
        {
            return await m_workflowRepository.GetGridDataValues(processTypeNum, meritCycleYear);
        }

        public async Task<IEnumerable<TemplateDataModel>> GetApprovalGridData(int userNum)
        {
            return await m_workflowRepository.GetApprovalGridData(userNum);
        }

        public int GetDefaultProcess()
        {
            return m_workflowRepository.GetDefaultProcess();
        }

        public bool GetWorkFlowDataIsCustomized()
        {
            return m_workflowRepository.GetWorkFlowDataIsCustomized();
        }

        public string GetWorkFlowLevel()
        {
            return m_workflowRepository.GetWorkFlowLevel();
        }

        // Author         :   Muthuvel Sabarish M	
        // Creation Date  :   30-Mar-2017 
        /// <summary>
        /// Update the data in Workflow grid
        /// </summary>
        /// <returns>Update workflow data</returns>
        public async Task<bool> UpdateWorkFlowGrid(List<WorkFlowGrid> workFlow, int processNum, int year, int userNum)
        {
            return await m_workflowRepository.UpdateWorkFlowGrid(workFlow, processNum, year, userNum);
        }

        // Author         :   Muthuvel Sabarish M	
        // Creation Date  :   31-Mar-2017 
        /// <summary>
        /// Get the employeeid 
        /// </summary>
        /// <returns>Returns employeeid</returns>
        public List<EmployeeModel> GetEmployeeIdList(int year)
        {

            return m_workflowRepository.GetEmployeeIdList(year);
        }

        // Author         :   Muthuvel Sabarish M	
        // Creation Date  :   31-Mar-2017 
        /// <summary>
        /// Complete Update/Delete data in workflow grid based on action status 
        /// </summary>
        /// <returns>Update/Delete data in workflow grid </returns>
        public async Task<bool> CopyInWorkFlowGrid(int meritCycleYear, string headerID, string headerValue, string FilteredEmployee, int ProcessNum, string actionStatus,int userNum)
        {
            return await m_workflowRepository.CopyInWorkFlowGrid(meritCycleYear, headerID, headerValue, FilteredEmployee, ProcessNum, actionStatus, userNum);
        }

        // Author         :   Muthuvel Sabarish M	
        // Creation Date  :   3-Mar-2017 
        /// <summary>
        /// Get the Workflow data based on org structure 
        /// </summary>
        /// <returns>Returns Workflow data based on org structure </returns>
        public async Task<int> ReloadGridData(int processNum, int userNum,int Level,bool LevelBasedWFData)
        {
            return await m_workflowRepository.ReloadGridDataValues(processNum, userNum, Level, LevelBasedWFData);
        }


        // Author       : Shaheena Shaik
        // Creation Date:6-april-2017
        // Reviewed By  :Raja Ganapathy
        // Reviewed Date: 6-April-2017
        /// <summary>
        /// GettingWorkflowExport Details from database
        /// </summary>
        /// <param name="moduleNum">ModelNum of a type Compensation</param>
        /// <returns>Returning a DataTable of WorkFlow Export values</returns>
        public async Task<DataTable> GetWorkFlowExportDetails(int moduleNum)
        {
            return await m_workflowRepository.GetWorkFlowExportDetails(moduleNum);
        }        

        public async Task<int> DeleteApprovalTemplateData(int xmlProcessNum)
        {
            return await m_workflowRepository.DeleteApprovalTemplateData(xmlProcessNum);
        }

        public async Task<DataTable> GetApprovalDataTemplate()
        {
            return await m_workflowRepository.GetApprovalDataTemplate();
        }

        public int UpdateXmlProcess(string sheetName, string fileSourcePath, int recordCount,int userNum)
        {
            return m_workflowRepository.UpdateXmlProcess(sheetName, fileSourcePath, recordCount, userNum );
        }

        public async Task<bool> InsertStagingTableData(DataTable userData, string DbName, List<string> columnsName, int xmlProcessNum)
        {
            return await m_workflowRepository.InsertStagingTableData(userData, DbName, columnsName, xmlProcessNum);
        }

        public async Task<int> ValidateApprovalData()
        {
            return await m_workflowRepository.ValidateApprovalData();
        }

        public async Task<int> ProcessApprovalData()
        {
            return await m_workflowRepository.ProcessApprovalData();
        }

        public async Task<DataTable> GetApprovalDataErrorExport()
        {
            return await m_workflowRepository.GetApprovalDataErrorExport();
        }

        public async Task<List<TemplateErrorListModel>> GetApprovalDataErrorList()
        {
            var result = await m_workflowRepository.GetApprovalDataErrorList();
            return (from x in result
                    select new TemplateErrorListModel
                    {
                        Error = x.Error,
                        AffectedData = x.AffectedData,
                        HowToFix = x.HowToFix
                    }).ToList();

        }

        public async Task<int> GetWorkFlowDataErrorCount()
        {
            return await m_workflowRepository.GetWorkFlowDataErrorCount();
        }

        public async Task<DataTable> GetExportXmlFile(int xmlProcessNum)
        {
            return await m_workflowRepository.GetExportXmlFile(xmlProcessNum);
        }

        #region Excel

        #region Variable Declaration

        private OleDbConnection cn;
        private OleDbDataAdapter daAdapter;

        //private string ExcelCon = @"Provider=Microsoft.Jet.OLEDB.4.0;";
        private string strConnectionString;

        //private string strParseError = "";
        private string SheetName, Range;

        #endregion

        // Author         :   Raja Ganapathy
        // Creation Date  :   30-Mar-2017 
        /// <summary>
        /// Initialize ExcelXML component using the sepecifed File name, By default HDR property will be false.
        /// </summary>
        /// <param name="strFileName"></param>
        public void InitializeConnection(string filePath)
        {
            strConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + (filePath) + ";Extended Properties=\"Excel 12.0;HDR=YES;\"";
            cn = new OleDbConnection(strConnectionString);
        }




        #region Excel File Info

        public String[] GetExcelSheetNames()
        {
            System.Data.DataTable dt = null;
            try
            {
                cn.Open();
                // Get the data table containing the schema
                dt = cn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                if (dt == null)
                {
                    return null;
                }

                String[] excelSheets = new String[dt.Rows.Count];
                int i = 0;
                // Add the sheet name to the string array.
                foreach (DataRow row in dt.Rows)
                {
                    string strSheetTableName = row["TABLE_NAME"].ToString();
                    excelSheets[i] = strSheetTableName.Replace("'", "").Split('$')[0];
                    break;
                }
                return excelSheets;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                // Clean up.
                cn.Close();
            }
        }

        public DataTable GetDataTable(string strSheetName)
        {
            try
            {
                string strComand;
                if (strSheetName.IndexOf("|") > 0)
                {
                    SheetName = strSheetName.Substring(0, strSheetName.IndexOf("|"));
                    Range = strSheetName.Substring(strSheetName.IndexOf("|") + 1);
                    strComand = "select * from [" + SheetName + "$" + Range + "]";
                }
                else
                {
                    strComand = "select * from [" + strSheetName + "$]";
                }
                daAdapter = new OleDbDataAdapter(strComand, cn);
                DataTable dt = new DataTable("Table");
                daAdapter.FillSchema(dt, SchemaType.Source);
                daAdapter.Fill(dt);
                cn.Close();
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion
        #endregion

        #region Chart
        public async Task<DataTable> GetTreantOrgChart(int year)
        {
            return await m_workflowRepository.GetTreantOrgChart(year);
        }

        public async Task<DataTable> GetGoogleOrgChart(int year)
        {
            return await m_workflowRepository.GetGoogleOrgChart(year);
        }

        public async Task<IEnumerable<ManagerTree>> GetKendoOrgChart(int year)
        {
            return await m_workflowRepository.GetKendoOrgChart(year);
        }
        #endregion
    }
}
