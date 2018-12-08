// Copyright (c) 2015 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.

// Component Name :   IRatingRepository 
// Description    :   Interface signature for RatingRepository
// Author         :   Shaheena Shaik	
// Creation Date  :   28-Mar-2017
// Reviewed By    :Hari.C
// Reviewed Date  : 3-March-2017

using System.Linq;
using Laserbeam.BusinessObject.Common;
using Laserbeam.EntityManager.Common;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Laserbeam.DataManager.Interfaces.Common
{
    public interface IRatingRepository
    {
        // Author       : Shaheena Shaik
        // Creation Date:28-March-2017
        // Reviewed By  :Hari.C
        // Reviewed Date: 3-March-2017
        /// <summary>
        /// Getting Rating Grid data from database
        /// </summary>
        /// <returns>Returning a query of details of RatingGrid</returns>
        IQueryable<Rating> GetConfigureRatingGridData();
        Task<bool> UpdateRatingRange(List<ConfigureRating> RatingGridData, int UserNum);


        // Author       : Shaheena Shaik
        // Creation Date:28-March-2017
        // Reviewed By  :Hari.C
        // Reviewed Date: 3-March-2017
        /// <summary>
        /// Adding or updating Rating into the database
        /// </summary>
        /// <param name="configRatingObject">model object which includes edited Rating values</param>
        /// <returns>Returning a boolean result</returns>
        Task<bool> UpdateAndAddRatingDetails(RatingPopupModel configRatingObject);

        // Author       : Shaheena Shaik
        // Creation Date:28-March-2017
        // Reviewed By  :Hari.C
        // Reviewed Date: 3-March-2017
        /// <summary>
        /// Deleting a Rating from the database
        /// </summary>
        /// <param name="ratingNum">Rating num of which is going to delete</param>
        /// <returns>Returning a boolean result</returns>
        bool DeleteRatingDetails(int ratingNum);
        bool RatingValidation(int ratingNum,string ratingDescription);
    }
}
