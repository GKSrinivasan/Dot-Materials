using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laserbeam.BusinessObject.Common
{
    public class RuleConfiguration
    {
        public bool ProrationRuleProrate { get; set; }
        public bool ProrationApplyMeritDiscretion { get; set; }
        public bool ProrationApplyBudgetCalculations { get; set; }
        public bool ProrationApplyAdjustmentCalculations { get; set; }
        public string ProrationRuleProrateIncreaseStartDate { get; set; }
        public string ProrationRuleProrateIncreaseEndDate { get; set; }
        public string ProrationRuleProrationType { get; set; }
        public int ProrationRuleProrationLength { get; set; }
        public int ProrationRuleProrationLengthtoInclude { get; set; }
        public bool IsMultiCurrencyEnable { get; set; }
        public string LumpSumRuleLumpSumType { get; set; }
        public decimal LumpSumRuleRangeMaxPct { get; set; }
        public decimal LumpSumRuleRangeMaxAmt { get; set; }
        public bool MeritValuesReCalculate { get; set; }
        public bool AutoCalculateLumpSum { get; set; }
        public bool LumpSumRuleTurnOff { get; set; }
        public bool BudgetProration { get; set; }
        public string BudgetProrateIncreaseStartDate { get; set; }
        public string BudgetProrateIncreaseEndDate { get; set; }
        public string BudgetProrationType { get; set; }
        public int BudgetProrationDuration { get; set; }
        public int BudgetProrationDatesPerMonth { get; set; }
        public string MeritCalculation { get; set; }
        public bool ComparativeRatio { get; set; }
        

    }
}
