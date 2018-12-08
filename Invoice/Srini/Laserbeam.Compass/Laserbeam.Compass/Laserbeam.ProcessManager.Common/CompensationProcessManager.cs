// Copyright (c) 2016 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.
// Component Name  : 	Compensation Business Manager
// Description     : 	Compensation related business logics	
// Author          :  Raja Ganapathy
// Creation Date   :  05-Jul-2016  
using Laserbeam.BusinessObject.Common;
using Laserbeam.BusinessObject.Common.CachedModels;
using Laserbeam.BusinessObject.Common.MeritBusinussObjects;
using Laserbeam.Constant.HR;
using Laserbeam.DataManager.Interfaces.Common;
using Laserbeam.EntityManager.Common;
using Laserbeam.ProcessManager.Interfaces.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
namespace Laserbeam.ProcessManager.Common
{
    public class CompensationProcessManager : ICompensationProcessManager
    {
        #region Fields
        // Author         :   Raja Ganapathy
        // Creation Date  :   05-Jul-2016  
        /// <summary>
        /// Instance of CompensationRepository
        /// </summary>
        ICompensationRepository m_compensationRepository;        
        #endregion

        #region Constructors
        // Author         :  Raja Ganapathy
        // Creation Date  :  05-Jul-2016  
        /// <summary>
        /// Initializes objects used in this class
        /// </summary>
        /// <param name="budgetRepository">compensationRepository objcets</param>
        public CompensationProcessManager(ICompensationRepository compensationRepository)
        {
            m_compensationRepository = compensationRepository;            
        }
        #endregion

        // Author        :  Raja Ganapathy		
        // Creation Date :  20-06-2016       
        // Reviewed By   :  Hari		
        // Reviewed Date :  13-10-2016
        /// <summary>
        /// Gets list of ratings for compensation
        /// </summary>
        /// <returns>A list of ratings</returns>
        public IQueryable<RatingModel> GetRatings()
        {
            return m_compensationRepository.GetRatings();
        }

        public bool IsEmployeeDataEmpty()
        {
            return m_compensationRepository.IsEmployeeDataEmpty();

        }

        // Author        :  Raja Ganapathy		
        // Creation Date :  20-06-2016
        // Reviewed By   :  Hari		
        // Reviewed Date :  13-10-2016
        /// <summary>
        /// To get alias name and visibility configuration of employees criteria
        /// </summary>
        /// <returns>CompensationConfiguration object</returns>
        public CompensationConfiguration GetReporteeConfiguration()
        {
            var metaColumn = m_compensationRepository.GetReporteeConfiguration().Where(metaColumns => metaColumns.MasterDisplay == true);
            AliasName GeneralAliasName = new AliasName();            
            CompensationGridDisplay compensationGridDisplay = new CompensationGridDisplay();

            foreach (var item in metaColumn)
            {
                switch (item.FieldName)
                {
                    case "EmployeeID":
                        GeneralAliasName.EmployeeID = item.GeneralAliasName;
                        compensationGridDisplay.EmployeeID = item.GridDisplay;
                        break;
                    case "NewSalary":
                        GeneralAliasName.NewSalary = item.GeneralAliasName;
                        compensationGridDisplay.NewSalary = item.GridDisplay;
                        break;
                    case "TotalNewComp":
                        GeneralAliasName.TotalNewComp = item.GeneralAliasName;
                        compensationGridDisplay.TotalNewComp = item.GridDisplay;
                        break;
                    case "EmployeeName":
                        GeneralAliasName.EmployeeName = item.GeneralAliasName;
                        compensationGridDisplay.EmployeeName = item.GridDisplay;
                        break;
                    case "JobCode":
                        GeneralAliasName.JobCode = item.GeneralAliasName;
                        compensationGridDisplay.JobCode = item.GridDisplay;
                        break;
                    case "JobTitle":
                        GeneralAliasName.JobTitle = item.GeneralAliasName;
                        compensationGridDisplay.JobTitle = item.GridDisplay;
                        break;
                    case "CurrentGrade":
                        GeneralAliasName.CurrentGrade = item.GeneralAliasName;
                        compensationGridDisplay.CurrentGrade = item.GridDisplay;
                        break;
                    case "BusinessUnit":
                        GeneralAliasName.BusinessUnit = item.GeneralAliasName;
                        compensationGridDisplay.BusinessUnit = item.GridDisplay;
                        break;
                    case "Function":
                        GeneralAliasName.Function = item.GeneralAliasName;
                        compensationGridDisplay.Function = item.GridDisplay;
                        break;
                    case "Department":
                        GeneralAliasName.Department = item.GeneralAliasName;
                        compensationGridDisplay.Department = item.GridDisplay;
                        break;
                    case "EmployeeClass":
                        GeneralAliasName.EmployeeClass = item.GeneralAliasName;
                        compensationGridDisplay.EmployeeClass = item.GridDisplay;
                        break;
                    case "FLSAStatus":
                        GeneralAliasName.FLSAStatus = item.GeneralAliasName;
                        compensationGridDisplay.FLSAStatus = item.GridDisplay;
                        break;
                    case "EmployeeStatus":
                        GeneralAliasName.EmployeeStatus = item.GeneralAliasName;
                        compensationGridDisplay.EmployeeStatus = item.GridDisplay;
                        break;
                    case "FTE":
                        GeneralAliasName.FTE = item.GeneralAliasName;
                        compensationGridDisplay.FTE = item.GridDisplay;
                        break;
                    case "WorkCountry":
                        GeneralAliasName.WorkCountry = item.GeneralAliasName;
                        compensationGridDisplay.WorkCountry = item.GridDisplay;
                        break;
                    case "WorkLocation":
                        GeneralAliasName.WorkLocation = item.GeneralAliasName;
                        compensationGridDisplay.WorkLocation = item.GridDisplay;
                        break;
                    case "HireDate":
                        GeneralAliasName.HireDate = item.GeneralAliasName;
                        compensationGridDisplay.HireDate = item.GridDisplay;
                        break;
                    case "PayCurrency":
                        GeneralAliasName.PayCurrency = item.GeneralAliasName;
                        compensationGridDisplay.PayCurrency = item.GridDisplay;
                        break;
                    case "CurrentHourlyRate":
                        GeneralAliasName.CurrentHourlyRate = item.GeneralAliasName;
                        compensationGridDisplay.CurrentHourlyRate = item.GridDisplay;
                        break;
                    case "NewHourlyRate":
                        GeneralAliasName.NewHourlyRate = item.GeneralAliasName;
                        compensationGridDisplay.NewHourlyRate = item.GridDisplay;
                        break;
                    case "CurrentAnnualizedSalary":
                        GeneralAliasName.CurrentAnnualizedSalary = item.GeneralAliasName;
                        compensationGridDisplay.CurrentAnnualizedSalary = item.GridDisplay;
                        break;
                    case "CurrentAnnualSalary":
                        GeneralAliasName.CurrentAnnualSalary = item.GeneralAliasName;
                        compensationGridDisplay.CurrentAnnualSalary = item.GridDisplay;
                        break;
                    case "SalaryMin":
                        GeneralAliasName.SalaryMin = item.GeneralAliasName;
                        compensationGridDisplay.SalaryMin = item.GridDisplay;
                        break;
                    case "SalaryMid":
                        GeneralAliasName.SalaryMid = item.GeneralAliasName;
                        compensationGridDisplay.SalaryMid = item.GridDisplay;
                        break;
                    case "SalaryMax":
                        GeneralAliasName.SalaryMax = item.GeneralAliasName;
                        compensationGridDisplay.SalaryMax = item.GridDisplay;
                        break;
                    case "MeritProrationDate":
                        GeneralAliasName.MeritProrationDate = item.GeneralAliasName;
                        compensationGridDisplay.MeritProrationDate = item.GridDisplay;
                        break;
                    case "MeritProrationFactor":
                        GeneralAliasName.MeritProrationFactor = item.GeneralAliasName;
                        compensationGridDisplay.MeritProrationFactor = item.GridDisplay;
                        break;
                    case "MeritPerformanceRating":
                        GeneralAliasName.MeritPerformanceRating = item.GeneralAliasName;
                        compensationGridDisplay.MeritPerformanceRating = item.GridDisplay;
                        break;
                    case "MeritPct":
                        GeneralAliasName.MeritPct = item.GeneralAliasName;
                        compensationGridDisplay.MeritPct = item.GridDisplay;
                        break;
                    case "MeritAmount":
                        GeneralAliasName.MeritAmount = item.GeneralAliasName;
                        compensationGridDisplay.MeritAmount = item.GridDisplay;
                        break;
                    case "MeritIncreaseGuideline":
                        GeneralAliasName.MeritIncreaseGuideline = item.GeneralAliasName;
                        compensationGridDisplay.MeritIncreaseGuideline = item.GridDisplay;
                        break;
                    case "LumpSumPct":
                        GeneralAliasName.LumpSumPct = item.GeneralAliasName;
                        compensationGridDisplay.LumpSumPct = item.GridDisplay;
                        break;
                    case "LumpSumAmount":
                        GeneralAliasName.LumpSumAmount = item.GeneralAliasName;
                        compensationGridDisplay.LumpSumAmount = item.GridDisplay;
                        break;
                    case "PromotionPct":
                        GeneralAliasName.PromotionPct = item.GeneralAliasName;
                        compensationGridDisplay.PromotionPct = item.GridDisplay;
                        break;
                    case "PromotionAmount":
                        GeneralAliasName.PromotionAmount = item.GeneralAliasName;
                        compensationGridDisplay.PromotionAmount = item.GridDisplay;
                        break;
                    case "PromoteTo":
                        GeneralAliasName.PromoteTo = item.GeneralAliasName;
                        compensationGridDisplay.PromoteTo = item.GridDisplay;
                        break;
                    case "AdjustmentPct":
                        GeneralAliasName.AdjustmentPct = item.GeneralAliasName;
                        compensationGridDisplay.AdjustmentPct = item.GridDisplay;
                        break;
                    case "AdjustmentAmount":
                        GeneralAliasName.AdjustmentAmount = item.GeneralAliasName;
                        compensationGridDisplay.AdjustmentAmount = item.GridDisplay;
                        break;
                    case "MoreInfo1":
                        GeneralAliasName.MoreInfo1 = item.GeneralAliasName;
                        compensationGridDisplay.MoreInfo1 = item.GridDisplay;
                        break;
                    case "MoreInfo2":
                        GeneralAliasName.MoreInfo2 = item.GeneralAliasName;
                        compensationGridDisplay.MoreInfo2 = item.GridDisplay;
                        break;
                    case "MoreInfo3":
                        GeneralAliasName.MoreInfo3 = item.GeneralAliasName;
                        compensationGridDisplay.MoreInfo3 = item.GridDisplay;
                        break;
                    case "MoreInfo4":
                        GeneralAliasName.MoreInfo4 = item.GeneralAliasName;
                        compensationGridDisplay.MoreInfo4 = item.GridDisplay;
                        break;
                    case "MoreInfo5":
                        GeneralAliasName.MoreInfo5 = item.GeneralAliasName;
                        compensationGridDisplay.MoreInfo5 = item.GridDisplay;
                        break;
                    case "SupervisorName":
                        GeneralAliasName.SupervisorName = item.GeneralAliasName;
                        compensationGridDisplay.SupervisorName = item.GridDisplay;
                        break;
                    case "Comments":
                        GeneralAliasName.Comments = item.GeneralAliasName;
                        compensationGridDisplay.Comments = item.GridDisplay;
                        break;
                    case "CompaRatio":
                        GeneralAliasName.CompaRatio = item.GeneralAliasName;
                        compensationGridDisplay.CompaRatio = item.GridDisplay;
                        break;
                    case "NewCompaRatio":
                        GeneralAliasName.NewCompaRatio = item.GeneralAliasName;
                        compensationGridDisplay.NewCompaRatio = item.GridDisplay;
                        break;

                }
            }
            CompensationConfiguration compensationConfiguration = new CompensationConfiguration();
            compensationConfiguration.aliasName = GeneralAliasName;            
            compensationConfiguration.compensationGridDisplay = compensationGridDisplay;
            return compensationConfiguration;
        }

        // Author        :  Raja Ganapathy		
        // Creation Date :  20-06-2016
        // Reviewed By   :  Hari		
        // Reviewed Date :  13-10-2016
        /// <summary>
        /// To get business configuration for compensation
        /// </summary>
        /// <returns>CompensationTypeConfiguration object</returns>
        public CompensationTypeConfiguration GetCompensationTypeConfiguration()
        {
            CompensationTypeConfiguration meritConfiguration = new CompensationTypeConfiguration();
            //List<BusSetting> busSettings = m_compensationRepository.GetCompensationTypeConfiguration().ToList();
            BusinessSettingModel busSettings = m_compensationRepository.GetCompensationTypeConfiguration();

            var eitherLumpSum = busSettings.FeatureConfiguration.EitherMeritOrLumpSum;
            var lumpSumPct = busSettings.LumpsumRule.RangeMaxPct;
            var lumpSumAmt = busSettings.LumpsumRule.RangeMaxAmt;
            meritConfiguration.MeritOverrideNoJustification = busSettings.MeritOverrideRule.MeritOverrideNoJustification;
            meritConfiguration.MeritOverrideHardStop = busSettings.MeritOverrideRule.MeritOverrideHardStop;
            meritConfiguration.MeritOverrideIncreaseWithinGuideline = busSettings.MeritOverrideRule.MeritIncreaseWithinGuideline;
            meritConfiguration.MeritOverrideSoftStop = busSettings.MeritOverrideRule.MeritOverrideSoftStop;
            meritConfiguration.MeritOverrideMandatoryJustification = busSettings.MeritOverrideRule.MandatoryJustification;
            meritConfiguration.EitherMeritOrlumpsum = busSettings.FeatureConfiguration.EitherMeritOrLumpSum;

            meritConfiguration.LumpSumRuleLumpSumType = busSettings.LumpsumRule.LumpsumType;
            meritConfiguration.LumpSumRuleRangeMaxPct = busSettings.LumpsumRule.RangeMaxPct;
            meritConfiguration.LumpSumRuleRangeMaxAmt = busSettings.LumpsumRule.RangeMaxAmt;
            meritConfiguration.MeritValuesReCalculate = busSettings.LumpsumRule.MeritValuesReCalculate;
            meritConfiguration.AutoCalculateLumpSum = busSettings.LumpsumRule.AutoCalculateLumpSum;

            meritConfiguration.ProrationRuleProrate = busSettings.ProrationRule.Prorate;
            meritConfiguration.ProrationApplyMeritDiscretion = busSettings.ProrationRule.ApplyMeritDiscretion;
            meritConfiguration.ProrationApplyBudgetCalculations = busSettings.ProrationRule.ApplyBudgetCalculations;
            meritConfiguration.ProrationApplyAdjustmentCalculations = busSettings.ProrationRule.ApplyAdjustmentCalculations;
            meritConfiguration.ProrationRuleProrateIncreaseStartDate = Convert.ToDateTime(busSettings.ProrationRule.ProrateIncreaseStartDate, CultureInfo.InvariantCulture).ToLongDateString();
            meritConfiguration.ProrationRuleProrateIncreaseEndDate = Convert.ToDateTime(busSettings.ProrationRule.ProrateIncreaseEndDate, CultureInfo.InvariantCulture).ToLongDateString();
            meritConfiguration.ProrationRuleProrationType = busSettings.ProrationRule.ProrationType;
            meritConfiguration.ProrationRuleProrationLength = busSettings.ProrationRule.ProrationLength;
            meritConfiguration.ProrationRuleProrationLengthtoInclude = busSettings.ProrationRule.ProrationLengthtoInclude;

            meritConfiguration.FeatureConfigurationMerit = busSettings.FeatureConfiguration.Merit;
            meritConfiguration.FeatureConfigurationAdjustment = busSettings.FeatureConfiguration.Adjustment;
            meritConfiguration.FeatureConfigurationLumpSum = busSettings.FeatureConfiguration.Lumpsum;
            meritConfiguration.FeatureConfigurationBonus = busSettings.FeatureConfiguration.Bonus;
            meritConfiguration.FeatureConfigurationPromotion = busSettings.FeatureConfiguration.Promotion;
            meritConfiguration.FeatureConfigurationRatingDisplay = busSettings.FeatureConfiguration.RatingDisplay;
            meritConfiguration.FeatureConfigurationTCC = busSettings.FeatureConfiguration.TCC;
          
            meritConfiguration.FeatureConfigurationRatingDropdown = busSettings.FeatureConfiguration.RatingDropdown;
            meritConfiguration.FeatureConfigurationMultiCurrencyDisplay = busSettings.FeatureConfiguration.MultiCurrency;
            meritConfiguration.FeatureConfigurationCurrencyCodeDisplay = busSettings.FeatureConfiguration.CurrencyCode;
            meritConfiguration.FeatureConfigurationWorkFlow = busSettings.FeatureConfiguration.WorkFlow;

            meritConfiguration.GeneralConfigurationRoundingMeritPct = busSettings.GeneralConfiguration.RoundingMeritPct;
            meritConfiguration.GeneralConfigurationRoundingMeritHourly = busSettings.GeneralConfiguration.RoundingMeritHourly;
            meritConfiguration.GeneralConfigurationRoundingMeritAnnual = busSettings.GeneralConfiguration.RoundingMeritAnnual;
            meritConfiguration.GeneralConfigurationDecimalMeritPct = busSettings.GeneralConfiguration.DecimalMeritPct;
            meritConfiguration.GeneralConfigurationDecimalMeritHourly = busSettings.GeneralConfiguration.DecimalMeritHourly;
            meritConfiguration.GeneralConfigurationDecimalMeritAnnual = busSettings.GeneralConfiguration.DecimalMeritAnnual;

            meritConfiguration.GeneralConfigurationRoundingPromotionPct = busSettings.GeneralConfiguration.RoundingPromotionPct;
            meritConfiguration.GeneralConfigurationRoundingPromotionHourly = busSettings.GeneralConfiguration.RoundingPromotionHourly;
            meritConfiguration.GeneralConfigurationRoundingPromotionAnnual = busSettings.GeneralConfiguration.RoundingPromotionAnnual;
            meritConfiguration.GeneralConfigurationDecimalPromotionPct = busSettings.GeneralConfiguration.DecimalPromotionPct;
            meritConfiguration.GeneralConfigurationDecimalPromotionHourly = busSettings.GeneralConfiguration.DecimalPromotionHourly;
            meritConfiguration.GeneralConfigurationDecimalPromotionAnnual = busSettings.GeneralConfiguration.DecimalPromotionAnnual;

            meritConfiguration.GeneralConfigurationRoundingLumpSumPct = busSettings.GeneralConfiguration.RoundingLumpSumPct;
            meritConfiguration.GeneralConfigurationRoundingLumpSumHourly = busSettings.GeneralConfiguration.RoundingLumpSumHourly;
            meritConfiguration.GeneralConfigurationRoundingLumpSumAnnual = busSettings.GeneralConfiguration.RoundingLumpSumAnnual;
            meritConfiguration.GeneralConfigurationDecimalLumpSumPct = busSettings.GeneralConfiguration.DecimalLumpSumPct;
            meritConfiguration.GeneralConfigurationDecimalLumpSumHourly = busSettings.GeneralConfiguration.DecimalLumpSumHourly;
            meritConfiguration.GeneralConfigurationDecimalLumpSumAnnual = busSettings.GeneralConfiguration.DecimalLumpSumAnnual;

            meritConfiguration.GeneralConfigurationRoundingAdjustmentPct = busSettings.GeneralConfiguration.RoundingAdjustmentPct;
            meritConfiguration.GeneralConfigurationRoundingAdjustmentHourly = busSettings.GeneralConfiguration.RoundingAdjustmentHourly;
            meritConfiguration.GeneralConfigurationRoundingAdjustmentAnnual = busSettings.GeneralConfiguration.RoundingAdjustmentAnnual;
            meritConfiguration.GeneralConfigurationDecimalAdjustmentPct = busSettings.GeneralConfiguration.DecimalAdjustmentPct;
            meritConfiguration.GeneralConfigurationDecimalAdjustmentHourly = busSettings.GeneralConfiguration.DecimalAdjustmentHourly;
            meritConfiguration.GeneralConfigurationDecimalAdjustmentAnnual = busSettings.GeneralConfiguration.DecimalAdjustmentAnnual;

            meritConfiguration.GeneralConfigurationRoundingCompaRatioPct = busSettings.GeneralConfiguration.RoundingCompaRatioPct;
            meritConfiguration.GeneralConfigurationRoundingCompaRatioHourly = busSettings.GeneralConfiguration.RoundingCompaRatioHourly;
            meritConfiguration.GeneralConfigurationRoundingCompaRatioAnnual = busSettings.GeneralConfiguration.RoundingCompaRatioAnnual;
            meritConfiguration.GeneralConfigurationDecimalCompaRatioPct = busSettings.GeneralConfiguration.DecimalCompaRatioPct;
            meritConfiguration.GeneralConfigurationDecimalCompaRatioHourly = busSettings.GeneralConfiguration.DecimalCompaRatioHourly;
            meritConfiguration.GeneralConfigurationDecimalCompaRatioAnnual = busSettings.GeneralConfiguration.DecimalCompaRatioAnnual;

            meritConfiguration.GeneralConfigurationRoundingBonusPct = busSettings.GeneralConfiguration.RoundingBonusPct;
            meritConfiguration.GeneralConfigurationRoundingBonusHourly = busSettings.GeneralConfiguration.RoundingBonusHourly;
            meritConfiguration.GeneralConfigurationRoundingBonusAnnual = busSettings.GeneralConfiguration.RoundingBonusAnnual;
            meritConfiguration.GeneralConfigurationRoundingNewSalaryHourly = busSettings.GeneralConfiguration.RoundNewSalaryHourly.ToString();
            meritConfiguration.GeneralConfigurationRoundingNewSalaryAnnual = busSettings.GeneralConfiguration.RoundNewSalaryAnnual.ToString();
           meritConfiguration.GeneralConfigurationDecimalNewSalaryHourly = busSettings.GeneralConfiguration.DecimalNewSalaryHourly;
            meritConfiguration.GeneralConfigurationDecimalNewSalaryAnnual = busSettings.GeneralConfiguration.DecimalNewSalaryAnnual;


            meritConfiguration.BudgetCurrencyFormat = busSettings.CurrencyConfiguration.CurrencyFormat;
            meritConfiguration.DateFormat = busSettings.DateConfiguration.DateFormat;
            // Current Salary Rule
            meritConfiguration.GeneralConfigurationRoundingCurrentSalaryHourly = busSettings.GeneralConfiguration.RoundCurrentSalaryHourly.ToString();
            meritConfiguration.GeneralConfigurationRoundingCurrentSalaryAnnual = busSettings.GeneralConfiguration.RoundCurrentSalaryAnnual.ToString();
            meritConfiguration.GeneralConfigurationDecimalCurrentSalaryHourly = busSettings.GeneralConfiguration.DecimalCurrentSalaryHourly;
            meritConfiguration.GeneralConfigurationDecimalCurrentSalaryAnnual = busSettings.GeneralConfiguration.DecimalCurrentSalaryAnnual;
            //////////////////////
            return meritConfiguration;
        }

        // Author        :  Raja Ganapathy		
        // Creation Date :  20-06-2016
        // Reviewed By   :  Hari		
        // Reviewed Date :  13-10-2016
        /// <summary>
        /// Returns true or false based on value
        /// </summary>
        /// <param name="value">(Yes/No)</param>
        /// <returns>bool</returns>
        private bool setTrueorFalse(string value)
        {
            if (value != null && value.ToLower().Trim() == "yes")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // Author        :  Raja Ganapathy		
        // Creation Date :  20-06-2016
        // Reviewed By   :  Hari		
        // Reviewed Date :  13-10-2016
        /// <summary>
        /// To convert the decimal value
        /// </summary>
        /// <param name="value">The value</param>
        /// <returns>Returns the converted value</returns>
        private decimal setDecimalValue(string value)
        {
            value = value.ToLower().Trim();
            decimal num;
            if (Decimal.TryParse(value, out num))
            {
                return Convert.ToDecimal(value);
            }
            else
            {
                return 0;
            }
        }
       

        // Author        :  Raja Ganapathy		
        // Creation Date :  20-06-2016
        // Reviewed By   :  Hari		
        // Reviewed Date :  13-10-2016
        /// <summary>
        /// To get sorted employees list
        /// </summary>
        /// <returns>List<CompensationEmployeeFilter></returns>
        public async Task<IEnumerable<DropDownListItems>> GetEmpSort()
        {
            return await m_compensationRepository.GetFilterConfiguration();

        }

        public async Task DeleteComments(int commentKey)
        {
           await m_compensationRepository.deleteComments(commentKey);
        }

        // Author        :  Raja Ganapathy		
        // Creation Date :  20-06-2016
        // Reviewed By   :  Hari		
        // Reviewed Date :  13-10-2016
        /// <summary>
        /// To update the compensation reportees data
        /// </summary>
        /// <param name="compensationReportees">Denotes the updated records</param>
        /// <param name="jobYear">Denotes the merit cycle year</param>
        /// <param name="userNum">Denotes the userNum</param>
        /// <param name="compensationTypeConfiguration">Denotes the CompensationTypeConfiguration</param>
        public void UpdateCompReportees(List<MeritGridModel> compensationReportees, int jopbYear, int userNum)
        {
            m_compensationRepository.UpdateCompReportees(compensationReportees, jopbYear, userNum);
        }

        // Author        :  Raja Ganapathy		
        // Creation Date :  20-06-2016
        // Reviewed By   :  Hari		
        // Reviewed Date :  13-10-2016
        /// <summary>
        /// To get the merit reportee grid data
        /// </summary>
        /// <param name="managerNum">Denotes selected manager num in manager tree</param>
        /// <param name="compExclusion">Denotes compExclusion</param>
        /// <param name="loggedInEmployeeNum">Denotes the logged in employeenum</param>
        /// <param name="compMenuType">Denotes the menu type assign group or my team</param>
        /// <param name="isRollup">Denotes the rollup is selected or not</param>
        /// <returns>Returns compensation reportees data</returns>
        public async Task<IEnumerable<MeritGridModel>> GetCompReportees(int managerNum, int loggedInEmployeeNum, int loggedInUserNum, MenuType compMenuType, bool isRollup, int loggedSelectedUserNum)
        {
            return await m_compensationRepository.GetCompReportees(managerNum, loggedInEmployeeNum, loggedInUserNum, compMenuType, isRollup, loggedSelectedUserNum);            
        }
        public async Task<IEnumerable<SubmitReporteesModel>> GetCompSubmitReportees(int managerNum, int loggedInEmployeeNum, int loggedInUserNum, MenuType compMenuType, bool isRollup, int loggedSelectedUserNum)
        {
            return await m_compensationRepository.GetCompSubmitReportees(managerNum, loggedInEmployeeNum, loggedInUserNum, compMenuType, isRollup, loggedSelectedUserNum);
        }
        public async Task<IEnumerable<SubmitReporteesModel>> GetCompApprovalReportees(int managerNum, int loggedInEmployeeNum, int loggedInUserNum, MenuType compMenuType, bool isRollup, int loggedSelectedUserNum)
        {
            return await m_compensationRepository.GetCompApprovalReportees(managerNum, loggedInEmployeeNum, loggedInUserNum, compMenuType, isRollup, loggedSelectedUserNum);
        }
        public async Task<IEnumerable<SubmitReporteesModel>> GetCompReopenReportees(int managerNum, int loggedInEmployeeNum, int loggedInUserNum, MenuType compMenuType, bool isRollup, int loggedSelectedUserNum)
        {
            return await m_compensationRepository.GetCompReopenReportees(managerNum, loggedInEmployeeNum, loggedInUserNum, compMenuType, isRollup, loggedSelectedUserNum);
        }

        public async Task<IEnumerable<ApprovalEmployeeSearchData>> GetCompApprovalReporteesSearch(int managerNum, int loggedInUserNum, MenuType compMenuType, bool isRollup, string approvalType)
        {
            return await m_compensationRepository.GetCompApprovalReporteesSearch(managerNum, loggedInUserNum, compMenuType, isRollup, approvalType);
        }

        // Author        :  Raja Ganapathy		
        // Creation Date :  20-06-2016
        // Reviewed By   :  Hari		
        // Reviewed Date :  13-10-2016
        /// <summary>
        /// To get the currency type for budget grid
        /// </summary>
        /// <returns>Returns the Currency exchange</returns>
        public async Task<IEnumerable<ExchangeCurrencies>> GetCurrencies()
        {
            return await m_compensationRepository.GetCurrencies();
            //    .Select(x => new ExchangeCurrencies
            //     {
            //         CurrencyCode = x.Currency.CurrencyCode,
            //         CurrencyNum = x.CurrencyCodeNum,
            //         CultureCode = x.CultureCode == null ? "en-US" : x.CultureCode,
            //         ExchangeRate = x.ExchangeRates.Select(y => y.MeritExchangeRate).FirstOrDefault() == null ? 1 : x.ExchangeRates.Select(y => y.MeritExchangeRate).FirstOrDefault()
            //     }).OrderBy(x => x.CurrencyCode);

            //return null;
        }

        // Author        :  Revathy		
        // Creation Date :  19-01-2015
        // Reviewed By   :  Hari		
        // Reviewed Date :  13-10-2016
        /// <summary>
        /// To get the employee'sbasic information
        /// </summary>
        /// <param name="employeeNum">Gets the details of this employee</param>
        /// <returns>List<EmployeeInfoModel></returns>
        public async Task<IEnumerable<EmployeeInfoDetails>> GetEmployeeInfo(int employeeNum, int loggedInUserNum)
        {
            var employeeInfo =await m_compensationRepository.GetEmployeeInfo(employeeNum, loggedInUserNum);
            return employeeInfo;
        }

        // Author        :  Bala Murugan M		
        // Creation Date :  07-Aug-2017
        // Reviewed By   :  
        // Reviewed Date :  
        /// <summary>
        /// To get the employee's basic information
        /// </summary>
        /// <param name="employeeNum">Gets the details of this employee</param>
        /// <returns>List<EmployeeInfoModel></returns>
        public Task<IEnumerable<EmployeeInfoBasicDetails>> GetEmployeeBasicInfo(int employeeNum)
        {
            var employeeBasicInfo = m_compensationRepository.GetEmployeeBasicInfo(employeeNum);
            return employeeBasicInfo;
        }

        // Author        :  Raja Ganapathy		
        // Creation Date :  20-06-2016
        // Reviewed By   :  Hari		
        // Reviewed Date :  13-10-2016
        /// <summary>
        /// To get list of comment related to promotion in that employee
        /// </summary>
        /// <param name="empJobNum">Denotes the employee jobnum</param>
        /// <returns>Returns list of comments</returns>
        public IQueryable<CommentModel> GetPromotionComment(int empJobNum)
        {

            BusinessSettingModel busSettings = m_compensationRepository.GetCompensationTypeConfiguration();
            string DateFormat = busSettings.DateConfiguration.DateFormat;
            var compensationTypeNum = m_compensationRepository.getCompensationTypeNum("Promotion");
            return m_compensationRepository.GetCompComments(empJobNum).Where(x => x.CompensationTypeNum == compensationTypeNum).Select(x => new CommentModel
            {
                EmpCommentNum = x.EmployeeCompCommentsNum,
                EmployeeName = x.AppUser.UserName,
                CommentUpdatedDate = x.UpdatedDate,
                Comment = x.Comments,
                UpdatedByUserNumOrEmpNum = x.UpdatedBy, 
                CreatedByUserNumOrEmpNum=x.CreatedBy,
                CommentCreatedDate=x.CreatedDate,
                FirstName = x.AppUser.FirstName,
                LastName = x.AppUser.LastName,
                DateFormat = DateFormat
            }).OrderByDescending(x => x.CommentUpdatedDate);
        }


        // Author        :  Raja Ganapathy		
        // Creation Date :  22-06-2016   
        // Reviewed By   :  Hari		
        // Reviewed Date :  13-10-2016 
        /// <summary>
        /// Get manager tree data
        /// </summary>
        /// <param name="managerNum">Selected group num</param>
        /// <param name="loggedInEmployeeNum">Denotes logged in employee num</param>
        /// <param name="jobYear">Denotes merit cycle year</param>
        /// <param name="isRollup">Denotes rollup</param>
        /// <param name="userNum">Denotes logged in employee user num</param>
        /// <param name="MenuType">Denotes the menu type</param>
        /// <param name="exclusionType">Denotes comp exclusion type</param>
        /// <returns>Returns manager tree data</returns>
        public async Task<IEnumerable<ManagerTree>> GetCompManagerTree(int managerNum, int loggedInEmployeeNum, int loggedInUserNum, int userNum, ViewPageType pageType)
        {
            return await m_compensationRepository.GetCompManagerTree(managerNum, loggedInEmployeeNum, loggedInUserNum, userNum, pageType);
        }
        
        // Author        :  Raja Ganapathy		
        // Creation Date :  22-06-2016   
        // Reviewed By   :  Hari		
        // Reviewed Date :  13-10-2016 
        /// <summary>
        ///  To update the employee comment
        /// </summary>
        /// <param name="empJobNum">Denotes the employeejob num</param>
        /// <param name="userNum">Denotes the user num</param>
        /// <param name="employeeCompCommentNum">Denotes the employee comp comment num</param>
        /// <param name="comments">Denotes the comments</param>
        /// <param name="grade">Denotes the grade</param>
        public void UpdateEmployeeComment(int empJobNum, int userNum, int employeeCompCommentNum, string comments, string grade)
        {
            m_compensationRepository.UpdateEmployeeComment(empJobNum, userNum, employeeCompCommentNum, comments, grade);
        }

        // Author        :  Raja Ganapathy		
        // Creation Date :  22-06-2016
        // Reviewed By   :  Hari		
        // Reviewed Date :  13-10-2016
        /// <summary>
        /// To get Budget details
        /// </summary>        
        /// <param name="employeeNum">Denotes selected employee num</param>
        /// <param name="compensationTypeConfiguration">Denotes compensation type configuration</param>
        /// <param name="compMenuType">Denotes the comp menu type</param>
        public async Task<BudgetModel> GetBudgetData(int loggedInEmpNum, int employeeNum, int loggedInUserNum, MenuType compMenuType, string currencyCulture, int currencyCodeNum, bool isRollup, bool isSelectedRollup)
        {
            return await m_compensationRepository.GetBudgetData(loggedInEmpNum, employeeNum, loggedInUserNum, compMenuType, currencyCulture, currencyCodeNum, isRollup, isSelectedRollup);
        }

       
        // Author        :  Raja Ganapathy		
        // Creation Date :  22-08-2016
        // Reviewed By   :  Hari		
        // Reviewed Date :  13-10-2016
        /// <summary>
        /// To update the employee comp approval status
        /// </summary>
        /// <param name="approvalStatus">Approval status</param>
        /// <param name="selectedManagerNum">Selected manager num</param>
        /// <returns>Returns the json result</returns>
        public async Task<int> UpdateApprovalStatus(List<SubmitReporteesModel> selectedRows, int loggedInEmployeeNum, int selectedManagerNum, MenuType MenuType, bool isRollup, ApprovalStatus approvalStatus, AppConfigModel appConfig, int userNum, string Comment)
        {
            return await m_compensationRepository.UpdateApprovalStatus(selectedRows,loggedInEmployeeNum, selectedManagerNum, MenuType, isRollup, approvalStatus,appConfig, userNum, Comment);
        }

        // Author        :  Raja Ganapathy		
        // Creation Date :  22-08-2016
        // Reviewed By   :  Hari		
        // Reviewed Date :  13-10-2016
        /// <summary>
        /// To get the comp completed count
        /// </summary>
        /// <param name="selectedManagerNum">Denotes the selected manager num</param>
        /// <returns>Returns the comp completed count</returns>
        public async Task<IEnumerable<int>> GetCompCompletedCount(int selectedManagerNum, int year, int loggedInUserNum)
        {
            return await m_compensationRepository.GetCompCompletedCount(selectedManagerNum, year,loggedInUserNum);
        }
        
        // Author        :  Raja Ganapathy		
        // Creation Date :  22-06-2016
        // Reviewed By   :  Hari		
        // Reviewed Date :  13-10-2016
        /// <summary>
        /// Returns the employeename
        /// </summary>
        /// <param name="selectedManagerNum">Denotes the selected manager num</param>
        
        public async Task<string> GetManagerName(int managerNum)
        {
            return await m_compensationRepository.GetManagerName(managerNum);
        }

        public bool IsInDirects(int managerNum)
        {
            return m_compensationRepository.IsInDirects(managerNum);
        }

        // Author        :  Raja Ganapathy		
        // Creation Date :  22-08-2016
        // Reviewed By   :  Hari		
        // Reviewed Date :  13-10-2016
        /// <summary>
        /// To revert the promotion
        /// </summary>
        /// <param name="empJobNum">Denotes the employee job num</param>
        public async Task RevertPromotion(int empJobNum, decimal newSalaryLocal, decimal newHrlyRate, decimal newCompRatio, decimal TCC)
        {
            await m_compensationRepository.RevertPromotion(empJobNum,newSalaryLocal,newHrlyRate,newCompRatio,TCC);
        }

        public async Task<int> GetApprovalStatus(int selectedManagerNum, int loggedInEmployeeNum)
        {
            return await m_compensationRepository.GetApprovalStatus(selectedManagerNum, loggedInEmployeeNum);
        }

        public IQueryable<CommentModel> GetComments(int empJobNum, CommentType commentType, int userNum)
        {
            IQueryable<CommentModel> comments;
            CompensationTypeConfiguration ruleConfiguration = new CompensationTypeConfiguration();
            BusinessSettingModel busSettings = m_compensationRepository.GetCompensationTypeConfiguration();
            ruleConfiguration.DateFormat = busSettings.DateConfiguration.DateFormat;

            if (commentType == CommentType.CompensationMeritMandit)
            {
                int meritTypeNum = m_compensationRepository.getCompensationTypeNum("Merit");
                return m_compensationRepository.GetCompComments(empJobNum)
                    .Where(x => x.CompensationTypeNum == meritTypeNum)
                    .Select(x => new CommentModel
                    {
                        EmpCommentNum = x.EmployeeCompCommentsNum,
                        EmployeeName = x.AppUser.UserName,
                        FirstName = x.AppUser.FirstName,
                        LastName = x.AppUser.LastName,
                        CommentUpdatedDate = x.UpdatedDate,
                        CommentCreatedDate = x.CreatedDate,
                        Comment = x.Comments,
                        UpdatedByUserNumOrEmpNum = x.UpdatedBy,
                        CreatedByUserNumOrEmpNum = x.CreatedBy,
                        CompensationTypeNum = x.CompensationTypeNum,
                        DateFormat = ruleConfiguration.DateFormat
                    }).OrderByDescending(x => x.CommentUpdatedDate);
            }
            else
            {
                comments = null;
            }
            return comments;
        }
        public CompensationTypeConfiguration GetRuleConfiguration()
        {
            CompensationTypeConfiguration ruleConfiguration = new CompensationTypeConfiguration();
            BusinessSettingModel busSettings = m_compensationRepository.GetCompensationTypeConfiguration();
            ruleConfiguration.FeatureConfigurationMerit = busSettings.FeatureConfiguration.Merit;
            ruleConfiguration.FeatureConfigurationAdjustment = busSettings.FeatureConfiguration.Adjustment;
            ruleConfiguration.FeatureConfigurationPromotion = busSettings.FeatureConfiguration.Promotion;
            ruleConfiguration.FeatureConfigurationBonus = busSettings.FeatureConfiguration.Bonus;
            return ruleConfiguration;
        }

        public string GetworkflowStatus()
        {
            return m_compensationRepository.GetworkflowStatus();
        }

        public void PutUpdateComments(CommentInputModel comment, CommentType type, bool isEditItem)
        {            
            int meritTypeNum = m_compensationRepository.getCompensationTypeNum("Merit");
            var localTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.Local);

            if (type == CommentType.CompensationMeritMandit)
            {
                if (isEditItem)
                {
                    EmployeeCompComment dataComment = m_compensationRepository.GetCompComments(comment.CommentKey).Where(x => x.EmployeeCompCommentsNum == comment.EmpCommentNum).First();                    
                    dataComment.CompensationTypeNum = meritTypeNum;
                    dataComment.EmpJobNum = comment.CommentKey;
                    dataComment.Comments = comment.Comment;
                    dataComment.UpdatedBy = comment.CommentedEmployeeNum;
                    dataComment.UpdatedDate = DateTime.Now;
                    m_compensationRepository.PutUpdateCompensationComments(dataComment);
                }
                else
                {
                    EmployeeCompComment dataComment = new EmployeeCompComment();                    
                    dataComment.CreatedBy = comment.CommentedEmployeeNum;
                    dataComment.CreatedDate = DateTime.Now;
                    dataComment.EmpJobNum = comment.CommentKey;
                    dataComment.Comments = comment.Comment;
                    dataComment.CompensationTypeNum = meritTypeNum;
                    m_compensationRepository.PutUpdateCompensationComments(dataComment);
                }

            }
        }
    }
}

