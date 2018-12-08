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
    
    public partial class EmployeeJob
    {
        public EmployeeJob()
        {
            this.Budgets = new HashSet<Budget>();
            this.EmployeeCompApprovalLevels = new HashSet<EmployeeCompApprovalLevel>();
            this.EmployeeCompApprovalLevels1 = new HashSet<EmployeeCompApprovalLevel>();
            this.EmployeeCompApprovalLevels2 = new HashSet<EmployeeCompApprovalLevel>();
            this.EmployeeCompApprovalLevels3 = new HashSet<EmployeeCompApprovalLevel>();
            this.EmployeeCompApprovalLevels4 = new HashSet<EmployeeCompApprovalLevel>();
            this.EmployeeCompApprovalLevels5 = new HashSet<EmployeeCompApprovalLevel>();
            this.EmployeeCompApprovalLevels6 = new HashSet<EmployeeCompApprovalLevel>();
            this.EmployeeCompApprovalLevels7 = new HashSet<EmployeeCompApprovalLevel>();
            this.EmployeeCompApprovalLevels8 = new HashSet<EmployeeCompApprovalLevel>();
            this.EmployeeCompApprovalLevels9 = new HashSet<EmployeeCompApprovalLevel>();
            this.EmployeeCompApprovalLevels10 = new HashSet<EmployeeCompApprovalLevel>();
            this.EmployeeCompComments = new HashSet<EmployeeCompComment>();
        }
    
        public int EmpJobNum { get; set; }
        public int EmployeeNum { get; set; }
        public int JobYear { get; set; }
        public Nullable<int> ManagerNum { get; set; }
        public Nullable<int> CurrencyCodeNum { get; set; }
        public string JobStatus { get; set; }
        public Nullable<decimal> FLSAPct { get; set; }
        public Nullable<int> CountryCodeNum { get; set; }
        public string City { get; set; }
        public Nullable<int> JobNum { get; set; }
        public Nullable<int> ReviewTypeNum { get; set; }
        public string EmpFunction { get; set; }
        public Nullable<int> LocationNum { get; set; }
        public Nullable<int> StateNum { get; set; }
        public Nullable<int> GradeNum { get; set; }
        public Nullable<int> BonusTypeNum { get; set; }
        public Nullable<int> BusinessUnitNum { get; set; }
        public Nullable<int> DivisionNum { get; set; }
        public Nullable<int> CountryNum { get; set; }
        public Nullable<int> EntityCodeNum { get; set; }
        public Nullable<int> DepartmentNum { get; set; }
        public Nullable<int> PayGroupNum { get; set; }
        public Nullable<int> EmployeeSubStatusNum { get; set; }
        public string EmployeeType { get; set; }
        public Nullable<int> ProgramNum { get; set; }
        public Nullable<int> ClassNum { get; set; }
        public string FileNumber { get; set; }
        public string PayFrequency { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
    
        public virtual BonusType BonusType { get; set; }
        public virtual BusinessUnit BusinessUnit { get; set; }
        public virtual Country Country { get; set; }
        public virtual Currency Currency { get; set; }
        public virtual Department Department { get; set; }
        public virtual Division Division { get; set; }
        public virtual EmployeeClass EmployeeClass { get; set; }
        public virtual EntityCode EntityCode { get; set; }
        public virtual Grade Grade { get; set; }
        public virtual Job Job { get; set; }
        public virtual Location Location { get; set; }
        public virtual PayGroup PayGroup { get; set; }
        public virtual Program Program { get; set; }
        public virtual ReviewType ReviewType { get; set; }
        public virtual State State { get; set; }
        public virtual SubStatu SubStatu { get; set; }
        public virtual ICollection<Budget> Budgets { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual Employee Employee1 { get; set; }
        public virtual EmployeeCompAdjustment EmployeeCompAdjustment { get; set; }
        public virtual EmployeeCompApprovalComment EmployeeCompApprovalComment { get; set; }
        public virtual ICollection<EmployeeCompApprovalLevel> EmployeeCompApprovalLevels { get; set; }
        public virtual ICollection<EmployeeCompApprovalLevel> EmployeeCompApprovalLevels1 { get; set; }
        public virtual ICollection<EmployeeCompApprovalLevel> EmployeeCompApprovalLevels2 { get; set; }
        public virtual ICollection<EmployeeCompApprovalLevel> EmployeeCompApprovalLevels3 { get; set; }
        public virtual ICollection<EmployeeCompApprovalLevel> EmployeeCompApprovalLevels4 { get; set; }
        public virtual ICollection<EmployeeCompApprovalLevel> EmployeeCompApprovalLevels5 { get; set; }
        public virtual ICollection<EmployeeCompApprovalLevel> EmployeeCompApprovalLevels6 { get; set; }
        public virtual ICollection<EmployeeCompApprovalLevel> EmployeeCompApprovalLevels7 { get; set; }
        public virtual ICollection<EmployeeCompApprovalLevel> EmployeeCompApprovalLevels8 { get; set; }
        public virtual ICollection<EmployeeCompApprovalLevel> EmployeeCompApprovalLevels9 { get; set; }
        public virtual ICollection<EmployeeCompApprovalLevel> EmployeeCompApprovalLevels10 { get; set; }
        public virtual EmployeeCompApprovalStatu EmployeeCompApprovalStatu { get; set; }
        public virtual ICollection<EmployeeCompComment> EmployeeCompComments { get; set; }
        public virtual EmployeeCompMarket EmployeeCompMarket { get; set; }
        public virtual EmployeeCompMerit EmployeeCompMerit { get; set; }
        public virtual EmployeeCompNew EmployeeCompNew { get; set; }
        public virtual EmployeeCompPromotion EmployeeCompPromotion { get; set; }
        public virtual EmployeeCompRating EmployeeCompRating { get; set; }
        public virtual EmployeeCompBonu EmployeeCompBonu { get; set; }
    }
}