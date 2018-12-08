// Copyright (c) 2015 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.

// Component Name :   IWorkForceProcessManager
// Description    :   Interface signature for WorkForceProcessManager
// Author         :   Raja Ganapathy
// Creation Date  :  30-Mar-2017 

using Laserbeam.BusinessObject.Common;
using Laserbeam.BusinessObject.Common.MeritBusinussObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Laserbeam.ProcessManager.Interfaces.Common
{
    public interface IWorkForceProcessManager
    {
        IQueryable<TemplateMetaColumns> GetTemplateMetaColumnDetails();
        Task <bool> UpdateSelectedTemplateColumns(List<int> selectedFields);
        Task<DataTable> GetEmployeeDataTemplate();
        IQueryable<EmployeeListModel> GetEmployeeList(int loggedInUserNum);
        Task<WorkForceTileData> GetWorkForceTileData(int loggedInUserNum);
        Task<DataTable> GetUploadedEmployeeDetails(int loggedInUserNum, string isMeritEligible);
        Task<IEnumerable<DropDownListItems>> GetSelectedDropDownDetails(string columnName);
        Task<IEnumerable<EmployeeErrorData>> GetSearchedEmployeeDetails(string employeeID);
        void InitializeConnection(string filePath);
        String[] GetExcelSheetNames();
        DataTable GetDataTable(string sheetName);
        int UpdateXmlProcess(string sheetName, string fileSourcePath, int recordCount, int userNum);
        Task<bool> InsertStagingTableData(DataTable userData, string DbName, List<string> columnsName, int xmlProcessNum);
        Task<int> ValidateEmployeeData(int userNum);
        Task<int> ProcessEmployeeData(int userNum);
        Task<bool> ClearAllData(string tenantName);
        Task<List<TemplateErrorListModel>> GetEmployeeDataErrorList();
        Task<DataTable> GetEmployeeDataErrorExport();        
         Task<IEnumerable<TemplateDataModel>> GetEmployeeDataGridDetails(int userNum);
        Task<int> DeleteEmployeeTemplateData(int xmlProcessNum);
        Task<bool> UpdateEmployeeData(DataCorrectionModel data, int userNum, string currentYear);
        List<OrphanManagerDetails> GetOrphanEmployeeDetails();
        Task<int> GetEmployeeDataErrorCount();
        Task<int> AssignEmployeeToCorporate(List<int> EmployeeJobNum);
        RuleConfiguration GetMeritConfiguration();
        Task<string> GetCirCularReference(string employeeID, string supervisorID, string payrollStatus);
        Task<IEnumerable<EmployeeErrorData>> GetEmployeeErrorRecordDetails(string errorType);
        Task<DataTable> GetExportXmlFile(int xmlProcessNum);
        Task<DataTable> GetExportTrainingData();
        bool GetEmployeeCount(string employeeID);

        // Author       : Shaheena Shaik
        // Creation date:4-July-2017
        /// <summary>
        /// Validating PayCurrency whether it is already exists in database or not
        /// </summary>
        /// <param name="payCurrencyCode">The currencyCode which is newly added</param>
        /// <returns></returns>
        Task<string> payCurrencyValidation(string payCurrencyCode);
        IQueryable<RatingModel> GetRatings();
    }
}
