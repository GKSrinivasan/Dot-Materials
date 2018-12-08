using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laserbeam.BusinessObject.Common
{
    public class BudgetProrationUpdateModel
    {
        public bool BudgetProrationValue { get; set; }
       
        public string ProrateStartDate { get; set; }

        public string ProrateEndDate { get; set; }

        public string ProrationType { get; set; }
      
        public string ProrationDuration { get; set; }
      
        public string ProrationDatesPerMonth { get; set; }

        public bool IsMerit { get; set; }
    }
}
