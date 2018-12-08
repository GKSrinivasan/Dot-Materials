using Laserbeam.BusinessObject.Common.Constants;
using Laserbeam.EntityManager.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laserbeam.BusinessObject.Common.CachedModels
{
    public class SSOConfigurationModel
    {
        public readonly bool EnableSSO;
        public readonly string IDPEndPoint;
        public SSOConfigurationModel(IEnumerable<BusSetting> busSetting)
        {
            EnableSSO = Conversion.convertToBool(busSetting.Single(m => m.KeyValue == BusSettingConstants.EnableSSO).KeyDataValue);
            IDPEndPoint = busSetting.Single(m => m.KeyValue == BusSettingConstants.IDPEndPoint).KeyDataValue;

        }
    }
}
