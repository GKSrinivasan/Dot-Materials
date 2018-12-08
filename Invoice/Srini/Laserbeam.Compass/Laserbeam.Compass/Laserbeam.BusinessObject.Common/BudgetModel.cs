
using Laserbeam.Resource.HR.BudgetResources;
using System.ComponentModel.DataAnnotations;
namespace Laserbeam.BusinessObject.Common
{
    public partial class BudgetModel
    {   
        public decimal? AdjustmentSpentUSD { get; set; }                        
        public decimal? MeritBudgetUSD { get; set; }        
        public decimal? MeritSpentUSD { get; set; }        
        public decimal? PromotionSpentUSD { get; set; }        
        public decimal? LumpSumSpentUSD { get; set; }
        public decimal? BaseSalaryUSD { get; set; }
        public decimal? BaseSalary { get; set; }
        public decimal? NewSalaryUSD { get; set; }
        public decimal? NewSalary { get; set; }
        public decimal? BalanceUSD
        {
            get
            {
                return MeritBudgetUSD - (MeritSpentUSD + PromotionSpentUSD + LumpSumSpentUSD);
            }
        }
        

        [Display(Name = "AdjustmentSpent", ResourceType = typeof(BudgetResource))]
        public decimal? AdjustmentSpent { get; set; }
        
        [Display(Name = "Budget", ResourceType = typeof(BudgetResource))]
        public decimal? MeritBudget { get; set; }
        [Display(Name = "Spent", ResourceType = typeof(BudgetResource))]
        public decimal? MeritSpent { get; set; }
        [Display(Name = "PromotionSpent", ResourceType = typeof(BudgetResource))]
        public decimal? PromotionSpent { get; set; }
        [Display(Name = "LumpSumSpent", ResourceType = typeof(BudgetResource))]
        public decimal? LumpSumSpent { get; set; }
        public decimal? TotalSpent
        {
            get
            {
                return MeritSpent + PromotionSpent + LumpSumSpent;
            }
        }

        public decimal? Balance
        {
            get
            {
               
                return MeritBudget - (MeritSpent + PromotionSpent + LumpSumSpent);
            }
        }  
        
        public int? BeforeIncreaseMin { get; set; }
        public int? BeforeIncreaseMid { get; set; }
        public int? BeforeIncreaseMax { get; set; }
        public int? AfterIncreaseMin { get; set; }
        public int? AfterIncreaseMid { get; set; }
        public int? AfterIncreaseMax { get; set; }
        public decimal? TotalNewSalary { get; set; }
        public decimal? TotalCurrentSalary { get; set; }
        public decimal? BudgetPct { get; set; }
        public bool isInDirects { get; set; }
    }
}
