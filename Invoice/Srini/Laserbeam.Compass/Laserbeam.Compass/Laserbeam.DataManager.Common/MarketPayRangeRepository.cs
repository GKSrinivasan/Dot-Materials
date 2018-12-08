// Copyright (c) 2015 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.

// Component Name  :    MarketPayRangeRepository
// Description     : 	Repository for MarketPayRange
// Author          :	Arunraj C
// Creation Date   : 	11-OCT-2017

using Laserbeam.BusinessObject.Common;
using Laserbeam.BusinessObject.Common.Constants;
using Laserbeam.Constant.HR;
using Laserbeam.DataManager.Interfaces.Common;
using Laserbeam.DataManager.Interfaces.Core;
using Laserbeam.EntityManager.Common;
using Laserbeam.Libraries.Core.Interfaces.Common;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Laserbeam.DataManager.Common
{
    public class MarketPayRangeRepository : IMarketPayRangeRepository
    {
        #region Fields
        // Author         :   Arunraj C
        // Creation Date  :   11-OCT-2017
        /// <summary>
        /// Instance of BaseRepository
        /// </summary>
        private IBaseRepository m_baseRepository;
        private ITenantCacheProvider m_tenantCacheProvider;
        #endregion

        #region Constructors
        // Author        :  Arunraj C
        // Creation Date :  11-OCT-2017
        /// <summary>
        ///Initializes objects used in this class 
        ///</summary>        
        /// <param name="baseRepository">Base Repository Object</param>  

        public MarketPayRangeRepository(IBaseRepository baseRepository, ITenantCacheProvider tenantCacheProvider)
        {
            m_baseRepository = baseRepository;
            m_tenantCacheProvider = tenantCacheProvider;
        }
        #endregion

        #region Public Methods
        public BusSettingModel GetBusSetting()
        {
            List<BusSetting> busSetting = m_baseRepository.GetQuery<BusSetting>().ToList();
          //  var appSetting = m_baseRepository.GetQuery<AppSetting>().ToList();
            BusSettingModel busSettingModel = new BusSettingModel();
            busSettingModel.MarketPayRange = BusSettingValue(busSetting, "MarketPayRange");
            return busSettingModel;
        }

        public async Task SetBusSetting(BusSettingModel busSetting)
        {
            SqlParameter[] sqlParameter = { new SqlParameter("@SelectedMarketPayRange", busSetting.MarketPayRange) };
            await m_baseRepository.ExecuteStoredProcedure("Talent.USP_MPR_PUT_UpdateBusSettingAndMarketPayRange", sqlParameter);
        }

        public async Task<List<MarketPayRangeGridModel>> GetMarketPayRangeData(int loggedInUserNum, string selectedMarketPayRange)
        {
            SqlParameter[] sqlParameter = { new SqlParameter("@loggedInUserNum", loggedInUserNum) };
            var result = await m_baseRepository.GetData<MarketPayRangeGridModel>("[Talent].[USP_MPR_GET_MarketPayRangeList] @loggedInUserNum", sqlParameter);
            return result.ToList();
        }

        public bool IsMarketDataExist(string selectedMarketPayRange, string updateDataValue)
        {
            int count = 0;
            bool result = false;
            if (selectedMarketPayRange == MarketPayRangeConstants.JobCode)
                count = m_baseRepository.GetQuery<Job>().Where(x => x.JobCode.Trim() == updateDataValue.Trim() && x.Active == true).Count();
            else if (selectedMarketPayRange == MarketPayRangeConstants.Grade)
                count = m_baseRepository.GetQuery<Grade>().Where(x => x.GradeCode.Trim() == updateDataValue.Trim() && x.Active == true).Count();
            else if (selectedMarketPayRange == MarketPayRangeConstants.ByEmployee)
                count = m_baseRepository.GetQuery<Employee>().Where(x => x.EmployeeID.Trim() == updateDataValue.Trim()).Count();
            if (count > 0)
                result = true;
            return result;
        }

        public async Task AddUpdateMarketPayRange(MarketPayRangeModel model, int loginUserNum)
        {
            if (model != null)
            {                
                if (model.MarketPayRangeNum == 0) // Add Market Pay Range
                {
                    SqlParameter[] sqlParameter = {
                        new SqlParameter("@MarketPayRangeNum", model.MarketPayRangeNum),
                        new SqlParameter("@JobCode", model.JobCode??""),
                        new SqlParameter("@Grade", model.Grade??""),
                        new SqlParameter("@EmployeeID", model.EmployeeID??""),
                        new SqlParameter("@CurrentMin", model.CurrentMin??0),
                        new SqlParameter("@CurrentMid", model.CurrentMid??0),
                        new SqlParameter("@CurrentMax", model.CurrentMax??0),
                        new SqlParameter("@HourlyCurrentMin", model.HourlyCurrentMin??0),
                        new SqlParameter("@HourlyCurrentMid", model.HourlyCurrentMid??0),
                        new SqlParameter("@HourlyCurrentMax", model.HourlyCurrentMax??0),
                        new SqlParameter("@FutureMin", model.FutureMin??0),
                        new SqlParameter("@FutureMid", model.FutureMid??0),
                        new SqlParameter("@FutureMax", model.FutureMax??0),
                        new SqlParameter("@HourlyFutureMin", model.HourlyFutureMin??0),
                        new SqlParameter("@HourlyFutureMid", model.HourlyFutureMid??0),
                        new SqlParameter("@HourlyFutureMax", model.HourlyFutureMax??0),
                        new SqlParameter("@CreatedBy", loginUserNum),
                        new SqlParameter("@UpdatedBy", loginUserNum),
                        new SqlParameter("@Action", "Add"),
                            };
                   await m_baseRepository.ExecuteStoredProcedure("Talent.USP_MPR_PUT_InsertUpdateMarketPayRange", sqlParameter);
                }
                else // Edit Market Pay Range
                {
                    SqlParameter[] sqlParameter = {
                        new SqlParameter("@MarketPayRangeNum", model.MarketPayRangeNum),
                        new SqlParameter("@JobCode", model.JobCode??""),
                        new SqlParameter("@Grade", model.Grade??""),
                        new SqlParameter("@EmployeeID", model.EmployeeID??""),
                        new SqlParameter("@CurrentMin", model.CurrentMin??0),
                        new SqlParameter("@CurrentMid", model.CurrentMid??0),
                        new SqlParameter("@CurrentMax", model.CurrentMax??0),
                        new SqlParameter("@HourlyCurrentMin", model.HourlyCurrentMin??0),
                        new SqlParameter("@HourlyCurrentMid", model.HourlyCurrentMid??0),
                        new SqlParameter("@HourlyCurrentMax", model.HourlyCurrentMax??0),
                        new SqlParameter("@FutureMin", model.FutureMin??0),
                        new SqlParameter("@FutureMid", model.FutureMid??0),
                        new SqlParameter("@FutureMax", model.FutureMax??0),
                        new SqlParameter("@HourlyFutureMin", model.HourlyFutureMin??0),
                        new SqlParameter("@HourlyFutureMid", model.HourlyFutureMid??0),
                        new SqlParameter("@HourlyFutureMax", model.HourlyFutureMax??0),
                        new SqlParameter("@CreatedBy", loginUserNum),
                        new SqlParameter("@UpdatedBy", loginUserNum),
                        new SqlParameter("@Action", "Update"),
                            };
                    await m_baseRepository.ExecuteStoredProcedure("Talent.USP_MPR_PUT_InsertUpdateMarketPayRange", sqlParameter);
                }
            }
        }

        public MarketPayRangeModel GetSelectedMarketPayRange(int MarketRangeNum, string selectedMarketPayRange)
        {
            var marketPayRangeValue = (from mar in m_baseRepository.GetQuery<MarketPayRange>().ToList()
                                       join j in m_baseRepository.GetQuery<Job>().ToList() on mar.JobNum equals j.JobNum into jobTmp
                                       from jb in jobTmp.DefaultIfEmpty()
                                       join g in m_baseRepository.GetQuery<Grade>().ToList() on mar.GradeNum equals g.GradeNum into gradeTmp
                                       from gr in gradeTmp.DefaultIfEmpty()
                                       join e in m_baseRepository.GetQuery<Employee>().ToList() on mar.EmployeeNum equals e.EmployeeNum into empTmp
                                       from emp in empTmp.DefaultIfEmpty()
                                       where mar.MarketPayRangeNum == MarketRangeNum
                                       select new MarketPayRangeModel
                                       {
                                           JobNum = (selectedMarketPayRange == MarketPayRangeConstants.JobCode) ? mar.JobNum : 0,
                                           JobCode = (selectedMarketPayRange == MarketPayRangeConstants.JobCode) ? jb.JobCode : "",
                                           GradeNum = (selectedMarketPayRange == MarketPayRangeConstants.Grade) ? mar.GradeNum : 0,
                                           Grade = (selectedMarketPayRange == MarketPayRangeConstants.Grade) ? gr.GradeCode : "",
                                           EmployeeNum = (selectedMarketPayRange == MarketPayRangeConstants.ByEmployee) ? mar.EmployeeNum : 0,
                                           EmployeeID = (selectedMarketPayRange == MarketPayRangeConstants.ByEmployee) ? emp.EmployeeID : "",
                                           EmployeeName = (selectedMarketPayRange == MarketPayRangeConstants.ByEmployee) ? emp.EmployeeName : "",
                                           CurrentMin = mar.CurrentMin,
                                           CurrentMid = mar.CurrentMid,
                                           CurrentMax = mar.CurrentMax,
                                           HourlyCurrentMin = mar.HourlyCurrentMin,
                                           HourlyCurrentMid = mar.HourlyCurrentMid,
                                           HourlyCurrentMax = mar.HourlyCurrentMax,
                                           FutureMin = mar.FutureMin,
                                           FutureMid = mar.FutureMid,
                                           FutureMax = mar.FutureMax,
                                           HourlyFutureMin = mar.HourlyFutureMin,
                                           HourlyFutureMid = mar.HourlyFutureMid,
                                           HourlyFutureMax = mar.HourlyFutureMax
                                       }).FirstOrDefault();
            return marketPayRangeValue;
        }
        #endregion

        #region Private Method
        private string BusSettingValue(List<BusSetting> busSetting, string key)
        {
            var busSettingData = busSetting.Where(x => x.KeyValue.Trim() == key.Trim()).FirstOrDefault();
            return busSettingData != null ? Convert.ToString(busSettingData.KeyDataValue) : null;
        }

        private void UpdateBusSettingValue(string key, string KeyDataValue)
        {
            var busSettingValue = m_baseRepository.GetQuery<BusSetting>().Where(x => x.KeyValue == key).FirstOrDefault();
            busSettingValue.KeyDataValue = KeyDataValue;
            m_baseRepository.Edit<BusSetting>(busSettingValue);
            m_baseRepository.SaveChanges();
            m_tenantCacheProvider.RemoveCache(ApplicationCacheConstants.BussinessSetting);
        }
        #endregion
    }
}
