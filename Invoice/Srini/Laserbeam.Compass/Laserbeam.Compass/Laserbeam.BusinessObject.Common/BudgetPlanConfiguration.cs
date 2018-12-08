
// Component Name :   BudgetPlanConfiguration 
// Description    :   BudgetPlanConfiguration for BudgetPlan view 	
// Author         :   
// Creation Date  :   
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laserbeam.BusinessObject.Common
{
    public class BudgetPlanConfiguration
    {
        public string RoundingBudgetPercentage { get; set; }
        public string RoundingBudgetDoller { get; set; }
        public string DecimalBudgetPercentage { get; set; }
        public string DecimalBudgetDoller { get; set; }
        public bool BudgetProration { get; set; }
        public string BudgetCurrencyFormat { get; set; }
        public bool isEnableMerit { get; set; }
        public bool isEnablePromotion { get; set; }
        public bool isEnableAdjustment { get; set; }
        public bool isEnableLumpSum { get; set; }
    }
}
