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
    
    public partial class Division
    {
        public Division()
        {
            this.EmployeeJobs = new HashSet<EmployeeJob>();
        }
    
        public int DivisionNum { get; set; }
        public string DivisionCode { get; set; }
        public string DivisionDescr { get; set; }
        public Nullable<bool> Active { get; set; }
    
        public virtual ICollection<EmployeeJob> EmployeeJobs { get; set; }
    }
}