//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Laserbeam.EntityManager.TenantMaster
{
    using System;
    using System.Collections.Generic;
    
    public partial class TenantUser
    {
        public int TenantUserNum { get; set; }
        public string UserId { get; set; }
        public int TenantNum { get; set; }
    
        public virtual Tenant Tenant { get; set; }
    }
}
