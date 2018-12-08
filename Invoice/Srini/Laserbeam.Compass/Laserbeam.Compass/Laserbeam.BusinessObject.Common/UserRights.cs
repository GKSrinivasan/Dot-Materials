
using System;
namespace Laserbeam.BusinessObject.Common
{
    [Serializable()]
    public class UserRights
    {
        public string MenuID { get; set; }
        public string MenuName { get; set; }
        public string ParentMenuID { get; set; }
        public string MenuLinks { get; set; }
        public bool ReadOnly { get; set; }
        public bool isAdminLinks { get; set; }
    }
}
