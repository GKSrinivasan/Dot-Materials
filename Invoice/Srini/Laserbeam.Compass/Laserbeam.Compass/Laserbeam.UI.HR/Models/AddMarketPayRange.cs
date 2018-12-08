using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Laserbeam.UI.HR.Models
{
    public class AddMarketPayRange
    {
        public string SelectedMarketPayRange { get; set; }
        public int MarketPayRangeNum { get; set; }
        public Nullable<int> JobNum { get; set; }
        public string JobCode { get; set; }
        public Nullable<int> GradeNum { get; set; }
        public string Grade { get; set; }
        public Nullable<int> EmployeeNum { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeID { get; set; }
        [DisplayFormat(DataFormatString = "{0:n0}", ApplyFormatInEditMode = true)]
        public Nullable<decimal> CurrentMin { get; set; }
        [DisplayFormat(DataFormatString = "{0:n0}", ApplyFormatInEditMode = true)]
        public Nullable<decimal> CurrentMid { get; set; }
        [DisplayFormat(DataFormatString = "{0:n0}", ApplyFormatInEditMode = true)]
        public Nullable<decimal> CurrentMax { get; set; }
        [DisplayFormat(DataFormatString = "{0:n0}", ApplyFormatInEditMode = true)]
        public Nullable<decimal> FutureMin { get; set; }
        [DisplayFormat(DataFormatString = "{0:n0}", ApplyFormatInEditMode = true)]
        public Nullable<decimal> FutureMid { get; set; }
        [DisplayFormat(DataFormatString = "{0:n0}", ApplyFormatInEditMode = true)]
        public Nullable<decimal> FutureMax { get; set; }
        [DisplayFormat(DataFormatString = "{0:n0}", ApplyFormatInEditMode = true)]
        public Nullable<decimal> HourlyCurrentMin { get; set; }
        [DisplayFormat(DataFormatString = "{0:n0}", ApplyFormatInEditMode = true)]
        public Nullable<decimal> HourlyCurrentMid { get; set; }
        [DisplayFormat(DataFormatString = "{0:n0}", ApplyFormatInEditMode = true)]
        public Nullable<decimal> HourlyCurrentMax { get; set; }
        [DisplayFormat(DataFormatString = "{0:n0}", ApplyFormatInEditMode = true)]
        public Nullable<decimal> HourlyFutureMin { get; set; }
        [DisplayFormat(DataFormatString = "{0:n0}", ApplyFormatInEditMode = true)]
        public Nullable<decimal> HourlyFutureMid { get; set; }
        [DisplayFormat(DataFormatString = "{0:n0}", ApplyFormatInEditMode = true)]
        public Nullable<decimal> HourlyFutureMax { get; set; }
        public bool IsEdit { get; set; }
    }
}