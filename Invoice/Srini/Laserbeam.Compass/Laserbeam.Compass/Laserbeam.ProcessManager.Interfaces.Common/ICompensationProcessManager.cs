// Copyright (c) 2015 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.

// Component Name :   ICompensationProcessManager
// Description    :   Interface signature for CompensationProcessManager
// Author         :   Raja Ganapathy		
// Creation Date  :   05-Jul-2016  

using Laserbeam.BusinessObject.Common;
using Laserbeam.BusinessObject.Common.MeritBusinussObjects;
using Laserbeam.Constant.HR;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Laserbeam.ProcessManager.Interfaces.Common
{
    public interface ICompensationProcessManager
    {
        Task<IEnumerable<int>> GetCompCompletedCount(int selectedManagerNum, int year, int loggedInUserNum);
        Task<int> UpdateApprovalStatus(List<SubmitReporteesModel> selectedRows, int loggedInEmployeeNum, int selectedManagerNum, MenuType MenuType, bool isRollup, ApprovalStatus approvalStatus, AppConfigModel appConfig, int userNum, string Comment);
        IQueryable<RatingModel> GetRatings();
        bool IsEmployeeDataEmpty();
        CompensationConfiguration GetReporteeConfiguration();
        Task<IEnumerable<DropDownListItems>> GetEmpSort();
        CompensationTypeConfiguration GetCompensationTypeConfiguration();
        void UpdateCompReportees(List<MeritGridModel> compensationReportees, int jopbYear, int userNum);                        
        Task<IEnumerable<EmployeeInfoDetails>> GetEmployeeInfo(int employeeNum, int loggedInUserNum);
        IQueryable<CommentModel> GetPromotionComment(int empJobNum);
        Task<IEnumerable<ManagerTree>> GetCompManagerTree(int managerNum, int loggedInEmployeeNum, int loggedInUserNum, int userNum, ViewPageType pageType);
        Task<IEnumerable<MeritGridModel>> GetCompReportees(int managerNum, int loggedInEmployeeNum, int loggedInUserNum, MenuType compMenuType, bool isRollup, int loggedSelectedUserNum);
        Task<IEnumerable<SubmitReporteesModel>> GetCompSubmitReportees(int managerNum, int loggedInEmployeeNum, int loggedInUserNum, MenuType compMenuType, bool isRollup, int loggedSelectedUserNum);
        Task<IEnumerable<SubmitReporteesModel>> GetCompApprovalReportees(int managerNum, int loggedInEmployeeNum, int loggedInUserNum, MenuType compMenuType, bool isRollup, int loggedSelectedUserNum);
        Task<IEnumerable<SubmitReporteesModel>> GetCompReopenReportees(int managerNum, int loggedInEmployeeNum, int loggedInUserNum, MenuType compMenuType, bool isRollup, int loggedSelectedUserNum);
        Task<IEnumerable<ExchangeCurrencies>> GetCurrencies();
        void UpdateEmployeeComment(int empJobNum,int userNum, int employeeCompCommentNum, string comments, string grade);
        Task<BudgetModel> GetBudgetData(int loggedInEmpNum, int employeeNum, int loggedInUserNum, MenuType compMenuType, string currencyCulture,int currencyCodeNum, bool isRollup, bool isSelectedRollup);
        
        // Author        :  Shaheena Shaik
        // Creation Date :  4-July-2017
        /// <summary>
        /// Returns the employeename
        /// </summary>
        /// <param name="selectedManagerNum">Denotes the selected manager num</param>
        Task<string> GetManagerName(int managerNum);
        Task RevertPromotion(int empJobNum, decimal newSalaryLocal, decimal newHrlyRate, decimal newCompRatio, decimal TCC);
        Task<int>GetApprovalStatus(int selectedManagerNum, int loggedInEmployeeNum);
        IQueryable<CommentModel> GetComments(int commentKey, CommentType commentType, int userNum);
        CompensationTypeConfiguration GetRuleConfiguration();
        void PutUpdateComments(CommentInputModel comment, CommentType type, bool isEditItem);
        Task DeleteComments(int commentKey);
        Task<IEnumerable<ApprovalEmployeeSearchData>> GetCompApprovalReporteesSearch(int managerNum, int loggedInUserNum, MenuType compMenuType, bool isRollup, string approvalType);
        string GetworkflowStatus();
        Task<IEnumerable<EmployeeInfoBasicDetails>> GetEmployeeBasicInfo(int employeeNum);
        bool IsInDirects(int managerNum);
    }
}
