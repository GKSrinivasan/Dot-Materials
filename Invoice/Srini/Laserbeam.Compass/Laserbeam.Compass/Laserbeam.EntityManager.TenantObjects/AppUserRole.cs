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
    
    public partial class AppUserRole
    {
        public AppUserRole()
        {
            this.AppUsers = new HashSet<AppUser>();
        }
    
        public int UserRoleNum { get; set; }
        public string UserRole { get; set; }
        public string KeyName { get; set; }
        public Nullable<bool> Active { get; set; }
    
        public virtual ICollection<AppUser> AppUsers { get; set; }
    }
}
