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
    
    public partial class OrgGroupCriteria
    {
        public OrgGroupCriteria()
        {
            this.OrgGroupAssignedCriterias = new HashSet<OrgGroupAssignedCriteria>();
        }
    
        public int OrgGroupCriteriaID { get; set; }
        public string CriteriaName { get; set; }
        public byte CriteriaOrderBy { get; set; }
        public bool Active { get; set; }
    
        public virtual ICollection<OrgGroupAssignedCriteria> OrgGroupAssignedCriterias { get; set; }
    }
}