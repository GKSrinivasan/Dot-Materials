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
    
    public partial class Employee
    {
        public Employee()
        {
            this.AppUsers = new HashSet<AppUser>();
            this.EmployeeGroups = new HashSet<EmployeeGroup>();
            this.EmployeeGroupInclusionExclusions = new HashSet<EmployeeGroupInclusionExclusion>();
            this.EmployeeInclusionAndExclusions = new HashSet<EmployeeInclusionAndExclusion>();
            this.EmployeeJobs = new HashSet<EmployeeJob>();
            this.EmployeeJobs1 = new HashSet<EmployeeJob>();
            this.ManagerRelations = new HashSet<ManagerRelation>();
            this.ProxyManagers = new HashSet<ProxyManager>();
        }
    
        public int EmployeeNum { get; set; }
        public string EmployeeID { get; set; }
        public string MyHRID { get; set; }
        public string EmployeeName { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleInitial { get; set; }
        public string PreferredName { get; set; }
        public Nullable<System.DateTime> DateofBirth { get; set; }
        public System.DateTime HireDate { get; set; }
        public string EmailAddress { get; set; }
        public Nullable<int> BasicPH { get; set; }
        public string Gender { get; set; }
        public string Ethnicity { get; set; }
        public Nullable<int> Age { get; set; }
        public string SortOrderEmployeeName { get; set; }
        public Nullable<System.DateTime> TerminationDate { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
    
        public virtual ICollection<AppUser> AppUsers { get; set; }
        public virtual ICollection<EmployeeGroup> EmployeeGroups { get; set; }
        public virtual ICollection<EmployeeGroupInclusionExclusion> EmployeeGroupInclusionExclusions { get; set; }
        public virtual ICollection<EmployeeInclusionAndExclusion> EmployeeInclusionAndExclusions { get; set; }
        public virtual ICollection<EmployeeJob> EmployeeJobs { get; set; }
        public virtual ICollection<EmployeeJob> EmployeeJobs1 { get; set; }
        public virtual ICollection<ManagerRelation> ManagerRelations { get; set; }
        public virtual ICollection<ProxyManager> ProxyManagers { get; set; }
    }
}
