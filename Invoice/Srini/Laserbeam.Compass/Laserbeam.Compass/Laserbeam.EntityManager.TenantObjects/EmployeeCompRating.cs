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
    
    public partial class EmployeeCompRating
    {
        public int EmployeeCompRatingNum { get; set; }
        public int EmpJobNum { get; set; }
        public Nullable<int> MeritPerformanceRating { get; set; }
        public Nullable<int> BonusPerformanceRating { get; set; }
        public Nullable<int> OtherPerformanceRating { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
    
        public virtual Rating Rating { get; set; }
        public virtual Rating Rating1 { get; set; }
        public virtual EmployeeJob EmployeeJob { get; set; }
    }
}
