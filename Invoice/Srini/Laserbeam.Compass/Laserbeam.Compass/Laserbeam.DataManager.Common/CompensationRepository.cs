// Copyright (c) 2016 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.
// Component Name  :  CompensationRepository
// Description     :  To fetch data related to compensation
// Author          :  Raja Ganapathy		
// Creation Date   :  22-06-2016
using Laserbeam.BusinessObject.Common;
using Laserbeam.BusinessObject.Common.CachedModels;
using Laserbeam.BusinessObject.Common.MeritBusinussObjects;
using Laserbeam.Constant.HR;
using Laserbeam.DataManager.Interfaces.Common;
using Laserbeam.DataManager.Interfaces.Core;
using Laserbeam.EntityManager.Common;
using Laserbeam.Libraries.Core.Interfaces.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
namespace Laserbeam.DataManager.Common
{
    public class CompensationRepository : ICompensationRepository
    {
        #region Fields
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
        public CompensationRepository(IBaseRepository baseRepository, ITenantCacheProvider tenantCacheProvider)            
        {
            m_baseRepository = baseRepository;
            m_tenantCacheProvider = tenantCacheProvider;
        }
        
        #endregion

        #region Public Methods

        // Author        :  Raja Ganapathy		
        // Creation Date :  22-08-2016
        // Reviewed By   :  Hari		
        // Reviewed Date :  13-10-2016
        /// <summary>
        /// To revert the promotion
        /// </summary>
        /// <param name="empJobNum">Denotes the employee job num</param>
        public async Task RevertPromotion(int empJobNum, decimal newSalaryLocal, decimal newHrlyRate, decimal newCompRatio, decimal TCC)
        {
            SqlParameter[] Parameter = {   new SqlParameter("@EmpJobNum", empJobNum),
                                              new SqlParameter("@NewSalaryLocal",newSalaryLocal),
                                              new SqlParameter("@NewHrlyRate",newHrlyRate),
                                              new SqlParameter("@NewCompRatio",newCompRatio),
                                              new SqlParameter("@TCC",TCC),
                                          };


            await m_baseRepository.ExecuteStoredProcedure("[Talent].[USP_Comp_PUT_RevertPromotion]", Parameter);
        }

        // Author        :  Raja Ganapathy		
        // Creation Date :  22-08-2016
        // Reviewed By   :  Hari		
        // Reviewed Date :  13-10-2016
        /// <summary>
        /// To update the employee comp approval status
        /// </summary>
        /// <param name="approvalStatus">Approval status</param>
        /// <param name="selectedManagerNum">Selected manager num</param>
        /// <returns>Returns the json result</returns>
        public async Task<int> UpdateApprovalStatus(List<SubmitReporteesModel> selectedRows, int loggedInEmployeeNum, int selectedManagerNum, MenuType MenuType, bool isRollup, ApprovalStatus approvalStatus, AppConfigModel appConfig, int userNum, string Comment)
        {            
            var selectedEmpJobNums = selectedRows.Select(x => x.EmpJobNum).ToList();
            var employeeJobNums = string.Join(",", selectedEmpJobNums);
            SqlParameter[] updateApprovalParameter = {   new SqlParameter("@LoggedEmployeeNum", loggedInEmployeeNum),
                                              new SqlParameter("@managerNum",selectedManagerNum),
                                              new SqlParameter("@UserNum",userNum),
                                              new SqlParameter("@ApprovalStatus",(int)approvalStatus),
                                              new SqlParameter("@empJobNum",employeeJobNums),                                              
                                              new SqlParameter("@ApprovalComment",Comment)                                           
                                          };

            var updatedCount = await m_baseRepository.GetData<int>("Talent.USP_Comp_PUT_UpdateApprovalStatus @managerNum,@LoggedEmployeeNum,@UserNum,@ApprovalStatus,@empJobNum,@ApprovalComment", updateApprovalParameter);
            return updatedCount.FirstOrDefault();
        }

        

        /// <summary>
        /// Get EmployeeNames 
        /// </summary>
        /// <param name="empJobNums">List of Employee JobNum</param>
        /// <returns>Return List of EmployeeName</returns>
        private List<string> GetEmployeeName(List<int> empJobNums)
        {
            return m_baseRepository.GetQuery<EmployeeJob>(new string[] { "Employee" }, x => empJobNums.Contains(x.EmpJobNum)).Select(x => x.Employee.EmployeeName).ToList();
        }


        // Author        :  Raja Ganapathy		
        // Creation Date :  22-08-2016
        // Reviewed By   :  Hari		
        // Reviewed Date :  13-10-2016
        /// <summary>
        /// To get the comp completed count
        /// </summary>
        /// <param name="selectedManagerNum">Denotes the selected manager num</param>
        /// <returns>Returns the comp completed count</returns>
        public async Task<IEnumerable<int>> GetCompCompletedCount(int selectedManagerNum, int year, int loggedInUserNum)
        {    
            SqlParameter[] sqlParameter = {   new SqlParameter("@JobYear", year),
                                              new SqlParameter("@SelectedManagerNum",selectedManagerNum),
                                              new SqlParameter("@LoggedInUserNum",loggedInUserNum),
                                              new SqlParameter("@Module","Compensation")
                                          };
            var compensationReportees = await m_baseRepository.GetData<CompStatus>("[Talent].[USP_Comp_GET_CompCompletedStatus] @JobYear,@SelectedManagerNum,@LoggedInUserNum,@Module", sqlParameter);
            var outPut = compensationReportees.FirstOrDefault();
            List<int> finalresult = new List<int>();
            finalresult.Add(outPut.CompCompletedCount);
            finalresult.Add(outPut.SpentStatus);
            return finalresult; 
        }             
        // Author        :  Raja Ganapathy		
        // Creation Date :  20-06-2016
        // Reviewed By   :  Hari		
        // Reviewed Date :  13-10-2016
        /// <summary>
        /// Return list of ratings for compensation
        /// </summary>
        /// <returns>A list of ratings</returns>
        public IQueryable<RatingModel> GetRatings()
        {
            return m_baseRepository.GetQuery<Rating>(rating => rating.RatingType == CompensationConstants.Compensation).Select(m => new RatingModel 
            {
                RatingNum = m.RatingNum,
                RatingID = m.RatingID,
                RatingDescr = m.RatingDescr,
                RatingOrder = m.RatingOrder,
                RatingRange=m.RatingRange

            });            
        }
        public bool IsEmployeeDataEmpty()
        {
            return m_baseRepository.GetQuery<Employee>().ToList().Count <= 1 ? true : false;
        }
        // Author        :  Raja Ganapathy		
        // Creation Date :  20-06-2016
        // Reviewed By   :  Hari		
        // Reviewed Date :  13-10-2016
        /// <summary>
        /// Delete comments for compensation
        /// </summary>
        /// <param name="commentKey">Delete comment key</param>
        public async Task deleteComments(int commentKey)
        {
           await m_baseRepository.ExecuteStoredProcedure("[Talent].[USP_Comp_DeleteCommentDetails]", new SqlParameter[] { new SqlParameter("@CommentKey", commentKey) });     
        }
        
        
        // Author        :  Raja Ganapathy		
        // Creation Date :  20-06-2016
        // Reviewed By   :  Hari		
        // Reviewed Date :  13-10-2016
        /// <summary>
        /// To update the compensation reportees data
        /// </summary>
        /// <param name="compensationReportees">Denotes the updated records</param>
        /// <param name="jobYear">Denotes the merit cycle year</param>
        /// <param name="userNum">Denotes the userNum</param>        
        public void UpdateCompReportees(List<MeritGridModel> compensationReportees, int jobYear, int userNum)
        {
                 
                foreach (var item in compensationReportees)
                {
                    var employeeRecord = m_baseRepository.GetQuery<EmployeeJob>(new string[] { "EmployeeCompMerit", "EmployeeCompNew", "EmployeeCompBonu", "EmployeeCompRating", "EmployeeCompPromotion", "EmployeeCompAdjustment", "EmployeeCompLTIP" }, x => x.EmpJobNum == item.Empjobnum);
                    if (item.IsMeritEdited)
                    UpdateMeritData(item, employeeRecord,userNum);
                    if (item.IsMeritEdited)
                    UpdateCompRatingData(item, employeeRecord, userNum);
                UpdateCompNewData(item, employeeRecord, userNum);
                UpdateEmployeeBudgetData(item, userNum);
                    if (item.IsPromotionEdited)
                        UpdatePromotionData(item, employeeRecord, userNum);
                    if (item.IsAdjustmentEdited)                    
                    UpdateAdjustmentData(item, employeeRecord, userNum);
                if (item.IsBonusEdited)
                    UpdateBonusData(item, employeeRecord, userNum);
            }                
                m_baseRepository.SaveChanges();
                             
        }

        // Author        :  Raja Ganapathy		
        // Creation Date :  20-06-2016
        // Reviewed By   :  Hari		
        // Reviewed Date :  13-10-2016
        /// <summary>
        /// To get list of comment related to promotion in that employee
        /// </summary>
        /// <param name="empJobNum">Denotes the employee jobnum</param>
        /// <returns>Returns list of comments</returns>
        public IQueryable<EmployeeCompComment> GetCompComments(int empJobNum)
        {
            return m_baseRepository.GetQuery<EmployeeCompComment>(new string[] { "AppUser" }, empCompComment => empCompComment.EmpJobNum == empJobNum);            
        }


        public IQueryable<EmployeeApprovalDetail> GetWorkFlowComments(int empJobNum)
        {
            int moduleNum = m_baseRepository.GetQuery<Module>(x => x.ModuleKey == MeritConstants.Compensation).Select(x => x.ModuleNum).FirstOrDefault();
            return m_baseRepository.GetQuery<EmployeeApprovalDetail>(new string[] { "AppUser" }, x => x.EmpJobNum == empJobNum && x.ModuleNum == moduleNum);
        }

        // Author        :  Raja Ganapathy		
        // Creation Date :  20-06-2016
        // Reviewed By   :  Hari		
        // Reviewed Date :  13-10-2016
        /// <summary>
        /// To get the compensation type num
        /// </summary>
        /// <param name="type">Denotes the compensation type</param>
        /// <returns>Returns compensation type num</returns>
        public int getCompensationTypeNum(string type)
        {
            return m_baseRepository.GetQuery<CompensationType>(compensationType => compensationType.CompensationTypeShortName == type).Select(compensationType => compensationType.CompensationTypeNum).FirstOrDefault();            
        }

        // Author        :  Raja Ganapathy		
        // Creation Date :  20-06-2016
        // Reviewed By   :  Hari		
        // Reviewed Date :  13-10-2016
        /// <summary>
        /// To get the list of compensation types
        /// </summary>        
        /// <returns>Returns all the compensation types</returns>
        public IQueryable<CompensationType> GetCompensationTypes()
        {
            return m_baseRepository.GetQuery<CompensationType>();
        }

     
        // Author        :  Raja Ganapathy		
        // Creation Date :  06-08-2016
        // Reviewed By   :  Hari		
        // Reviewed Date :  13-10-2016
        /// <summary>
        /// Get filter configuration data
        /// </summary>
        /// <returns>Returns Filter data</returns>
        public async Task<IEnumerable<DropDownListItems>> GetFilterConfiguration()
        {
            var filterConfiguration = await m_baseRepository.GetData<DropDownListItems>("[Talent].[USP_Comp_GET_FilterConfiguration]", new SqlParameter[] { });
            return filterConfiguration;
        }

        // Author        :  Raja Ganapathy		
        // Creation Date :  20-06-2016
        // Reviewed By   :  Hari		
        // Reviewed Date :  13-10-2016
        /// <summary>
        /// To get the merit reportee grid data
        /// </summary>
        /// <param name="managerNum">Denotes selected manager num in manager tree</param>
        /// <param name="compExclusion">Denotes compExclusion</param>
        /// <param name="loggedInEmployeeNum">Denotes the logged in employeenum</param>
        /// <param name="compMenuType">Denotes the menu type assign group or my team</param>
        /// <param name="isRollup">Denotes the rollup is selected or not</param>
        /// <returns>Returns compensation reportees data</returns>
        public async Task<IEnumerable<MeritGridModel>> GetCompReportees(int managerNum, int loggedInEmployeeNum, int loggedInUserNum, MenuType compMenuType, bool isRollup, int loggedSelectedUserNum)
        {            
            SqlParameter[] sqlParameter = {   new SqlParameter("@managerNum", managerNum),
                                              new SqlParameter("@loggedInEmpNum",(int)loggedInEmployeeNum),
                                              new SqlParameter("@rollup", (bool)isRollup),
                                              new SqlParameter("@MenuType",(int)compMenuType),
                                              new SqlParameter("@loggedInUserNum",loggedInUserNum),
                                              new SqlParameter("@loggedSelectedUserNum",loggedSelectedUserNum)
                                          };
            var compensationReportees = await m_baseRepository.GetData<MeritGridModel>("[Talent].[USP_Comp_GET_ReporteesData] @managerNum,@loggedInEmpNum,@rollup,@MenuType,@loggedInUserNum,@loggedSelectedUserNum", sqlParameter);
            return compensationReportees;           
        }
        public async Task<IEnumerable<SubmitReporteesModel>> GetCompSubmitReportees(int managerNum, int loggedInEmployeeNum, int loggedInUserNum, MenuType compMenuType, bool isRollup, int loggedSelectedUserNum)
        {
            SqlParameter[] sqlParameter = {   new SqlParameter("@managerNum", managerNum),
                                              new SqlParameter("@loggedInEmpNum",(int)loggedInEmployeeNum),
                                              new SqlParameter("@rollup", (bool)isRollup),
                                              new SqlParameter("@MenuType",(int)compMenuType),
                                              new SqlParameter("@loggedInUserNum",loggedInUserNum),
                                              new SqlParameter("@loggedSelectedUserNum",loggedSelectedUserNum)
                                          };
            var compensationReportees = await m_baseRepository.GetData<SubmitReporteesModel>("[Talent].[USP_Comp_GET_SubmitReporteesData] @managerNum,@loggedInEmpNum,@rollup,@MenuType,@loggedInUserNum,@loggedSelectedUserNum", sqlParameter);
            return compensationReportees;
        }
        public async Task<IEnumerable<SubmitReporteesModel>> GetCompApprovalReportees(int managerNum, int loggedInEmployeeNum, int loggedInUserNum, MenuType compMenuType, bool isRollup, int loggedSelectedUserNum)
        {
            SqlParameter[] sqlParameter = {   new SqlParameter("@managerNum", managerNum),
                                              new SqlParameter("@loggedInEmpNum",(int)loggedInEmployeeNum),
                                              new SqlParameter("@rollup", (bool)isRollup),
                                              new SqlParameter("@MenuType",(int)compMenuType),
                                              new SqlParameter("@loggedInUserNum",loggedInUserNum),
                                              new SqlParameter("@loggedSelectedUserNum",loggedSelectedUserNum)
                                          };
            var compensationReportees = await m_baseRepository.GetData<SubmitReporteesModel>("[Talent].[USP_Comp_GET_ApprovalReporteesData] @managerNum,@loggedInEmpNum,@rollup,@MenuType,@loggedInUserNum,@loggedSelectedUserNum", sqlParameter);
            return compensationReportees;
        }

        public async Task<IEnumerable<SubmitReporteesModel>> GetCompReopenReportees(int managerNum, int loggedInEmployeeNum, int loggedInUserNum, MenuType compMenuType, bool isRollup, int loggedSelectedUserNum)
        {
            SqlParameter[] sqlParameter = {   new SqlParameter("@managerNum", managerNum),
                                              new SqlParameter("@loggedInEmpNum",(int)loggedInEmployeeNum),
                                              new SqlParameter("@rollup", (bool)isRollup),
                                              new SqlParameter("@MenuType",(int)compMenuType),
                                              new SqlParameter("@loggedInUserNum",loggedInUserNum),
                                              new SqlParameter("@loggedSelectedUserNum",loggedSelectedUserNum)
                                          };
            var compensationReportees = await m_baseRepository.GetData<SubmitReporteesModel>("[Talent].[USP_Comp_GET_ReopenReporteesData] @managerNum,@loggedInEmpNum,@rollup,@MenuType,@loggedInUserNum,@loggedSelectedUserNum", sqlParameter);
            return compensationReportees;
        }

        public async Task<IEnumerable<ApprovalEmployeeSearchData>> GetCompApprovalReporteesSearch(int managerNum, int loggedInUserNum, MenuType compMenuType, bool isRollup, string approvalType)
        {
            SqlParameter[] sqlParameter = {   new SqlParameter("@managerNum", managerNum),                                              
                                              new SqlParameter("@rollup", (bool)isRollup),
                                              new SqlParameter("@MenuType",(int)compMenuType),
                                              new SqlParameter("@loggedInUserNum",loggedInUserNum),
                                              new SqlParameter("@Approval",approvalType)
                                          };
            var compApprovalReporteesSearch = await m_baseRepository.GetData<ApprovalEmployeeSearchData>("[Talent].[USP_Comp_GET_ApprovalReporteesSearch] @managerNum,@rollup,@MenuType,@loggedInUserNum,@Approval", sqlParameter);
            return compApprovalReporteesSearch;
        }

        // Author        :  Raja Ganapathy		
        // Creation Date :  20-06-2016
        // Reviewed By   :  Hari		
        // Reviewed Date :  13-10-2016
        /// <summary>
        /// To get the currency type for budget grid
        /// </summary>
        /// <returns>Returns the Currency exchange</returns>
        public async Task<IEnumerable<ExchangeCurrencies>> GetCurrencies()
        {
            SqlParameter[] sqlParameter = {  };
            var currencies = await m_baseRepository.GetData<ExchangeCurrencies>("[Talent].[USP_Comp_Get_CurrencyData]", sqlParameter);
            return currencies;            
        }

        public async Task<int> UpdateCommentStatus(int empJobNum, int userNum)
        {
            SqlParameter[] parameter = new SqlParameter[] {
                new SqlParameter("@empJobNum",empJobNum),
                new SqlParameter("@userNum",userNum),
            };

            return await m_baseRepository.ExecuteStoredProcedure("Talent.USP_Comp_PUT_UpdateCommentStatus", parameter);
        }


        // Author        :  Raja Ganapathy		
        // Creation Date :  20-06-2016
        // Reviewed By   :  Hari		
        // Reviewed Date :  13-10-2016 
        /// <summary>
        /// Get the configuration details of the compensation
        /// </summary>
        /// <returns>CompensationTypeConfiguration</returns>
        public BusinessSettingModel GetCompensationTypeConfiguration()
        {         
            return m_tenantCacheProvider.GetBusinessSetting();
        }

        // Author        :  Raja Ganapathy		
        // Creation Date :  25-06-2016      
        // Reviewed By   :  Hari		
        // Reviewed Date :  13-10-2016 
        /// <summary>
        /// To get alias name and visibility of employees grid
        /// </summary>
        /// <returns>List merit grid configuration</returns>
        public IQueryable<MetaColumn> GetReporteeConfiguration()
        {
            return m_baseRepository.GetQuery<MetaColumn>();            
        }

        public bool IsInDirects(int managerNum)
        {
            var managerRelation = m_baseRepository.GetQuery<ManagerRelation>();
            var result = (from a in managerRelation
                          join b in managerRelation on a.EmployeeNum equals b.ManagerNum
                          where a.ManagerNum == managerNum
                          select b).FirstOrDefault();
            return (result == null) ? false : true;
        }
        
        // Author         :  Raja Ganapathy
        // Creation Date  :  05-Jul-2016 
        // Reviewed By   :  Hari		
        // Reviewed Date :  13-10-2016 
        /// <summary>
        /// Insert comment for the employee
        /// </summary>
        /// <param name="comment">Comment</param>
        public void PutUpdateCompensationComments(EmployeeCompComment comment)
        {            
            if(comment.EmployeeCompCommentsNum==0)
            m_baseRepository.Add<EmployeeCompComment>(comment);         
            else
            m_baseRepository.Edit<EmployeeCompComment>(comment);         
            m_baseRepository.SaveChanges();
        }
                      
        // Author         :  Raja Ganapathy
        // Creation Date  :  05-Jul-2016 
        // Reviewed By   :  Hari		
        // Reviewed Date :  13-10-2016 
        /// <summary>
        /// To get active job status
        /// </summary>
        /// <returns>Returns jobstatus list</returns>
        public static List<string> GetStatus()
        {
            List<string> status = new List<string>();
            status.Add("T");
            status.Add("I");
            status.Add("N");
            return status;
        }

        

        // Author        :  Raja Ganapathy		
        // Creation Date :  18-10-2016       
        // Reviewed By   :  Hari		
        // Reviewed Date :  18-10-2016 
        // To get the approval status
        public async Task<int> GetApprovalStatus(int selectedManagerNum, int loggedInEmployeeNum)
        {
            var returnValue = new SqlParameter("@RETURN_VALUE", SqlDbType.Int);
            returnValue.Direction = ParameterDirection.ReturnValue;
            SqlParameter[] parameter = new SqlParameter[] {
                new SqlParameter("@LoggedInEmployeeNum",loggedInEmployeeNum),
                new SqlParameter("@SelectedManagerNum",selectedManagerNum),
                returnValue
            };
                        
            await m_baseRepository.ExecuteStoredProcedure("Talent.fn_GetMeritApprovalStatus", parameter);
            return (int)returnValue.Value;
            
        }


        // Author        :  Raja Ganapathy		
        // Creation Date :  22-06-2016       
        // Reviewed By   :  Hari		
        // Reviewed Date :  13-10-2016 
        /// <summary>
        /// Get manager tree data
        /// </summary>
        /// <param name="managerNum">Selected group num</param>
        /// <param name="loggedInEmployeeNum">Denotes logged in employee num</param>
        /// <param name="jobYear">Denotes merit cycle year</param>
        /// <param name="isRollup">Denotes rollup</param>
        /// <param name="userNum">Denotes logged in employee user num</param>
        /// <param name="MenuType">Denotes the menu type</param>
        /// <param name="exclusionType">Denotes comp exclusion type</param>
        /// <returns>Returns manager tree data</returns>
        public async Task<IEnumerable<ManagerTree>> GetCompManagerTree(int managerNum, int loggedInEmployeeNum, int loggedInUserNum, int userNum, ViewPageType pageType)
        {
            string moduleKey = (pageType == ViewPageType.Compensation) ? ModuleConstants.Compensation : ModuleConstants.Analytics;
            string pageTypeValue = (pageType == ViewPageType.Compensation) ? "Compensation" : "Analytics";  
            SqlParameter[] sqlParameter = {   new SqlParameter("@ManagerNum", managerNum),
                                              new SqlParameter("@LoggedInEmployeeNum",loggedInEmployeeNum),
                                              new SqlParameter("@UserNum",userNum),
                                              new SqlParameter("@PageType",pageTypeValue),
                                              new SqlParameter("@ModuleKey",moduleKey),
                                              new SqlParameter("@loggedInUserNum",loggedInUserNum)  
                                              
                                          };
            var managerTree = await m_baseRepository.GetData<ManagerTree>("Common.USP_GET_ManagerTree @ManagerNum,@LoggedInEmployeeNum,@UserNum,@PageType,@ModuleKey,@loggedInUserNum", sqlParameter);
            return managerTree;            
        }

        
        
        
        // Author        :  Raja Ganapathy		
        // Creation Date :  20-06-2016
        // Reviewed By   :  Hari		
        // Reviewed Date :  13-10-2016 
        /// <summary>
        /// Get the list of comments provided by the manager.
        /// </summary>
        /// <param name="empJobNum">empJobNum of the employee</param>
        /// <returns>Returns list of employee comp comments<EmployeeCompComment></returns>
        public IEnumerable<EmployeeCompComment> GetCommentsBasedOnType(int empJobNum, string type)
        {
            int compTypeNum = m_baseRepository.GetQuery<CompensationType>(m => m.CompensationTypeShortName == type).Select(mmm => mmm.CompensationTypeNum).First();
            return m_baseRepository.GetQuery<EmployeeCompComment>(employeeCompComment => employeeCompComment.EmpJobNum == empJobNum).OrderByDescending(o => o.UpdatedDate).Where(mm => mm.CompensationTypeNum == compTypeNum);
        }

        // Author        :  Raja Ganapathy		
        // Creation Date :  22-06-2016
        // Reviewed By   :  Hari		
        // Reviewed Date :  13-10-2016
        /// <summary>
        /// To get Budget details
        /// </summary>        
        /// <param name="employeeNum">Denotes selected employee num</param>
        /// <param name="compensationTypeConfiguration">Denotes compensation type configuration</param>
        /// <param name="compMenuType">Denotes the comp menu type</param>
        public async Task<BudgetModel> GetBudgetData(int loggedInEmpNum, int employeeNum, int loggedInUserNum, MenuType compMenuType, string currencyCulture, int currencyCodeNum, bool isRollup, bool isSelectedRollup)
        {
            SqlParameter[] sqlParameter = {   new SqlParameter("@loggedInEmpNum", loggedInEmpNum.ToString()),
                                              new SqlParameter("@managerNum",employeeNum.ToString()),                                              
                                              new SqlParameter("@rollup", (bool)isRollup),
                                              new SqlParameter("@isSelectedRollup", (bool)isSelectedRollup),
                                              new SqlParameter("@MenuType",compMenuType.ToString()),     
                                              new SqlParameter("@currencyCodeNum",currencyCodeNum),
                                              new SqlParameter("@loggedInUserNum",loggedInUserNum),                                              
                                          };
            var BudgetData = await m_baseRepository.GetData<BudgetModel>("[Talent].[USP_Comp_GET_BudgetSpentData] @loggedInEmpNum,@managerNum,@rollup,@isSelectedRollup,@MenuType,@currencyCodeNum,@loggedInUserNum", sqlParameter);
            if (BudgetData == null)          
            {
                BudgetModel budgetModel = new BudgetModel();                                
                budgetModel.AdjustmentSpent = 0;                
                budgetModel.MeritBudget = 0;                
                budgetModel.PromotionSpent = 0;
                budgetModel.MeritSpent = 0;
                budgetModel.LumpSumSpent = 0;
                return budgetModel;
            }
            
            return BudgetData.FirstOrDefault();                       
        }

        // Author        :  Raja Ganapathy		
        // Creation Date :  22-06-2016   
        // Reviewed By   :  Hari		
        // Reviewed Date :  13-10-2016 
        /// <summary>
        ///  To update the employee comment
        /// </summary>
        /// <param name="empJobNum">Denotes the employeejob num</param>
        /// <param name="userNum">Denotes the user num</param>
        /// <param name="employeeCompCommentNum">Denotes the employee comp comment num</param>
        /// <param name="comments">Denotes the comments</param>
        /// <param name="grade">Denotes the grade</param>
        // Modified By    : Karthikeyan Shanmugam
        // Modified Date  : 14-Feb-2017
        // Reviewed By    : Hariharasubramaniyan
        // Reviewed Date  : 14-Feb-2017
        // comments       : To show the cilent time based on UTC time it shows depend upon the client location or time zone
        public void UpdateEmployeeComment(int empJobNum, int userNum, int employeeCompCommentNum, string comments, string grade)
        {
            EmployeeCompComment employeeCompComment = new EmployeeCompComment();
            var empCompComment = m_baseRepository.GetQuery<EmployeeCompComment>(x => x.EmployeeCompCommentsNum == employeeCompCommentNum).FirstOrDefault();
            var localTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.Local);
            if (empCompComment == null)
            {
                employeeCompComment.EmpJobNum = empJobNum;
                employeeCompComment.CreatedBy = userNum;
                employeeCompComment.CreatedDate = localTime;
                employeeCompComment.Comments = comments;
                employeeCompComment.CompensationTypeNum = m_baseRepository.GetQuery<CompensationType>().Where(x => x.CompensationTypeShortName == "Promotion").Select(x => x.CompensationTypeNum).FirstOrDefault();
                m_baseRepository.Add<EmployeeCompComment>(employeeCompComment);
            }
            else
            {
                employeeCompComment = empCompComment;
                employeeCompComment.Comments = comments;
                employeeCompComment.CompensationTypeNum = m_baseRepository.GetQuery<CompensationType>().Where(x => x.CompensationTypeShortName == "Promotion").Select(x => x.CompensationTypeNum).FirstOrDefault();
                m_baseRepository.Edit<EmployeeCompComment>(employeeCompComment);
                employeeCompComment.UpdatedBy = userNum;
                employeeCompComment.UpdatedDate = localTime;
            }
            
            EmployeeCompPromotion employeeCompPromotion = new EmployeeCompPromotion();
            var empCompPromotion = m_baseRepository.GetQuery<EmployeeCompPromotion>(x => x.EmpJobNum == empJobNum).FirstOrDefault();
            if (empCompPromotion == null)
            {
                employeeCompPromotion.EmpJobNum = empJobNum;
                employeeCompPromotion.NewGrade = grade;
                employeeCompPromotion.UpdatedBy = userNum;
                employeeCompPromotion.UpdatedDate = localTime;
                m_baseRepository.Add<EmployeeCompPromotion>(employeeCompPromotion);
            }
            else
            {
                employeeCompPromotion = empCompPromotion;
                employeeCompPromotion.NewGrade = grade;
                employeeCompPromotion.UpdatedBy = userNum;
                employeeCompPromotion.UpdatedDate = localTime;
                m_baseRepository.Edit<EmployeeCompPromotion>(employeeCompPromotion);
            }
            
            m_baseRepository.SaveChanges();
        }

        

        // Author        :  Raja Ganapathy		
        // Creation Date :  22-06-2016 
        // Reviewed By   :  Hari		
        // Reviewed Date :  13-10-2016
        /// <summary>
        /// To get the employee info
        /// </summary>
        /// <param name="employeeNum">Denotes selected employee number</param>
        /// <returns>Returns employee info details</returns>
        public async Task<IEnumerable<EmployeeInfoDetails>> GetEmployeeInfo(int employeeNum, int loggedInUserNum)
        {
            return await m_baseRepository.GetData<EmployeeInfoDetails>("[Talent].[USP_Comp_GET_EmployeeInfo] @employeeNum,@loggedInUserNum", new SqlParameter[] { new SqlParameter("@employeeNum", employeeNum), new SqlParameter("@loggedInUserNum", loggedInUserNum) });
        }

        // Author        : Balamurugan M	
        // Creation Date :  07-Aug-2017
        // Reviewed By   :  		
        // Reviewed Date :  
        /// <summary>
        /// To get the employee Basic info
        /// </summary>
        /// <param name="employeeNum">Denotes selected employee number</param>
        /// <returns>Returns employee info details</returns>
        public async Task<IEnumerable<EmployeeInfoBasicDetails>> GetEmployeeBasicInfo(int employeeNum)
        {
            return await m_baseRepository.GetData<EmployeeInfoBasicDetails>("[Talent].[USP_Comp_GET_EmployeeInfoBasicDetails] @employeeNum", new SqlParameter[] { new SqlParameter("@employeeNum", employeeNum) });
        }
        
        // Author        :  Shaheena Shaik
        // Creation Date :  4-July-2017
        /// <summary>
        /// Returns the employeename
        /// </summary>
        /// <param name="selectedManagerNum">Denotes the selected manager num</param>
        public async Task<string> GetManagerName(int managerNum)
        {
            SqlParameter[] sqlParameter = {   new SqlParameter("@ManagerNum", managerNum)};
            var managerTree = await m_baseRepository.GetData<string>("[Talent].[USP_Comp_GET_GetManagerReporteeInfo] @ManagerNum", sqlParameter);
            return managerTree.FirstOrDefault();
        }

        public string GetworkflowStatus()
        {
            //  var a = m_baseRepository.GetQuery<BusSetting>().Where(x => x.KeyValue == "WorkFlow").Select(x => x.KeyDataValue).FirstOrDefault();
            return m_tenantCacheProvider.GetBusinessSetting().FeatureConfiguration.WorkFlow ==true ? "Yes": "No";
            //return a;
        }

        #endregion

        #region private Methods   

        // Author        :  Raja Ganapathy		
        // Creation Date :  22-06-2016   
        // Reviewed By   :  Hari		
        // Reviewed Date :  13-10-2016
        /// <summary>
        /// Get the employee budget
        /// </summary>
        /// <param name="empJobNum">empJobNum of an employee</param>
        /// <returns>Budget</returns>
        private Budget getEmployeeBudget(int empJobNum)
        {            
            return m_baseRepository.GetQuery<Budget>(employeeBudget => employeeBudget.EmpJobNum == empJobNum).FirstOrDefault();            
        }

        

        // Author        :  Raja Ganapathy		
        // Creation Date :  22-06-2016    
        // Reviewed By   :  Hari		
        // Reviewed Date :  13-10-2016
        /// <summary>
        /// To update the merit data
        /// </summary>
        /// <param name="reporteeData">Denotes the meritgrid model</param>
        /// <param name="employeeRecord">Denotes the employee record</param>
        /// 
        // Modified By    : Karthikeyan Shanmugam
        // Modified Date  : 14-Feb-2017
        // Reviewed By    : Hariharasubramaniyan
        // Reviewed Date  : 14-Feb-2017
        // comments       : To show the cilent time based on UTC time it shows depend upon the client location or time zone
        private void UpdateMeritData(MeritGridModel reporteeData, IQueryable<EmployeeJob> employeeRecord, int userNum)
        {
            EmployeeCompMerit employeeCompMerit = new EmployeeCompMerit();
            var employeeMeritRecord = employeeRecord.Select(x => x.EmployeeCompMerit).FirstOrDefault();
            var localTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.Local);
            if (employeeMeritRecord == null)
            {
                employeeCompMerit.EmpJobNum = reporteeData.Empjobnum;
                employeeCompMerit.MeritPCT = reporteeData.MeritPCT;
                employeeCompMerit.MeritAmt = reporteeData.MeritAmtLocal;
                employeeCompMerit.LumpSumPCT = reporteeData.LumpSumPct;
                employeeCompMerit.LumpSumAmt = reporteeData.LumpSumAmtLocal;
                if (reporteeData.MeritPerformanceRatingNum.HasValue && reporteeData.MeritRange != null)
                {
                    employeeCompMerit.MeritRangeExceed = reporteeData.MeritRangeExceed;
                    employeeCompMerit.MeritRange = reporteeData.MeritRange;
                }
                employeeCompMerit.UpdatedBy = userNum;
                employeeCompMerit.UpdatedDate = localTime;
                m_baseRepository.Add<EmployeeCompMerit>(employeeCompMerit);
            }
            else
            {
                employeeCompMerit = employeeMeritRecord;
                employeeCompMerit.MeritPCT = reporteeData.MeritPCT;
                employeeCompMerit.MeritAmt = reporteeData.MeritAmtLocal;
                employeeCompMerit.LumpSumPCT = reporteeData.LumpSumPct;
                employeeCompMerit.LumpSumAmt = reporteeData.LumpSumAmtLocal;
                if (reporteeData.MeritPerformanceRatingNum.HasValue && reporteeData.MeritRange != null)
                {
                    employeeCompMerit.MeritRangeExceed = reporteeData.MeritRangeExceed;
                    employeeCompMerit.MeritRange = reporteeData.MeritRange;
                }
                employeeCompMerit.UpdatedBy = userNum;
                employeeCompMerit.UpdatedDate = localTime;
                m_baseRepository.Edit<EmployeeCompMerit>(employeeCompMerit);
            }
            
        }

        // Author        :  Raja Ganapathy		
        // Creation Date :  22-06-2016    
        // Reviewed By   :  Hari		
        // Reviewed Date :  13-10-2016
        /// <summary>
        /// To update the merit data
        /// </summary>
        /// <param name="reporteeData">Denotes the meritgrid model</param>
        /// <param name="employeeRecord">Denotes the employee record</param>
        /// <param name="comment"></param>
        // Modified By    : Karthikeyan Shanmugam
        // Modified Date  : 14-Feb-2017
        // Reviewed By    : Hariharasubramaniyan
        // Reviewed Date  : 14-Feb-2017
        // comments       : To show the cilent time based on UTC time it shows depend upon the client location or time zone
        private void UpdateCompNewData(MeritGridModel reporteeData, IQueryable<EmployeeJob> employeeRecord, int userNum)
        {
            EmployeeCompNew employeeCompNew = new EmployeeCompNew();
            var employeeCompNewRecord = employeeRecord.Select(x => x.EmployeeCompNew).FirstOrDefault();
            var localTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.Local);
            if (employeeCompNewRecord == null)
            {
                employeeCompNew.EmpJobNum = reporteeData.Empjobnum;
                employeeCompNew = employeeCompNewRecord;
                employeeCompNew.NewSalary = reporteeData.NewSalaryLocal;
                employeeCompNew.HrlyNewSalary = reporteeData.NewHourlyRateLocal;
                employeeCompNew.CompCompleted = (reporteeData.MeritAmtLocal).HasValue == true ? (reporteeData.MeritAmtLocal).HasValue : (reporteeData.LumpSumAmtLocal).HasValue;
                employeeCompNew.TotalNewComp = reporteeData.TCCLocal;
                //employeeCompNew.TotalNewComp = reporteeData.NewSalaryLocal +(reporteeData.LumpSumAmtLocal==null ? 0 : reporteeData.LumpSumAmtLocal);
                employeeCompNew.NewCompaRatio = reporteeData.NewCompaRatio;
                employeeCompNew.UpdatedBy = userNum;
                employeeCompNew.UpdatedDate = localTime;
                m_baseRepository.Add<EmployeeCompNew>(employeeCompNew);
            }
            else
            {
                employeeCompNew = employeeCompNewRecord;
                employeeCompNew.NewSalary = reporteeData.NewSalaryLocal;
                employeeCompNew.HrlyNewSalary = reporteeData.NewHourlyRateLocal;
                employeeCompNew.CompCompleted = (reporteeData.MeritAmtLocal).HasValue == true ? (reporteeData.MeritAmtLocal).HasValue : (reporteeData.LumpSumAmtLocal).HasValue;
                employeeCompNew.TotalNewComp = reporteeData.TCCLocal;
                //employeeCompNew.TotalNewComp = reporteeData.NewSalaryLocal + (reporteeData.LumpSumAmtLocal == null ? 0 : reporteeData.LumpSumAmtLocal);
                employeeCompNew.NewCompaRatio = reporteeData.NewCompaRatio;
                employeeCompNew.UpdatedBy = userNum;
                employeeCompNew.UpdatedDate = localTime;
                m_baseRepository.Edit<EmployeeCompNew>(employeeCompNew);
            }
            
        }


        // Author        :  Raja Ganapathy		
        // Creation Date :  22-06-2016   
        // Reviewed By   :  Hari		
        // Reviewed Date :  13-10-2016
        /// <summary>
        /// To update the merit data
        /// </summary>
        /// <param name="reporteeData">Denotes the meritgrid model</param>
        /// <param name="employeeRecord">Denotes the employee record</param>
        // Modified By    : Karthikeyan Shanmugam
        // Modified Date  : 14-Feb-2017
        // Reviewed By    : Hariharasubramaniyan
        // Reviewed Date  : 14-Feb-2017
        // comments       : To show the cilent time based on UTC time it shows depend upon the client location or time zone
        private void UpdateCompRatingData(MeritGridModel reporteeData, IQueryable<EmployeeJob> employeeRecord, int userNum)
        {
            EmployeeCompRating employeeCompRating = new EmployeeCompRating();
            var employeeCompRatingRecord = employeeRecord.Select(x => x.EmployeeCompRating).FirstOrDefault();
            var localTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.Local);
            if (employeeCompRatingRecord == null)
            {
                employeeCompRating.EmpJobNum = reporteeData.Empjobnum;
                if (reporteeData.MeritPerformanceRatingNum.HasValue)
                {
                    employeeCompRating.MeritPerformanceRating = reporteeData.MeritPerformanceRatingNum;
                }
                employeeCompRatingRecord.UpdatedBy = userNum;
                employeeCompRatingRecord.UpdatedDate = localTime;
                m_baseRepository.Add<EmployeeCompRating>(employeeCompRatingRecord);
            }
            else
            {
                employeeCompRating = employeeCompRatingRecord;
                if (reporteeData.MeritPerformanceRatingNum.HasValue)
                {
                    employeeCompRating.MeritPerformanceRating = reporteeData.MeritPerformanceRatingNum;
                }
                employeeCompRatingRecord.UpdatedBy = userNum;
                employeeCompRatingRecord.UpdatedDate = localTime;
                m_baseRepository.Edit<EmployeeCompRating>(employeeCompRatingRecord);
            }

            
        }

        // Author        :  Raja Ganapathy		
        // Creation Date :  22-06-2016    
        // Reviewed By   :  Hari		
        // Reviewed Date :  13-10-2016
        /// <summary>
        /// To update the merit data
        /// </summary>
        /// <param name="reporteeData">Denotes the meritgrid model</param>
        /// <param name="employeeRecord">Denotes the employee record</param>
        /// <param name="userNum">Denotes the user num</param>
        /// <param name="compensationTypeNum">Denotes compensation type num</param> 
        // Modified By    : Karthikeyan Shanmugam
        // Modified Date  : 14-Feb-2017
        // Reviewed By    : Hariharasubramaniyan
        // Reviewed Date  : 14-Feb-2017
        // comments       : To show the cilent time based on UTC time it shows depend upon the client location or time zone
        private void UpdatePromotionData(MeritGridModel reporteeData, IQueryable<EmployeeJob> employeeRecord, int userNum)
        {
            EmployeeCompPromotion employeeCompPromotion = new EmployeeCompPromotion();
            EmployeeCompNew employeeCompNew = new EmployeeCompNew();
            var employeeCompNewRecord = employeeRecord.Select(x => x.EmployeeCompNew).FirstOrDefault();
            var employeeCompPromotionRecord = employeeRecord.Select(x => x.EmployeeCompPromotion).FirstOrDefault();
            var localTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.Local);
            if (employeeCompPromotionRecord == null)
            {
                employeeCompPromotion.EmpJobNum = reporteeData.Empjobnum;
                employeeCompPromotion.NewGrade = reporteeData.NewTitle;
                employeeCompPromotion.PromotionPct = reporteeData.PromotionPct;
                employeeCompPromotion.PromotionAmt = reporteeData.PromotionAmtLocal;
                employeeCompPromotion.UpdatedBy = userNum;
                employeeCompPromotion.UpdatedDate = localTime;
                m_baseRepository.Add<EmployeeCompPromotion>(employeeCompPromotion);
                m_baseRepository.SaveChanges();
            }
            else
            {
                employeeCompPromotion = employeeCompPromotionRecord;
                employeeCompPromotion.NewGrade = reporteeData.NewTitle;
                employeeCompPromotion.PromotionPct = reporteeData.PromotionPct;
                employeeCompPromotion.PromotionAmt = reporteeData.PromotionAmtLocal;
                employeeCompPromotion.UpdatedBy = userNum;
                employeeCompPromotion.UpdatedDate = localTime;
                m_baseRepository.Edit<EmployeeCompPromotion>(employeeCompPromotion);
                m_baseRepository.SaveChanges();
            }
            if (employeeCompNewRecord == null)
            {
                employeeCompNew.EmpJobNum = reporteeData.Empjobnum;
                employeeCompNew.CompCompleted = (reporteeData.PromotionAmtLocal).HasValue;
                m_baseRepository.Add<EmployeeCompNew>(employeeCompNew);
                m_baseRepository.SaveChanges();
            }
            else
            {
                employeeCompNew = employeeCompNewRecord;
                employeeCompNew.CompCompleted = (reporteeData.PromotionAmtLocal).HasValue;
                m_baseRepository.Edit<EmployeeCompNew>(employeeCompNew);
                m_baseRepository.SaveChanges();
            }
        }

        // Author        :  Raja Ganapathy		
        // Creation Date :  22-06-2016    
        // Reviewed By   :  Hari		
        // Reviewed Date :  13-10-2016
        /// <summary>
        /// To update the merit data
        /// </summary>
        /// <param name="reporteeData">Denotes the meritgrid model</param>
        /// <param name="employeeRecord">Denotes the employee record</param> 
        // Modified By    : Karthikeyan Shanmugam
        // Modified Date  : 14-Feb-2017
        // Reviewed By    : Hariharasubramaniyan
        // Reviewed Date  : 14-Feb-2017
        // comments       : To show the cilent time based on UTC time it shows depend upon the client location or time zone
        private void UpdateAdjustmentData(MeritGridModel reporteeData, IQueryable<EmployeeJob> employeeRecord, int userNum)
        {

            EmployeeCompAdjustment employeeCompAdjustment = new EmployeeCompAdjustment();
            EmployeeCompNew employeeCompNew = new EmployeeCompNew();
            var employeeCompNewRecord = employeeRecord.Select(x => x.EmployeeCompNew).FirstOrDefault();
            var employeeCompAdjustmentRecord = employeeRecord.Select(x => x.EmployeeCompAdjustment).FirstOrDefault();
            var localTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.Local);
            if (employeeCompAdjustmentRecord == null)
            {
                employeeCompAdjustment.EmpJobNum = reporteeData.Empjobnum;
                employeeCompAdjustment.AdjustmentPct = reporteeData.AdjustmentPct;
                employeeCompAdjustment.AdjustmentAmt = reporteeData.AdjustmentAmtLocal;
                employeeCompAdjustment.UpdatedBy = userNum;
                employeeCompAdjustment.UpdatedDate = localTime;
                m_baseRepository.Add<EmployeeCompAdjustment>(employeeCompAdjustment);
                m_baseRepository.SaveChanges();
                
            }
            else
            {
                employeeCompAdjustment = employeeCompAdjustmentRecord;
                employeeCompAdjustment.AdjustmentPct = reporteeData.AdjustmentPct;
                employeeCompAdjustment.AdjustmentAmt = reporteeData.AdjustmentAmtLocal;
                employeeCompAdjustment.UpdatedBy = userNum;
                employeeCompAdjustment.UpdatedDate = localTime;
                m_baseRepository.Edit<EmployeeCompAdjustment>(employeeCompAdjustment);
                m_baseRepository.SaveChanges();
               
            }
            if (employeeCompNewRecord == null)
            {
                employeeCompNew.EmpJobNum = reporteeData.Empjobnum;
                employeeCompNew.CompCompleted = (reporteeData.AdjustmentAmtLocal).HasValue;
                m_baseRepository.Add<EmployeeCompNew>(employeeCompNew);
                m_baseRepository.SaveChanges();
            }
            else
            {
                employeeCompNew = employeeCompNewRecord;
                employeeCompNew.CompCompleted = (reporteeData.AdjustmentAmtLocal).HasValue;
                m_baseRepository.Edit<EmployeeCompNew>(employeeCompNew);
                m_baseRepository.SaveChanges();
            }

            }


        private void UpdateBonusData(MeritGridModel reporteeData, IQueryable<EmployeeJob> employeeRecord, int userNum)
        {

            EmployeeCompBonu employeeCompBonus = new EmployeeCompBonu();
         
           
            var employeeCompBonusRecord = employeeRecord.Select(x => x.EmployeeCompBonu).FirstOrDefault();
            var localTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.Local);
            if (employeeCompBonusRecord == null)
            {
                employeeCompBonus.EmpJobNum = reporteeData.Empjobnum;
                employeeCompBonus.BonusPct = reporteeData.BonusPct;
                employeeCompBonus.BonusAmt = reporteeData.BonusAmt;
                employeeCompBonus.Payout = reporteeData.BonusAmt!=null ? reporteeData.BonusAmt+ employeeCompBonus.BonusTeamAmt : employeeCompBonus.Payout;

                employeeCompBonus.UpdatedBy = userNum;
                employeeCompBonus.UpdatedDate = localTime;
                m_baseRepository.Add<EmployeeCompBonu>(employeeCompBonus);
                m_baseRepository.SaveChanges();

            }
            else
            {
                employeeCompBonus = employeeCompBonusRecord;
                employeeCompBonus.BonusPct = reporteeData.BonusPct;
                employeeCompBonus.BonusAmt = reporteeData.BonusAmt;
                employeeCompBonus.Payout = reporteeData.BonusAmt != null ? reporteeData.BonusAmt + employeeCompBonus.BonusTeamAmt : employeeCompBonus.BonusTeamAmt;
                employeeCompBonus.UpdatedBy = userNum;
                employeeCompBonus.UpdatedDate = localTime;
                m_baseRepository.Edit<EmployeeCompBonu>(employeeCompBonus);
                m_baseRepository.SaveChanges();

            }
           

        }



        // Author        :  Raja Ganapathy		
        // Creation Date :  22-06-2016    
        /// <summary>
        /// To update the merit data
        /// </summary>
        /// <param name="reporteeData">Denotes the meritgrid model</param>    
        // Modified By    : Karthikeyan Shanmugam
        // Modified Date  : 14-Feb-2017
        // Reviewed By    : Hariharasubramaniyan
        // Reviewed Date  : 14-Feb-2017
        // comments       : To show the cilent time based on UTC time it shows depend upon the client location his time zone
        private void UpdateEmployeeBudgetData(MeritGridModel reporteeData, int userNum)
        {
            Budget employeeBudget = new Budget();
            var employeeBudgetRecord = getEmployeeBudget(reporteeData.Empjobnum);
            var localTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.Local);
            if (employeeBudgetRecord == null)
            {
                employeeBudget.EmpJobNum = reporteeData.Empjobnum;
                employeeBudget.CompensationTypeNum = getCompensationTypeNum(CompensationConstants.MeritBudget);
                employeeBudget.Spent =reporteeData.EmployeeStatus.ToLower()=="annual"? (reporteeData.MeritAmtLocal??0) :((reporteeData.TotalWorkHrs !=null ? (reporteeData.MeritAmtLocal * reporteeData.TotalWorkHrs ?? 0): (reporteeData.MeritAmtLocal *2080 ?? 0)));
                employeeBudget.PromotionSpent = reporteeData.EmployeeStatus.ToLower() == "annual" ? (reporteeData.PromotionAmtLocal ?? 0) : ((reporteeData.TotalWorkHrs != null ? (reporteeData.PromotionAmtLocal * reporteeData.TotalWorkHrs ?? 0) : (reporteeData.PromotionAmtLocal * 2080 ?? 0)));
                employeeBudget.AdjustmentSpent = reporteeData.EmployeeStatus.ToLower() == "annual" ? (reporteeData.AdjustmentAmtLocal ?? 0) : ((reporteeData.TotalWorkHrs != null ? (reporteeData.AdjustmentAmtLocal * reporteeData.TotalWorkHrs ?? 0) : (reporteeData.AdjustmentAmtLocal * 2080 ?? 0)));
                //employeeBudget.LumpSumSpent = reporteeData.EmployeeStatus.ToLower() == "annual" ? (reporteeData.LumpSumAmtLocal ?? 0) : ((reporteeData.TotalWorkHrs != null ? (reporteeData.LumpSumAmtLocal * reporteeData.TotalWorkHrs ?? 0) : (reporteeData.LumpSumAmtLocal * 2080 ?? 0)));
                employeeBudget.LumpSumSpent = reporteeData.LumpSumAmtLocal ?? 0;
                employeeBudget.UpdatedBy = userNum;
                employeeBudget.UpdatedDate = localTime;
                m_baseRepository.Add<Budget>(employeeBudget);
            }
            else
            {
                employeeBudget = employeeBudgetRecord;
                employeeBudget.Spent = reporteeData.EmployeeStatus.ToLower() == "annual" ? (reporteeData.MeritAmtLocal ?? 0) : ((reporteeData.TotalWorkHrs != null ? (reporteeData.MeritAmtLocal * reporteeData.TotalWorkHrs ?? 0) : (reporteeData.MeritAmtLocal * 2080 ?? 0)));
                employeeBudget.PromotionSpent = reporteeData.EmployeeStatus.ToLower() == "annual" ? (reporteeData.PromotionAmtLocal ?? 0) : ((reporteeData.TotalWorkHrs != null ? (reporteeData.PromotionAmtLocal * reporteeData.TotalWorkHrs ?? 0) : (reporteeData.PromotionAmtLocal * 2080 ?? 0)));
                employeeBudget.AdjustmentSpent = reporteeData.EmployeeStatus.ToLower() == "annual" ? (reporteeData.AdjustmentAmtLocal ?? 0) : ((reporteeData.TotalWorkHrs != null ? (reporteeData.AdjustmentAmtLocal * reporteeData.TotalWorkHrs ?? 0) : (reporteeData.AdjustmentAmtLocal * 2080 ?? 0)));
                //employeeBudget.LumpSumSpent = reporteeData.EmployeeStatus.ToLower() == "annual" ? (reporteeData.LumpSumAmtLocal ?? 0) : ((reporteeData.TotalWorkHrs != null ? (reporteeData.LumpSumAmtLocal * reporteeData.TotalWorkHrs ?? 0) : (reporteeData.LumpSumAmtLocal * 2080 ?? 0)));
                employeeBudget.LumpSumSpent = reporteeData.LumpSumAmtLocal ?? 0;
                employeeBudget.UpdatedBy = userNum;
                employeeBudget.UpdatedDate = localTime;
                m_baseRepository.Edit<Budget>(employeeBudget);
            }
           
        }

        
        #endregion
    }
}
        
