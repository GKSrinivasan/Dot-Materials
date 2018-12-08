
using Laserbeam.Resource.HR.BudgetResources;
using System.ComponentModel.DataAnnotations;
namespace Laserbeam.BusinessObject.Common
{
    public partial class BudgetModel
    {   
        public decimal? AdjustmentSpentUSD { get; set; }                        
        public decimal? Budget1USD { get; set; }        
        public decimal? SpentUSD { get; set; }        
        public decimal? PromotionSpentUSD { get; set; }        
        public decimal? LumpSumSpentUSD { get; set; }
        public decimal? BaseSalaryUSD { get; set; }        
        public decimal? BalanceUSD
        {
            get
            {
                return Budget1USD - SpentUSD - PromotionSpentUSD - LumpSumSpentUSD - AdjustmentSpentUSD;
            }
        }
        public decimal? BonusBudgetUSD { get; set; }
        public decimal? BonusSpentUSD { get; set; }
        public decimal? BonusBalanceUSD
        {
            get
            {
                return BonusBudgetUSD - BonusSpentUSD;
            }
        }

        public decimal? BonusBudget { get; set; }
        public decimal? BonusSpent { get; set; }
        public decimal? BonusBalance
        {
            get
            {
                return BonusBudget - BonusSpent;
            }
        }

        public decimal? LTIPBudgetUSD { get; set; }
        public decimal? LTIPSpentUSD { get; set; }
        public decimal? LTIPBalanceUSD
        {
            get
            {
                return LTIPBudgetUSD - LTIPSpentUSD;
            }
        }

        public decimal? LTIPBudget { get; set; }
        public decimal? LTIPSpent { get; set; }
        public decimal? LTIPBalance
        {
            get
            {
                return LTIPBudget - LTIPSpent;
            }
        }

        [Display(Name = "AdjustmentSpent", ResourceType = typeof(BudgetResource))]
        public decimal? AdjustmentSpent { get; set; }
        
        [Display(Name = "Budget", ResourceType = typeof(BudgetResource))]
        public decimal? Budget1 { get; set; }
        [Display(Name = "Spent", ResourceType = typeof(BudgetResource))]
        public decimal? Spent { get; set; }
        [Display(Name = "PromotionSpent", ResourceType = typeof(BudgetResource))]
        public decimal? PromotionSpent { get; set; }
        [Display(Name = "LumpSumSpent", ResourceType = typeof(BudgetResource))]
        public decimal? LumpSumSpent { get; set; }
        
        public decimal? Balance
        {
            get
            {
                return Budget1 - Spent - PromotionSpent - LumpSumSpent - AdjustmentSpent;
            }
        }        


    }
}
