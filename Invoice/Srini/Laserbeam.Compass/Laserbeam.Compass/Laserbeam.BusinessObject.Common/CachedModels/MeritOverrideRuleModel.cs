using Laserbeam.BusinessObject.Common.Constants;
using Laserbeam.EntityManager.Common;
using System.Collections.Generic;
using System.Linq;

namespace Laserbeam.BusinessObject.Common.CachedModels
{
    public class MeritOverrideRuleModel
    {
        public readonly bool MeritOverrideHardStop;
        public readonly bool MeritOverrideSoftStop;
        public readonly bool MeritIncreaseWithinGuideline;
        public readonly bool MandatoryJustification;
        public readonly bool MeritOverrideNoJustification;
        public MeritOverrideRuleModel(IEnumerable<BusSetting> busSetting)
        {
            MeritOverrideHardStop = Conversion.convertToBool(busSetting.Single(m => m.KeyValue == BusSettingConstants.MeritOverrideHardStop).KeyDataValue);
            MeritOverrideSoftStop = Conversion.convertToBool(busSetting.Single(m => m.KeyValue == BusSettingConstants.MeritOverrideSoftStop).KeyDataValue);
            MeritIncreaseWithinGuideline = Conversion.convertToBool(busSetting.Single(m => m.KeyValue == BusSettingConstants.MeritIncreaseWithinGuideline).KeyDataValue);
            MandatoryJustification = Conversion.convertToBool(busSetting.Single(m => m.KeyValue == BusSettingConstants.MandatoryJustification).KeyDataValue);
            MeritOverrideNoJustification = Conversion.convertToBool(busSetting.Single(m => m.KeyValue == BusSettingConstants.MeritOverrideNoJustification).KeyDataValue);
        }
    }
}
