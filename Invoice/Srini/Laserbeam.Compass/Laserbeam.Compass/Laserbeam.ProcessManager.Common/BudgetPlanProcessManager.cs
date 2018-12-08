// Copyright (c) 2015 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.

// Component Name :   BudgetPlanProcessManager 
// Description    :   Allows to perform various operation on  Budget Planning. 	
// Author         :   Hariharasubramaniyan Chandrasekaran	
// Creation Date  :   10-Feb-2017 
using Laserbeam.BusinessObject.Common;
using Laserbeam.BusinessObject.Common.CachedModels;
using Laserbeam.Constant.HR;
using Laserbeam.DataManager.Interfaces.Common;
using Laserbeam.EntityManager.Common;
using Laserbeam.ProcessManager.Interfaces.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Laserbeam.ProcessManager.Common
{
    public class BudgetPlanProcessManager : IBudgetPlanProcessManager
    {
        #region Fields
        // Author         :   Hariharasubramaniyan Chandrasekaran	
        // Creation Date  :   10-Feb-2017 
        /// <summary>
        /// Instance of BudgetPlanRepository
        /// </summary>
        private IBudgetPlanRepository m_budgetPlanRepository;
       
        #endregion

        #region Constructor

        // Author         :   Hariharasubramaniyan Chandrasekaran	
        // Creation Date  :   10-Feb-2017
        /// <summary>
        /// Constructor for BudgetPlanProcessManager
        /// </summary>
        /// <param name="groupRepository">Instance of ConfigurationRepository</param>
        public BudgetPlanProcessManager(IBudgetPlanRepository budgetPlanRepository)
        {
            m_budgetPlanRepository = budgetPlanRepository;
        }

        #endregion

        #region BudgetPlan Implementation

        // Author         :   Hariharasubramaniyan Chandrasekaran	
        // Creation Date  :   10-Feb-2017
        /// <summary>
        /// Gets enumerable of budget plan grid data
        /// </summary>
        /// <returns>Returns enumerable of budget plan grid data</returns>

        public async Task<IEnumerable<BudgetPlanGridData>> GetEmployeeBudget(int loggedInEmployeeNum, int selectedCurrencyNum)
        {
            return await m_budgetPlanRepository.GetBudgetGridData(loggedInEmployeeNum,selectedCurrencyNum );
        }

        public async Task<DataTable> GetExportBudgetData(int loggedInEmployeeNum, int selectedCurrencyNum)
        {
            return await m_budgetPlanRepository.GetExportBudgetData(loggedInEmployeeNum, selectedCurrencyNum);
        }

        // Comment        :   Put the budget pct based on multicountry
       
        /// <summary>
        /// Update budget percentage
        /// </summary>
        /// <param name="BudgetPercent">New budget percentage to be updated</param>
        /// <param name="UserNum">Loggedin UserNum</param>
        /// <returns></returns>
        public async Task<bool> PutBudgetPct(string BudgetPercent, int UserNum,bool isProration,string filteredEmployee)
        {
            return await m_budgetPlanRepository.PutBudgetPct(BudgetPercent, UserNum, isProration, filteredEmployee);
        }
        public async Task<bool> ClearProrationValues(string isProration, int UserNum)
        {
            return await m_budgetPlanRepository.ClearProrationValues(isProration, UserNum);
        }
        public async Task<bool> PutBudgetProration(BudgetProrationUpdateModel budgetProration, int userNum)
        {
            return await m_budgetPlanRepository.PutBudgetProration(budgetProration,userNum);
        }
        
        public async Task<IEnumerable<ExchangeCurrencies>> GetCurrencies()
        {
            return await m_budgetPlanRepository.GetCurrencies();
        }
        public  bool IsBudgetDataEmpty()
        {
            return  m_budgetPlanRepository.IsBudgetDataEmpty();

        }
        // Author         :   
        // Creation Date  :  
        /// <summary>
        /// Fetches the base Currency  details
        /// </summary>
        /// <returns>Currency data</returns>
        public CountryCodeModel GetBaseCountryDesc(string  baseCurrency)
        {
            return m_budgetPlanRepository.GetCountryData(baseCurrency);
        }

        public BudgetPlanConfiguration GetBudgetPlanConfiguration()
        {
            BudgetPlanConfiguration budgetPlanConfiguration = new BudgetPlanConfiguration();
            BusinessSettingModel busSettings = m_budgetPlanRepository.GetBusSettingData();
            budgetPlanConfiguration.RoundingBudgetPercentage = busSettings.BudgetConfiguration.RoundingBudgetPerc;
            budgetPlanConfiguration.RoundingBudgetDoller = busSettings.BudgetConfiguration.RoundingBudgetDol;
            budgetPlanConfiguration.DecimalBudgetPercentage = busSettings.BudgetConfiguration.DecimalBudgetPerc.ToString();
            budgetPlanConfiguration.DecimalBudgetDoller = busSettings.BudgetConfiguration.DecimalBudgetDol.ToString();
            budgetPlanConfiguration.BudgetProration = busSettings.BudgetProrationRule.BudgetProrate;
            budgetPlanConfiguration.BudgetCurrencyFormat = busSettings.CurrencyConfiguration.CurrencyFormat;
            budgetPlanConfiguration.isEnableMerit = busSettings.FeatureConfiguration.Merit;
            budgetPlanConfiguration.isEnablePromotion = busSettings.FeatureConfiguration.Promotion;
            budgetPlanConfiguration.isEnableLumpSum = busSettings.FeatureConfiguration.Lumpsum;
            budgetPlanConfiguration.isEnableAdjustment = busSettings.FeatureConfiguration.Adjustment; 
            return budgetPlanConfiguration;
        }

        public BudgetProration GetBudgetProration()
        {
            BudgetProration budgetProration = new BudgetProration();
            BusinessSettingModel busSettings = m_budgetPlanRepository.GetBusSettingData();
            budgetProration.BudgetProrationValue = busSettings.BudgetProrationRule.BudgetProrate;
            budgetProration.ProrateStartDate = Convert.ToDateTime(busSettings.BudgetProrationRule.BudgetProrateIncreaseStartDate);
            budgetProration.ProrateEndDate = Convert.ToDateTime(busSettings.BudgetProrationRule.BudgetProrateIncreaseEndDate);
            budgetProration.ProrationType = busSettings.BudgetProrationRule.BudgetProrationType;
            budgetProration.ProrationDuration = busSettings.BudgetProrationRule.BudgetProrationDuration.ToString();
            budgetProration.ProrationDatesPerMonth = busSettings.BudgetProrationRule.BudgetProrationDatesPerMonth.ToString();
            return budgetProration;
        }

        // Author         :   Hariharasubramaniyan Chandrasekaran	
        // Creation Date  :   10-Feb-2017
        /// <summary>
        /// updates Employee budget data
        /// </summary>
        /// <param name="newBudget">new Budget data to be updated</param>
        /// <param name="managerNum">managerNum for whom budget to be updated</param>
        /// <param name="divisionNum">divionNum of managerNum</param>
        /// <returns>status of record updated</returns>
        public async Task<bool> UpdateEmployeeBudget(List<BudgetPlanGridData> BudgetPlanGridData, int UserNum)
        {
            return await m_budgetPlanRepository.UpdateEmployeeBudget(BudgetPlanGridData, UserNum);
        }
        #endregion
    }
}
