using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laserbeam.BusinessObject.Common.MeritBusinussObjects
{
    public class BudgetData
    {
        public decimal? Budget { get; set; }
        public decimal? Spent { get; set; }
        public decimal? BonusBudget { get; set; }
        public decimal? BonusSpent { get; set; }
        public decimal? PromotionSpent { get; set; }
        public decimal? AdjustmentSpent { get; set; }
        public decimal? LumpSumSpent { get; set; }
        public int CompensationTypeNum { get; set; }
        public int EmpJobNum { get; set; }  
    }
}
