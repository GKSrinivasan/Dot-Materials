// Copyright (c) 2015 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.

// Component Name :   BudgetPlan Repository 
// Description    :   Allows to perform various operation on  Budget Planning. 	
// Author         :   Hariharasubramaniyan Chandrasekaran	
// Creation Date  :   10-Feb-2017 

using Laserbeam.BusinessObject.Common;
using Laserbeam.BusinessObject.Common.CachedModels;
using Laserbeam.BusinessObject.Common.Constants;
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
    public class BudgetPlanRepository : IBudgetPlanRepository
    {
        #region Fields
        // Author         :   Hariharasubramaniyan Chandrasekaran	
        // Creation Date  :   10-Feb-2017 
        /// <summary>
        /// Instance of BaseRepository
        /// </summary>
        private IBaseRepository m_baseRepository;
        private ITenantCacheProvider m_tenantCacheProvider;

        #endregion

        #region Constructor
        // Author         :   Hariharasubramaniyan Chandrasekaran	
        // Creation Date  :   10-Feb-2017 
        /// <summary>
        /// Constructor to create an instance of ConfigurationRepository
        /// </summary>
        /// <param name="baseRepository">Instance of BaseRepository</param>
        public BudgetPlanRepository(IBaseRepository baseRepository, ITenantCacheProvider tenantCacheProvider)
        {
            m_baseRepository = baseRepository;
            m_tenantCacheProvider = tenantCacheProvider;
        }

        #endregion

        #region Implementation
        // Author         :   Hariharasubramaniyan Chandrasekaran	
        // Creation Date  :   10-Feb-2017 
        /// <summary>
        /// Gets the data to bind to budget grid
        /// </summary>
        /// <returns>Returns enumerable of BudgetPlanGridData</returns>
        public async Task<IEnumerable<BudgetPlanGridData>> GetBudgetGridData(int loggedInEmployeeNum, int selectedCurrencyNum)
        {

            SqlParameter[] sqlParameter = {   new SqlParameter("@LoggedInEmployeeNum",loggedInEmployeeNum),
                                              new SqlParameter("@SelectedCurrencyNum",selectedCurrencyNum)                                            
                                                                                         
                                          };

            var a= await m_baseRepository.GetData<BudgetPlanGridData>("Talent.USP_BP_Get_BudgetPlanList @LoggedInEmployeeNum,@SelectedCurrencyNum", sqlParameter);
            return a;
        }
        // Created By    :  Balamurugan M
        // Created Date  :  31-July-2017 
        // Comment       :  Get BudgetPlan Export Data
        public async Task<DataTable> GetExportBudgetData(int loggedInEmployeeNum, int selectedCurrencyNum)
        {
               SqlParameter[] sqlParameter = {   new SqlParameter("@LoggedInEmployeeNum",loggedInEmployeeNum),
                                              new SqlParameter("@SelectedCurrencyNum",selectedCurrencyNum)                                            
                                                                                         
                                          };
            var BudgetPlanExport = await m_baseRepository.GetDataTableFromStoredProcedure("[Talent].[USP_BP_Get_ManagerWiseBudgetPlanExport]",sqlParameter);
            return BudgetPlanExport;
        }

        // Created By    :  
        // Created Date  :   
        // Comment       :   Get Configuration Data
        public BusinessSettingModel GetBusSettingData()
        {
            return m_tenantCacheProvider.GetBusinessSetting();
            
        }

        // Author         :   
        // Creation Date  :  
        /// <summary>
        /// Fetches the base Currency  details
        /// </summary>
        /// <returns>Currency data</returns>
        public CountryCodeModel GetCountryData(string baseCurrency)
        {   var currency = m_baseRepository.GetQuery<Currency>(s => s.Active == true);
            var exchangeRate = m_baseRepository.GetQuery<ExchangeRate>(s => s.Active == true);
       
            return (from cur in currency
                    join er in exchangeRate on cur.CurrencyCodeNum equals er.CurrencyCodeNum
                    where cur.CurrencyCode== baseCurrency
                    select new CountryCodeModel
                    {
                        CurrencyCode = cur.CurrencyCode,
                        CurrencyNum = cur.CurrencyCodeNum,
                        CultureCode = cur.CultureCode,
                        BaseExchangeRate=er.MeritExchangeRate
                    }).FirstOrDefault();
        }

        // Author         :  
        // Creation Date  :  

        /// <summary>
        /// Update budget percentage
        /// </summary>
        /// <param name="BudgetPercent">New budget percentage to be updated</param>
        /// <param name="UserNum">Loggedin UserNum</param>
        /// <returns></returns>
        public async Task<bool> PutBudgetPct(string BudgetPercent, int UserNum, bool isProration, string filteredEmployee)
        {
            int count = 0;

                List<BusSetting> busSetting = m_baseRepository.GetQuery<BusSetting>().ToList();
                var busSettingData = busSetting.Where(x => x.KeyValue.Trim() == "BudgetPercent").FirstOrDefault();
                if (busSettingData.KeyDataValue != BudgetPercent.Trim())
                {
                    busSettingData.KeyDataValue = BudgetPercent.Trim();
                    busSettingData.UpdatedBy = UserNum;
                    busSettingData.UpdatedDate = DateTime.Now;
                    busSettingData.IsChanged = true;
                    m_baseRepository.Edit<BusSetting>(busSettingData);
                    await m_baseRepository.SaveChangesAsync();
                m_tenantCacheProvider.RemoveCache(ApplicationCacheConstants.BussinessSetting);
                }

            SqlParameter[] parameter = { 
                new SqlParameter ("@FilteredEmployee",filteredEmployee)
            };

                count = await m_baseRepository.ExecuteStoredProcedure("Talent.USP_BP_PUT_ApplyBudgetPercentage", parameter);
           

            return count > 0;
        }

        public async Task<bool> ClearProrationValues(string isProration, int UserNum)
        {
            int count = 0;
            count = await m_baseRepository.ExecuteStoredProcedure("Talent.USP_BP_ClearBudgetProration");
            return count > 0;
        }


        public async Task<bool> PutBudgetProration(BudgetProrationUpdateModel budgetProration, int userNum)
        {
            int count = 0;
            var isProration = "YES";
          
            List<BusSetting> busSetting = m_baseRepository.GetQuery<BusSetting>().ToList();
            var busSettingData = busSetting.Where(x => x.KeyValue.Trim() == "BudgetProrate").FirstOrDefault();
                busSettingData.KeyDataValue = isProration.Trim();
                busSettingData.UpdatedBy = userNum;
                busSettingData.UpdatedDate = DateTime.Now;
                busSettingData.IsChanged = true;
                m_baseRepository.Edit<BusSetting>(busSettingData);
                await m_baseRepository.SaveChangesAsync();
            var processStartData = busSetting.Where(x => x.KeyValue.Trim() == "BudgetProrateIncreaseStartDate").FirstOrDefault();
          
                processStartData.KeyDataValue = Convert.ToDateTime(budgetProration.ProrateStartDate).ToString("yyyy-MM-dd").Trim();
                processStartData.UpdatedBy = userNum;
                processStartData.UpdatedDate = DateTime.Now;
                processStartData.IsChanged = true;
                m_baseRepository.Edit<BusSetting>(processStartData);
                await m_baseRepository.SaveChangesAsync();
          
            var processEndData = busSetting.Where(x => x.KeyValue.Trim() == "BudgetProrateIncreaseEndDate").FirstOrDefault();
          
                processEndData.KeyDataValue = Convert.ToDateTime(budgetProration.ProrateEndDate).ToString("yyyy-MM-dd").Trim();
                processEndData.UpdatedBy = userNum;
                processEndData.UpdatedDate = DateTime.Now;
                processEndData.IsChanged = true;
                m_baseRepository.Edit<BusSetting>(processEndData);
                await m_baseRepository.SaveChangesAsync();
          
            var prorationTypes = busSetting.Where(x => x.KeyValue.Trim() == "BudgetProrationType").FirstOrDefault();
         
                prorationTypes.KeyDataValue = budgetProration.ProrationType;
                prorationTypes.UpdatedBy = userNum;
                prorationTypes.UpdatedDate = DateTime.Now;
                prorationTypes.IsChanged = true;
                m_baseRepository.Edit<BusSetting>(prorationTypes);
                await m_baseRepository.SaveChangesAsync();
        
            var prorationDurations = busSetting.Where(x => x.KeyValue.Trim() == "BudgetProrationDuration").FirstOrDefault();
          
                prorationDurations.KeyDataValue = budgetProration.ProrationDuration.ToString().Trim();
                prorationDurations.UpdatedBy = userNum;
                prorationDurations.UpdatedDate = DateTime.Now;
                prorationDurations.IsChanged = true;
                m_baseRepository.Edit<BusSetting>(prorationDurations);
                await m_baseRepository.SaveChangesAsync();
          

            var prorationDates = busSetting.Where(x => x.KeyValue.Trim() == "BudgetProrationDatesPerMonth").FirstOrDefault();
          
                prorationDates.KeyDataValue = budgetProration.ProrationDatesPerMonth!=null?budgetProration.ProrationDatesPerMonth.ToString().Trim():"";
                prorationDates.UpdatedBy = userNum;
                prorationDates.UpdatedDate = DateTime.Now;
                prorationDates.IsChanged = true;
                m_baseRepository.Edit<BusSetting>(prorationDates);
                await m_baseRepository.SaveChangesAsync();
            m_tenantCacheProvider.RemoveCache(ApplicationCacheConstants.BussinessSetting);

            //
            if (budgetProration.IsMerit == true)
            {
                var meritBusSetting = busSetting.Where(x => x.KeyValue.Trim() == "Prorate").FirstOrDefault();
              
                    meritBusSetting.KeyDataValue = isProration.Trim();
                    meritBusSetting.UpdatedBy = userNum;
                    meritBusSetting.UpdatedDate = DateTime.Now;
                    meritBusSetting.IsChanged = true;
                    m_baseRepository.Edit<BusSetting>(meritBusSetting);
                    await m_baseRepository.SaveChangesAsync();
              


                var meritProcessStartData = busSetting.Where(x => x.KeyValue.Trim() == "ProrateIncreaseStartDate").FirstOrDefault();
            
                    meritProcessStartData.KeyDataValue = Convert.ToDateTime(budgetProration.ProrateStartDate).ToString("yyyy-MM-dd").Trim();
                    meritProcessStartData.UpdatedBy = userNum;
                    meritProcessStartData.UpdatedDate = DateTime.Now;
                    meritProcessStartData.IsChanged = true;
                    m_baseRepository.Edit<BusSetting>(meritProcessStartData);
                    await m_baseRepository.SaveChangesAsync();
              
                var meritProcessEndData = busSetting.Where(x => x.KeyValue.Trim() == "ProrateIncreaseEndDate").FirstOrDefault();
               
                    meritProcessEndData.KeyDataValue = Convert.ToDateTime(budgetProration.ProrateEndDate).ToString("yyyy-MM-dd").Trim();
                   meritProcessEndData.UpdatedBy = userNum;
                    meritProcessEndData.UpdatedDate = DateTime.Now;
                    meritProcessEndData.IsChanged = true;
                    m_baseRepository.Edit<BusSetting>(meritProcessEndData);
                    await m_baseRepository.SaveChangesAsync();
               
                var meritProrationTypes = busSetting.Where(x => x.KeyValue.Trim() == "ProrationType").FirstOrDefault();
              
                    meritProrationTypes.KeyDataValue = budgetProration.ProrationType.Trim();
                    meritProrationTypes.UpdatedBy = userNum;
                    meritProrationTypes.UpdatedDate = DateTime.Now;
                    meritProrationTypes.IsChanged = true;
                    m_baseRepository.Edit<BusSetting>(meritProrationTypes);
                    await m_baseRepository.SaveChangesAsync();
             
                var meritProrationDurations = busSetting.Where(x => x.KeyValue.Trim() == "ProrationLength").FirstOrDefault();
               
                    meritProrationDurations.KeyDataValue = budgetProration.ProrationDuration.ToString().Trim();
                    meritProrationDurations.UpdatedBy = userNum;
                    meritProrationDurations.UpdatedDate = DateTime.Now;
                    meritProrationDurations.IsChanged = true;
                    m_baseRepository.Edit<BusSetting>(meritProrationDurations);
                    await m_baseRepository.SaveChangesAsync();
              

                var meritProrationDates = busSetting.Where(x => x.KeyValue.Trim() == "ProrationLengthtoInclude").FirstOrDefault();
                  meritProrationDates.KeyDataValue =(budgetProration.ProrationDatesPerMonth==null)?"0":budgetProration.ProrationDatesPerMonth.ToString().Trim();
                    meritProrationDates.UpdatedBy = userNum;
                    meritProrationDates.UpdatedDate = DateTime.Now;
                    meritProrationDates.IsChanged = true;
                    m_baseRepository.Edit<BusSetting>(meritProrationDates);
                    await m_baseRepository.SaveChangesAsync();
                m_tenantCacheProvider.RemoveCache(ApplicationCacheConstants.BussinessSetting);
                List<MetaColumn> data = m_baseRepository.GetQuery<MetaColumn>(x => x.FieldName == "MeritProrationFactor" || x.FieldName == "MeritProrationDate").ToList();
                var metaData = data.Where(x => x.FieldName == "MeritProrationFactor").FirstOrDefault();
                metaData.TemplateDisplay = true;
                m_baseRepository.Edit<MetaColumn>(metaData);
                var metaColumnData = data.Where(x => x.FieldName == "MeritProrationDate").FirstOrDefault();
                metaColumnData.TemplateDisplay = true;
                m_baseRepository.Edit<MetaColumn>(metaColumnData);
                await m_baseRepository.SaveChangesAsync();

                count = await m_baseRepository.ExecuteStoredProcedure("Common.USP_RC_PUT_ApplyProration");
            }
            count = await m_baseRepository.ExecuteStoredProcedure("Talent.USP_BP_PUT_ApplyBudgetProration");



            return count > 0;
        }

        // Author         :   Hariharasubramaniyan Chandrasekaran	
        // Creation Date  :   10-Feb-2017 
        /// <summary>
        /// Updates Employee budget data
        /// </summary>
        /// <param name="newBudget">New Budget data to be updated</param>
        /// <param name="managerNum">ManagerNum for whom budget to be updated</param>
        /// <param name="divisionNum">DivionNum of managerNum</param>
        /// <returns>Status of record updated</returns>
        public async Task<bool> UpdateEmployeeBudget(List<BudgetPlanGridData> BudgetPlanGridData, int UserNum)
        {
            var data = new DataTable("BudgetParamTable");
            data.Columns.Add("ManagerNum", typeof(int));

            data.Columns.Add("CountryNum", typeof(int));
            data.Columns[1].AllowDBNull = true;

            data.Columns.Add("BudgetPct", typeof(decimal));
            data.Columns[2].AllowDBNull = true;

            data.Columns.Add("Budget", typeof(decimal));
            data.Columns[3].AllowDBNull = true;

            data.Columns.Add("UpdatedBy", typeof(int));
            data.Columns[4].AllowDBNull = true;

            foreach (var item in BudgetPlanGridData)
            {
                DataRow dr = data.NewRow();
                dr["Budget"] = item.AdjustedBudget;
                dr["ManagerNum"] = item.ManagerNum;
                dr["CountryNum"] = item.CountryNum;
                dr["BudgetPct"] = item.AdjustedBudgetPct;
                dr["UpdatedBy"] = UserNum;
                data.Rows.Add(dr);
            }
            SqlParameter parameter = new SqlParameter("@BudgetParamTable", data);
            parameter.SqlDbType = SqlDbType.Structured;
            parameter.Direction = ParameterDirection.Input;
            SqlParameter[] parameters = new SqlParameter[] { parameter };
            var rows = await m_baseRepository.ExecuteStoredProcedure("Talent.USP_BP_PUT_UpdateBudgetPlan", parameters);
            return rows > 0;
        }
        public async Task<IEnumerable<ExchangeCurrencies>> GetCurrencies()
        {
            SqlParameter[] sqlParameter = { };
            var currencies = await m_baseRepository.GetData<ExchangeCurrencies>("[Talent].[USP_Comp_Get_CurrencyData]", sqlParameter);
            return currencies;
        }
        public bool IsBudgetDataEmpty()
        {
            return m_baseRepository.GetQuery<Budget>().ToList().Count <=0 ? true : false;
        }
        #endregion
    }
}
