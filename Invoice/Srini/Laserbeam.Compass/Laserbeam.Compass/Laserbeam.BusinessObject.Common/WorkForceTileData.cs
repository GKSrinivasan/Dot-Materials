using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laserbeam.BusinessObject.Common
{
    public class WorkForceTileData
    {
        public string TotalEmployees { get; set; }
        public string TotalEmployeesSalary { get; set; }
        public string TotalMeritEligibleEmployees { get; set; }
        public string TotalMeritEligibleEmployeesSalary { get; set; }
    }
}
