using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laserbeam.BusinessObject.Common
{
    public class BindChart
    {
        public string name { get; set; }
        public decimal? value { get; set; }
        public int? RatingNum { get; set; }
        public decimal? Budget { get; set; }
        public decimal? Spent { get; set; }
        public decimal? CurrentSalary { get; set; }
        public decimal? NewSalary { get; set; }
        public string CurrentSalaryAmount { get; set; }
        public string NewSalaryAmount { get; set; }
        public string BudgetAmount { get; set; }
        public string SpentAmount { get; set; }
        public int? Count { get; set; }
        public string RatingID { get; set; }
    }
}
