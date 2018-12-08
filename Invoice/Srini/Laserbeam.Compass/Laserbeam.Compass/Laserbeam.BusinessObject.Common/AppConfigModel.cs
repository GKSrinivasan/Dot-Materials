// Copyright (c) 2014 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.

// Component Name  :  AppConfigModel
// Description     :  Contains the objects needed for application configuration
// Author          :  Revathy		
// Creation Date   :  DD-MM-YYYY
      
using System;
namespace Laserbeam.BusinessObject.Common
{
    [Serializable()]
    public class AppConfigModel
    {
        public string ObjectiveDueDate { get; set; }
        public string CompletionStartDate { get; set; }
        public string CompletionEndDate { get; set; }
        public string PerformancePeriodFrom { get; set; }
        public string PerformancePeriodTo { get; set; }
        public int PerformanceReviewYear { get; set; }
        public string FutureObjectiveDueDate { get; set; }
        public string SMTPServer { get; set; }
        public string ManagerReviewDate { get; set; }
        public string IsSAMLLogin { get; set; }
        public string AdminEmailID { get; set; }
        public string Version { get; set; }
        public int? LoginAttempt { get; set; }
        public string AdminPassword { get; set; }
        public int? SMTPPort { get; set; }        
        public int BonusCycleYear { get; set; }        
        public DateTime GoalApprovalDate { get; set; }
        public string ObjectiveStartDate { get; set; }
        public string ObjectiveEndDate { get; set; }
        public string CurrentYear { get; set; }
        public string MeritCycleYear { get; set; }        
        public string ToolName { get; set; }
        public string ToolVersion { get; set; }
        public string Instance { get; set; }
        public string SuccessionPlanYear { get; set; }
        public bool EnableNotification { get; set; }
        public bool EnableFeedBack { get; set; }
        public string ManagerUrl { get; set; }
        public string AnalyticsUrl { get; set; }
        public string AdminUrl { get; set; }
        public int PasswordDisableDuration { get; set; }
        public bool TriggerEmailSameServer { get; set; }
        public int RangeExceed { get; set; }
        public string BaseCurrency { get; set; }
        public int BaseCurrencyNum { get; set; }
        public string CurrencyApiAccessKey { get; set; }
        public int TokBoxApiKey { get; set; }
        public string TokBoxSecretKey { get; set; }
        public DateTime TenantExpireDate { get; set; }
        public string AdminUserID { get; set; }
        public string PayChangeURL { get; set; }
    }
}
