// Copyright (c) 2016 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.
// Component Name  : 	Dashboard ProcessManager
// Description     : 	Contains Dashboard related logics	
// Author          :    Raja Ganapathy		
// Creation Date   :    20-06-2016
using Laserbeam.BusinessObject.Common;
using Laserbeam.BusinessObject.Common.CachedModels;
using Laserbeam.Constant.HR;
using Laserbeam.DataManager.Interfaces.Common;
using Laserbeam.EntityManager.Common;
using Laserbeam.Libraries.Interfaces.Common;
using Laserbeam.ProcessManager.Interfaces.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Laserbeam.ProcessManager.Common
{
    public class DashboardProcessManager : IDashboardProcessManager
    {
        #region Fields
        // Author         :   Raja Ganapathy
        // Creation Date  :   05-Jul-2016  
        /// <summary>
        /// Instance of DashboardRepository
        /// </summary>
        IDashboardRepository m_dashboardRepository;
        private readonly IEmail m_email;
        #endregion

        #region Constructors
        public DashboardProcessManager(IDashboardRepository DashboardRepository, IEmail email)
        {
            m_dashboardRepository = DashboardRepository;
            m_email = email;
        }
        #endregion

        #region Implements
        // Author        :  Raja Ganapathy		
        // Creation Date :  20-06-2016
        // Reviewed By    :  Srinivasan kamalakannan
        // Reviewed Date  :  31-08-2016  
        /// <summary>
        /// This method gets details of all employees with "Active" or "Leave of Absence" as jobstatus
        /// </summary>
        /// <returns>All active users</returns>
        public async Task<IEnumerable<DropDownListItems>> GetAllUsers(int loggedInUserNum)
        {
            return await m_dashboardRepository.GetAllUsers(loggedInUserNum);
        }

        public string loggedInCultureCode(int userNum, int year)
        {
            return m_dashboardRepository.loggedInCultureCode(userNum, year);
        }

        // Author        :  Raja Ganapathy		
        // Creation Date :  20-06-2016
        // Reviewed By    :  Srinivasan kamalakannan
        // Reviewed Date  :  31-08-2016  
        /// <summary>
        /// Gets the AppUser details of the logged-in user from the DB
        /// </summary>
        /// <param name="userNum">user num of the logged-in user or the user selected in the dropdown list </param>
        /// <returns>AppUser details</returns>
        public UserModel GetUserDetails(int userNum)
        {
            return m_dashboardRepository.GetUserDetails(userNum);
        }
        public UserModel GetUserInfo(string userId)
        {
            return m_dashboardRepository.GetUserInfo(userId);
        }
        

        // Author        :  Raja Ganapathy		
        // Creation Date :  20-06-2016
        // Reviewed By    :  Srinivasan kamalakannan
        // Reviewed Date  :  31-08-2016  
        /// <summary>
        /// Gets the initiative links for the logged-in user
        /// </summary>
        /// <returns>Initiatives Links</returns>
        /// Commented by Arunraj
        //public async Task<IEnumerable<string>> GetInitiatives(int userNum)
        //{           
        // return await m_dashboardRepository.GetAllInitiatives(userNum);         
        //}

        // Author        :  Raja Ganapathy		
        // Creation Date :  20-06-2016
        // Reviewed By    :  Srinivasan kamalakannan
        // Reviewed Date  :  31-08-2016  
        /// <summary>
        /// To get the events for logged user
        /// </summary>
        /// <param name="userNum"></param>
        /// <returns></returns>
        public List<EventsCommunicationModel> GetEventsForLoggedInUser(int userNum)
        {
            return m_dashboardRepository.GetEventsForLoggedInUser(userNum);

        }

        // Author         :   Revathy		
        // Creation Date  :   25-07-2016        
        // Reviewed By    :  Srinivasan kamalakannan
        // Reviewed Date  :  31-08-2016  
        /// <summary>
        /// To get the list of records to be approved/reopened and the total count of records
        /// </summary>
        /// <param name="empJobNum">Employee's job num</param>
        /// <param name="meritCycleYear">Merit cycle year</param>
        /// <param name="page">denotes, whether the request is from notification panel or grid</param>
        /// <returns>Tuple<int,List<MyApproval>></returns>
        public async Task<Tuple<int, List<MyApproval>>> GetManagerApproval(int empJobNum, int loggedInUserNum, int userNumSelected)
        {
            return await m_dashboardRepository.GetManagerApproval(empJobNum, loggedInUserNum, userNumSelected);
        }

        // Author        :  Raja Ganapathy		
        // Creation Date :  20-06-2016
        /// <summary>
        /// To get business configuration for compensation
        /// </summary>
        /// <returns>CompensationTypeConfiguration object</returns>
        public CompensationTypeConfiguration GetRuleConfiguration()
        {
            CompensationTypeConfiguration ruleConfiguration = new CompensationTypeConfiguration();
            BusinessSettingModel busSettings = m_dashboardRepository.GetRuleConfiguration();
            ruleConfiguration.FeatureConfigurationMerit = busSettings.FeatureConfiguration.Merit;
            ruleConfiguration.FeatureConfigurationAdjustment = busSettings.FeatureConfiguration.Adjustment;
            ruleConfiguration.FeatureConfigurationLumpSum = busSettings.FeatureConfiguration.Lumpsum;
            ruleConfiguration.FeatureConfigurationBonus = busSettings.FeatureConfiguration.Bonus;
            ruleConfiguration.FeatureConfigurationPromotion = busSettings.FeatureConfiguration.Promotion;
            ruleConfiguration.FeatureConfigurationLTIP = busSettings.FeatureConfiguration.LTIP;
            ruleConfiguration.DateFormat = busSettings.DateConfiguration.DateFormat;
            ruleConfiguration.BudgetCurrencyFormat = busSettings.CurrencyConfiguration.CurrencyFormat;
            ruleConfiguration.FeatureConfigurationCurrencyCodeDisplay = busSettings.FeatureConfiguration.CurrencyCode;
            ruleConfiguration.MeritValuesReCalculate = busSettings.LumpsumRule.MeritValuesReCalculate;
            return ruleConfiguration;
        }

        public bool isWorkFlowEnable()
        {
            return m_dashboardRepository.isWorkFlowEnable();
        }

        // Author         :  Raja Ganapathy
        // Creation Date  :  10-Oct-2016  
        // Reviewed By    :  Hari
        // Reviewed Date  :  10-Oct-2016 
        /// <summary>
        /// To get the currency type for budget grid
        /// </summary>
        /// <returns>Returns the Currency exchange</returns>
        public IQueryable<ExchangeCurrencies> GetCurrencies()
        {
            return m_dashboardRepository.GetCurrencies().Select(x => new ExchangeCurrencies
            {
                CurrencyCode = x.CurrencyCode,
                CurrencyNum = x.CurrencyCodeNum,
                CultureCode = x.CultureCode == null ? "en-US" : x.CultureCode,
                ExchangeRate = x.ExchangeRates.Select(y => y.MeritExchangeRate).FirstOrDefault() == null ? 1 : x.ExchangeRates.Select(y => y.MeritExchangeRate).FirstOrDefault()
            }).OrderBy(x => x.CurrencyCode);
        }
        #endregion

        #region PrivateMethods

        private bool setTrueorFalse(string value)
        {
            if (value != null && value.ToLower().Trim() == "yes")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // Author         :  Arunraj C
        // Creation Date  :  05-May-2017  
        // Reviewed By    :  
        // Reviewed Date  :  
        /// <summary>
        /// To get the daily task and return
        /// </summary>
        public List<DailyTaskModel> GetDailyTask(int UserNum)
        {
            return m_dashboardRepository.GetDailyTask(UserNum).Select(x => new DailyTaskModel
            {
                TaskNum = x.TaskNum,
                UserNum = x.UserNum,
                TaskTitle = x.TaskTitle,
                TaskDescr = x.TaskDescr,
                CreatedDate = x.CreatedDate,
                TaskCompleted = x.TaskCompleted,
                UpdatedDate = x.UpdatedDate,
                Active = x.Active
            }).ToList();
        }

        // Author         :  Arunraj C
        // Creation Date  :  05-May-2017  
        // Reviewed By    :  
        // Reviewed Date  :  
        /// <summary>
        /// To add the daily task
        /// </summary>
        public void AddDailyTask(DailyTaskModel dailyTask)
        {
            m_dashboardRepository.AddDailyTask(dailyTask);
        }

        // Author         :  Arunraj C
        // Creation Date  :  05-May-2017  
        // Reviewed By    :  
        // Reviewed Date  :  
        /// <summary>
        /// To get the total employee salary details based on budget
        /// </summary>
        public async Task<EmployeeSalaryDetails> GetEmployeeSalaryDetails(int userNum)
        {
            EmployeeSalaryDetails EmpSalaryDetails = new EmployeeSalaryDetails();
            var result = await m_dashboardRepository.GetEmployeeSalaryDetails(userNum);

            EmpSalaryDetails = result;
            EmpSalaryDetails.DirectBalance = (EmpSalaryDetails.DirectBudget - EmpSalaryDetails.DirectSpent);
            EmpSalaryDetails.IndirectBalance = (EmpSalaryDetails.IndirectBudget - EmpSalaryDetails.IndirectSpent);
            return EmpSalaryDetails;
        }

        // Author         :  Arunraj C
        // Creation Date  :  09-May-2017  
        // Reviewed By    :  
        // Reviewed Date  :  
        /// <summary>
        /// To get edit task details
        /// </summary>
        public DailyTaskModel GetEditDailyTask(int TaskID)
        {
            var getEditTaskDetails = m_dashboardRepository.GetEditDailyTask(TaskID);
            DailyTaskModel getTask = new DailyTaskModel();
            getTask.TaskNum = getEditTaskDetails.TaskNum;
            getTask.UserNum = getEditTaskDetails.UserNum;
            getTask.TaskTitle = getEditTaskDetails.TaskTitle;
            getTask.TaskDescr = getEditTaskDetails.TaskDescr;
            getTask.CreatedDate = getEditTaskDetails.CreatedDate;
            getTask.TaskCompleted = getEditTaskDetails.TaskCompleted;
            getTask.Active = getEditTaskDetails.Active;
            return getTask;
        }

        // Author         :  Arunraj C
        // Creation Date  :  09-May-2017  
        // Reviewed By    :  
        // Reviewed Date  :  
        /// <summary>
        /// To edit the daily task
        /// </summary>
        public void EditDailyTask(DailyTaskModel dailyTask)
        {
            m_dashboardRepository.EditDailyTask(dailyTask);
        }

        // Author         :  Arunraj C
        // Creation Date  :  09-May-2017  
        // Reviewed By    :  
        // Reviewed Date  :  
        /// <summary>
        /// To delete the daily task
        /// </summary>
        public string DeleteDailyTask(int TaskID)
        {
            return m_dashboardRepository.DeleteDailyTask(TaskID);
        }

        // Author         :  Arunraj C
        // Creation Date  :  09-May-2017  
        // Reviewed By    :  
        // Reviewed Date  :  
        /// <summary>
        /// To get the manager budget details baed on logged user
        /// </summary>
        public List<ManagerBudgetDetails> GetManagerBudgetDetails(int userNum, int year)
        {
            return m_dashboardRepository.GetManagerBudgetDetails(userNum, year);
        }

        // Author         :  Arunraj C
        // Creation Date  :  11-May-2017  
        // Reviewed By    :  
        // Reviewed Date  :  
        /// <summary>
        /// To set the task is completed
        /// </summary>
        public string SetDailyTaskComplete(int TaskID)
        {
            return m_dashboardRepository.SetDailyTaskComplete(TaskID);
        }

        // Author         :  Arunraj C
        // Creation Date  :  12-May-2017  
        // Reviewed By    :  
        // Reviewed Date  :  
        /// <summary>
        /// To get the Merit detailt to export
        /// </summary>
        public async Task<DataTable> GetExportMeritData(int EmployeeNum, int userNum)
        {
            return await m_dashboardRepository.GetExportMeritData(EmployeeNum, userNum);
        }

        // Author         :  Arunraj C
        // Creation Date  :  12-May-2017  
        // Reviewed By    :  
        // Reviewed Date  :  
        /// <summary>
        /// To get the Promotion details to export
        /// </summary>
        public async Task<DataTable> GetExportPromotionData(int EmployeeNum, int userNum)
        {
            return await m_dashboardRepository.GetExportPromotionData(EmployeeNum, userNum);
        }

        public async Task<DataTable> GetExportWorkFlowData(int UserNum)
        {
            return await m_dashboardRepository.GetExportWorkFlowData(UserNum);
        }
        public async Task<DataTable> GetExportBudgetUtilizData(int UserNum)
        {
            return await m_dashboardRepository.GetExportBudgetUtilizData(UserNum);
        }
        // Author         :  Arunraj C
        // Creation Date  :  12-May-2017  
        // Reviewed By    :  
        // Reviewed Date  :  
        /// <summary>
        /// To get the Adjustment details to export
        /// </summary>
        public async Task<DataTable> GetExportAdjustmentData(int EmployeeNum, int userNum)
        {
            return await m_dashboardRepository.GetExportAdjustmentData(EmployeeNum, userNum);
        }

        // Author         :  Arunraj C
        // Creation Date  :  12-May-2017  
        // Reviewed By    :  
        // Reviewed Date  :  
        /// <summary>
        /// To get the User count whoever loggedin
        /// </summary>
        public async Task<int> UserLoginStatusCount(int userNum)
        {
            return await m_dashboardRepository.UserLoginStatusCount(userNum);
        }

        public async Task<List<DailyTaskModel>> GetMyTaskApprovalList(int userNum)
        {
            return await m_dashboardRepository.GetMyTaskApprovalList(userNum);
        }

        //public async Task<List<DailyTaskModel>> GetYetToSubmitList(int userNum)
        //{
        //    return await m_dashboardRepository.GetYetToSubmitList(userNum);
        //}

        public async Task<List<DashboardApprovalData>> GetApprovalData(int userNum, string type)
        {
            return await m_dashboardRepository.GetApprovalData(userNum, type);
        }

        public async Task<IEnumerable<DashboardApprovalSearchData>> GetApprovalSearchData(int userNum, string type)
        {
            return await m_dashboardRepository.GetApprovalSearchData(userNum, type);
        }

        public async Task<List<TeamApprovalStatus>> GetTeamApprovalData(int userNum, string type)
        {
            return await m_dashboardRepository.GetTeamApprovalData(userNum, type);
        }

        public async Task<WorkFlowBudgetSpendCount> GetWorkFolwBudgetCount(int userNum)
        {
            return await m_dashboardRepository.GetWorkFolwBudgetCount(userNum);
        }

        //public async Task<List<TeamApprovalStatus>> GetTeamStatusList(int userNum)
        //{
        //    return await m_dashboardRepository.GetTeamStatusList(userNum);
        //}

        public async Task<List<LevelWiseApprovalDetails>> GetLevelApprovalData(int managerNum, int level)
        {
            return await m_dashboardRepository.GetLevelApprovalData(managerNum, level);
        }

        public async Task<bool> GetYetToSubmitMailList(int managerNum, string messageSubject, string messageBody, AppConfigModel appConfig)
        {
            var result = await m_dashboardRepository.GetYetToSubmitMailList(managerNum);
            foreach (var item in result)
            {
                EmailDetails email = new EmailDetails();
                email.EmailSubject = messageSubject;
                email.ToEmailID = item.EmailAddress;
                email.EmailBody = messageBody + "<br/>" + item.EmployeeName;
                var emailStatus = m_email.SendEmail(appConfig, email, true);
            }
            return true;
        }
        #endregion


        // Author         :  Bala Murugan
        // Creation Date  : 06-July-2017
        // Reviewed By    :  
        // Reviewed Date  :  
        /// <summary>
        /// Get the Unread Mesage Count For User
        /// </summary>
        /// <returns>UnreadMeaageCount</returns>
        public int GetChatUnReadMessageCount(int UserNum)
        {
            return m_dashboardRepository.GetChatUnReadMessageCount(UserNum);
        }
    }
}
