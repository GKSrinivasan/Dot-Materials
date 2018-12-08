// Copyright (c) 2014 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.

// Component Name :   FeatureConfigurationModel
// Description    :   Model that converts BusSetting table to FeatureConfigurationModel
// Author         :   Hariharasubramaniyan Chandrasekaran		
// Creation Date  :   11-March-2018
// Ticket ID      :   CL-1288
using Laserbeam.BusinessObject.Common.Constants;
using Laserbeam.EntityManager.Common;
using System.Collections.Generic;
using System.Linq;

namespace Laserbeam.BusinessObject.Common.CachedModels
{
    public class FeatureConfigurationModel
    {
        public readonly bool Merit;
        public readonly bool Adjustment;
        public readonly bool Lumpsum;
        public readonly bool RatingDisplay;
        public readonly bool TCC;
        public readonly bool RatingDropdown;
        public readonly bool MultiCurrency;
        public readonly bool Promotion;
        public readonly bool CurrencyCode;
        public readonly bool LTIP;
        public readonly bool MeritCalculation;
        public readonly bool WorkFlow;
        public readonly bool EitherMeritOrLumpSum;
        public readonly bool Bonus;
        public readonly bool ComparativeRatio;
        public readonly string WorkFlowMode;
        public readonly int WorkFlowLevel;
        public readonly int SupportHours;

        // Author         :   Hariharasubramaniyan Chandrasekaran
        // Creation Date  :   11-March-2018
        // Ticket ID      :   CL-1288
        /// <summary>
        /// Constructor of FeatureConfiguration Model
        /// </summary>
        /// <param name="busSetting">List of Budget data</param>
        public FeatureConfigurationModel(IEnumerable<BusSetting> busSetting)
        {
            Merit = Conversion.convertToBool(busSetting.Single(m => m.KeyValue == BusSettingConstants.Merit).KeyDataValue);
            Adjustment = Conversion.convertToBool(busSetting.Single(m => m.KeyValue == BusSettingConstants.Adjustment).KeyDataValue);
            Lumpsum = Conversion.convertToBool(busSetting.Single(m => m.KeyValue == BusSettingConstants.Lumpsum).KeyDataValue);
            RatingDisplay = Conversion.convertToBool(busSetting.Single(m => m.KeyValue == BusSettingConstants.RatingDisplay).KeyDataValue);
            TCC = Conversion.convertToBool(busSetting.Single(m => m.KeyValue == BusSettingConstants.TCC).KeyDataValue);
            RatingDropdown = Conversion.convertToBool(busSetting.Single(m => m.KeyValue == BusSettingConstants.RatingDropdown).KeyDataValue);
            MultiCurrency = Conversion.convertToBool(busSetting.Single(m => m.KeyValue == BusSettingConstants.MultiCurrency).KeyDataValue);
            Promotion = Conversion.convertToBool(busSetting.Single(m => m.KeyValue == BusSettingConstants.Promotion).KeyDataValue);
            CurrencyCode = Conversion.convertToBool(busSetting.Single(m => m.KeyValue == BusSettingConstants.CurrencyCode).KeyDataValue);
            LTIP = Conversion.convertToBool(busSetting.Single(m => m.KeyValue == BusSettingConstants.LTIP).KeyDataValue);
            MeritCalculation = Conversion.convertToBool(busSetting.Single(m => m.KeyValue == BusSettingConstants.MeritCalculation).KeyDataValue);
            WorkFlow = Conversion.convertToBool(busSetting.Single(m => m.KeyValue == BusSettingConstants.WorkFlow).KeyDataValue);
            EitherMeritOrLumpSum = Conversion.convertToBool(busSetting.Single(m => m.KeyValue == BusSettingConstants.EitherMeritOrLumpSum).KeyDataValue);
            WorkFlowMode =busSetting.Single(m => m.KeyValue == BusSettingConstants.WorkFlowMode).KeyDataValue;
            WorkFlowLevel = Conversion.convertToInteger(busSetting.Single(m => m.KeyValue == BusSettingConstants.WorkFlowLevel).KeyDataValue);
            SupportHours = Conversion.convertToInteger(busSetting.Single(m => m.KeyValue == BusSettingConstants.SupportHours).KeyDataValue);
            Bonus = Conversion.convertToBool(busSetting.Single(m => m.KeyValue == BusSettingConstants.Bonus).KeyDataValue);
            ComparativeRatio = Conversion.convertToBool(busSetting.Single(m => m.KeyValue == BusSettingConstants.ComparativeRatio).KeyDataValue);
        }

    }
}
