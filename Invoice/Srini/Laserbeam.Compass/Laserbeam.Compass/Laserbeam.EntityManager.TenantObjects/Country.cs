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
    
    public partial class Country
    {
        public Country()
        {
            this.EmployeeJobs = new HashSet<EmployeeJob>();
        }
    
        public int CountryNum { get; set; }
        public string Country1 { get; set; }
        public bool Active { get; set; }
    
        public virtual ICollection<EmployeeJob> EmployeeJobs { get; set; }
    }
}