using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Laserbeam.BusinessObject.Common
{    
        public  class AppUsers
        {
            public int TotalCount { get; set; }
            public int YetToStartCount { get; set; }
            public int ActiveCount { get; set; }    
            public int LockedUsersCount { get; set; }               
        }        
}
