using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laserbeam.BusinessObject.Common
{
    public class OrphanManagerDetails
    {
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public int EmployeeNum { get; set; }
        public int EmployeeJobNum { get; set; }
        public bool IsChecked { get; set; }
    }
}
