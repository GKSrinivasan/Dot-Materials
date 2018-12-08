using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laserbeam.BusinessObject.Common.MeritBusinussObjects
{
    public class ApprovalFlowByManager
    {
        public int EmployeeCompApprovalLevelNum { get; set; }
        public string ManagerName { get; set; }
        public int ManagerNum { get; set; }
        public int EmpJobNum { get; set; }
        public int ApprovalManager { get; set; }        
        public int ApprovalLevel { get; set; }
        public Nullable<int> CurrentApprovedManager { get; set; }
        public Nullable<int> CurrentApprovedManagerLevel { get; set; }
        public Nullable<int> CurrentApprovedStatus { get; set; }
        public string CurrentApprovedManagerName { get; set; }
        public string CurrentApprovedManagerEmailAddress { get; set; }
        public Nullable<int> PreviousLevelManager { get; set; }
        public Nullable<int> PreviousLevelManagerLevel { get; set; }
        public string PreviousLevelManagerName { get; set; }
        public string PreviousLevelManagerEmailAddress { get; set; }
        public Nullable<int> NextLevelManager { get; set; }
        public Nullable<int> NextLevelManagerLevel { get; set; }
        public string NextLevelManagerName { get; set; }
        public string NextLevelManagerEmailAddress { get; set; }   
        public int? CompCompleted { get; set; }
        public Nullable<int> FirstLevelManager { get; set; }       
        public Nullable<int> TopLevelManager { get; set; }       
        public int? isSubmit { get; set; }
        public int? isApprove { get; set; }
        public int? isLocked { get; set; }			
    }
}
