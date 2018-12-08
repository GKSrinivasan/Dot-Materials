using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laserbeam.BusinessObject.Common
{
    
    public partial class AppUserRoleModel 
    {
      

        public int UserRoleNum { get; set; }
        public string UserRole { get; set; }
        public Nullable<bool> Active { get; set; }
        public string KeyName { get; set; }


        public virtual ICollection<AppUserModel> AppUsers { get; set; }
    }
}
