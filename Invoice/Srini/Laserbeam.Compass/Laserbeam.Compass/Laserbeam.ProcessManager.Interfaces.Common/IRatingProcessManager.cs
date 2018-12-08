// Copyright (c) 2015 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.

// Component Name  : 	IARatingProcessManager
// Description     : 	Interface for RatingProcessManager	
// Author          :	Shaheena Shaik		
// Creation Date   : 	3-March-2017
// Reviewed By    :     Hari.C
// Reviewed Date  :     3-March-2017

using System.Linq;
using Laserbeam.BusinessObject.Common;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Laserbeam.ProcessManager.Interfaces.Common
{
    public interface IRatingProcessManager
    {
        // Author       : Shaheena Shaik
        // Creation Date:28-March-2017
        // Reviewed By  :Hari.C
        // Reviewed Date: 3-March-2017
        /// <summary>
        /// Getting Rating Grid data from database
        /// </summary>
        /// <returns>Returning a query of details of Rating Grid</returns>
        IQueryable<ConfigureRating> GetConfigureRatingGridData();
        IQueryable<ConfigureRating> GetConfigureRatingGridDataForRange();
        Task<bool> UpdateRatingRange(List<ConfigureRating> RatingGridData, int UserNum);

        // Author       : Shaheena Shaik
        // Creation Date:28-March-2017
        // Reviewed By  :Hari.C
        // Reviewed Date: 3-March-2017
        /// <summary>
        /// Adding or updadting Rating into the database
        /// </summary>
        /// <param name="configRatingObject">Model object which includes the edited Rating values</param>
        /// <returns>Returning a boolean result</returns>
        Task<bool> UpdateAndAddRatingDetails(RatingPopupModel configRatingObject);

        // Author       : Shaheena Shaik
        // Creation Date:28-March-2017
        // Reviewed By  :Hari.C
        // Reviewed Date: 3-March-2017
        /// <summary>
        /// Deleting Rating from database
        /// </summary>
        /// <param name="ratingNum">Rating num of which is going to delete</param>
        /// <returns>Returning  a boolean result</returns>
        bool DeleteRatingDetails(int ratingNum);
        bool RatingValidation(int ratingNum,string ratingDescription);
    }
}
