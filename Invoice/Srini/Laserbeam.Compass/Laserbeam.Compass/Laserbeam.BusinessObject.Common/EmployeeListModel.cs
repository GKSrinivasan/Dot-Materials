using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laserbeam.BusinessObject.Common
{
    public class EmployeeListModel
    {
        public bool Selected { get; set; }
        public string Text { get; set; }
        public string Value { get; set; }
        public bool IsMandatory { get; set; }
    }
}
