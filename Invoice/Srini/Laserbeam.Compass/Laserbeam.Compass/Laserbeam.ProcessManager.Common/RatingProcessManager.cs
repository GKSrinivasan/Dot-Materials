// Copyright (c) 2014 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.

// Component Name :   Rating
// Description    :   All Business logic related to Rating is placed here 	
// Author         :   Shaheena Shaik
// Creation Date  :   28-March-2017
using System.Linq;
using Laserbeam.ProcessManager.Interfaces.Common;
using Laserbeam.DataManager.Interfaces.Common;
using Laserbeam.BusinessObject.Common;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Laserbeam.ProcessManager.Common
{
    public class RatingProcessManager : IRatingProcessManager
    {
        #region Fields
        IRatingRepository m_ratingRepository;
        #endregion

        #region Constructors
        public RatingProcessManager(IRatingRepository RatingRepository)
        {
            m_ratingRepository = RatingRepository;
        }
        #endregion


        #region Implements

        // Author       : Shaheena Shaik
        // Creation Date:28-March-2017
        // Reviewed By  :Hari.C
        // Reviewed Date: 3-March-2017
        /// <summary>
        /// Getting Rating Grid data from database
        /// </summary>
        /// <returns>Returning Grid details from database</returns>
        public IQueryable<ConfigureRating> GetConfigureRatingGridData()
        {
            var ratingGridDetails = m_ratingRepository.GetConfigureRatingGridData();
            return ratingGridDetails
                .Select(x => new ConfigureRating
                {
                    RatingNum = x.RatingNum,
                    RatingId = x.RatingID,
                    UpdatedBy = x.UpdatedBy,
                    RatingType = x.RatingType,
                    RatingDescription = x.RatingDescr,
                    LowRange = x.LowRange,
                    HighRange = x.HighRange,
                    MinRange = x.MinRange != null ? x.MinRange : "",
                    MaxRange = x.MaxRange != null ? x.MaxRange : "",
                    RatingOrder = x.RatingOrder
                }).AsQueryable();
        }
        public IQueryable<ConfigureRating> GetConfigureRatingGridDataForRange()
        {
            var ratingGridDetails = m_ratingRepository.GetConfigureRatingGridData();
            return ratingGridDetails
                .Select(x => new ConfigureRating
                {
                    RatingNum = x.RatingNum,
                    RatingId = x.RatingID,
                    UpdatedBy = x.UpdatedBy,
                    RatingType = x.RatingType,
                    RatingDescription = x.RatingDescr,
                    MinRange = x.MinRange!=null? x.MinRange:"",
                    MaxRange = x.MaxRange != null ? x.MaxRange : "",
                    RatingOrder = x.RatingOrder
                }).AsQueryable();
        }
        
        public async Task<bool> UpdateRatingRange(List<ConfigureRating> RatingGridData, int UserNum)
        {
            return await m_ratingRepository.UpdateRatingRange(RatingGridData, UserNum);
        }
        // Author       : Shaheena Shaik
        // Creation Date:28-March-2017
        // Reviewed By  :Hari.C
        // Reviewed Date: 3-March-2017
        /// <summary>
        /// Adding and updating Rating details into the database
        /// </summary>
        /// <param name="configRatingObject"> Modelobject containg the rating details</param>
        /// <returns>Returning a string message</returns>
        public async Task<bool> UpdateAndAddRatingDetails(RatingPopupModel configRatingObject)
        {
            return await m_ratingRepository.UpdateAndAddRatingDetails(configRatingObject);
        }

        // Author       : Shaheena Shaik
        // Creation Date:28-March-2017
        // Reviewed By  :Hari.C
        // Reviewed Date: 3-March-2017
        /// <summary>
        /// deleting a Rating from database
        /// </summary>
        /// <param name="ratingNum">Rating Num of which we are going to delete</param>
        /// <returns>Reting a boolean result</returns>
        public bool DeleteRatingDetails(int ratingNum)
        {
            return m_ratingRepository.DeleteRatingDetails(ratingNum);
        }
        public bool RatingValidation(int ratingNum,string ratingDescription)
        {
            return m_ratingRepository.RatingValidation(ratingNum,ratingDescription);
        }
        #endregion

    }
}
