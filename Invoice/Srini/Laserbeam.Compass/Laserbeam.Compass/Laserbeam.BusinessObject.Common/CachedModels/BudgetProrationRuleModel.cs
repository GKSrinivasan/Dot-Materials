
// Copyright (c) 2014 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.

// Component Name :   BudgetProrationRuleModel
// Description    :   Model that converts BusSetting table to BudgetProrationRuleModel
// Author         :   Hariharasubramaniyan Chandrasekaran		
// Creation Date  :   09-March-2018
// Ticket ID      :   CL-1288

using Laserbeam.BusinessObject.Common.Constants;
using Laserbeam.EntityManager.Common;
using System.Collections.Generic;
using System.Linq;

namespace Laserbeam.BusinessObject.Common.CachedModels
{
    public class BudgetProrationRuleModel
    {
        public readonly bool BudgetProrate;
        public readonly string BudgetProrateIncreaseStartDate;
        public readonly string BudgetProrateIncreaseEndDate;
        public readonly string BudgetProrationType;
        public readonly int BudgetProrationDuration;
        public readonly int BudgetProrationDatesPerMonth;
        // Author         :   Hariharasubramaniyan Chandrasekaran
        // Creation Date  :   09-March-2018
        // Ticket ID      :   CL-1288
        /// <summary>
        /// Constructor of Budget ProrationRule Model
        /// </summary>
        /// <param name="busSetting">List of Budget data</param>
        public BudgetProrationRuleModel(IEnumerable<BusSetting> busSetting)
        {
            BudgetProrate =Conversion.convertToBool(busSetting.Single(m => m.KeyValue == BusSettingConstants.BudgetProrate).KeyDataValue);
            BudgetProrateIncreaseStartDate = busSetting.Single(m => m.KeyValue == BusSettingConstants.BudgetProrateIncreaseStartDate).KeyDataValue;
            BudgetProrateIncreaseEndDate = busSetting.Single(m => m.KeyValue == BusSettingConstants.BudgetProrateIncreaseEndDate).KeyDataValue;
            BudgetProrationType = busSetting.Single(m => m.KeyValue == BusSettingConstants.BudgetProrationType).KeyDataValue;
            BudgetProrationDuration = Conversion.convertToInteger(busSetting.Single(m => m.KeyValue == BusSettingConstants.BudgetProrationDuration).KeyDataValue);
            BudgetProrationDatesPerMonth = Conversion.convertToInteger(busSetting.Single(m => m.KeyValue == BusSettingConstants.BudgetProrationDatesPerMonth).KeyDataValue);

        }
    }
}
