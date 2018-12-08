using Laserbeam.BusinessObject.Common.Constants;
using Laserbeam.EntityManager.Common;
using System.Collections.Generic;
using System.Linq;

namespace Laserbeam.BusinessObject.Common.CachedModels
{
    public  class NamingConfigurationModel
    {
        public readonly string EmployeeNameFormat;
        public readonly string UserNameFormat;
        public readonly string SortOrderEmployeeNameFormat;
        public readonly string EmployeeNameEmailFormat;

        public NamingConfigurationModel(IEnumerable<BusSetting> busSetting)
        {
            EmployeeNameFormat = busSetting.Single(m => m.KeyValue == BusSettingConstants.EmployeeNameFormat).KeyDataValue;
            UserNameFormat = busSetting.Single(m => m.KeyValue == BusSettingConstants.UserNameFormat).KeyDataValue;
            SortOrderEmployeeNameFormat = busSetting.Single(m => m.KeyValue == BusSettingConstants.SortOrderEmployeeNameFormat).KeyDataValue;
            EmployeeNameEmailFormat = busSetting.Single(m => m.KeyValue == BusSettingConstants.EmployeeNameEmailFormat).KeyDataValue;
        }
    }
}
