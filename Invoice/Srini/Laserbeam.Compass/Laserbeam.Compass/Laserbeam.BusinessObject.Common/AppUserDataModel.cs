using Laserbeam.Resource.HR.UserManagementResources;
using System;
using System.ComponentModel.DataAnnotations;

namespace Laserbeam.BusinessObject.Common
{
   public  class AppUserDataModel
    {
        public int UserNum { get; set; }
        public string LoginID { get; set; }
        public string UserID { get; set; }
        public int UserRoleNum { get; set; }
        public Nullable<System.DateTime> LastLoginDt { get; set; }
        public Nullable<int> AppUserStatusID { get; set; }
        public string UserStatus { get; set; }
        public string UserRole { get; set; }
        public string EmailAddress { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PreferredName { get; set; }
        public string EmailID { get; set; }
        public string EmployeeID { get; set; }
        public bool? MailDeliveryStatus { get; set; }
        public DateTime? MailDeliveryDate { get; set; }


    }
}
