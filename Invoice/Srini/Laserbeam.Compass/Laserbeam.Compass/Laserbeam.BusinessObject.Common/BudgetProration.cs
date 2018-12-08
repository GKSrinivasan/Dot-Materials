using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laserbeam.BusinessObject.Common
{
    public class BudgetProration
    {
        public bool BudgetProrationValue { get; set; }

        [DataType(DataType.Date, ErrorMessage = "Please enter a valid date.")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Please Enter Your ProrateStartDate ")]
        public DateTime? ProrateStartDate { get; set; }

        [Required(ErrorMessage = "Please Enter Your ProrateEndDate ")]
        [DataType(DataType.Date, ErrorMessage = "Please enter a valid date.")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ProrateEndDate { get; set; }

        public string ProrationType { get; set; }

        [RegularExpression("^[0-9]*$", ErrorMessage = "ProrationDuration must be numeric")]
        [Required(ErrorMessage = "Please Enter Your ProrationDuration ")]
        public string ProrationDuration { get; set; }

        [RegularExpression("^[0-9]*$", ErrorMessage = "ProrationDatesPerMonth must be numeric")]
        //[Required(ErrorMessage = "Please Enter Your ProrationDatesPerMonth ")]
        public string ProrationDatesPerMonth { get; set; }


    }
}
