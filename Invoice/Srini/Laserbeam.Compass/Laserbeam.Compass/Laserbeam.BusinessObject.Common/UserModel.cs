// Copyright (c) 2014 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.

// Component Name  :  User Model
// Description     :  Contains the objects needed to get user details to be maintained in the session object	
// Author          :  Revathy		
// Creation Date   :  23-10-2014
      
using System;
namespace Laserbeam.BusinessObject.Common
{
    [Serializable()]
    public class UserModel
    {
        public int UserNum { get; set; }
        public string UserID { get; set; }
        public string PreferredName { get; set; }
        public string UserName { get; set; }
        public int UserRoleNum { get; set; }
        public string UserRole { get; set; }
        public string KeyName { get; set; }
        public string EmailID { get; set; }
        public int EmployeeNum { get; set; }
        public string EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string UserPassword { get; set; }
        public string DefaultPassword { get; set; }
        public Nullable<int> FailedLoginAttempts { get; set; }
        public Nullable<System.DateTime> LastLoginDt { get; set; }
        public Nullable<int> AppUserStatusID { get; set; }
        public string SecretKey { get; set; }
        public bool IsActive { get; set; }
        public bool IsSSOLogin { get; set; }
        public string Grade { get; set; }
        public bool IsGradeAboveL_D { get; set; }        
        public int WizardStepsCompleted { get; set; }
        public bool? IsDefaultUser { get; set; }
        public bool IsAdminAccess { get; set; }
        public int? AdminEmpNum { get; set; } 
        public int? AdminUserNum { get; set; }
        public bool enableSwitchtoAdmin { get; set; }
        public bool IsSampleData { get; set; }
        public string TenantName { get; set; }
        public string UrlAuthority { get; set; }
    }
}