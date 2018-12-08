// Copyright (c) 2016 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.
// Component Name  :  IWorkForceRepository
// Description     :  Repository for WorkForce
// Author         :  Raja Ganapathy
// Creation Date  :  30-Mar-2017

using Laserbeam.BusinessObject.Common;
using Laserbeam.BusinessObject.Common.MeritBusinussObjects;
using Laserbeam.EntityManager.Common;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Laserbeam.DataManager.Interfaces.Common
{
    public interface IWorkForceRepository
    {
        IQueryable<MetaColumn> GetTemplateMetaColumnDetails();
        Task<bool> UpdateSelectedTemplateColumns(List<int> selectedFields);
        Task<DataTable> GetEmployeeDataTemplate();
        IQueryable<EmployeeListModel> GetEmployeeList(int loggedInUserNum);
        Task<WorkForceTileData> GetWorkForceTileData(int loggedInUserNum);
        Task<DataTable> GetUploadedEmployeeDetails(int loggedInUserNum, string isMeritEligible);
        Task<IEnumerable<DropDownListItems>> GetSelectedDropDownDetails(string columnName);
        Task<IEnumerable<EmployeeErrorData>> GetSearchedEmployeeDetails(string employeeID);
        int UpdateXmlProcess(string sheetName, string fileSourcePath, int recordCount, int userNum);
        Task<bool> InsertStagingTableData(DataTable userData, string DbName, List<string> columnsName, int xmlProcessNum);
        Task<int> ValidateEmployeeData(int userNum);
        Task<int> ProcessEmployeeData(int userNum);
        Task<bool> ClearAllData(string tenantName);
        Task<DataTable> GetEmployeeDataErrorExport();
        Task<IEnumerable<TemplateErrorListModel>> GetEmployeeDataErrorList();
        Task<bool> UpdateEmployeeData(decimal? budgetProration, EmployeeDetailsCorrection data,int stagingNumber,string currentYear);
        Task<IEnumerable<TemplateDataModel>> GetEmployeeDataGridDetails(int userNum);
        Task<int> DeleteEmployeeTemplateData(int xmlProcessNum);        
        List<OrphanManagerDetails> GetOrphanEmployeeDetails();
        RuleConfiguration GetMeritConfiguration();
        Task<int> GetEmployeeDataErrorCount();
        Task<int> AssignEmployeeToCorporate(List<int> EmployeeJobNum);
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
