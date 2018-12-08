using Laserbeam.BusinessObject.Common.Constants;
using Laserbeam.EntityManager.Common;
using System.Collections.Generic;
using System.Linq;

namespace Laserbeam.BusinessObject.Common.CachedModels
{
    public class MeritProrationRuleModel
    {
        public readonly bool Prorate;
        public readonly bool ApplyMeritDiscretion;
        public readonly bool ApplyBudgetCalculations;
        public readonly bool ApplyAdjustmentCalculations;
        public readonly string ProrateIncreaseStartDate;
        public readonly string ProrateIncreaseEndDate;
        public readonly string ProrationType;
        public readonly int ProrationLength;
        public readonly int  ProrationLengthtoInclude;

        public MeritProrationRuleModel(IEnumerable<BusSetting> busSetting)
        {
            Prorate =Conversion.convertToBool(busSetting.Single(m => m.KeyValue == BusSettingConstants.Prorate).KeyDataValue);
            ApplyMeritDiscretion = Conversion.convertToBool(busSetting.Single(m => m.KeyValue == BusSettingConstants.ApplyMeritDiscretion).KeyDataValue);
            ApplyBudgetCalculations = Conversion.convertToBool(busSetting.Single(m => m.KeyValue == BusSettingConstants.ApplyBudgetCalculations).KeyDataValue);
            ApplyAdjustmentCalculations = Conversion.convertToBool(busSetting.Single(m => m.KeyValue == BusSettingConstants.ApplyAdjustmentCalculations).KeyDataValue);
            ProrateIncreaseStartDate = busSetting.Single(m => m.KeyValue == BusSettingConstants.ProrateIncreaseStartDate).KeyDataValue;
            ProrateIncreaseEndDate = busSetting.Single(m => m.KeyValue == BusSettingConstants.ProrateIncreaseEndDate).KeyDataValue;
            ProrationType = busSetting.Single(m => m.KeyValue == BusSettingConstants.ProrationType).KeyDataValue;
            ProrationLength = Conversion.convertToInteger(busSetting.Single(m => m.KeyValue == BusSettingConstants.ProrationLength).KeyDataValue);
            ProrationLengthtoInclude = Conversion.convertToInteger(busSetting.Single(m => m.KeyValue == BusSettingConstants.ProrationLengthtoInclude).KeyDataValue);
        }
    }
}
