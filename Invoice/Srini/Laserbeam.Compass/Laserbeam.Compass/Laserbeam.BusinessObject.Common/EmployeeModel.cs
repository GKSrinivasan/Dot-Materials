// Copyright (c) 2014 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.

// Component Name  :  EmployeeModel
// Description     :  Contains objects to get Employee information to be maintained in session	
// Author          :  Revathy		
// Creation Date   :  23-10-2014

using System;
namespace Laserbeam.BusinessObject.Common
{
    [Serializable()]
    public class EmployeeModel
    {
        public int EmployeeNum { get; set; }
        public int UserNum { get; set; }
        public int EmployeeJobNum { get; set; }
        public string EmployeeID { get; set; }
        public string JobDescr { get; set; }
        public string Location { get; set; }
        public string BonusType { get; set; }
        public string Currency { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeFirstName { get; set; }
        public string EmployeeLastName { get; set; }
        public string EmpEmailID { get; set; }
        public int ManagerNum { get; set; }
        public string ManagerName { get; set; }
        public string ManagerFirstName { get; set; }
        public string ManagerLastName { get; set; }
        public string ManagerEmailID { get; set; }
        public int ManagerJobNum { get; set; }
        public string Grade { get; set; }
        public bool IsGradeAboveL_D { get; set; }
        public string MyHRID { get; set; }
    }
}
