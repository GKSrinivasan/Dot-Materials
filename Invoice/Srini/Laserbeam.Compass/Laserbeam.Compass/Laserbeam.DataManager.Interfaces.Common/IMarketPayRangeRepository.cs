// Copyright (c) 2016 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.
// Component Name  :  IMarketPayRangeRepository
// Description     :  Interface signature for MarketPayRangeRepository
// Author         :   Arunraj C
// Creation Date  :  10-OCT-2017

using Laserbeam.BusinessObject.Common;
using Laserbeam.EntityManager.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Laserbeam.DataManager.Interfaces.Common
{
    public interface IMarketPayRangeRepository
    {
        BusSettingModel GetBusSetting();
        Task SetBusSetting(BusSettingModel busSetting);
        Task<List<MarketPayRangeGridModel>> GetMarketPayRangeData(int loggedInUserNum,string selectedMarketPayRange);
        bool IsMarketDataExist(string selectedMarketPayRange,string updateDataValue);
        Task AddUpdateMarketPayRange(MarketPayRangeModel model,int loginUserNum);
        MarketPayRangeModel GetSelectedMarketPayRange(int MarketRangeNum, string selectedMarketPayRange);
    }
}
