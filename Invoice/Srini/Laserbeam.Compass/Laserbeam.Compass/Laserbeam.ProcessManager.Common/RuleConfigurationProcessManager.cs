// Copyright (c) 2014 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.

// Component Name :   RuleConfiguration
// Description    :   All Business logic related to RuleConfiguration is placed here 	
// Author         :   Raja Ganapathy
// Creation Date  :   30-Mar-201

using Laserbeam.BusinessObject.Common;
using Laserbeam.DataManager.Interfaces.Common;
using Laserbeam.ProcessManager.Interfaces.Common;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Laserbeam.ProcessManager.Common
{
    public class RuleConfigurationProcessManager : IRuleConfigurationProcessManager
    {
        #region Fields
        // Author         :   Raja Ganapathy
        // Creation Date  :   30-Mar-2017 
        /// <summary>
        /// Object of IWorkForceRepository
        /// </summary>
        private IRuleConfigurationRepository m_ruleConfigurationRepository;
        #endregion

        #region Constructors
        // Author         :   Raja Ganapathy
        // Creation Date  :   30-Mar-2017 
        /// <summary>
        /// Initialize the repository
        /// </summary>
        /// <param name="workForceProcessManager">Object of IWorkForceRepository</param>        
        public RuleConfigurationProcessManager(IRuleConfigurationRepository ruleConfigurationRepository)
        {
            m_ruleConfigurationRepository = ruleConfigurationRepository;
        }

        #endregion

        #region Implementations


        public List<DropDownListModel> GetDateFormats()
        {
            var dates = m_ruleConfigurationRepository.GetDateFormats().ToList();
            var result = (from dateFormat in dates
                          select new DropDownListModel
                              {
                                  Text = dateFormat.FormatType,
                                  Value = dateFormat.FormatType.ToString()
                              }).ToList();
            return result;
        }
        
        public async Task PutBusSetting(BusSettingModel BusSettingDetails, int UserNum)
        {
            await m_ruleConfigurationRepository.PutBusSetting(BusSettingDetails, UserNum);
        }

        public async Task RunBuildManagerTree()
        {
           await m_ruleConfigurationRepository.RunBuildManagerTree();
        }

        public BusSettingModel GetBusSetting()
        {
            return m_ruleConfigurationRepository.GetBusSetting();
        }


        public async Task<bool> PutApplyRules()
        {
            return await m_ruleConfigurationRepository.PutApplyRules();
        }

        public decimal GetBudgetPct()
        {
            return m_ruleConfigurationRepository.GetBudgetPct();
        }

        public List<RoundingTypeModel> GetRoundingType()
        {
            return (from roundingFormat in m_ruleConfigurationRepository.GetRoundingType()
                    select new RoundingTypeModel
                    {
                        RoundingTypeId = roundingFormat.RoundingTypeId,
                        RoundTypeKey = roundingFormat.RoundTypeKey,
                        RoundTypeValue = roundingFormat.RoundTypeValue,
                        Active = roundingFormat.Active
                    }).ToList();

        }


        public List<DecimalTypeModel> GetDecimalType()
        {
            return (from decimalFormat in m_ruleConfigurationRepository.GetDecimalType()
                    select new DecimalTypeModel
                    {
                        DecimalTypeId = decimalFormat.DecimalTypeId,
                        DecimalTypeKey = decimalFormat.DecimalTypeKey,
                        DecimalTypeValue = decimalFormat.DecimalTypeValue,
                        Active = decimalFormat.Active,
                        Type = decimalFormat.Type
                    }).ToList();
        }
        public bool PutWizardDetails(int userNum, byte stepInfo, bool isWizard)
        {
            return m_ruleConfigurationRepository.PutWizardDetails(userNum, stepInfo, isWizard);
        }
        public byte GetWizardDetails(int userNum)
        {
            return m_ruleConfigurationRepository.GetWizardDetails(userNum);
        }

        //public bool clearAllData ()
        //{
        //   return m_ruleConfigurationRepository.ClearAllData();
        //}

        public int UpdateExchangeRate(Countries exchangeRate, string baseCurrency)
        {
            return m_ruleConfigurationRepository.UpdateExchangeRate(exchangeRate, baseCurrency);
        }

        public bool CheckMultiCurrencyChanged(string key, string value)
        {
            return m_ruleConfigurationRepository.CheckMultiCurrencyChanged(key, value);
        }

        public async Task<bool> clearApprovalDetails()
        {
            return await m_ruleConfigurationRepository.clearApprovalDetails();
        }

        #endregion
    }
}
