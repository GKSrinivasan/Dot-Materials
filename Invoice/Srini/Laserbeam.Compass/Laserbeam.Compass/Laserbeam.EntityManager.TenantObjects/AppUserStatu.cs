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
    
    public partial class AppUserStatu
    {
        public AppUserStatu()
        {
            this.AppUsers = new HashSet<AppUser>();
        }
    
        public int AppUserStatusID { get; set; }
        public string UserStatus { get; set; }
    
        public virtual ICollection<AppUser> AppUsers { get; set; }
    }
}
