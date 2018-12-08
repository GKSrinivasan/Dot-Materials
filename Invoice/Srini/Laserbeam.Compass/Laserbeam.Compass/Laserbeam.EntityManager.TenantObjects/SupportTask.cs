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
    
    public partial class SupportTask
    {
        public SupportTask()
        {
            this.SupportTaskComments = new HashSet<SupportTaskComment>();
        }
    
        public int SupportTaskNum { get; set; }
        public int TeamNum { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public string TaskTitle { get; set; }
        public string TaskDescription { get; set; }
        public System.DateTime TaskCreatedDate { get; set; }
        public Nullable<System.DateTime> TaskUpdatedDate { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public string HoursSpent { get; set; }
    
        public virtual Team Team { get; set; }
        public virtual ICollection<SupportTaskComment> SupportTaskComments { get; set; }
    }
}
