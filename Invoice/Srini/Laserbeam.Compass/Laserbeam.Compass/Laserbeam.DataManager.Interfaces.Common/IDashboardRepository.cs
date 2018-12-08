// Copyright (c) 2015 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.

// Component Name :   IDashboardRepository 
// Description    :   Interface signature for DashboardRepository
// Author         :   Arunraj C	
// Creation Date  :   10-May-2017
// Reviewed By    :   Hari 
// Reviewed Date  :   16-May-2017

using Laserbeam.BusinessObject.Common;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;
using Laserbeam.EntityManager.Common;
using System.Data;
using Laserbeam.BusinessObject.Common.CachedModels;

namespace Laserbeam.DataManager.Interfaces.Common
{
    public interface IDashboardRepository
    {
        Task<IEnumerable<DropDownListItems>> GetAllUsers(int loggedInUserNum);
        UserModel GetUserDetails(int userNum);
        UserModel GetUserInfo(string userId);
        
        //Task<IEnumerable<string>> GetAllInitiatives(int userNum);                   
        List<EventsCommunicationModel> GetEventsForLoggedInUser(int userNum);
        Task<Tuple<int, List<MyApproval>>> GetManagerApproval(int empJobNum, int loggedInUserNum, int userNumSelected);
        BusinessSettingModel GetRuleConfiguration();
        IQueryable<Currency> GetCurrencies();
        List<DailyTask> GetDailyTask(int UserNum);
        void AddDailyTask(DailyTaskModel dailyTask);
        Task<EmployeeSalaryDetails> GetEmployeeSalaryDetails(int userNum);
        DailyTask GetEditDailyTask(int TaskID);
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
        Task<List<TeamApprovalStatus>> GetYetToSubmitMailList(int managerNum);
        Task<DataTable> GetExportWorkFlowData(int UserNum);
        int GetChatUnReadMessageCount(int UserNum);
        Task<DataTable> GetExportBudgetUtilizData(int UserNum);
        bool isWorkFlowEnable();
        string loggedInCultureCode(int userNum, int year);
    }
}
