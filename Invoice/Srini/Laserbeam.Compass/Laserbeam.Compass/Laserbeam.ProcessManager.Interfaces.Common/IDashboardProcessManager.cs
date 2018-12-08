// Copyright (c) 2015 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.

// Component Name :   IADashboardProcessManager
// Description    :   Interface signature for DashboardProcessManager
// Author         :   Arunraj C	
// Creation Date  :   10-May-2017
// Reviewed By    :   Hari 
// Reviewed Date  :   16-May-2017

using Laserbeam.BusinessObject.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Laserbeam.ProcessManager.Interfaces.Common
{
    public interface IDashboardProcessManager
    {
        Task<IEnumerable<DropDownListItems>> GetAllUsers(int loggedInUserNum);
        UserModel GetUserDetails(int userNum);
        UserModel GetUserInfo(string userId);
        //Task<IEnumerable<string>> GetInitiatives(int userNum);                
        List<EventsCommunicationModel> GetEventsForLoggedInUser(int userNum);
        Task<Tuple<int, List<MyApproval>>> GetManagerApproval(int empJobNum, int loggedInUserNum, int userNumSelected);
        CompensationTypeConfiguration GetRuleConfiguration();
        IQueryable<ExchangeCurrencies> GetCurrencies();
        List<DailyTaskModel> GetDailyTask(int UserNum);
        void AddDailyTask(DailyTaskModel dailyTask);
        Task<EmployeeSalaryDetails> GetEmployeeSalaryDetails(int userNum);
        DailyTaskModel GetEditDailyTask(int TaskID);
        void EditDailyTask(DailyTaskModel dailyTask);
        string DeleteDailyTask(int TaskID);
        List<ManagerBudgetDetails> GetManagerBudgetDetails(int userNum, int year);
        string SetDailyTaskComplete(int TaskID);
        Task<DataTable> GetExportMeritData(int EmployeeNum, int userNum);
        Task<DataTable> GetExportPromotionData(int EmployeeNum, int userNum);
        Task<DataTable> GetExportAdjustmentData(int EmployeeNum, int userNum);
        Task<int> UserLoginStatusCount(int userNum);
        Task<List<DailyTaskModel>> GetMyTaskApprovalList(int userNum);
        //Task<List<DailyTaskModel>> GetYetToSubmitList(int userNum);
        Task<List<DashboardApprovalData>> GetApprovalData(int userNum, string type);
        Task<IEnumerable<DashboardApprovalSearchData>> GetApprovalSearchData(int userNum, string type);
        Task<List<TeamApprovalStatus>> GetTeamApprovalData(int userNum, string type);
        Task<WorkFlowBudgetSpendCount> GetWorkFolwBudgetCount(int userNum);
        //Task<List<TeamApprovalStatus>> GetTeamStatusList(int userNum);
        Task<List<LevelWiseApprovalDetails>> GetLevelApprovalData(int managerNum, int level);
        Task<bool> GetYetToSubmitMailList(int managerNum, string messageSubject, string messageBody, AppConfigModel appConfig);
        Task<DataTable> GetExportWorkFlowData(int UserNum);
        int GetChatUnReadMessageCount(int UserNum);
        Task<DataTable> GetExportBudgetUtilizData(int UserNum);
        bool isWorkFlowEnable();
        string loggedInCultureCode(int userNum, int year);
    }
}
