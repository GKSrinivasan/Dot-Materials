//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Laserbeam.EntityManager.Common
{
    using System;
    using System.Collections.Generic;
    
    public partial class EmployeeCompAdjustment
    {
        public int EmployeeCompAdjustmentNum { get; set; }
        public int EmpJobNum { get; set; }
        public string AdjustmentRange { get; set; }
        public Nullable<decimal> AdjustmentPct { get; set; }
        public Nullable<decimal> AdjustmentAmt { get; set; }
        public Nullable<decimal> HrlyAdjustmentAmt { get; set; }
        public Nullable<System.DateTime> EffectiveDate { get; set; }
        public Nullable<int> AdjustmentPH { get; set; }
        public bool Eligibility { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
    
        public virtual EmployeeJob EmployeeJob { get; set; }
    }
}
