// Copyright (c) 2014 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.

// Component Name :   MarketPayRangeProcessManager
// Description    :   All Business logic related to MarketPayRange is placed here 	
// Author         :   Arunraj C
// Creation Date  :   11-OCT-2017

using Laserbeam.BusinessObject.Common;
using Laserbeam.DataManager.Interfaces.Common;
using Laserbeam.EntityManager.Common;
using Laserbeam.ProcessManager.Interfaces.Common;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Laserbeam.ProcessManager.Common
{
    public class MarketPayRangeProcessManager : IMarketPayRangeProcessManager
    {
        #region Fields
        // Author         :   Arunraj C	
        // Creation Date  :   11-OCT-2017 
        /// <summary>
        /// Instance of  MarketPayRangeRepository
        /// </summary>
        private IMarketPayRangeRepository m_marketPayRangeRepository;// {get;set;}

        #endregion
        #region Constructor
        // Author         :   Arunraj C	
        // Creation Date  :   11-OCT-2017 
        /// <summary>
        /// Instance of  MarketPayRangeRepository
        /// </summary>
        public MarketPayRangeProcessManager(IMarketPayRangeRepository marketPayRangeRepository)
        {
            m_marketPayRangeRepository = marketPayRangeRepository;
        }
        #endregion

        public BusSettingModel GetBusSetting()
        {
            return m_marketPayRangeRepository.GetBusSetting();
        }

        public async Task SetBusSetting(BusSettingModel busSetting)
        {
           await m_marketPayRangeRepository.SetBusSetting(busSetting);
        }

        public async Task<List<MarketPayRangeGridModel>> GetMarketPayRangeData(int loggedInUserNum,string selectedMarketPayRange)
        {
            var result = await m_marketPayRangeRepository.GetMarketPayRangeData(loggedInUserNum,selectedMarketPayRange);
            return result;
        }

        public bool IsMarketDataExist(string selectedMarketPayRange, string updateDataValue)
        {
            var result =  m_marketPayRangeRepository.IsMarketDataExist(selectedMarketPayRange, updateDataValue);
            return result;
        }

        public async Task AddUpdateMarketPayRange(MarketPayRangeModel model,int loginUserNum)
        {
           await m_marketPayRangeRepository.AddUpdateMarketPayRange(model, loginUserNum);
        }

        public MarketPayRangeModel GetSelectedMarketPayRange(int MarketRangeNum, string selectedMarketPayRange)
        {
            return m_marketPayRangeRepository.GetSelectedMarketPayRange(MarketRangeNum, selectedMarketPayRange);
        }
    }
}
