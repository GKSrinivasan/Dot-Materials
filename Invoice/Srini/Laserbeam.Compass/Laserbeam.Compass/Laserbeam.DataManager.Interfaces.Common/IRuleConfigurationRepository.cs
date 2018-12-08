// Copyright (c) 2016 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.
// Component Name  :  IRuleConfigurationRepository
// Description     :  Interface signature for RuleConfigurationRepository
// Author         : Raja Ganapathy
// Creation Date  :  30-Mar-2017


using Laserbeam.BusinessObject.Common;
using Laserbeam.EntityManager.Common;
using System.Linq;
using System.Threading.Tasks;

namespace Laserbeam.DataManager.Interfaces.Common
{
    public interface IRuleConfigurationRepository
    {
        IQueryable<DateFormat> GetDateFormats(); 
        Task PutBusSetting(BusSettingModel BusSettingDetails, int UserNum);
        BusSettingModel GetBusSetting();
        IQueryable<RoundingType> GetRoundingType();
        IQueryable<DecimalType> GetDecimalType();
        Task<bool> PutApplyRules();
        decimal GetBudgetPct();
        bool PutWizardDetails(int userNum, byte stepInfo, bool isWizard);
        byte GetWizardDetails(int userNUm);
        //bool ClearAllData();
        int UpdateExchangeRate(Countries exchangeRates, string baseCurrency);
        bool CheckMultiCurrencyChanged(string key, string value);
        Task RunBuildManagerTree();
        Task<bool> clearApprovalDetails();
    }
}
