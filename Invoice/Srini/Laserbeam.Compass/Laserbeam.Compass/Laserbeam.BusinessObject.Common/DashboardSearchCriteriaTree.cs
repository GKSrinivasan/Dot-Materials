using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laserbeam.BusinessObject.Common
{
    public class DashboardSearchCriteriaTree
    {
        public string UserRoleName { get; set; }
        public int UserRoleNum { get; set; }
        public int ManagerEmpJobNum { get; set; }
        public int? ReportingManagerNum { get; set; }
        public string ManagerLineage { get; set; }
        public bool IsTreeTop { get; set; }
        public int ReporteeCount { get; set; }
        public int OrderByTree { get; set; }
        public int SelectedType { get; set; }
    }
}
