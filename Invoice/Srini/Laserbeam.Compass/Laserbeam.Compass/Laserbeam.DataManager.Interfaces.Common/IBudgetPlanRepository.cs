// Copyright (c) 2015 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.

// Component Name :   IBudgetPlanRepository 
// Description    :   Interface signature for BudgetPlanRepository
// Author         :   Hariharasubramaniyan Chandrasekaran	
// Creation Date  :   10-Feb-2017 
using Laserbeam.BusinessObject.Common;
using Laserbeam.BusinessObject.Common.CachedModels;
using Laserbeam.EntityManager.Common;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Laserbeam.DataManager.Interfaces.Common
{
    public interface IBudgetPlanRepository
    {

        // Author         :   Hariharasubramaniyan Chandrasekaran	
        // Creation Date  :   10-Feb-2017 
        /// <summary>
        /// Gets the data to bind to budget grid
        /// </summary>
        /// <returns>Returns enumerable of BudgetPlanGridData</returns>
       Task<IEnumerable<BudgetPlanGridData>> GetBudgetGridData(int loggedInEmployeeNum, int selectedCurrencyNum);

       //Task<DataSet> GetExportBudgetData();
       Task<DataTable> GetExportBudgetData(int loggedInEmployeeNum, int selectedCurrencyNum);

       // Created By    :   
       // Created Date  :  
       // Comment       :   GetBusSettingData
       //IEnumerable<BusSetting> GetBusSettingData();
        BusinessSettingModel GetBusSettingData();


       // Author         :   Hariharasubramaniyan Chandrasekaran	
       // Creation Date  :   10-Feb-2017 
       /// <summary>
       /// Updates employee budget data
       /// </summary>
       /// <param name="newBudget">New Budget data to be updated</param>
       /// <param name="managerNum">ManagerNum for whom budget to be updated</param>
       /// <param name="divisionNum">DivionNum of managerNum</param>
       /// <returns>Status of record updated</returns>
        Task<bool> UpdateEmployeeBudget(List<BudgetPlanGridData> BudgetPlanGridData, int UserNum);

         Task<IEnumerable<ExchangeCurrencies>> GetCurrencies();
        bool IsBudgetDataEmpty();
        // Author         :   
        // Creation Date  :   
        /// <summary>
        /// Fetches the base Currency  details
        /// </summary>
        /// <returns>Currency data</returns>
        CountryCodeModel GetCountryData(string baseCurrency);

       //Task<BudgetPlanData> GetBudgetData(int selectedCurrencyNum);

       // Author         :  
       // Creation Date  :   
       /// <summary>
       /// Update budget percentage
       /// </summary>
       /// <param name="BudgetPercent">New budget percentage to be updated</param>
       /// <param name="UserNum">Loggedin UserNum</param>
       /// <returns></returns>
       Task<bool> PutBudgetPct(string BudgetPercent, int UserNum,bool isProration, string filteredEmployee);
       Task<bool> ClearProrationValues(string isProration, int UserNum);
       Task<bool> PutBudgetProration(BudgetProrationUpdateModel budgetProration, int userNum);
    }
}
