using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laserbeam.BusinessObject.Common
{
    public class MarketPayRangeGridModel
    {
        public int MarketPayRangeNum { get; set; }
        public Nullable<int> JobNum { get; set; }
        public string JobCode { get; set; }
        public Nullable<int> GradeNum { get; set; }
        public string GradeCode { get; set; }
        public Nullable<int> EmployeeNum { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeID { get; set; }
        public Nullable<decimal> CurrentMin { get; set; }
        public Nullable<decimal> CurrentMid { get; set; }
        public Nullable<decimal> CurrentMax { get; set; }
        public Nullable<decimal> FutureMin { get; set; }
        public Nullable<decimal> FutureMid { get; set; }
        public Nullable<decimal> FutureMax { get; set; }
        public Nullable<decimal> HourlyCurrentMin { get; set; }
        public Nullable<decimal> HourlyCurrentMid { get; set; }
        public Nullable<decimal> HourlyCurrentMax { get; set; }
        public Nullable<decimal> HourlyFutureMin { get; set; }
        public Nullable<decimal> HourlyFutureMid { get; set; }
        public Nullable<decimal> HourlyFutureMax { get; set; }
    }
}
