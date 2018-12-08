// Copyright (c) 2015 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.

// Component Name :   IMarketPayRangeProcessManager
// Description    :   Interface signature for MarketPayRangeProcessManager
// Author         :   Arunraj C	
// Creation Date  :    11-OCT-2017

using Laserbeam.BusinessObject.Common;
using Laserbeam.EntityManager.Common;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Laserbeam.ProcessManager.Interfaces.Common
{
    public interface IMarketPayRangeProcessManager
    {
        BusSettingModel GetBusSetting();
        Task SetBusSetting(BusSettingModel busSetting);
        Task<List<MarketPayRangeGridModel>> GetMarketPayRangeData(int loggedInUserNum,string selectedMarketPayRange);
        bool IsMarketDataExist(string selectedMarketPayRange, string updateDataValue);
        Task AddUpdateMarketPayRange(MarketPayRangeModel model,int loginUserNum);
        MarketPayRangeModel GetSelectedMarketPayRange(int MarketRangeNum, string selectedMarketPayRange);
    }
}
