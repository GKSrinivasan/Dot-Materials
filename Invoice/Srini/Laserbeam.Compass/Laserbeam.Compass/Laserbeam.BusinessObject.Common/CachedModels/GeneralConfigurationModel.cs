// Copyright (c) 2014 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.

// Component Name :   GeneralConfigurationModel
// Description    :   Model that converts BusSetting table to GeneralConfigurationModel
// Author         :   Hariharasubramaniyan Chandrasekaran		
// Creation Date  :   11-March-2018
// Ticket ID      :   CL-1288

using Laserbeam.BusinessObject.Common.Constants;
using Laserbeam.EntityManager.Common;
using System.Collections.Generic;
using System.Linq;

namespace Laserbeam.BusinessObject.Common.CachedModels
{
    public  class GeneralConfigurationModel
    {
        public readonly string RoundingMeritPct;
        public readonly string RoundingMeritHourly;
        public readonly string RoundingMeritAnnual;
        public readonly string RoundingLumpSumPct;
        public readonly string RoundingLumpSumHourly;
        public readonly string RoundingLumpSumAnnual;
        public readonly string RoundingAdjustmentPct;
        public readonly string RoundingAdjustmentHourly;
        public readonly string RoundingAdjustmentAnnual;
        public readonly string RoundingCompaRatioPct;
        public readonly string RoundingCompaRatioHourly;
        public readonly string RoundingCompaRatioAnnual;
        public readonly string RoundingBonusPct;
        public readonly string RoundingBonusHourly;
        public readonly string RoundingBonusAnnual;
        public readonly string RoundingPromotionPct;
        public readonly string RoundingPromotionHourly;
        public readonly string RoundingPromotionAnnual;
        public readonly decimal DecimalMeritPct;
        public readonly decimal DecimalMeritHourly;
        public readonly decimal DecimalMeritAnnual;
        public readonly decimal DecimalLumpSumPct;
        public readonly decimal DecimalLumpSumHourly;
        public readonly decimal DecimalLumpSumAnnual;
        public readonly decimal DecimalAdjustmentPct;
        public readonly decimal DecimalAdjustmentHourly;
        public readonly decimal DecimalAdjustmentAnnual;
        public readonly decimal DecimalCompaRatioPct;
        public readonly decimal DecimalCompaRatioHourly;
        public readonly decimal DecimalCompaRatioAnnual;
        public readonly decimal DecimalBonusPct;
        public readonly decimal DecimalBonusHourly;
        public readonly decimal DecimalBonusAnnual;
        public readonly decimal DecimalPromotionPct;
        public readonly decimal DecimalPromotionHourly;
        public readonly decimal DecimalPromotionAnnual;
        public readonly string RoundNewSalaryPct;
        public readonly string RoundNewSalaryHourly;
        public readonly string RoundNewSalaryAnnual;
        public readonly decimal DecimalNewSalaryPct;
        public readonly decimal DecimalNewSalaryHourly;
        public readonly decimal DecimalNewSalaryAnnual;
        public readonly bool IsSampleData;
        public readonly string RoundCurrentSalaryPct;
        public readonly string RoundCurrentSalaryHourly;
        public readonly string RoundCurrentSalaryAnnual;
        public readonly decimal DecimalCurrentSalaryPct;
        public readonly decimal DecimalCurrentSalaryHourly;
        public readonly decimal DecimalCurrentSalaryAnnual;

        // Author         :   Hariharasubramaniyan Chandrasekaran
        // Creation Date  :   11-March-2018
        // Ticket ID      :   CL-1288
        /// <summary>
        /// Constructor of GeneralConfiguration Model
        /// </summary>
        /// <param name="busSetting">List of Budget data</param>
        public GeneralConfigurationModel(IEnumerable<BusSetting> busSetting)
        {
            RoundingMeritPct = busSetting.Single(m => m.KeyValue == BusSettingConstants.RoundingMeritPct).KeyDataValue;
            RoundingMeritHourly = busSetting.Single(m => m.KeyValue == BusSettingConstants.RoundingMeritHourly).KeyDataValue;
            RoundingMeritAnnual = busSetting.Single(m => m.KeyValue == BusSettingConstants.RoundingMeritAnnual).KeyDataValue;
            RoundingLumpSumPct = busSetting.Single(m => m.KeyValue == BusSettingConstants.RoundingLumpSumPct).KeyDataValue;
            RoundingLumpSumHourly = busSetting.Single(m => m.KeyValue == BusSettingConstants.RoundingLumpSumHourly).KeyDataValue;
            RoundingLumpSumAnnual = busSetting.Single(m => m.KeyValue == BusSettingConstants.RoundingLumpSumAnnual).KeyDataValue;
            RoundingAdjustmentPct = busSetting.Single(m => m.KeyValue == BusSettingConstants.RoundingAdjustmentPct).KeyDataValue;
            RoundingAdjustmentHourly = busSetting.Single(m => m.KeyValue == BusSettingConstants.RoundingAdjustmentHourly).KeyDataValue;
            RoundingAdjustmentAnnual = busSetting.Single(m => m.KeyValue == BusSettingConstants.RoundingAdjustmentAnnual).KeyDataValue;
            RoundingCompaRatioPct = busSetting.Single(m => m.KeyValue == BusSettingConstants.RoundingCompaRatioPct).KeyDataValue;
            RoundingCompaRatioHourly = busSetting.Single(m => m.KeyValue == BusSettingConstants.RoundingCompaRatioHourly).KeyDataValue;
            RoundingCompaRatioAnnual = busSetting.Single(m => m.KeyValue == BusSettingConstants.RoundingCompaRatioAnnual).KeyDataValue;
            RoundingBonusPct = busSetting.Single(m => m.KeyValue == BusSettingConstants.RoundingBonusPct).KeyDataValue;
            RoundingBonusHourly = busSetting.Single(m => m.KeyValue == BusSettingConstants.RoundingBonusHourly).KeyDataValue;
            RoundingBonusAnnual = busSetting.Single(m => m.KeyValue == BusSettingConstants.RoundingBonusAnnual).KeyDataValue;
            RoundingPromotionPct = busSetting.Single(m => m.KeyValue == BusSettingConstants.RoundingPromotionPct).KeyDataValue;
            RoundingPromotionHourly = busSetting.Single(m => m.KeyValue == BusSettingConstants.RoundingPromotionHourly).KeyDataValue;
            RoundingPromotionAnnual = busSetting.Single(m => m.KeyValue == BusSettingConstants.RoundingPromotionAnnual).KeyDataValue;
            DecimalMeritPct = Conversion.convertToDecimal(busSetting.Single(m => m.KeyValue == BusSettingConstants.DecimalMeritPct).KeyDataValue);
            DecimalMeritHourly = Conversion.convertToDecimal(busSetting.Single(m => m.KeyValue == BusSettingConstants.DecimalMeritHourly).KeyDataValue);
            DecimalMeritAnnual = Conversion.convertToDecimal(busSetting.Single(m => m.KeyValue == BusSettingConstants.DecimalMeritAnnual).KeyDataValue);
            DecimalLumpSumPct = Conversion.convertToDecimal(busSetting.Single(m => m.KeyValue == BusSettingConstants.DecimalLumpSumPct).KeyDataValue);
            DecimalLumpSumHourly = Conversion.convertToDecimal(busSetting.Single(m => m.KeyValue == BusSettingConstants.DecimalLumpSumHourly).KeyDataValue);
            DecimalLumpSumAnnual = Conversion.convertToDecimal(busSetting.Single(m => m.KeyValue == BusSettingConstants.DecimalLumpSumAnnual).KeyDataValue);
            DecimalAdjustmentPct = Conversion.convertToDecimal(busSetting.Single(m => m.KeyValue == BusSettingConstants.DecimalAdjustmentPct).KeyDataValue);
            DecimalAdjustmentHourly = Conversion.convertToDecimal(busSetting.Single(m => m.KeyValue == BusSettingConstants.DecimalAdjustmentHourly).KeyDataValue);
            DecimalAdjustmentAnnual = Conversion.convertToDecimal(busSetting.Single(m => m.KeyValue == BusSettingConstants.DecimalAdjustmentAnnual).KeyDataValue);
            DecimalCompaRatioPct = Conversion.convertToDecimal(busSetting.Single(m => m.KeyValue == BusSettingConstants.DecimalCompaRatioPct).KeyDataValue);
            DecimalCompaRatioHourly = Conversion.convertToDecimal(busSetting.Single(m => m.KeyValue == BusSettingConstants.DecimalCompaRatioHourly).KeyDataValue);
            DecimalCompaRatioAnnual = Conversion.convertToDecimal(busSetting.Single(m => m.KeyValue == BusSettingConstants.DecimalCompaRatioAnnual).KeyDataValue);
            DecimalBonusPct = Conversion.convertToDecimal(busSetting.Single(m => m.KeyValue == BusSettingConstants.DecimalBonusPct).KeyDataValue);
            DecimalBonusHourly = Conversion.convertToDecimal(busSetting.Single(m => m.KeyValue == BusSettingConstants.DecimalBonusHourly).KeyDataValue);
            DecimalBonusAnnual = Conversion.convertToDecimal(busSetting.Single(m => m.KeyValue == BusSettingConstants.DecimalBonusAnnual).KeyDataValue);
            DecimalPromotionPct = Conversion.convertToDecimal(busSetting.Single(m => m.KeyValue == BusSettingConstants.DecimalPromotionPct).KeyDataValue);
            DecimalPromotionHourly = Conversion.convertToDecimal(busSetting.Single(m => m.KeyValue == BusSettingConstants.DecimalPromotionHourly).KeyDataValue);
            DecimalPromotionAnnual = Conversion.convertToDecimal(busSetting.Single(m => m.KeyValue == BusSettingConstants.DecimalPromotionAnnual).KeyDataValue);
            RoundNewSalaryPct = busSetting.Single(m => m.KeyValue == BusSettingConstants.RoundNewSalaryPct).KeyDataValue;
            RoundNewSalaryHourly = busSetting.Single(m => m.KeyValue == BusSettingConstants.RoundNewSalaryHourly).KeyDataValue;
            RoundNewSalaryAnnual = busSetting.Single(m => m.KeyValue == BusSettingConstants.RoundNewSalaryAnnual).KeyDataValue;
            DecimalNewSalaryPct = Conversion.convertToDecimal(busSetting.Single(m => m.KeyValue == BusSettingConstants.DecimalNewSalaryPct).KeyDataValue);
            DecimalNewSalaryHourly = Conversion.convertToDecimal(busSetting.Single(m => m.KeyValue == BusSettingConstants.DecimalNewSalaryHourly).KeyDataValue);
            DecimalNewSalaryAnnual = Conversion.convertToDecimal(busSetting.Single(m => m.KeyValue == BusSettingConstants.DecimalNewSalaryAnnual).KeyDataValue);
            IsSampleData = Conversion.convertToBool(busSetting.Single(m => m.KeyValue == BusSettingConstants.IsSampleData).KeyDataValue);
            RoundCurrentSalaryPct = busSetting.Single(m => m.KeyValue == BusSettingConstants.RoundCurrentSalaryPct).KeyDataValue;
            RoundCurrentSalaryHourly = busSetting.Single(m => m.KeyValue == BusSettingConstants.RoundCurrentSalaryHourly).KeyDataValue;
            RoundCurrentSalaryAnnual = busSetting.Single(m => m.KeyValue == BusSettingConstants.RoundCurrentSalaryAnnual).KeyDataValue;
            DecimalCurrentSalaryPct = Conversion.convertToDecimal(busSetting.Single(m => m.KeyValue == BusSettingConstants.DecimalCurrentSalaryPct).KeyDataValue);
            DecimalCurrentSalaryHourly = Conversion.convertToDecimal(busSetting.Single(m => m.KeyValue == BusSettingConstants.DecimalCurrentSalaryHourly).KeyDataValue);
            DecimalCurrentSalaryAnnual = Conversion.convertToDecimal(busSetting.Single(m => m.KeyValue == BusSettingConstants.DecimalCurrentSalaryAnnual).KeyDataValue);

        }
    }
}
