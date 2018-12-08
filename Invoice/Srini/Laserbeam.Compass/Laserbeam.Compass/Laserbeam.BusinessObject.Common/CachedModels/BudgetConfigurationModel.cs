
// Copyright (c) 2014 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.

// Component Name :   BudgetConfigurationModel
// Description    :   Model that converts BusSetting table to BudgetConfigurationModel
// Author         :   Hariharasubramaniyan Chandrasekaran		
// Creation Date  :   08-March-2018
// Ticket ID      :   CL-1288

using Laserbeam.BusinessObject.Common.Constants;
using Laserbeam.EntityManager.Common;
using System.Collections.Generic;
using System.Linq;

namespace Laserbeam.BusinessObject.Common.CachedModels
{
    public class BudgetConfigurationModel
    {
        public readonly decimal? BudgetPercent;
        public readonly bool ApplyBudgetMethod;
        public readonly string RoundingBudgetPerc;
        public readonly string RoundingBudgetDol;
        public readonly decimal? DecimalBudgetPerc;
        public readonly decimal? DecimalBudgetDol;

        // Author         :   Hariharasubramaniyan Chandrasekaran
        // Creation Date  :   08-March-2018
        // Ticket ID      :   CL-1288
        /// <summary>
        /// Constructor of Budget Config Model
        /// </summary>
        /// <param name="busSetting">List of Budget data</param>
        public BudgetConfigurationModel(IEnumerable<BusSetting> busSetting)
        {
            BudgetPercent = Conversion.convertToDecimal(busSetting.Single(m => m.KeyValue == BusSettingConstants.BudgetPercent).KeyDataValue);
            ApplyBudgetMethod = Conversion.convertToBool(busSetting.Single(m => m.KeyValue == BusSettingConstants.ApplyBudgetMethod).KeyDataValue);
            RoundingBudgetPerc = busSetting.Single(m => m.KeyValue == BusSettingConstants.RoundingBudgetPerc).KeyDataValue;
            RoundingBudgetDol = busSetting.Single(m => m.KeyValue == BusSettingConstants.RoundingBudgetDol).KeyDataValue;
            DecimalBudgetPerc= Conversion.convertToDecimal(busSetting.Single(m => m.KeyValue == BusSettingConstants.DecimalBudgetPerc).KeyDataValue);
            DecimalBudgetDol = Conversion.convertToDecimal(busSetting.Single(m => m.KeyValue == BusSettingConstants.DecimalBudgetDol).KeyDataValue);

        }
    }
}
