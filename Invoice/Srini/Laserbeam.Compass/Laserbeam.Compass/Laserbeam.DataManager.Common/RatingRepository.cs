// Copyright (c) 2015 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.

// Component Name  :    RatingRepository
// Description     : 	Repository for Rating
// Author          :	Shaheena Shaik
// Creation Date   : 	3-March-2017

using System;
using System.Collections.Generic;
using System.Linq;
using Laserbeam.BusinessObject.Common;
using Laserbeam.DataManager.Interfaces.Common;
using Laserbeam.DataManager.Interfaces.Core;
using Laserbeam.EntityManager.Common;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace Laserbeam.DataManager.Common
{
    public class RatingRepository : IRatingRepository
    {
        #region Fields
        // Author         :  Shaheena Shaik
        // Creation Date  :  27-March-2017   
        // Reviewed By    :Hari.C
        // Reviewed Date  : 3-March-2017
        /// <summary>
        /// Object of Base Repository
        /// </summary>
        private IBaseRepository m_baseRepository;

        #endregion

        #region Constructors
        // Author         :  Shaheena Shaik
        // Creation Date  :  27-March-2017   
        // Reviewed By    :Hari.C
        // Reviewed Date  : 3-March-2017
        /// <summary>
        /// Initializes objects used in this class
        /// </summary>
        /// <param name="baseRepository">Base Repository Object</param>
        public RatingRepository(IBaseRepository baseRepository)           
        {
            m_baseRepository = baseRepository;
        }
        #endregion

        #region Implements

        // Author         :  Shaheena Shaik
        // Creation Date  :  27-March-2017   
        // Reviewed By    :Hari.C
        // Reviewed Date  : 3-March-2017
        /// <summary>
        /// Getting Rating Grid data from database
        /// </summary>
        /// <returns>Returning a query of Rating details for grid</returns>
        public IQueryable<Rating> GetConfigureRatingGridData()
        {
            return m_baseRepository.GetQuery<Rating>();
        }

        public async Task<bool> UpdateRatingRange(List<ConfigureRating> RatingGridData, int UserNum)
        {
            var data = new DataTable("RatingTable");
            data.Columns.Add("RatingNum", typeof(int));

            data.Columns.Add("MinRange", typeof(string));
            data.Columns[1].AllowDBNull = true;

            data.Columns.Add("MaxRange", typeof(string));
            data.Columns[2].AllowDBNull = true;

            

            foreach (var item in RatingGridData)
            {
                DataRow dr = data.NewRow();
              
                dr["RatingNum"] = item.RatingNum;
                dr["MinRange"] = item.MinRange;
                dr["MaxRange"] = item.MaxRange;
               
                data.Rows.Add(dr);
            }
            SqlParameter parameter = new SqlParameter("@RatingTable", data);
            parameter.SqlDbType = SqlDbType.Structured;
            parameter.Direction = ParameterDirection.Input;
            SqlParameter[] parameters = new SqlParameter[] { parameter };
            var rows = await m_baseRepository.ExecuteStoredProcedure("Talent.USP_RAT_PUT_UpdateRatingRange", parameters);
            return rows > 0;
        }



        // Author         :  Shaheena Shaik
        // Creation Date  :  27-March-2017   
        // Reviewed By    :Hari.C
        // Reviewed Date  : 3-March-2017
        /// <summary>
        /// Adding and Updating RAting details into the database
        /// </summary>
        /// <param name="configRatingObject">Model object which includes the edited or newly created Rating values</param>
        /// <returns></returns>
        public async Task<bool> UpdateAndAddRatingDetails(RatingPopupModel configRatingObject)
        {
            var isUpdated = false;
            Rating updateRatingModel = new Rating();            
            if (configRatingObject.RatingNum != 0)
            { 
                updateRatingModel = GetRatingTableData(configRatingObject.RatingNum);
                updateRatingModel.RatingID = updateRatingModel.RatingID;
                updateRatingModel.RatingType = updateRatingModel.RatingType;
                updateRatingModel.RatingDetailDescr = updateRatingModel.RatingDetailDescr;
                updateRatingModel.RatingDescr = configRatingObject.RatingDescription;
                updateRatingModel.IsCommentMandatory = updateRatingModel.IsCommentMandatory;
                updateRatingModel.LowRange = configRatingObject.LowRange;
                updateRatingModel.HighRange = configRatingObject.HighRange;
                updateRatingModel.MinRange = configRatingObject.MinRange;
                updateRatingModel.MaxRange = configRatingObject.MaxRange;
                if(configRatingObject.MinRange!=null && configRatingObject.MaxRange!=null)
                {
                    updateRatingModel.RatingRange = configRatingObject.MinRange + " %- " + configRatingObject.MaxRange + "%";
                }
                else if(configRatingObject.MinRange != null && configRatingObject.MaxRange == null)
                {
                    updateRatingModel.RatingRange = configRatingObject.MinRange + " %";
                }
                else if (configRatingObject.MinRange == null && configRatingObject.MaxRange != null)
                {
                    updateRatingModel.RatingRange = configRatingObject.MaxRange + " %";
                }
                else
                {
                    updateRatingModel.RatingRange = null;
                }

                updateRatingModel.UpdatedBy = configRatingObject.UpdatedBy;
                updateRatingModel.UpdatedDate = DateTime.Now;
                updateRatingModel.RatingOrder = configRatingObject.RatingOrder;

                m_baseRepository.Edit<Rating>(updateRatingModel);
                m_baseRepository.SaveChanges();
            }
            else
            {
                Rating addRatingModel = new Rating();
                addRatingModel.RatingID = "1";
                addRatingModel.RatingType = "Compensation";
                addRatingModel.RatingDetailDescr = "Eligible";
                addRatingModel.RatingDescr = configRatingObject.RatingDescription;
                addRatingModel.IsCommentMandatory = false;
                addRatingModel.LowRange = configRatingObject.LowRange;
                addRatingModel.HighRange = configRatingObject.HighRange;
                addRatingModel.UpdatedBy = configRatingObject.UpdatedBy;
                addRatingModel.UpdatedDate = DateTime.Now;
                addRatingModel.MinRange = configRatingObject.MinRange;
                addRatingModel.MaxRange = configRatingObject.MaxRange;
                addRatingModel.RatingOrder = configRatingObject.RatingOrder;
                m_baseRepository.Add<Rating>(addRatingModel);
                m_baseRepository.SaveChanges();
                isUpdated = true;
            }
            
           await m_baseRepository.ExecuteStoredProcedure("Talent.USP_RAT_PUT_UpdateMeritRange");
            return isUpdated;
        }


        // Author         :  Shaheena Shaik
        // Creation Date  :  27-March-2017   
        // Reviewed By    :Hari.C
        // Reviewed Date  : 3-March-2017
        /// <summary>
        /// Getting Rating data of a selected ratingNum
        /// </summary>
        /// <param name="ratingNum">Rating Num of selected row</param>
        /// <returns>Returninig a query of Rating details of selected RatingNum</returns>
        public Rating GetRatingTableData(int ratingNum)
        {
            return m_baseRepository.GetQuery<Rating>().Where(x => x.RatingNum == ratingNum).FirstOrDefault();
        }

        // Author         :  Shaheena Shaik
        // Creation Date  :  27-March-2017   
        // Reviewed By    :Hari.C
        // Reviewed Date  : 3-March-2017
        /// <summary>
        /// Deleting Rating Details from database
        /// </summary>
        /// <param name="ratingNum">Rating Num of selectedRow to be deleted</param>
        /// <returns>Returning a boolean value</returns>
        public bool DeleteRatingDetails(int ratingNum)
        {
            Rating deleteRatingData = m_baseRepository.GetQuery<Rating>().Where(x => x.RatingNum == ratingNum).FirstOrDefault();
            List<EmployeeCompRating> empCompRatingDetails = m_baseRepository.GetQuery<EmployeeCompRating>().Where(x => x.MeritPerformanceRating == ratingNum).ToList();
            foreach (var empCompRating in empCompRatingDetails)
            {               
                    empCompRating.MeritPerformanceRating = null;
                    m_baseRepository.Edit<EmployeeCompRating>(empCompRating);                 
                                    
            }
            m_baseRepository.Delete<Rating>(deleteRatingData);
            return m_baseRepository.SaveChanges() > 0;            
        }

        public bool RatingValidation(int ratingNum,string ratingDescription)
        {
            if (ratingNum == 0)
            {
                var a = m_baseRepository.GetQuery<Rating>().Where(x => x.RatingDescr == ratingDescription).FirstOrDefault();
                return a == null ? false : true;
            }
            else
            {
                var b=m_baseRepository.GetQuery<Rating>().Where(x => x.RatingNum == ratingNum).Select(x=>x.RatingDescr).FirstOrDefault();
                if(b==ratingDescription)
                {
                    return false;
                }
                var a = m_baseRepository.GetQuery<Rating>().Where(x => x.RatingDescr == ratingDescription).FirstOrDefault();
                return a == null ? false : true;
            }
        }
        #endregion
    }
}
