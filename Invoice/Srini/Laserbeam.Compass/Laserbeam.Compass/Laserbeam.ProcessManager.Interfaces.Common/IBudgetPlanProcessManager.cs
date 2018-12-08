// Copyright (c) 2015 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.

// Component Name :   IBudgetPlanProcessManager
// Description    :   Interface signature for BudgetPlanProcessManager
// Author         :   Hariharasubramaniyan Chandrasekaran	
// Creation Date  :   10-Feb-2017 
using Laserbeam.BusinessObject.Common;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Laserbeam.ProcessManager.Interfaces.Common
{
    public interface IBudgetPlanProcessManager
    {


       Task<IEnumerable<BudgetPlanGridData>> GetEmployeeBudget(int loggedInEmployeeNum, int selectedCurrencyNum);
       Task<bool> PutBudgetPct(string BudgetPercent, int UserNum,bool isProration, string filteredEmployee);
       Task<bool> ClearProrationValues(string isProration, int UserNum);
       Task<bool> UpdateEmployeeBudget(List<BudgetPlanGridData> BudgetPlanGridData, int UserNum);
        Task<IEnumerable<ExchangeCurrencies>> GetCurrencies();
       BudgetPlanConfiguration GetBudgetPlanConfiguration();
       BudgetProration GetBudgetProration();
       Task<bool> PutBudgetProration(BudgetProrationUpdateModel budgetProration,int userNum);
       CountryCodeModel GetBaseCountryDesc(string baseCurrency);
       Task<DataTable> GetExportBudgetData(int loggedInEmployeeNum, int selectedCurrencyNum);
        bool IsBudgetDataEmpty();
      
    }
}
