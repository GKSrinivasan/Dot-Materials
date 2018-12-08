// Copyright (c) 2015 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.

// Component Name :   IWorkFlowProcessManager
// Description    :   Interface signature for WorkFlowProcessManager
// Author         :   Muthuvel Sabarish M
// Creation Date  :  27-Mar-2017 
using Laserbeam.BusinessObject.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Laserbeam.ProcessManager.Interfaces.Common
{
    public interface IWorkFlowProcessManager
    {
        IEnumerable<DropDownListModel> GetProcessList();

        Task<IEnumerable<WorkFlowGrid>> GetGridData(int processTypeNum, int meritCycleYear);

        Task<bool> UpdateWorkFlowGrid(List<WorkFlowGrid> workFlow, int processNum, int year, int userNum);

        int GetDefaultProcess();

        Task<bool> CopyInWorkFlowGrid(int year, string headerID, string headerValue, string FilteredEmployee, int ProcessNum, string actionStatus,int userNum);

        Task<int> ReloadGridData(int processNum, int userNum, int Level,bool LevelBasedWFData);

        List<EmployeeModel> GetEmployeeIdList(int year);

        Task<IEnumerable<TemplateDataModel>> GetApprovalGridData(int userNum);

        // Author       : Shaheena Shaik
        // Creation Date:6-april-2017
        /// Reviewed By  :Raja Ganapathy
        // Reviewed Date: 6-April-2017
        /// <summary>
        /// Getting WorkFlowExportDetails from database
        /// </summary>
        /// <param name="moduleNum">Module Num of type Compensation</param>
        /// <returns>Returning a Datatable of WorkFlow Export Values</returns>
        Task<DataTable> GetWorkFlowExportDetails(int moduleNum);        

        Task<int> DeleteApprovalTemplateData(int xmlProcessNum);

        Task<DataTable> GetApprovalDataTemplate();
        void InitializeConnection(string filePath);
        String[] GetExcelSheetNames();
        DataTable GetDataTable(string sheetName);
        int UpdateXmlProcess(string sheetName, string fileSourcePath, int recordCount,int userNum);
        Task<bool> InsertStagingTableData(DataTable userData, string DbName, List<string> columnsName, int xmlProcessNum);
        Task<int> ValidateApprovalData();
        Task<int> ProcessApprovalData();
        Task<List<TemplateErrorListModel>> GetApprovalDataErrorList();
        Task<DataTable> GetApprovalDataErrorExport();
        bool GetWorkFlowDataIsCustomized();

        Task<int> GetWorkFlowDataErrorCount();
        Task<DataTable> GetExportXmlFile(int xmlProcessNum);
        string GetWorkFlowLevel();

        #region Chart
        Task<DataTable> GetTreantOrgChart(int year);
        Task<DataTable> GetGoogleOrgChart(int year);
        Task<IEnumerable<ManagerTree>> GetKendoOrgChart(int year);
        #endregion
    }
}
