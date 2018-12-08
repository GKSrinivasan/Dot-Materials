using Laserbeam.BusinessObject.Common.Constants;
using Laserbeam.EntityManager.Common;
using System.Collections.Generic;
using System.Linq;

namespace Laserbeam.BusinessObject.Common.CachedModels
{
    public  class LumpSumRuleModel
    {
        public readonly string LumpsumType;
        public readonly decimal RangeMaxPct;
        public readonly decimal RangeMaxAmt;
        public readonly bool MeritValuesReCalculate;
        public readonly bool AutoCalculateLumpSum;
        public readonly bool LumpSumRuleTurnOff;

        public LumpSumRuleModel(IEnumerable<BusSetting> busSetting)
        {
            LumpsumType = busSetting.Single(m => m.KeyValue == BusSettingConstants.LumpsumType).KeyDataValue;
            RangeMaxPct = Conversion.convertToDecimal(busSetting.Single(m => m.KeyValue == BusSettingConstants.RangeMaxPct).KeyDataValue);
            RangeMaxAmt = Conversion.convertToDecimal(busSetting.Single(m => m.KeyValue == BusSettingConstants.RangeMaxAmt).KeyDataValue);
            MeritValuesReCalculate = Conversion.convertToBool(busSetting.Single(m => m.KeyValue == BusSettingConstants.MeritValuesReCalculate).KeyDataValue);
            AutoCalculateLumpSum = Conversion.convertToBool(busSetting.Single(m => m.KeyValue == BusSettingConstants.AutoCalculateLumpSum).KeyDataValue);
            LumpSumRuleTurnOff = Conversion.convertToBool(busSetting.Single(m => m.KeyValue == BusSettingConstants.LumpSumRuleTurnOff).KeyDataValue);

        }

    }
}
