
// Copyright (c) 2014 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.

// Component Name :   CurrencyConfigurationModel
// Description    :   Model that converts BusSetting table to CurrencyConfigurationModel
// Author         :   Hariharasubramaniyan Chandrasekaran		
// Creation Date  :   10-March-2018
// Ticket ID      :   CL-1288
using Laserbeam.BusinessObject.Common.Constants;
using Laserbeam.EntityManager.Common;
using System.Collections.Generic;
using System.Linq;

namespace Laserbeam.BusinessObject.Common.CachedModels
{
    public class CurrencyConfigurationModel
    {
        public readonly string CurrencyFormat;

        // Author         :   Hariharasubramaniyan Chandrasekaran
        // Creation Date  :   10-March-2018
        // Ticket ID      :   CL-1288
        /// <summary>
        /// Constructor ofCurrencyConfiguration Model
        /// </summary>
        /// <param name="busSetting">List of Budget data</param>
        public CurrencyConfigurationModel(IEnumerable<BusSetting> busSetting)
        {
          
            CurrencyFormat = busSetting.Single(m => m.KeyValue == BusSettingConstants.CurrencyFormat).KeyDataValue;

        }
    }
}
