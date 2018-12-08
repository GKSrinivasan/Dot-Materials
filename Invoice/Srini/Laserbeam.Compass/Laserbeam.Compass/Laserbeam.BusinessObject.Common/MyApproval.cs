using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laserbeam.BusinessObject.Common
{
   public class MyApproval
    {
        public string EmployeeName { get; set; }
        public int EmployeeNum { get; set; }
        public bool IsMyTeam { get; set; }
        public string EmployeeID { get; set; }        
        public string Abbreviation { get; set; }
        public string ManagerName { get; set; }
        public string Module { get; set; }
        public int EmployeeCount { get; set; }
    }
}
