using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laserbeam.BusinessObject.Common.MeritBusinussObjects
{
    public class ApprovalFlowStatus
    {
         public bool isCompCompleted { get; set; }
         public bool isDirectManager { get; set; }
         public bool isSecondLevelManager { get; set; }
         public bool isTopLeval { get; set; }
         public bool isSubmit { get; set; }
         public bool isApprove { get; set; }
         public bool isReopen { get; set; }
         public bool isLocked { get; set; }
    }
}
