// Copyright (c) 2015 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.

// Component Name  :  ManagerBudgetDetails Model
// Description     :  Contains the objects needed for application configuration
// Author          :  Arunraj		
// Creation Date   :  16-May-2017
using System;

namespace Laserbeam.BusinessObject.Common
{
    public class ManagerBudgetDetails
    {
        public string ManagerName { get; set; }
        public int ManagerNum { get; set; }
        public string EmployeeName { get; set; }
        public int EmployeeNum { get; set; }
        public decimal ManagerBudget { get; set; }
        public decimal ManagerSpent { get; set; }
        public decimal ManagerBalance { get; set; }
        public decimal AdjustedBudget { get; set; }
        public decimal AnnulSalary { get; set; } 
        public decimal ManagerBudgetPct { get; set; }
        public int EmployeeCount { get; set; }
        public string cultureCode { get; set; }
    }
}
