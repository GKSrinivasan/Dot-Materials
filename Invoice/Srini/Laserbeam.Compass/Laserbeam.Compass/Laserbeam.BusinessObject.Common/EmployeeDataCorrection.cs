using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laserbeam.BusinessObject.Common
{
    public class EmployeeDataCorrection
    {
        public EmployeeTemplateFields employeeTemplate { get; set; }
        public UserDataTemplateFields userTemplate { get; set; }
        public ApprovalDataTemplateFields approvalTemplate { get; set; }
        public List<TemplateMetaColumns> templateGroup { get; set; }
        public int? templateNameColumnCount { get; set; }
    }
}
