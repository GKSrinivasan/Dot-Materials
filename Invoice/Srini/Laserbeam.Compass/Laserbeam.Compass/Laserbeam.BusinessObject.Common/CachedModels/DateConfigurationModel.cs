// Copyright (c) 2014 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.

// Component Name :   DateConfigurationModel
// Description    :   Model that converts BusSetting table to DateConfigurationModel
// Author         :   Hariharasubramaniyan Chandrasekaran		
// Creation Date  :   10-March-2018
// Ticket ID      :   CL-1288
using Laserbeam.BusinessObject.Common.Constants;
using Laserbeam.EntityManager.Common;
using System.Collections.Generic;
using System.Linq;

namespace Laserbeam.BusinessObject.Common.CachedModels
{
    public class DateConfigurationModel
    {
        public readonly string DateFormat;

        // Author         :   Hariharasubramaniyan Chandrasekaran
        // Creation Date  :   10-March-2018
        // Ticket ID      :   CL-1288
        /// <summary>
        /// Constructor of DateConfigurationModel Model
        /// </summary>
        /// <param name="busSetting">List of Budget data</param>
        public DateConfigurationModel(IEnumerable<BusSetting> busSetting)
        {
            DateFormat = busSetting.Single(m => m.KeyValue == BusSettingConstants.DateFormat).KeyDataValue;
         

        }
    }
}
