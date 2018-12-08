// Copyright (c) 2016 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.
// Component Name  :  IExchangeRateRepository
// Description     :  Interface signature for ExchangeRateRepository
// Author         : Hari.C
// Creation Date  :  10-Feb-2017 

using Laserbeam.BusinessObject.Common;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Laserbeam.DataManager.Interfaces.Common
{
    public interface IExchangeRateRepository
    {
       Task<IEnumerable<ExchangeRateGridData>> GetExchangeRateData();
       IQueryable<CultureCodeData> GetCultureCode();
       int AddExchangeRate(ExchangeRateData data);
       ExchangeRatePreviewData GetPreviewData();
        int UpdateExchangeRate(Countries exchangeRate,string baseCurrency);
        int UpdateBaseCurrency(CultureCodeData selectedBaseCurrency);
        string GetBaseCurrency();
        // Author       : Shaheena Shaik
        // Creation Date: 24-April-2017
        /// <summary>
        /// Getting existing Currency Codes from database
        /// </summary>
        /// <returns>Returning a query of currency codes</returns>
        Task<IEnumerable<ExchangeCurrencies>> GetCurrencyCode();

        bool ValidateCurrencyCode(string currencyCodeValue);

        // Author       : Shaheena Shaik
        // Creation date: 30-June-2017
        /// <summary>
        /// Getting ExchangeRate Export data from database
        /// </summary>
        /// <returns>Returning ExchangeRate Export</returns>
        Task<DataTable> GetExchangeRateExportData();
    }
}
