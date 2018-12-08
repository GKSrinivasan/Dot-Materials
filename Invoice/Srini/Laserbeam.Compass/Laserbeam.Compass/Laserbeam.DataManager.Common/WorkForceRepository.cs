// Copyright (c) 2016 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.
// Component Name  :  WorkForceRepository
// Description     :  Repository for WorkForce
// Author         :  Raja Ganapathy
// Creation Date  :  30-Mar-2017 

using Laserbeam.BusinessObject.Common;
using Laserbeam.BusinessObject.Common.CachedModels;
using Laserbeam.BusinessObject.Common.Constants;
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
using Master = Laserbeam.EntityManager.TenantMaster;

namespace Laserbeam.DataManager.Common
{
    public class WorkForceRepository : IWorkForceRepository
    {
        #region Fields
        // Author         :   Raja Ganapathy
        // Creation Date  :   30-Mar-2017 
        /// <summary>
        /// Object of IBaseRepository
        /// </summary>
        private IBaseRepository m_baseRepository;

        private IMasterBaseRepository m_masterRepository;

        private ITenantCacheProvider m_tenantCacheProvider;
        #endregion

        #region Constructors
        // Author         :   Raja Ganapathy
        // Creation Date  :   30-Mar-2017 
        /// <summary>
        /// Initialize the base repository
        /// </summary>
        /// <param name="workForceProcessManager">Object of IBaseRepository</param>   
        public WorkForceRepository(IBaseRepository baseRepository, IMasterBaseRepository masterRepository, ITenantCacheProvider tenantCacheProvider)
        {
            m_baseRepository = baseRepository;
            m_masterRepository = masterRepository;
            m_tenantCacheProvider = tenantCacheProvider;
        }
        #endregion

        // Author         :   Raja Ganapathy
        // Creation Date  :   30-Mar-2017 
        /// <summary>
        /// To get the template column details
        /// </summary>
        /// <returns>Returns the template fields</returns>
        public IQueryable<MetaColumn> GetTemplateMetaColumnDetails()
        {
            return m_baseRepository.GetQuery<MetaColumn>().OrderBy(x=>x.TemplateRowOrder);
        }

        // Author         :   Raja Ganapathy
        // Creation Date  :   30-Mar-2017 
        /// <summary>
        /// To update the selected field list
        /// </summary>
        /// <param name="selectedFields">Defines the selected fields</param>
        public async Task<bool> UpdateSelectedTemplateColumns(List<int> selectedFields)
        {
            var selectedTemplateID = string.Join(",", selectedFields);
            bool status = await m_baseRepository.ExecuteStoredProcedure("[Talent].[USP_SLP_PUT_UpdateSelectedTemplateFields]", new SqlParameter[] { new SqlParameter("@selectedTemplateID", selectedTemplateID) })>0;
            return status;
        }

        // Author         :   Raja Ganapathy
        // Creation Date  :   30-Mar-2017 
        /// <summary>
        /// To get the employee data template
        /// </summary>
        /// <returns>Returns the data table</returns>
        public async Task<DataTable> GetEmployeeDataTemplate()
        {
            return await m_baseRepository.GetDataTableFromStoredProcedure("[Talent].[USP_SLP_GET_EmployeeDataTemplate]");
        }

        // Author         :   Raja Ganapathy
        // Creation Date  :   30-Mar-2017 
        /// <summary>
        /// To get the employee list for employee search
        /// </summary>
        /// <param name="loggedInUserNum">Logged in user num</param>
        /// <returns>Returns the EmployeeList</returns>
        public IQueryable<EmployeeListModel> GetEmployeeList(int loggedInUserNum)
        {
            var empdetails = (from a in m_baseRepository.GetQuery<Employee>(x=>x.EmployeeID!="999999999")
                              select new EmployeeListModel
                              {
                                  Value = a.EmployeeID,
                                  Text = a.EmployeeID + "-" + a.EmployeeName
                              });
            return empdetails;
        }

        // Author         :   Raja Ganapathy
        // Creation Date  :   30-Mar-2017 
        /// <summary>
        /// To get the workforce tile data
        /// </summary>
        /// <param name="loggedInUserNum">Defines logged in user num</param>
        /// <returns>Returns the workforce data</returns>
        public async Task<WorkForceTileData> GetWorkForceTileData(int loggedInUserNum)
        {
            SqlParameter[] sqlParameter = { new SqlParameter("@loggedInUserNum", loggedInUserNum) };
            var result = await m_baseRepository.GetData<WorkForceTileData>("[Talent].[USP_SLP_GET_WorkForceTileData] @loggedInUserNum", sqlParameter);
            return result.FirstOrDefault();
        }

        // Author         :   Raja Ganapathy
        // Creation Date  :   30-Mar-2017 
        /// <summary>
        /// To get the uploaded employee data
        /// </summary>
        /// <param name="loggedInUserNum">loggedInUserNum</param>
        /// <param name="isMeritEligible">Defines all employees or Merit eligible only</param>
        /// <returns>Returns the DataTable</returns>
        public async Task<DataTable> GetUploadedEmployeeDetails(int loggedInUserNum, string isMeritEligible)
        {
            SqlParameter[] parameter = {   new SqlParameter("@LoggedInUserNum", loggedInUserNum),
                                           new SqlParameter("@IsMeritEligible",isMeritEligible),
                                       };

            return await m_baseRepository.GetDataTableFromStoredProcedure("[Talent].[USP_SLP_GET_ImportEmployeeData]", parameter);
        }

        // Author         :   Raja Ganapathy
        // Creation Date  :   30-Mar-2017 
        /// <summary>
        /// To get the dropdown or Combobox details
        /// </summary>
        /// <param name="columnName">Field Name</param>
        /// <returns>Returns the list</returns>
        public async Task<IEnumerable<DropDownListItems>> GetSelectedDropDownDetails(string columnName)
        {
            SqlParameter[] parameter = { new SqlParameter("@ColumnName", columnName) };
            return await m_baseRepository.GetData<DropDownListItems>("[Talent].[USP_SLP_GET_LookUpValues] @ColumnName", parameter);
        }

        // Author         :   Raja Ganapathy
        // Creation Date  :   30-Mar-2017 
        /// <summary>
        /// To get the searched employee details
        /// </summary>
        /// <param name="employeeID">Defines the employee ID</param>
        /// <returns>Returns the employee details</returns>
        public async Task<IEnumerable<EmployeeErrorData>> GetSearchedEmployeeDetails(string employeeID)
        {
            SqlParameter[] parameter = { new SqlParameter("@EmployeeID", employeeID) };
            return await m_baseRepository.GetData<EmployeeErrorData>("[Talent].[USP_SLP_GET_DataCorrectionEmployeeData] @EmployeeID", parameter);
        }

        public async Task<IEnumerable<EmployeeErrorData>> GetEmployeeErrorRecordDetails(string errorType)
        {
            SqlParameter[] parameter = { new SqlParameter("@ErrorType", errorType) };
            return await m_baseRepository.GetData<EmployeeErrorData>("[Talent].[USP_SLP_GET_ErrorCorrectionRecordDetails] @ErrorType", parameter);
        }

        public int UpdateXmlProcess(string sheetName, string fileSourcePath, int recordCount, int userNum)
        {
            XmlProcess xmlprocess = new XmlProcess();
            xmlprocess.ProcessDate = DateTime.Now;
            xmlprocess.RecordCount = recordCount;
            xmlprocess.XmlFilename = sheetName;
            xmlprocess.SheetName = sheetName;
            xmlprocess.FileSourcePath = fileSourcePath;
            xmlprocess.ValidateStatus = 0;
            xmlprocess.ProcessStatus = 0;
            xmlprocess.UserNum = userNum;
            xmlprocess.MetaXmlTemplateNum = m_baseRepository.GetQuery<MetaXmlTemplate>(x => x.TemplateName == "EmployeeData").Select(x => x.MetaXmlTemplateNum).FirstOrDefault();
            m_baseRepository.Add<XmlProcess>(xmlprocess);
            m_baseRepository.SaveChanges();
            return xmlprocess.XmlProcessNum;
        }

        public async Task<bool> InsertStagingTableData(DataTable userData, string DbName, List<string> columnsName, int xmlProcessNum)
        {
            bool result = await m_baseRepository.SqlBulkInsert(userData, DbName, columnsName) != "" ? false : true;
            SqlParameter[] parameters = { new SqlParameter("@XmlProcessNum", xmlProcessNum),
                                        new SqlParameter("@TableName", "Talent.StagingEmployeeData") };
            await m_baseRepository.ExecuteStoredProcedure("[Talent].[USP_SLP_PUT_XMLProcessNum]", parameters);
            return result;
        }

        public async Task<int> ValidateEmployeeData(int userNum)
        {
            int count = 0;
            if(await ValidateEmployeeDataLengthAndFormat(userNum) > 0)
            {
                if (await ValidateEmployeeDataMandateAndDuplicate(userNum) > 0)
                    count =await ValidateEmployeeDataCircularRef(userNum);
            }
            return count;
        }

        private async Task<int> ValidateEmployeeDataLengthAndFormat(int userNum)
        {
            SqlParameter[] parameter = { new SqlParameter("@UserNum", userNum) };
            var result = await m_baseRepository.GetData<int>("[Talent].[USP_SLP_PUT_EmployeeDataLengthFormatValidation] @UserNum", parameter);
            return result.FirstOrDefault();
        }

        private async Task<int> ValidateEmployeeDataMandateAndDuplicate(int userNum)
        {
            SqlParameter[] parameter = { new SqlParameter("@UserNum", userNum) };
            var result = await m_baseRepository.GetData<int>("[Talent].[USP_SLP_PUT_EmployeeDataMandateDuplicateValidation] @UserNum", parameter);
            return result.FirstOrDefault();
        }

        private async Task<int> ValidateEmployeeDataCircularRef(int userNum)
        {
            SqlParameter[] parameter = { new SqlParameter("@UserNum", userNum) };
            var result = await m_baseRepository.GetData<int>("[Talent].[USP_SLP_PUT_EmployeeDataCirucularRefValidation] @UserNum", parameter);
            return result.FirstOrDefault();
        }

        public async Task<int> ProcessEmployeeData(int userNum)
        {
            SqlParameter[] parameter = { new SqlParameter("@UserNum", userNum) };            
            return await m_baseRepository.ExecuteStoredProcedure("[Talent].[USP_SLP_PUT_ImportEmployeeData]", parameter);
        }

        public async Task<bool> ClearAllData(string tenantName)
        {
            bool success;
            try
            {
                await m_baseRepository.ExecuteStoredProcedure("[Talent].[USP_SLP_ClearAllDataScript]");
                var tenantNum = m_masterRepository.GetQuery<Master.Tenant>(m => m.TenantURLName == tenantName).Single().TenantNum;
                SqlParameter[] parameters = { new SqlParameter("@clearAll", true),
                                        new SqlParameter("@userID", "empty"),
                                        new SqlParameter("@tenantNum", tenantNum) };
               await  m_masterRepository.ExecuteStoredProcedure("[dbo].[USP_DEL_MasterUserList]", parameters);

                var userData = m_baseRepository.GetQuery<AppUser>().ToList();
                DataTable dataTable = new DataTable();
                dataTable.Columns.Add("UserId");
                dataTable.Columns.Add("TenantNum");
                foreach (var user in userData)
                {
                    var dataRow = dataTable.NewRow();
                    dataRow["UserId"] = user.UserID;
                    dataRow["TenantNum"] = tenantNum;
                    dataTable.Rows.Add(dataRow);
                }
                SqlParameter param = new SqlParameter("@UserList", dataTable);
                param.SqlDbType = SqlDbType.Structured;
                param.Direction = ParameterDirection.Input;
                await m_masterRepository.ExecuteStoredProcedure("USP_PUT_MasterUserList", new SqlParameter[] { param });
                success = true;
                m_tenantCacheProvider.RemoveCache(ApplicationCacheConstants.BussinessSetting);
            }
            catch
            {
                success = false;
            }

            return success;
        }

        public async Task<IEnumerable<TemplateErrorListModel>> GetEmployeeDataErrorList()
        {
            return await m_baseRepository.GetData<TemplateErrorListModel>("[Talent].[USP_SLP_GET_EmployeeErrorData]", new SqlParameter[] { });
        }

        public async Task<DataTable> GetEmployeeDataErrorExport()
        {
            return await m_baseRepository.GetDataTableFromStoredProcedure("[Talent].[USP_SLP_GET_EmployeeDataErrorExport]");
        }


        public async Task<IEnumerable<TemplateDataModel>> GetEmployeeDataGridDetails(int userNum)
        {
            SqlParameter[] parameters = { new SqlParameter("@UserNum", userNum),new SqlParameter("@TemplateName", "EmployeeData") };
            return await m_baseRepository.GetData<TemplateDataModel>("[Talent].[USP_SLP_GET_LoadedSheetDetails] @UserNum,@TemplateName", parameters);          
        }

        public async Task<int> DeleteEmployeeTemplateData(int xmlProcessNum)
        {
            SqlParameter[] parameters = { new SqlParameter("@XmlProcessNum", xmlProcessNum), new SqlParameter("@TemplateName", "EmployeeData") };
            return await m_baseRepository.ExecuteStoredProcedure("[Talent].[USP_SLP_DeleteLoadedSheetDetails]", parameters);
        }

        public async Task<int> GetEmployeeDataErrorCount()
        {
            SqlParameter[] parameter = { new SqlParameter("@TableName", "Talent.StagingEmployeeData") };
            var result = await m_baseRepository.GetData<int>("[Talent].[USP_SLP_GET_ErrorRecordCount] @TableName", parameter);
            return result.FirstOrDefault();
        }


        public async Task<bool> UpdateEmployeeData(decimal? budgetProration, EmployeeDetailsCorrection data,int stagingNumber,string currentYear)
        {
            try
            {
                m_baseRepository.Add<EmployeeDetailsCorrection>(data);
                bool result = m_baseRepository.SaveChanges() > 0;
                if (result)
                {
                    SqlParameter[] parameters = { new SqlParameter("@LoggedInUserNum", data.UserNum), new SqlParameter("@EmployeeDataCorrectionNum", data.EmployeeDataCorrectionNum), new SqlParameter("@StagingNumber", stagingNumber) };
                    await m_baseRepository.ExecuteStoredProcedure("[Talent].[USP_SLP_PUT_EmployeeDataCorrection]", parameters);
                }
                if(budgetProration!=null)
                {
                    var selectedEmployeeJobNum = m_baseRepository.GetQuery<EmployeeJob>(new string[] { "Employee" }, s => s.Employee.EmployeeID == data.EmployeeID).Select(z => z.EmpJobNum).FirstOrDefault();
                    Budget budgetData = m_baseRepository.GetQuery<Budget>(x => x.EmpJobNum == selectedEmployeeJobNum).FirstOrDefault();
                    budgetData.BudgetProration = budgetProration;
                    m_baseRepository.Edit<Budget>(budgetData);
                    return m_baseRepository.SaveChanges() > 0;
                }
                return result;
            }
            catch(Exception e)
            {
                return false;
            }            
        }

        public bool GetEmployeeCount(string employeeID)
        {
            return m_baseRepository.GetQuery<Employee>(x => x.EmployeeID == employeeID).ToList().Count() > 0;
        }

        public List<OrphanManagerDetails> GetOrphanEmployeeDetails()
        {
            return m_baseRepository.GetQuery<EmployeeJob>(new string[] { "Employee" }, x => x.ManagerNum == null && x.Employee.EmployeeID != "999999999").Select(x=>new OrphanManagerDetails
                                                           {
                                                               EmployeeNum = x.EmployeeNum,
                                                               EmployeeJobNum = x.EmpJobNum,
                                                               EmployeeId = x.Employee.EmployeeID,
                                                               EmployeeName = x.Employee.EmployeeName
                                                           }).ToList();          
        }

        public async Task<int> AssignEmployeeToCorporate(List<int> EmployeeJobNum)
        {
            var employeeJobNums = string.Join(",", EmployeeJobNum);
            SqlParameter[] paramsValues ={   new SqlParameter("@employeeJobNum", employeeJobNums),
                                          };
            return await m_baseRepository.ExecuteStoredProcedure("[Talent].[USP_SLP_PUT_AssignOrphanEmployeeToUnassigned]", paramsValues);

        }
        public IQueryable<RatingModel> GetRatings()
        {
            return m_baseRepository.GetQuery<Rating>(rating => rating.RatingType == CompensationConstants.Compensation).Select(m => new RatingModel
            {
                RatingNum = m.RatingNum,
                RatingID = m.RatingID,
                RatingDescr = m.RatingDescr,
                RatingOrder = m.RatingOrder,
                RatingRange = m.RatingRange

            });
        }
        public RuleConfiguration GetMeritConfiguration()
        {
            RuleConfiguration ruleConfiguration = new RuleConfiguration();
           // List<BusSetting> busSettings = m_baseRepository.GetQuery<BusSetting>().ToList();
            BusinessSettingModel busSettings = m_tenantCacheProvider.GetBusinessSetting();
            ruleConfiguration.ProrationRuleProrate = busSettings.ProrationRule.Prorate;
            ruleConfiguration.ProrationApplyAdjustmentCalculations = busSettings.ProrationRule.ApplyAdjustmentCalculations;
            ruleConfiguration.ProrationRuleProrateIncreaseStartDate = Convert.ToDateTime(busSettings.ProrationRule.ProrateIncreaseStartDate).ToLongDateString();
            ruleConfiguration.ProrationRuleProrateIncreaseEndDate = Convert.ToDateTime(busSettings.ProrationRule.ProrateIncreaseEndDate).ToLongDateString();
            ruleConfiguration.ProrationRuleProrationType = busSettings.ProrationRule.ProrationType;
            ruleConfiguration.ProrationRuleProrationLength = busSettings.ProrationRule.ProrationLength;
            ruleConfiguration.ProrationRuleProrationLengthtoInclude = busSettings.ProrationRule.ProrationLengthtoInclude;

            ruleConfiguration.BudgetProration = busSettings.BudgetProrationRule.BudgetProrate;

            string budgetProrateIncreaseStartDate = busSettings.BudgetProrationRule.BudgetProrateIncreaseStartDate;
            ruleConfiguration.BudgetProrateIncreaseStartDate = (budgetProrateIncreaseStartDate != "") ? Convert.ToDateTime(budgetProrateIncreaseStartDate).ToLongDateString() : "";

            string budgetProrateIncreaseEndDate = busSettings.BudgetProrationRule.BudgetProrateIncreaseEndDate;
            ruleConfiguration.BudgetProrateIncreaseEndDate = (budgetProrateIncreaseEndDate != "") ? Convert.ToDateTime(budgetProrateIncreaseEndDate).ToLongDateString() : "";

            ruleConfiguration.BudgetProrationType = busSettings.BudgetProrationRule.BudgetProrationType;

            
            ruleConfiguration.BudgetProrationDuration = busSettings.BudgetProrationRule.BudgetProrationDuration;

           
            ruleConfiguration.BudgetProrationDatesPerMonth = busSettings.BudgetProrationRule.BudgetProrationDatesPerMonth;

            ruleConfiguration.IsMultiCurrencyEnable = busSettings.FeatureConfiguration.MultiCurrency;
            ruleConfiguration.LumpSumRuleLumpSumType = busSettings.LumpsumRule.LumpsumType;
            ruleConfiguration.LumpSumRuleRangeMaxPct = busSettings.LumpsumRule.RangeMaxPct;
            ruleConfiguration.LumpSumRuleRangeMaxAmt = busSettings.LumpsumRule.RangeMaxAmt;
            ruleConfiguration.MeritValuesReCalculate = busSettings.LumpsumRule.MeritValuesReCalculate;
            ruleConfiguration.LumpSumRuleTurnOff = busSettings.LumpsumRule.LumpSumRuleTurnOff;
            ruleConfiguration.AutoCalculateLumpSum = busSettings.LumpsumRule.AutoCalculateLumpSum;
            ruleConfiguration.MeritCalculation = busSettings.FeatureConfiguration.MeritCalculation==true ? "YES" : "No";
            ruleConfiguration.ComparativeRatio = busSettings.FeatureConfiguration.ComparativeRatio;
            return ruleConfiguration;
        }

        public async Task<string> GetCirCularReference(string employeeID, string supervisorID, string payrollStatus)
        {
            SqlParameter[] paramsValues ={   new SqlParameter("@employeeID", employeeID),new SqlParameter("@SupervisorID", supervisorID),new SqlParameter("@payrollStatus", payrollStatus)
                                          };
            var status = await m_baseRepository.GetData<string>("[Talent].[USP_SLP_GET_CirCularReference] @employeeID,@SupervisorID,@payrollStatus", paramsValues);
            return status.FirstOrDefault();
        }

        public async Task<DataTable> GetExportXmlFile(int xmlProcessNum)
        {
            SqlParameter[] parameters = { new SqlParameter("@XmlProcessNum", xmlProcessNum),
                                        new SqlParameter("@TableName", "Talent.StagingEmployeeData") };
            return await m_baseRepository.GetDataTableFromStoredProcedure("[Talent].[USP_SLP_GET_LoadedDataExport]", parameters);
        }
        public async Task<DataTable> GetExportTrainingData()
        {
            SqlParameter[] parameters = { new SqlParameter("@XmlProcessNum", 1),
                                        new SqlParameter("@TableName", "Talent.StagingEmployeeData") };
            return await m_baseRepository.GetDataTableFromStoredProcedure("[Talent].[USP_SLP_GET_LoadedDataExport]", parameters);
        }
        
        // Author       : Shaheena Shaik
        // Creation date:4-July-2017
        /// <summary>
        /// Validating PayCurrency whether it is already exists in database or not
        /// </summary>
        /// <param name="payCurrencyCode">The currencyCode which is newly added</param>
        /// <returns></returns>
        public async Task<string> payCurrencyValidation(string payCurrencyCode)
        {
            SqlParameter[] parameters ={new SqlParameter("@payCurrency", payCurrencyCode)};
            var payCurrencystatus = await m_baseRepository.GetData<string>("[Talent].[USP_SLP_PUT_PayCurrencyValidation] @payCurrency", parameters);
            return payCurrencystatus.FirstOrDefault();
   
        }

        #region PrivateMethods
        private bool setTrueorFalse(string value)
        {
            return (value != null && value.ToLower().Trim() == "yes");
        }

        private decimal getNumberValue(string value)
        {
            if (value == "")
                return 0;
            else
                return  Convert.ToDecimal(value);            
        }
        #endregion

    }
}
