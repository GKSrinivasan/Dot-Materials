using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laserbeam.BusinessObject.Common.MeritBusinussObjects
{
   public class EmployeeInfoBasicDetails
    {
        public string EmployeeID { get; set; }
        public string JobTitle { get; set; }
        public Nullable<decimal> CurrentAnnualSalaryLocal { get; set; }
        public Nullable<decimal> NewSalaryLocal { get; set; }
        public Nullable<decimal> LumpSumAmountLocal { get; set; }
        public Nullable<double> TotalIncreasePCT { get; set; }
        public Nullable<decimal> TotalIncrease { get; set; }
        public Nullable<decimal> CurrentHourlySalaryLocal { get; set; }
        public Nullable<decimal> NewHourlySalaryLocal { get; set; }
        public string EmployeeStatus { get; set; }
        public string EmployeeName { get; set; }
        public int jobyear { get; set; }
        public string CultureCode { get; set; }
    }
}
