using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laserbeam.BusinessObject.Common.AnalyticsBusinessObjects
{
    public class CompRevenueChart
    {
        public string TotalRevenue { get; set; }
        public decimal RevenuePct { get; set; }
        public string MeritAmt { get; set; }
        public string MeritPct { get; set; }
        public string LumpSumAmt { get; set; }
        public string LumpSumPct { get; set; }
        public string PromotionAmt { get; set; }
        public string PromotionAmtPct { get; set; }
        public string AdjustmentAmt { get; set; }
        public string AdjustmentPct { get; set; }
    }
}
