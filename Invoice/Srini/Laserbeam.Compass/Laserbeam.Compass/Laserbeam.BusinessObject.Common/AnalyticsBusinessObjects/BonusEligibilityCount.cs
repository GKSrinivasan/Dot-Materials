using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laserbeam.BusinessObject.Common.AnalyticsBusinessObjects
{
    public class BonusEligibilityCount
    {
        public int TotalCount { get; set; }
        public int EligibleCount { get; set; }
        public int IncrBonusCount { get; set; }
        public string ReceivedPct { get; set; }
    }
}
