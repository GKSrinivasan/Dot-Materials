// Copyright (c) 2016 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.
// Component Name  :  IWorkFlowRepository
// Description     :  Interface signature for WorkFlowRepository
// Author         : Muthuvel Sabarish M	
// Creation Date  :  27-Mar-2017 

using Laserbeam.BusinessObject.Common;
using Laserbeam.EntityManager.Common;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Laserbeam.DataManager.Interfaces.Common
{
    public interface IWorkFlowRepository
    {
        IQueryable<Module> GetProcessList();

        Task<IEnumerable<WorkFlowGrid>> GetGridDataValues(int processTypeNum, int meritCycleYear);

        Task<bool> UpdateWorkFlowGrid(List<WorkFlowGrid> workFlow, int processNum, int year, int userNum);

        int GetDefaultProcess();

        List<EmployeeModel> GetEmployeeIdList(int year);

        Task<bool> CopyInWorkFlowGrid(int meritCycleYear, string headerID, string headerValue, string FilteredEmployee, int ProcessNum, string actionStatus,int userNum);

        Task<int> ReloadGridDataValues(int processNum, int userNum, int Level,bool LevelBasedWFData);

        Task<int> DeleteApprovalTemplateData(int xmlProcessNum);

        Task<DataTable> GetApprovalDataTemplate();


        // Author       : Shaheena Shaik
        // Creation Date:6-april-2017
        // Reviewed By  :Raja Ganapathy
        // Reviewed Date: 6-April-2017
        /// <summary>
        /// GettingWorkflowExport Details from database
        /// </summary>
        /// <param name="moduleNum">ModelNum of a type Compensation</param>
        /// <returns>Returning a DataTable of WorkFlow Export values</returns>
        Task<DataTable> GetWorkFlowExportDetails(int moduleNum);        

        int UpdateXmlProcess(string sheetName, string fileSourcePath, int recordCount, int userNum);
        Task<bool> InsertStagingTableData(DataTable userData, string DbName, List<string> columnsName, int xmlProcessNum);

        Task<int> ValidateApprovalData();
        Task<int> ProcessApprovalData();

        Task<DataTable> GetApprovalDataErrorExport();
        Task<IEnumerable<TemplateErrorListModel>> GetApprovalDataErrorList();
        bool GetWorkFlowDataIsCustomized();

        Task<int> GetWorkFlowDataErrorCount();
        Task<IEnumerable<TemplateDataModel>> GetApprovalGridData(int userNum);
        Task<DataTable> GetExportXmlFile(int xmlProcessNum);
        string GetWorkFlowLevel();

        #region Chart
        Task<DataTable> GetTreantOrgChart(int year);
        Task<DataTable> GetGoogleOrgChart(int year);
        Task<IEnumerable<ManagerTree>> GetKendoOrgChart(int year);
        #endregion
    }
}
