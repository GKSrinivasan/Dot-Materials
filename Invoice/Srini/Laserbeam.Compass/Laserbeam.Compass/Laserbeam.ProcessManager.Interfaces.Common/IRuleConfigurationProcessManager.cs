// Copyright (c) 2015 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.

// Component Name :   IRuleConfigurationProcessManager
// Description    :   Interface signature for RuleConfigurationProcessManager
// Author         :   Raja Ganapathy
// Creation Date  :   30-Mar-201


using Laserbeam.BusinessObject.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Laserbeam.ProcessManager.Interfaces.Common
{
    public interface IRuleConfigurationProcessManager
    {
        List<DropDownListModel> GetDateFormats();  
        Task PutBusSetting(BusSettingModel BusSettingDetails, int UserNum);
        BusSettingModel GetBusSetting();
        List<RoundingTypeModel> GetRoundingType();
        List<DecimalTypeModel> GetDecimalType();
        Task<bool> PutApplyRules();
        decimal GetBudgetPct();
        bool PutWizardDetails(int userNum,byte stepInfo,bool isWizard);
        byte GetWizardDetails(int userNUm);
        //bool clearAllData();
        int UpdateExchangeRate(Countries exchangeRate, string baseCurrency);
        bool CheckMultiCurrencyChanged(string key, string value);
        Task RunBuildManagerTree();
        Task<bool> clearApprovalDetails();
    }
}
