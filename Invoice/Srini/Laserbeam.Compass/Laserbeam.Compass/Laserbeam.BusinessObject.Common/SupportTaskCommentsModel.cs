using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laserbeam.BusinessObject.Common
{
    public class SupportTaskCommentsModel
    {
        public string Comments { get; set; }
        public int SupportTaskCommentsNum { get; set; }
        public int SupportTaskNum { get; set; }
        public int? UpdatedBy { get; set; }
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? UpdatedDate { get; set; }
        public int EmpJobNum { get; set; }
        public int CreatedBy { get; set; }
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime CreatedDate { get; set; }
        public string EmployeeName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmployeeNameShort
        {
            get
            {
                return string.Concat((this.FirstName != null ? this.FirstName[0] : ' '));
            }
        }
    }
}
