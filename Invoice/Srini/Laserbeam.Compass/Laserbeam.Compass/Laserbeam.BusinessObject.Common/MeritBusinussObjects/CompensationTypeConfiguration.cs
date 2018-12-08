namespace Laserbeam.BusinessObject.Common
{

    public class CompensationTypeConfiguration
    {
        public bool MeritOverrideNoJustification { get; set; }
        public bool MeritOverrideHardStop { get; set; }
        public bool MeritOverrideSoftStop { get; set; }
        public bool MeritOverrideIncreaseWithinGuideline { get; set; }
        public bool MeritOverrideMandatoryJustification { get; set; }
        public bool EitherMeritOrlumpsum { get; set; }

        public string LumpSumRuleLumpSumType { get; set; }
        public decimal LumpSumRuleRangeMaxPct { get; set; }
        public decimal LumpSumRuleRangeMaxAmt { get; set; }
        public bool MeritValuesReCalculate { get; set; }
        public bool AutoCalculateLumpSum { get; set; }
        

        public bool ProrationRuleProrate { get; set; }
        public bool ProrationApplyMeritDiscretion { get; set; }
        public bool ProrationApplyBudgetCalculations { get; set; }
        public bool ProrationApplyAdjustmentCalculations { get; set; }
        public string ProrationRuleProrateIncreaseStartDate { get; set; }
        public string ProrationRuleProrateIncreaseEndDate { get; set; }
        public string ProrationRuleProrationType { get; set; }
        public int ProrationRuleProrationLength { get; set; }
        public int ProrationRuleProrationLengthtoInclude { get; set; }

        public bool FeatureConfigurationMerit { get; set; }
        public bool FeatureConfigurationLTIP { get; set; }
        public bool FeatureConfigurationAdjustment { get; set; }
        public bool FeatureConfigurationLumpSum { get; set; }
        public bool FeatureConfigurationBonus { get; set; }
        public bool FeatureConfigurationPromotion { get; set; }
        public bool FeatureConfigurationRatingDisplay { get; set; }
        public bool FeatureConfigurationTCC { get; set; }
        public bool FeatureConfigurationReadOnly { get; set; }
        public bool FeatureConfigurationRatingDropdown { get; set; }
        public bool FeatureConfigurationMultiCurrencyDisplay { get; set; }
        public bool FeatureConfigurationCurrencyCodeDisplay { get; set; }
        public bool FeatureConfigurationWorkFlow { get; set; }

        public string GeneralConfigurationRoundingMeritPct { get; set; }
        public string GeneralConfigurationRoundingMeritHourly { get; set; }
        public string GeneralConfigurationRoundingMeritAnnual { get; set; }
        public decimal GeneralConfigurationDecimalMeritPct { get; set; }
        public decimal GeneralConfigurationDecimalMeritHourly { get; set; }
        public decimal GeneralConfigurationDecimalMeritAnnual { get; set; }

        public string GeneralConfigurationRoundingPromotionPct { get; set; }
        public string GeneralConfigurationRoundingPromotionHourly { get; set; }
        public string GeneralConfigurationRoundingPromotionAnnual { get; set; }
        public decimal GeneralConfigurationDecimalPromotionPct { get; set; }
        public decimal GeneralConfigurationDecimalPromotionHourly { get; set; }
        public decimal GeneralConfigurationDecimalPromotionAnnual { get; set; }

        public string GeneralConfigurationRoundingLumpSumPct { get; set; }
        public string GeneralConfigurationRoundingLumpSumHourly { get; set; }
        public string GeneralConfigurationRoundingLumpSumAnnual { get; set; }
        public decimal GeneralConfigurationDecimalLumpSumPct { get; set; }
        public decimal GeneralConfigurationDecimalLumpSumHourly { get; set; }
        public decimal GeneralConfigurationDecimalLumpSumAnnual { get; set; }

        public string GeneralConfigurationRoundingAdjustmentPct { get; set; }
        public string GeneralConfigurationRoundingAdjustmentHourly { get; set; }
        public string GeneralConfigurationRoundingAdjustmentAnnual { get; set; }
        public decimal GeneralConfigurationDecimalAdjustmentPct { get; set; }
        public decimal GeneralConfigurationDecimalAdjustmentHourly { get; set; }
        public decimal GeneralConfigurationDecimalAdjustmentAnnual { get; set; }

        public string GeneralConfigurationRoundingCompaRatioPct { get; set; }
        public string GeneralConfigurationRoundingCompaRatioHourly { get; set; }
        public string GeneralConfigurationRoundingCompaRatioAnnual { get; set; }
        public decimal GeneralConfigurationDecimalCompaRatioPct { get; set; }
        public decimal GeneralConfigurationDecimalCompaRatioHourly { get; set; }
        public decimal GeneralConfigurationDecimalCompaRatioAnnual { get; set; }

        public string GeneralConfigurationRoundingBonusPct { get; set; }
        public string GeneralConfigurationRoundingBonusHourly { get; set; }
        public string GeneralConfigurationRoundingBonusAnnual { get; set; }
        public decimal GeneralConfigurationDecimalBonusPct { get; set; }
        public decimal GeneralConfigurationDecimalBonusHourly { get; set; }
        public decimal GeneralConfigurationDecimalBonusAnnual { get; set; }

        public decimal GeneralConfigurationDecimalNewSalaryHourly { get; set; }
        public decimal GeneralConfigurationDecimalNewSalaryAnnual { get; set; }
        public string GeneralConfigurationRoundingNewSalaryHourly { get; set; }
        public string GeneralConfigurationRoundingNewSalaryAnnual { get; set; }
        public decimal GeneralConfigurationDecimalFinalBonusAnnual { get; set; }
        public string GeneralConfigurationRoundingFinalBonusAnnual { get; set; }
        public decimal HourlyRate { get; set; }
        public string DateFormat { get; set; }
        public string BudgetCurrencyFormat { get; set; }
        public int Year { get; set; }
        public int UserNum { get; set; }
        public int LoggedInEmployeeNum { get; set; }
        public bool CompExclusion { get; set; }
        public bool BonusOverrideHardStop { get; set; }
        public bool BonusOverrideSoftStop { get; set; }
        public bool BonusIncreaseWithinGuideline { get; set; }
        public bool BonusMandatoryJustification { get; set; }
        // Current Salary
        public decimal GeneralConfigurationDecimalCurrentSalaryHourly { get; set; }
        public decimal GeneralConfigurationDecimalCurrentSalaryAnnual { get; set; }
        public string GeneralConfigurationRoundingCurrentSalaryHourly { get; set; }
        public string GeneralConfigurationRoundingCurrentSalaryAnnual { get; set; }
        /////////////////
    }
}
