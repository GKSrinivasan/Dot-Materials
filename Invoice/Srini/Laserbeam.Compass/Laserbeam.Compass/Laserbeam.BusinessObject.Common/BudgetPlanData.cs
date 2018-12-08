using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laserbeam.BusinessObject.Common
{
    public class BudgetPlanData
    {
        public int TotalEmployeesCount { get; set; }
        public decimal TotalSalaryAmount { get; set; }
        public decimal BudgetPct { get; set; }
        public decimal TotalBudgetAmount { get; set; }
        public decimal MeritSpent { get; set; }
        public decimal LumpSumSpent { get; set; }
        public decimal PromotionSpent { get; set; }
        public decimal AdjustmentSpent { get; set; }
        public string ProrationStartDate { get; set; }
        public string ProrationEndDate { get; set; }
        public decimal TotalSpentAmt { get { return MeritSpent + LumpSumSpent + PromotionSpent + AdjustmentSpent; } }
        public decimal BalanceAmt { get { return TotalBudgetAmount-(MeritSpent + LumpSumSpent + PromotionSpent + AdjustmentSpent); } }
        public string StartDate { get { return ProrationStartDate != "" ? Convert.ToDateTime(ProrationStartDate).ToShortDateString() : " "; } }
        public string EndDate { get { return ProrationEndDate != "" ? Convert.ToDateTime(ProrationEndDate).ToShortDateString() : " "; } }
        public decimal AdjustedBudgetAmount { get; set; }
        public decimal AdjustedBudgetPct { get; set; }
    }
}
