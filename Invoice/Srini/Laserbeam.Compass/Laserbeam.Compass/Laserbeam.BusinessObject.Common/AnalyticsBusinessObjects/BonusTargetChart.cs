using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laserbeam.BusinessObject.Common.AnalyticsBusinessObjects
{
   public class BonusTargetChart
    {
        public string TargetAmt { get; set; }
        public string PayoutAmt { get; set; }
        public decimal TargetRatio { get; set; }
        public int TargetAmtValue { get; set; }
    }
}
