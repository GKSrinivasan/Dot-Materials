// Copyright (c) 2014 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.

// Component Name :   ExchangeRate
// Description    :   All Business logic related to ExchangeRate is placed here 	
// Author         :   Hariharasubramaniyan Chandrasekaran
// Creation Date  :   27-Mar-2017

using Laserbeam.BusinessObject.Common;
using Laserbeam.DataManager.Interfaces.Common;
using Laserbeam.ProcessManager.Interfaces.Common;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Laserbeam.ProcessManager.Common
{
    public class ExchangeRateProcessManager : IExchangeRateProcessManager
    {
        #region Fields
        // Author         :   Hariharasubramaniyan Chandrasekaran	
        // Creation Date  :   27-Mar-2017 
        /// <summary>
        /// Instance of  ExchangeRateRepository
        /// </summary>
        private IExchangeRateRepository m_exchangeRateRepository;// {get;set;}
       
        #endregion
        #region Fields
        // Author         :   Hariharasubramaniyan Chandrasekaran	
        // Creation Date  :   27-Mar-2017 
        /// <summary>
        /// Instance of  ExchangeRateRepository
        /// </summary>
        public ExchangeRateProcessManager(IExchangeRateRepository exchangeRateRepository)
        {
            m_exchangeRateRepository = exchangeRateRepository;
        }

        #endregion
        #region Implementation
        public async Task<IEnumerable<ExchangeRateGridData>> GetExchangeRateData()
        {
            return await m_exchangeRateRepository.GetExchangeRateData();
        }
         public string GetBaseCurrency()
        {
            return m_exchangeRateRepository.GetBaseCurrency();
        }
        public IQueryable<CultureCodeData> GetCultureCode()
        {
            return m_exchangeRateRepository.GetCultureCode();
        }
        public int AddExchangeRate(ExchangeRateData data)
        {
            return m_exchangeRateRepository.AddExchangeRate(data);
        }
        public ExchangeRatePreviewData GetPreviewData()
        {
            return m_exchangeRateRepository.GetPreviewData();
        }
       public int UpdateExchangeRate(Countries exchangeRate,string baseCurrency)
        {
            return m_exchangeRateRepository.UpdateExchangeRate(exchangeRate, baseCurrency);
        }

        public int UpdateBaseCurrency(CultureCodeData selectedBaseCurrency)
        {
            return m_exchangeRateRepository.UpdateBaseCurrency(selectedBaseCurrency);
        }
        // Author       : Shaheena Shaik
        // Creation Date: 24-April-2017
        /// <summary>
        /// Getting existing Currency Codes from database
        /// </summary>
        /// <returns>Returning a query of currency codes</returns>
        public async Task<IEnumerable<ExchangeCurrencies>> GetCurrencyCode()
        {
            return await m_exchangeRateRepository.GetCurrencyCode();
        }

        public bool ValidateCurrencyCode(string currencyCodeValue)
        {
            return m_exchangeRateRepository.ValidateCurrencyCode(currencyCodeValue);
        }

        // Author       : Shaheena Shaik
        // Creation date: 30-June-2017
        /// <summary>
        /// Getting ExchangeRate Export data from database
        /// </summary>
        /// <returns>Returning ExchangeRate Export</returns>
        public async Task<DataTable> GetExchangeRateExportData()
        {
            return await m_exchangeRateRepository.GetExchangeRateExportData();
        }
        #endregion



    }
}
