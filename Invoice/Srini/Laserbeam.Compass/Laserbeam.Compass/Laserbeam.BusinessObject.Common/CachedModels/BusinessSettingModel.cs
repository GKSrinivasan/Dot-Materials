
// Copyright (c) 2014 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.

// Component Name :   BusinessSettingModel
// Description    :   Model that converts BusSetting table to BusinessSettingModel
// Author         :   Hariharasubramaniyan Chandrasekaran		
// Creation Date  :   09-March-2018
// Ticket ID      :   CL-1288

using Laserbeam.BusinessObject.Common.Constants;
using Laserbeam.EntityManager.Common;
using System.Collections.Generic;
using System.Linq;

namespace Laserbeam.BusinessObject.Common.CachedModels
{
    public  class BusinessSettingModel
    {
        public readonly BudgetConfigurationModel BudgetConfiguration;
        public readonly BudgetProrationRuleModel BudgetProrationRule;
        public readonly FeatureConfigurationModel FeatureConfiguration;
        public readonly GeneralConfigurationModel GeneralConfiguration;
        public readonly LumpSumRuleModel LumpsumRule;
        public readonly MeritOverrideRuleModel MeritOverrideRule;
        public readonly NamingConfigurationModel NamingFormatConfiguration;
        public readonly MeritProrationRuleModel ProrationRule;
        public readonly CurrencyConfigurationModel CurrencyConfiguration;
        public readonly DateConfigurationModel DateConfiguration;
        public readonly SSOConfigurationModel SSOConfiguration;

        // Author         :   Hariharasubramaniyan Chandrasekaran
        // Creation Date  :   09-March-2018
        // Ticket ID      :   CL-1288
        /// <summary>
        /// Constructor of BusinessSettingModel
        /// </summary>
        /// <param name="busSetting">List of Budget data</param>
        public BusinessSettingModel(IEnumerable<BusSetting> busSetting)
        {
            BudgetConfiguration = new BudgetConfigurationModel(busSetting.Where(m => m.KeyId == BusSettingGroupConstants.BudgetConfiguration));
            BudgetProrationRule = new BudgetProrationRuleModel(busSetting.Where(m => m.KeyId == BusSettingGroupConstants.BudgetProrationRule));
           CurrencyConfiguration= new CurrencyConfigurationModel(busSetting.Where(m => m.KeyId == BusSettingGroupConstants.CurrencyConfiguration));
            DateConfiguration = new DateConfigurationModel(busSetting.Where(m => m.KeyId == BusSettingGroupConstants.DateFormatConfiguration));
            FeatureConfiguration = new FeatureConfigurationModel(busSetting.Where(m => m.KeyId == BusSettingGroupConstants.FeatureConfiguration));
            GeneralConfiguration = new GeneralConfigurationModel(busSetting.Where(m => m.KeyId == BusSettingGroupConstants.GeneralConfiguration));
            LumpsumRule = new LumpSumRuleModel(busSetting.Where(m => m.KeyId == BusSettingGroupConstants.LumpsumRule));
            MeritOverrideRule = new MeritOverrideRuleModel(busSetting.Where(m => m.KeyId == BusSettingGroupConstants.MeritOverrideRule));
            NamingFormatConfiguration = new NamingConfigurationModel(busSetting.Where(m => m.KeyId == BusSettingGroupConstants.NamingFormatConfiguration));
            ProrationRule = new MeritProrationRuleModel(busSetting.Where(m => m.KeyId == BusSettingGroupConstants.ProrationRule));
            SSOConfiguration = new SSOConfigurationModel(busSetting.Where(m => m.KeyId == BusSettingGroupConstants.SSOConfiguration));
        }

    }
}
