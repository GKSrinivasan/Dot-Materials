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
    
    public partial class EmployeeCompApprovalStatu
    {
        public int EmployeeCompApprovalStatusNum { get; set; }
        public int EmpJobNum { get; set; }
        public Nullable<int> CurrentApprovalStatus { get; set; }
        public Nullable<int> CurrentApprovedMgrNum { get; set; }
        public Nullable<int> CurrentApprovedMgrLevel { get; set; }
        public Nullable<int> PreviousLevelMgrNum { get; set; }
        public Nullable<int> PreviousApprovedMgrLevel { get; set; }
        public Nullable<int> NextLevelMgrNum { get; set; }
        public Nullable<int> NextApproverLevel { get; set; }
        public Nullable<int> FirstLevelMgrNum { get; set; }
        public Nullable<int> TopLevelMgrNum { get; set; }
        public Nullable<int> TopMgrLevel { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
    
        public virtual EmployeeJob EmployeeJob { get; set; }
    }
}