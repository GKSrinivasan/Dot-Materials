// Copyright (c) 2015 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.

// Component Name :   ICompensationRepository
// Description    :   Interface signature for CompensationRepository
// Author         :   Raja Ganapathy
// Creation Date  :   22-06-2016

using Laserbeam.BusinessObject.Common;
using Laserbeam.BusinessObject.Common.CachedModels;
using Laserbeam.BusinessObject.Common.MeritBusinussObjects;
using Laserbeam.Constant.HR;
using Laserbeam.EntityManager.Common;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace Laserbeam.DataManager.Interfaces.Common
{
    public interface ICompensationRepository 
    {
        Task deleteComments(int commentKey);
        int getCompensationTypeNum(string type);
        IQueryable<RatingModel> GetRatings();
        bool IsEmployeeDataEmpty();
        IQueryable<CompensationType> GetCompensationTypes();        
        void UpdateCompReportees(List<MeritGridModel> compensationReportees, int jopbYear, int userNum);
        Task<IEnumerable<int>> GetCompCompletedCount(int selectedManagerNum, int year, int loggedInUserNum);
              IQueryable<MetaColumn> GetReporteeConfiguration();
        BusinessSettingModel GetCompensationTypeConfiguration();
        Task<IEnumerable<EmployeeInfoDetails>> GetEmployeeInfo(int employeeNum, int loggedInUserNum);
        IQueryable<EmployeeCompComment> GetCompComments(int empJobNum);
        IQueryable<EmployeeApprovalDetail> GetWorkFlowComments(int empJobNum);
        void PutUpdateCompensationComments(EmployeeCompComment comment);                
        IEnumerable<EmployeeCompComment> GetCommentsBasedOnType(int empJobNum, string type);
        Task<IEnumerable<ManagerTree>> GetCompManagerTree(int managerNum, int loggedInEmployeeNum, int loggedInUserNum, int userNum,ViewPageType pageType);
        Task<IEnumerable<MeritGridModel>> GetCompReportees(int managerNum, int loggedInEmployeeNum, int loggedInUserNum, MenuType compMenuType, bool isRollup, int loggedSelectedUserNum);
        Task<IEnumerable<SubmitReporteesModel>> GetCompSubmitReportees(int managerNum, int loggedInEmployeeNum, int loggedInUserNum, MenuType compMenuType, bool isRollup, int loggedSelectedUserNum);
        Task<IEnumerable<SubmitReporteesModel>> GetCompApprovalReportees(int managerNum, int loggedInEmployeeNum, int loggedInUserNum, MenuType compMenuType, bool isRollup, int loggedSelectedUserNum);
        Task<IEnumerable<SubmitReporteesModel>> GetCompReopenReportees(int managerNum, int loggedInEmployeeNum, int loggedInUserNum, MenuType compMenuType, bool isRollup, int loggedSelectedUserNum);

         Task<IEnumerable<ExchangeCurrencies>> GetCurrencies();
        void UpdateEmployeeComment(int empJobNum, int userNum, int employeeCompCommentNum, string comments, string grade);
        Task<BudgetModel> GetBudgetData(int loggedInEmpNum, int employeeNum, int loggedInUserNum, MenuType compMenuType, string currencyCulture, int currencyCodeNum, bool isRollup = false, bool isSelectedRollup = false);
        Task<IEnumerable<DropDownListItems>> GetFilterConfiguration();
        Task<int> UpdateApprovalStatus(List<SubmitReporteesModel> selectedRows, int loggedInEmployeeNum, int selectedManagerNum, MenuType MenuType, bool isRollup, ApprovalStatus approvalStatus, AppConfigModel appConfig, int userNum, string Comment);
        
        // Author        :  Shaheena Shaik
        // Creation Date :  4-July-2017
        /// <summary>
        /// Returns the employeename
        /// </summary>
        /// <param name="selectedManagerNum">Denotes the selected manager num</param>
        Task<string> GetManagerName(int managerNum);
        Task RevertPromotion(int empJobNum, decimal newSalaryLocal, decimal newHrlyRate, decimal newCompRatio, decimal TCC);
        Task<int>GetApprovalStatus(int selectedManagerNum, int loggedInEmployeeNum);
        Task<int> UpdateCommentStatus(int empJobNum,int userNum);
        Task<IEnumerable<ApprovalEmployeeSearchData>> GetCompApprovalReporteesSearch(int managerNum, int loggedInUserNum, MenuType compMenuType, bool isRollup, string approvalType);
        string GetworkflowStatus();
        Task<IEnumerable<EmployeeInfoBasicDetails>> GetEmployeeBasicInfo(int employeeNum);
        bool IsInDirects(int managerNum);
    }
    }
