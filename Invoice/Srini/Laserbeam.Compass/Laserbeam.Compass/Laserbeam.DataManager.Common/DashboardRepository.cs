// Copyright (c) 2016 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.
// Component Name  :  DashboardRepository
// Description     :  This page is used to get application configuration and details of Employee logged in from database.
// Author          :  Raja Ganapathy
// Creation Date   :  30-08-2016     

using Laserbeam.BusinessObject.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using Laserbeam.DataManager.Interfaces.Core;
using System.Threading.Tasks;
using Laserbeam.EntityManager.Common;
using Laserbeam.DataManager.Interfaces.Common;
using System.Data;
using Laserbeam.Libraries.Core.Interfaces.Common;
using Laserbeam.BusinessObject.Common.CachedModels;

namespace Laserbeam.DataManager.Common
{
    public class DashboardRepository : IDashboardRepository
    {
        #region Fields
        // Author         :  Raja Ganapathy
        // Creation Date  :  05-Jul-2016        
        /// <summary>
        /// Object of Base Repository
        /// </summary>
        private IBaseRepository m_baseRepository;
        private ITenantCacheProvider m_tenantCacheProvider;

        #endregion

        #region Constructors
        // Author         :  Raja Ganapathy
        // Creation Date  :  05-Jul-2016  
        /// <summary>
        /// Initializes objects used in this class
        /// </summary>
        /// <param name="baseRepository">Base Repository Object</param>
        public DashboardRepository(IBaseRepository baseRepository, ITenantCacheProvider tenantCacheProvider)
        {
            m_baseRepository = baseRepository;
            m_tenantCacheProvider = tenantCacheProvider;
        }
        #endregion


        #region Implements

        // Author         :  Raja Ganapathy
        // Creation Date  :  05-Jul-2016  
        // Reviewed By    :  Srinivasan kamalakannan
        // Reviewed Date  :  31-08-2016  
        /// <summary>
        /// This method gets details of all employees who has a user information and their jobstatus is "Active" or "Leave of Absence"
        /// </summary>
        /// <returns>AppUser details</returns>
        public async Task<IEnumerable<DropDownListItems>> GetAllUsers(int loggedInUserNum)
        {
            var AppUsers = await m_baseRepository.GetData<DropDownListItems>("[Talent].[USP_DSB_GET_AllUsers] @LoggedinUserNum", new SqlParameter[] { new SqlParameter("@LoggedinUserNum", loggedInUserNum) });
            return AppUsers;
        }

        // Author         :  Raja Ganapathy
        // Creation Date  :  05-Jul-2016  
        // Reviewed By    :  Srinivasan kamalakannan
        // Reviewed Date  :  31-08-2016  
        /// <summary>
        /// Gets the AppUser details of the logged-in user
        /// </summary>
        /// <param name="userNum">user num of the logged-in user or the user selected in the dropdown list </param>
        /// <returns>AppUser details</returns>
        public UserModel GetUserDetails(int userNum)
        {
            return m_baseRepository.GetQuery<AppUser>(new string[] { "AppUserRole" }).Where(x => x.UserNum == userNum)
                .Select(m => new UserModel
                {
                    AppUserStatusID = m.AppUserStatusID,
                    DefaultPassword = m.DefaultPassword,
                    EmailID = m.EmailID,
                    EmployeeNum = m.EmployeeNum ?? 0,
                    FailedLoginAttempts = m.FailedLoginAttempts,
                    LastLoginDt = m.LastLoginDt,
                    UserID = m.UserID,
                    UserName = m.UserName,
                    UserNum = m.UserNum,
                    UserPassword = m.UserPassword,
                    UserRoleNum = m.UserRoleNum,
                    UserRole=m.AppUserRole.UserRole,
                    SecretKey = m.SecretKey
                }).FirstOrDefault();

        }
      
        public UserModel GetUserInfo(string userId)
        {
            return m_baseRepository.GetQuery<AppUser>(new string[] { "AppUserRole" }).Where(x => x.UserID == userId)
                .Select(m => new UserModel
                {
                    AppUserStatusID = m.AppUserStatusID,
                    DefaultPassword = m.DefaultPassword,
                    EmailID = m.EmailID,
                    EmployeeNum = m.EmployeeNum ?? 0,
                    FailedLoginAttempts = m.FailedLoginAttempts,
                    LastLoginDt = m.LastLoginDt,
                    UserID = m.UserID,
                    UserName = m.UserName,
                    UserNum = m.UserNum,
                    UserPassword = m.UserPassword,
                    UserRoleNum = m.UserRoleNum,
                    UserRole = m.AppUserRole.UserRole,
                    SecretKey = m.SecretKey
                }).FirstOrDefault();

        }
        // Author         :  Raja Ganapathy
        // Creation Date  :  05-Jul-2016  
        // Reviewed By    :  Srinivasan kamalakannan
        // Reviewed Date  :  31-08-2016  
        /// <summary>
        ///  This method gets the initiative links for the logged-in user 
        /// </summary>
        /// <param name="userNum">user num of the logged-in user or the user selected in the dropdown list </param>
        /// <returns>InitiativesLink</returns>
        /// Commented by Aruraj
        //public async Task<IEnumerable<string>> GetAllInitiatives(int userNum)
        //{
        //    var initiative = await m_baseRepository.GetData<string>("TALENT.USP_GET_UserInitativeLinks @LoggedSelectedUserNum", new SqlParameter[] { new SqlParameter("@LoggedSelectedUserNum", userNum) });
        //    return initiative;
        //}


        // Author         :  Raja Ganapathy
        // Creation Date  :  05-Jul-2016  
        // Reviewed By    :  Srinivasan kamalakannan
        // Reviewed Date  :  31-08-2016  
        /// <summary>
        /// This method gets the events for the logged-in user when the user.
        /// </summary>
        /// <param name="userNum">user num of the logged-in user or the user selected in the dropdown list </param>
        /// <returns>Returns the user events</returns>
        public List<EventsCommunicationModel> GetEventsForLoggedInUser(int userNum)
        {
            string DateFormat = GetRuleConfiguration().DateConfiguration.DateFormat;
            var userMessage = m_baseRepository.GetQuery<UserMessage>(new string[] { "AppMessage", "AppUser" }, x => x.UserNum == userNum);
            var defaultMessage = m_baseRepository.GetQuery<AppMessage>().Where(x => x.MessageSubject == "Welcome to Compass Cloud").FirstOrDefault();
            var userName = m_baseRepository.GetQuery<AppUser>().Where(x => x.UserNum == defaultMessage.UpdatedBy).Select(x => x.UserName).FirstOrDefault();
            List<EventsCommunicationModel> messages = userMessage.Select(
                                e => new EventsCommunicationModel
                                {
                                    EmailSubject = e.AppMessage.MessageSubject,
                                    EmailBody = e.AppMessage.MessageBody,
                                    UpdatedBy = e.AppUser.UserName,
                                    UpdatedDate = e.UpdatedDate,
                                    DateFormat = DateFormat
                                }).ToList();
            messages.Add(new EventsCommunicationModel { EmailSubject = defaultMessage.MessageSubject, EmailBody = defaultMessage.MessageBody, UpdatedBy = userName, UpdatedDate = defaultMessage.UpdatedDate, DateFormat = DateFormat });
            return messages.OrderBy(x => x.UpdatedDate).ToList();
        }

        // Author         :   Revathy		
        // Creation Date  :   25-07-2016
        // Reviewed By    :  Srinivasan kamalakannan
        // Reviewed Date  :  31-08-2016  
        /// <summary>
        /// To get the list of records to be approved/reopened and the count of records
        /// </summary>
        /// <param name="empJobNum">Employee's job num</param>
        /// <param name="meritCycleYear">Merit cycle year</param>
        /// <param name="page">denotes, whether the request is from notification panel or grid</param>
        /// <returns>Tuple<int,List<MyApproval>></returns>
        public async Task<Tuple<int, List<MyApproval>>> GetManagerApproval(int empNum, int loggedInUserNum, int userNumSelected)
        {
            SqlParameter[] sqlParameter = {   new SqlParameter("@LoggedEmployeeNum", empNum),
                                              new SqlParameter("@loggedSelectedUserNum",userNumSelected),
                                              new SqlParameter("@loggedInUserNum",loggedInUserNum)

                                          };
            var myApproval = await m_baseRepository.GetData<MyApproval>("[Common].[USP_WP_GET_ApprovalFlowByManager] @LoggedEmployeeNum,@loggedSelectedUserNum,@loggedInUserNum", sqlParameter);
            return new Tuple<int, List<MyApproval>>(myApproval.Count(), myApproval.ToList());
        }

        // Author         :  Raja Ganapathy
        // Creation Date  :  30-Sep-2016  
        // Reviewed By    :  Hari
        // Reviewed Date  :  30-Sep-2016  
        /// <summary>
        /// Get the configuration details of the compensation
        /// </summary>
        /// <returns>CompensationTypeConfiguration</returns>
        public BusinessSettingModel GetRuleConfiguration()
        {
            return m_tenantCacheProvider.GetBusinessSetting();
        }

        public bool isWorkFlowEnable()
        {
            // return m_baseRepository.GetQuery<BusSetting>().Where(busSetting => busSetting.KeyId == "FeatureConfiguration" && busSetting.KeyValue == "WorkFlow").Select(s => s.KeyDataValue).FirstOrDefault() == "YES" ? true : false;
            return m_tenantCacheProvider.GetBusinessSetting().FeatureConfiguration.WorkFlow;
        }

        // Author         :  Raja Ganapathy
        // Creation Date  :  10-Oct-2016  
        // Reviewed By    :  Hari
        // Reviewed Date  :  10-Oct-2016   
        /// <summary>
        /// To get the currency type for budget grid
        /// </summary>
        /// <returns>Returns the Currency exchange</returns>
        public IQueryable<Currency> GetCurrencies()
        {
            return m_baseRepository.GetQuery<Currency>();
        }

        // Author         :  Arunraj C
        // Creation Date  :  04-May-2017  
        // Reviewed By    :  
        // Reviewed Date  :  
        /// <summary>
        /// To get the daily taks details
        /// </summary>        
        public List<DailyTask> GetDailyTask(int UserNum)
        {
            var returnTaskList = m_baseRepository.GetQuery<DailyTask>().Where(x => x.UserNum == UserNum && x.Active == true).OrderByDescending(x => x.CreatedDate).ToList();
            return returnTaskList;
        }

        // Author         :  Arunraj C
        // Creation Date  :  04-May-2017  
        // Reviewed By    :  
        // Reviewed Date  :  
        /// <summary>
        /// To get the daily taks details
        /// </summary>        
        public void AddDailyTask(DailyTaskModel dailyTask)
        {
            DailyTask addDailyTask = new DailyTask();
            addDailyTask.UserNum = dailyTask.UserNum;
            addDailyTask.TaskTitle = dailyTask.TaskTitle;
            addDailyTask.TaskDescr = dailyTask.TaskDescr;
            addDailyTask.CreatedDate = dailyTask.CreatedDate;
            addDailyTask.UpdatedDate = dailyTask.UpdatedDate;
            addDailyTask.Active = dailyTask.Active;
            m_baseRepository.Add<DailyTask>(addDailyTask);
            m_baseRepository.SaveChanges();
        }

        // Author         :  Arunraj C
        // Creation Date  :  08-May-2017  
        // Reviewed By    :  
        // Reviewed Date  :  
        /// <summary>
        /// To get the employee salary details from the budget details
        /// </summary>        
        public async Task<EmployeeSalaryDetails> GetEmployeeSalaryDetails(int userNum)
        {
            //int EmpNum = m_baseRepository.GetQuery<AppUser>().Where(e => e.UserNum == userNum).Select(e => e.EmployeeNum).FirstOrDefault().Value;
            SqlParameter[] sqlParamenter = { new SqlParameter("@UserNum", userNum) };
            var returnSalaryList = await m_baseRepository.GetData<EmployeeSalaryDetails>("[Talent].[USP_DSB_GET_EmployeeSalaryDetails] @UserNum", sqlParamenter);
            if (returnSalaryList == null || returnSalaryList.Count() == 0)
            {
                EmployeeSalaryDetails EmpSalaryDetails = new EmployeeSalaryDetails();
                EmpSalaryDetails.DirectBalance = 0;
                EmpSalaryDetails.IndirectBalance = 0;
                EmpSalaryDetails.DirectBudget = 0;
                EmpSalaryDetails.DirectSpent = 0;
                EmpSalaryDetails.IndirectBudget = 0;
                EmpSalaryDetails.IndirectSpent = 0;
                EmpSalaryDetails.DirectMerit = 0;
                EmpSalaryDetails.IndirectMerit = 0;
                EmpSalaryDetails.DirectPromotion = 0;
                EmpSalaryDetails.IndirectPromotion = 0;
                EmpSalaryDetails.DirectAdjustment = 0;
                EmpSalaryDetails.IndirectAdjustment = 0;
                EmpSalaryDetails.MeritEnable = "YES";
                EmpSalaryDetails.PromotionEnable = "YES";
                EmpSalaryDetails.AdjustmentEnable = "YES";
                return EmpSalaryDetails;
            }
            else
                return returnSalaryList.FirstOrDefault();
        }

        // Author         :  Arunraj C
        // Creation Date  :  09-May-2017  
        // Reviewed By    :  
        // Reviewed Date  :  
        /// <summary>
        /// To get edit task details
        /// </summary>
        public DailyTask GetEditDailyTask(int TaskID)
        {
            var returnTaskList = m_baseRepository.GetQuery<DailyTask>().Where(x => x.TaskNum == TaskID).FirstOrDefault();
            return returnTaskList;
        }

        // Author         :  Arunraj C
        // Creation Date  :  09-May-2017  
        // Reviewed By    :  
        // Reviewed Date  :  
        /// <summary>
        /// To get the daily taks details
        /// </summary>        
        public void EditDailyTask(DailyTaskModel dailyTask)
        {
            DailyTask editDailyTask = new DailyTask();
            var getEditTaskList = m_baseRepository.GetQuery<DailyTask>().Where(x => x.TaskNum == dailyTask.TaskNum).FirstOrDefault();
            getEditTaskList.TaskNum = dailyTask.TaskNum;
            getEditTaskList.TaskDescr = dailyTask.TaskDescr;
            getEditTaskList.UpdatedDate = dailyTask.UpdatedDate;
            m_baseRepository.Edit<DailyTask>(getEditTaskList);
            m_baseRepository.SaveChanges();
        }

        // Author         :  Arunraj C
        // Creation Date  :  09-May-2017  
        // Reviewed By    :  
        // Reviewed Date  :  
        /// <summary>
        /// To delete the daily taks details
        /// </summary>        
        public string DeleteDailyTask(int TaskID)
        {
            var getDeleteTaskList = m_baseRepository.GetQuery<DailyTask>().Where(x => x.TaskNum == TaskID).FirstOrDefault();
            getDeleteTaskList.Active = false;
            m_baseRepository.Edit<DailyTask>(getDeleteTaskList);
            m_baseRepository.SaveChanges();
            return "Success";
        }

        public string loggedInCultureCode(int userNum, int year)
        {
            string cultureCode = "en-US";
            //var selectedCurrency = m_baseRepository.GetQuery<BusSetting>().Where(x => x.KeyValue == "CurrencyFormat" && x.KeyId == "CurrencyConfiguration").Select(x => x.KeyDataValue).FirstOrDefault() ?? "";
            var selectedCurrency = m_tenantCacheProvider.GetBusinessSetting().CurrencyConfiguration.CurrencyFormat ?? "";
            if (selectedCurrency == "UserCurrency")
            {
                cultureCode = (from a in m_baseRepository.GetQuery<AppUser>()
                                         join ej in m_baseRepository.GetQuery<EmployeeJob>() on a.EmployeeNum equals ej.EmployeeNum
                                         join c in m_baseRepository.GetQuery<Currency>() on ej.CurrencyCodeNum equals c.CurrencyCodeNum
                                         where a.UserNum == userNum && ej.JobYear == year
                                         select c.CultureCode).FirstOrDefault() ?? "en-US";
            }
            else { cultureCode = ""; }
            return cultureCode;
        }

        // Author         :  Arunraj C
        // Creation Date  :  09-May-2017  
        // Reviewed By    :  
        // Reviewed Date  :  
        /// <summary>
        /// To get the manager budget details based on the logged users
        /// </summary>        
        public List<ManagerBudgetDetails> GetManagerBudgetDetails(int userNum, int year)
        {
            decimal loggedInMeritExchange = 1;
            //var selectedCurrency = m_baseRepository.GetQuery<BusSetting>().Where(x => x.KeyValue == "CurrencyFormat" && x.KeyId == "CurrencyConfiguration").Select(x => x.KeyDataValue).FirstOrDefault()??"";
            var selectedCurrency = m_tenantCacheProvider.GetBusinessSetting().CurrencyConfiguration.CurrencyFormat ?? "";
            if (selectedCurrency== "UserCurrency")
            {
                loggedInMeritExchange = (from a in m_baseRepository.GetQuery<AppUser>()
                            join ej in m_baseRepository.GetQuery<EmployeeJob>() on a.EmployeeNum equals ej.EmployeeNum
                            join e in m_baseRepository.GetQuery<ExchangeRate>() on ej.CurrencyCodeNum equals e.CurrencyCodeNum
                            where a.UserNum == userNum && ej.JobYear == year
                            select e.MeritExchangeRate).FirstOrDefault() ?? Convert.ToDecimal(1);
            }
            else
            {
                var baseCurrency= m_tenantCacheProvider.GetApplicationSetting().BaseCurrency;
                var exchangeRate = (from c in m_baseRepository.GetQuery<Currency>() 
                                    join mer in m_baseRepository.GetQuery<ExchangeRate>() on c.CurrencyCodeNum equals mer.CurrencyCodeNum
                                    where c.CurrencyCode == baseCurrency
                                    select new
                                    {
                                        MeritExchangeRate = mer.MeritExchangeRate
                                    }).Select(x => x.MeritExchangeRate).FirstOrDefault();
                loggedInMeritExchange = Convert.ToDecimal(exchangeRate);
            }
            int EmpNum = m_baseRepository.GetQuery<AppUser>().Where(e => e.UserNum == userNum).Select(e => e.EmployeeNum).FirstOrDefault().Value;
            int node = m_baseRepository.GetQuery<ManagerRelation>(m => m.EmployeeNum == EmpNum).FirstOrDefault().Node;
            var GetManagerBudget = (from mr in m_baseRepository.GetQuery<ManagerRelation>()
                                    join e in m_baseRepository.GetQuery<Employee>() on mr.EmployeeNum equals e.EmployeeNum
                                    join ej in m_baseRepository.GetQuery<EmployeeJob>() on mr.EmployeeNum equals ej.EmployeeNum
                                    join man in m_baseRepository.GetQuery<Employee>() on ej.ManagerNum equals man.EmployeeNum
                                    join b in m_baseRepository.GetQuery<Budget>() on ej.EmpJobNum equals b.EmpJobNum
                                    into buget
                                    from b in buget.DefaultIfEmpty()
                                    join c in m_baseRepository.GetQuery<ExchangeRate>() on ej.CurrencyCodeNum equals c.CurrencyCodeNum
                                    into exchangeRate
                                    from c in exchangeRate.DefaultIfEmpty()
                                    join cm in m_baseRepository.GetQuery<EmployeeCompMerit>() on ej.EmpJobNum equals cm.EmpJobNum into compMerit
                                    from cm in compMerit.DefaultIfEmpty()
                                    join cp in m_baseRepository.GetQuery<EmployeeCompPromotion>() on ej.EmpJobNum equals cp.EmpJobNum into compPromotion
                                    from cp in compPromotion.DefaultIfEmpty()
                                    where ej.JobYear == year && mr.Lineage.Contains("/" + node + "/") && ej.JobStatus !="T" && ej.JobStatus != "I" && ej.JobStatus != "N" && (cm.Eligibility == true || cp.Eligibility==true)
                                    select new 
                                    {
                                        EmployeeName = e.EmployeeName,
                                        EmployeeNum = e.EmployeeNum,
                                        ManagerNum = man.EmployeeNum,
                                        ManagerName = man.EmployeeName,
                                        ManagerBudgetPct = (b != null) ? (Decimal)(b.BudgetPct == null ? 0 : b.BudgetPct) : 0,
                                        ManagerBudget = (b != null) ? ((cm.Eligibility == true)?(Decimal)(b.AdjustedBudget == null ? 0 : b.AdjustedBudget*((c.MeritExchangeRate!=null && c.MeritExchangeRate!=0)?(1/c.MeritExchangeRate):1)* loggedInMeritExchange):0): 0,
                                        ManagerSpent = (b != null) ? ((cm.Eligibility == true) ? ((Decimal)(b.Spent == null ? 0 : b.Spent * ((c.MeritExchangeRate != null && c.MeritExchangeRate != 0) ? (1 / c.MeritExchangeRate) : 1) * loggedInMeritExchange) + (int)(b.LumpSumSpent == null ? 0 : b.LumpSumSpent * ((c.MeritExchangeRate != null && c.MeritExchangeRate != 0) ? (1 / c.MeritExchangeRate) : 1) * loggedInMeritExchange)) : 0 )+ ((cp.Eligibility == true)?(int)(b.PromotionSpent == null ? 0 : b.PromotionSpent * ((c.MeritExchangeRate != null && c.MeritExchangeRate != 0) ? (1 / c.MeritExchangeRate) : 1) * loggedInMeritExchange):0 ): 0,
                                        ManagerBalance = (b != null) ? (Decimal)(((cm.Eligibility == true)?(b.AdjustedBudget == null ? 0 : b.AdjustedBudget):0 * ((c.MeritExchangeRate != null && c.MeritExchangeRate != 0) ? (1 / c.MeritExchangeRate) : 1) * loggedInMeritExchange) - ((((cm.Eligibility == true)?(b.Spent == null ? 0 : b.Spent):0 * ((c.MeritExchangeRate != null && c.MeritExchangeRate != 0) ? (1 / c.MeritExchangeRate) : 1) * loggedInMeritExchange) + ((cm.Eligibility == true)?(b.LumpSumSpent == null ? 0 : b.LumpSumSpent):0 * ((c.MeritExchangeRate != null && c.MeritExchangeRate != 0) ? (1 / c.MeritExchangeRate) : 1) * loggedInMeritExchange) )+ ((cp.Eligibility == true)?(b.PromotionSpent == null ? 0 : b.PromotionSpent):0 * ((c.MeritExchangeRate != null && c.MeritExchangeRate != 0) ? (1 / c.MeritExchangeRate) : 1) * loggedInMeritExchange))) : 0,
                                        AdjustedBudget = (b != null) ? (Decimal)(b.AdjustedBudget == null ? 0 : b.AdjustedBudget * ((c.MeritExchangeRate != null && c.MeritExchangeRate != 0) ? (1 / c.MeritExchangeRate) : 1) * loggedInMeritExchange) : 0
                                    });


            var managerBudget =
                               GetManagerBudget.GroupBy(n => new { n.ManagerNum, n.ManagerName })
                             .Select(g => new ManagerBudgetDetails()
                             {
                                 ManagerName = g.Key.ManagerName,
                                 ManagerNum = g.Key.ManagerNum,
                                 ManagerBudgetPct = g.Sum(x => x.ManagerBudgetPct) / g.Select(x => x.EmployeeNum).Count(),
                                 ManagerBudget = g.Sum(x => x.ManagerBudget),
                                 ManagerSpent = g.Sum(x => x.ManagerSpent),
                                 ManagerBalance = ((g.Sum(x => x.ManagerBudget)) - (g.Sum(x => x.ManagerSpent))) //Convert.ToInt32(g.Sum(x => x.ManagerBalance))
                             }).ToList();

            return managerBudget;

        }

        // Author         :  Arunraj C
        // Creation Date  :  09-May-2017  
        // Reviewed By    :  
        // Reviewed Date  :  
        /// <summary>
        /// To delete the daily taks details
        /// </summary>        
        public string SetDailyTaskComplete(int TaskID)
        {
            var getTaskList = m_baseRepository.GetQuery<DailyTask>().Where(x => x.TaskNum == TaskID).FirstOrDefault();
            getTaskList.TaskCompleted = true;
            getTaskList.TaskCompletedDate = DateTime.Now;
            m_baseRepository.Edit<DailyTask>(getTaskList);
            m_baseRepository.SaveChanges();
            return "Success";
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
            SqlParameter[] parameter = {
                                  new SqlParameter("@employeeNum", EmployeeNum),
                                  new SqlParameter("@userNum", userNum)
                            };
            return await m_baseRepository.GetDataTableFromStoredProcedure("Talent.USP_DSB_GET_MeritExportDetails", parameter);
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
            SqlParameter[] parameter = {
                                  new SqlParameter("@employeeNum", EmployeeNum),
                                  new SqlParameter("@userNum", userNum)
                            };
            var promotionExport = await m_baseRepository.GetDataTableFromStoredProcedure("Talent.USP_DSB_GET_PromotionExportDetails", parameter);
            return promotionExport;
        }

        public async Task<DataTable> GetExportWorkFlowData(int UserNum)
        {
            SqlParameter[] parameter = {
                                  new SqlParameter("@loggedInUserNum", UserNum)
                            };
            var workFlowExport = await m_baseRepository.GetDataTableFromStoredProcedure("Talent.DSB_GET_DashboardWorkFlowExport", parameter);
            return workFlowExport;
        }
        // Author         : Balamurugan M
        // Creation Date  :  28-jul-2017  
        // Reviewed By    :  
        // Reviewed Date  :  
        /// <summary>
        /// To get the BudgetUtilization details to export
        /// </summary>
        public async Task<DataTable> GetExportBudgetUtilizData(int UserNum)
        {
            SqlParameter[] parameter = {
                                  new SqlParameter("@loggedInUserNum", UserNum)
                            };
            var workFlowExport = await m_baseRepository.GetDataTableFromStoredProcedure("Talent.USP_DSB_GET_DashboardBudgetUtilizExport", parameter);
            return workFlowExport;
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
            SqlParameter[] parameter = {
                                  new SqlParameter("@employeeNum", EmployeeNum),
                                  new SqlParameter("@userNum", userNum)
                            };
            var adjustmentExport = await m_baseRepository.GetDataTableFromStoredProcedure("Talent.USP_DSB_GET_AdjustmentExportDetails", parameter);
            return adjustmentExport;
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
            SqlParameter[] sqlParameter = { new SqlParameter("@UserNum", userNum) };
            var userData = await m_baseRepository.GetData<AppUsers>("[Common].[USP_UA_GET_UserCountInfo] @UserNum", sqlParameter);
            return userData.Select(x => x.YetToStartCount).FirstOrDefault();
        }

        public async Task<List<DailyTaskModel>> GetMyTaskApprovalList(int userNum)
        {
            SqlParameter[] sqlParameter = { new SqlParameter("@loggedInUserNum", userNum) };
            var result = await m_baseRepository.GetData<DailyTaskModel>("[Talent].[USP_DSB_GET_ApprovalFlowDetails] @loggedInUserNum", sqlParameter);
            return result.ToList();
        }

        //public async Task<List<DailyTaskModel>> GetYetToSubmitList(int userNum)
        //{
        //    SqlParameter[] sqlParameter = { new SqlParameter("@loggedInUserNum", userNum) };
        //    var result = await m_baseRepository.GetData<DailyTaskModel>("[Talent].[USP_DSB_GET_YetToSubmitDetails] @loggedInUserNum", sqlParameter);
        //    return result.ToList();
        //}

        public async Task<List<DashboardApprovalData>> GetApprovalData(int userNum, string type)
        {
            SqlParameter[] sqlParameter = { new SqlParameter("@loggedInUserNum", userNum),
                                  new SqlParameter("@type", type) };
            var result = await m_baseRepository.GetData<DashboardApprovalData>("[Talent].[USP_DSB_GET_WorkFlowApprovalData] @loggedInUserNum,@type", sqlParameter);
            return result.OrderBy(x => x.ManagerName).ToList();
        }

        public async Task<IEnumerable<DashboardApprovalSearchData>> GetApprovalSearchData(int userNum, string type)
        {
            SqlParameter[] sqlParameter = { new SqlParameter("@loggedInUserNum", userNum),
                                  new SqlParameter("@type", type) };
            var result = await m_baseRepository.GetData<DashboardApprovalSearchData>("[Talent].[USP_DSB_GET_WorkFlowApprovalSearchData] @loggedInUserNum,@type", sqlParameter);
            return result;
        }

        public async Task<List<TeamApprovalStatus>> GetTeamApprovalData(int userNum, string type)
        {
            SqlParameter[] sqlParameter = { new SqlParameter("@loggedInUserNum", userNum),
                                  new SqlParameter("@type", type) };
            var result = await m_baseRepository.GetData<TeamApprovalStatus>("[Talent].[USP_DSB_GET_WorkFlowTeamApproval] @loggedInUserNum,@type", sqlParameter);
            return result.OrderBy(x => x.ManagerName).ToList();
        }        

        public async Task<WorkFlowBudgetSpendCount> GetWorkFolwBudgetCount(int userNum)
        {
            SqlParameter[] sqlParameter = { new SqlParameter("@loggedInUserNum", userNum) };
            var result = await m_baseRepository.GetData<WorkFlowBudgetSpendCount>("[Talent].[USP_DSB_WorkFlowBudgetCount] @loggedInUserNum", sqlParameter);
            return result.FirstOrDefault();
        }

        //public async Task<List<TeamApprovalStatus>> GetTeamStatusList(int userNum)
        //{
        //    SqlParameter[] sqlParameter = { new SqlParameter("@loggedInUserNum", userNum) };
        //    var result = await m_baseRepository.GetData<TeamApprovalStatus>("[Talent].[USP_DSB_GET_TeamApprovalStatus] @loggedInUserNum", sqlParameter);
        //    return result.ToList();
        //}

        public async Task<List<LevelWiseApprovalDetails>> GetLevelApprovalData(int managerNum, int level)
        {
            SqlParameter[] sqlParameter = { new SqlParameter("@managerNum", managerNum), new SqlParameter("@Level", level) };
            var result = await m_baseRepository.GetData<LevelWiseApprovalDetails>("[Talent].[USP_DSB_GET_SelectedLevelApprovalData] @managerNum,@Level", sqlParameter);
            return result.ToList();
        }

        public async Task<List<TeamApprovalStatus>> GetYetToSubmitMailList(int managerNum)
        {
            SqlParameter[] sqlParameter = { new SqlParameter("@selectedManagerNum", managerNum) };
            var result = await m_baseRepository.GetData<TeamApprovalStatus>("[Talent].[USP_DSB_GET_YetToSubmitEmployeeSendRemainderDetails] @selectedManagerNum", sqlParameter);
            return result.ToList();
        }

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
            return (from ChatDetails in m_baseRepository.GetQuery<ChatDetail>()
                    where ChatDetails.ReceiverUserNum == UserNum && ChatDetails.ViewedStatus == false
                    select ChatDetails).Count();
        }
        #endregion
    }
}

