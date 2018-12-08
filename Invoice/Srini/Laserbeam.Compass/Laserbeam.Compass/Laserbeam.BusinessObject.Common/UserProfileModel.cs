using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laserbeam.BusinessObject.Common
{
    public class UserProfileModel
    {
        public string UserName { get; set; }
        public string UserRole { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserID { get; set; }
        public string JobTitle { get; set; }
        public string URL { get; set; }
        public string EmailAddress { get; set; }
        public decimal TotalHoursSpent { get; set; }
        public string SupportHours { get; set; }
        public string UserNameShort { get; set; }
    }
}
