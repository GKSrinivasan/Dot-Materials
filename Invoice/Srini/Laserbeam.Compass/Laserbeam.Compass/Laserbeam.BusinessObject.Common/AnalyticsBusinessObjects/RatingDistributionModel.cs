using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laserbeam.BusinessObject.Common.AnalyticsBusinessObjects
{
    public class RatingDistributionModel
    {
        public List<PayRangeDistribution> PayRangeDistribution { get; set; }
        public List<PayRangeDistribution> PayRangeDistributionChart { get; set; }
    }
}
