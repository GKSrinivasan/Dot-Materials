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
    
    public partial class EmployeeCompApprovalLevel
    {
        public int EmployeeCompApprovalLevelNum { get; set; }
        public int EmpJobNum { get; set; }
        public Nullable<int> AppMgrOneEmpNum { get; set; }
        public Nullable<int> AppMgrTwoEmpNum { get; set; }
        public Nullable<int> AppMgrThreeEmpNum { get; set; }
        public Nullable<int> AppMgrFourEmpNum { get; set; }
        public Nullable<int> AppMgrFiveEmpNum { get; set; }
        public Nullable<int> AppMgrSixEmpNum { get; set; }
        public Nullable<int> AppMgrSevenEmpNum { get; set; }
        public Nullable<int> AppMgrEightEmpNum { get; set; }
        public Nullable<int> AppMgrNineEmpNum { get; set; }
        public Nullable<int> AppMgrTenEmpNum { get; set; }
        public int ModuleNum { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
    
        public virtual Module Module { get; set; }
        public virtual Module Module1 { get; set; }
        public virtual EmployeeJob EmployeeJob { get; set; }
        public virtual EmployeeJob EmployeeJob1 { get; set; }
        public virtual EmployeeJob EmployeeJob2 { get; set; }
        public virtual EmployeeJob EmployeeJob3 { get; set; }
        public virtual EmployeeJob EmployeeJob4 { get; set; }
        public virtual EmployeeJob EmployeeJob5 { get; set; }
        public virtual EmployeeJob EmployeeJob6 { get; set; }
        public virtual EmployeeJob EmployeeJob7 { get; set; }
        public virtual EmployeeJob EmployeeJob8 { get; set; }
        public virtual EmployeeJob EmployeeJob9 { get; set; }
        public virtual EmployeeJob EmployeeJob10 { get; set; }
    }
}
