using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laserbeam.BusinessObject.Common.AnalyticsBusinessObjects
{
   public  class IncreaseDistributionEmp
    {
        public int EmpCount { get; set; }
        public string TotalIncrease { get; set; }
        public decimal UtilizationPct { get; set; }
    }
}
