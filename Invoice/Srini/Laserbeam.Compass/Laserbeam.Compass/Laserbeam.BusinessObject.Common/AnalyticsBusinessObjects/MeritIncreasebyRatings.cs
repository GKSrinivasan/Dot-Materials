using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laserbeam.BusinessObject.Common.AnalyticsBusinessObjects
{
    public class MeritIncreasebyRatings
    {
        public string Rating { get; set; }
        public string Zero { get; set; }
        public string One { get; set; }
        public string Two { get; set; }
        public string Three { get; set; }
        public string Four { get; set; }

        public int? RatingNum { get; set; }
        public int? Count { get; set; }
        public string RatingID { get; set; }

        //department chart
        public string name { get; set; }
        public decimal? value { get; set; }
        public string DepartmentName { get; set; }
        public int? CountValue { get; set; }
        public string AverageValue { get; set; }
        public string BudgetAmount { get; set; }
        public string SpentAmount { get; set; }
        public string CurrentSalaryAmount { get; set; }
        public string NewSalaryAmount { get; set; }
        public decimal? CurrentSalary { get; set; }
        public decimal? NewSalary { get; set; }
        public List<BindChart> BindChart { get; set; }
    }
}
