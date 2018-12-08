using Laserbeam.Resource.HR.UserManagementResources;
using System;
using System.ComponentModel.DataAnnotations;

namespace Laserbeam.BusinessObject.Common
{
    public partial class AppUserModel
    {
        public int UserNum { get; set; }
        public Nullable<int> EmployeeNum { get; set; }       
        public string UserName { get; set; }
        public string UserPassword { get; set; }
        public string DefaultPassword { get; set; }       
        public string ProxyID { get; set; }
        public string UserGroup { get; set; }
        public int UserRoleNum { get; set; }
        public Nullable<int> FailedLoginAttempts { get; set; }
        public Nullable<System.DateTime> LastLoginDt { get; set; }
        public Nullable<System.DateTime> LastLogOutDt { get; set; }
        public Nullable<int> AppUserStatusID { get; set; }
        public Nullable<int> XMLProcessID { get; set; }
        public Nullable<System.DateTime> SSOLastLoginDate { get; set; }
        public Nullable<System.DateTime> SSOLastLogOutDate { get; set; }
        public string ApplicationSessionID { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<bool> DefaultPasswordExpired { get; set; }              
        public string SecretKey { get; set; }
        public string LoginID { get; set; }
        public bool MarkAsDelete { get; set; }
        public Nullable<System.DateTime> LockedDate { get; set; }        
        public Nullable<int> EmpID { get; set; }     
        public string UserStatus { get; set; }
        public string UserRole { get; set; }
        public string UserRoleKeyValue { get; set; }
        public bool ChkAdminAccessTool { get; set; }
        public string EmailAddress { get; set; }
        public bool IsAdminAccess { get; set; }
       

        [Display(Name = "LabelUserID", ResourceType = typeof(UserManagementResources))]
        public string UserID { get; set; }

        [Display(Name = "LabelEmployeeID", ResourceType = typeof(UserManagementResources))]
        public string EmployeeID { get; set; }

        [Display(Name = "LabelFirstName", ResourceType = typeof(UserManagementResources))] 
        public string FirstName { get; set; }

        [Display(Name = "LabelLastName", ResourceType = typeof(UserManagementResources))]
        public string LastName { get; set; }

        [Display(Name = "LabelPreferredName", ResourceType = typeof(UserManagementResources))]
        public string PreferredName { get; set; }

        [Display(Name = "LabelEmailAddress", ResourceType = typeof(UserManagementResources))]
        public string EmailID { get; set; }

        public int TotalUsersCount { get; set; }
        public int ActiveUsersCount { get; set; }
        public int YetToUsersCount { get; set; }
        public int LockedUsersCount { get; set; }
        public bool isEmail { get; set; }
        public bool? MailDeliveryStatus { get; set; }
        public DateTime? MailDeliveryDate { get; set; }
    }
}
