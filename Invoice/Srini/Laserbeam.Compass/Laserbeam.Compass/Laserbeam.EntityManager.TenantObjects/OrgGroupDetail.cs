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
    
    public partial class OrgGroupDetail
    {
        public int OrgGroupDetailID { get; set; }
        public int OrgGroupID { get; set; }
        public int OrgGroupCriteriaID { get; set; }
        public string CriteriaDataValue { get; set; }
        public bool Active { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
    
        public virtual OrgGroup OrgGroup { get; set; }
    }
}
