using Laserbeam.BusinessObject.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Laserbeam.UI.HR.Models
{
    public class WorkflowModel
    {
        public List<WorkFlowGrid> WorkFlowGrid { get; set; }
        public List<EmployeeModel> EmpIdList { get; set; }
        public int  ProcessNum { get; set; }
        public string EmpIds { get; set; }
        public bool IsCustomized { get; set; }
        public string WorkFlowLevel { get; set; }
    }
}