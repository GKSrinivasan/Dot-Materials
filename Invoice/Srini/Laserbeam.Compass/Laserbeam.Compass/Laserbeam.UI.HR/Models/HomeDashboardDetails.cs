// Copyright (c) 2015 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.

// Component Name  :  HomeDashboardDetails Model
// Description     :  Contains the objects needed for application configuration
// Author          :  Arunraj		
// Creation Date   :  16-May-2017

using Laserbeam.BusinessObject.Common;
using System.Collections.Generic;

namespace Laserbeam.UI.HR.Models
{
    public class HomeDashboardDetails
    {
        public string DefaultCurrencyCulture { get; set; }
        public string CurrencyCode { get; set; }
        public CompensationTypeConfiguration ruleConfiguration { get; set; }
        public List<EventsandCommunication> Announcement { get; set; }
        public EmployeeSalaryDetails EmployeeSalaryDetails { get; set; }
        public List<ManagerBudgetDetails> ManagerBudgetDetails { get; set; }
        public int UserLoginStatusCount { get; set; }
        public List<DailyTaskModel> YetToSubmitList { get; set; }
        public List<TeamApprovalStatus> TeamStatusList { get; set; }
        public WorkFlowBudgetSpendCount WorkFlowBudgetSpendCount { get; set; }
        public bool isWorkFlowEnabled { get; set; }
        public string CultureCode { get; set; }
    }
}