// Copyright (c) 2014 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.

// Component Name  :  Email Details
// Description     :  Contains objects to get the details to be sent in mail	
// Author          :  Thiyagu		
// Creation Date   :  01-11-2014

using System;

namespace Laserbeam.BusinessObject.Common
{
   public class EmailDetails
    {
        public string EmailSubject { get; set; }
        public string EmailBody { get; set; }
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string ManagerName { get; set; }
        public string EmployeeName { get; set; }
        public string DefaultPassword { get; set; }
        public string FromEmailID { get; set; }
        public string ToEmailID { get; set; }
        public string ClientUserID { get; set; }
        public string Password { get; set; }
        public string AdminEmailID { get; set; }
        public string EmailCC { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? ManagerEmpJobNum { get; set; }
        public int? RequesterEmpJobNum { get; set; }
        public string RequestedApprovalManagerName { get; set; }
        public string EmailID { get; set; }
        public bool Success { get; set; }
        public string SuccessStatus { get; set; }
        public string UpdatedDateForEmail { get; set; }
        public int UserNum { get; set; }
        public int UserRoleNum { get; set; }                
        public string SecretKey { get; set; }
        public string referenceUrl { get; set; }
    }
}
