using Laserbeam.BusinessObject.Common;
using System.Collections.Generic;

namespace Laserbeam.UI.HR.Models
{
    public class DashboardWorkFlowModel
    {
        public List<DashboardApprovalData> ManagerApproval { get; set; }
        public List<TeamApprovalStatus> TeamApproval { get; set; }
    }
}