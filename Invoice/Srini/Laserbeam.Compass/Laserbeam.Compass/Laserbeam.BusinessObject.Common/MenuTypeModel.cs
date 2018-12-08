using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laserbeam.BusinessObject.Common
{
    public class MenuTypeModel
    {
        public int MenuNum { get; set; }
        public string MenuName { get; set; }
        public int EmployeeOrGroupNum { get; set; }
        public string EmployeeOrGroupName { get; set; }
        public bool IsReadOnly { get; set; }
    }
}
