﻿
using System;
namespace Laserbeam.BusinessObject.Common
{
    public class CompensationReportees
    {
        public string SortOrderEmployeeName { get; set; }
        public int EmployeeNum { get; set; }
        public int Empjobnum { get; set; }
        public int Managernum { get; set; }
        public int? Jobnum { get; set; }
        public int? NewJobnum { get; set; }
        public int UpdateComp { get; set; }
        public bool LockEmployee { get; set; }
        public bool CompCompleted { get; set; }
        public bool ExceedCommentProvided { get; set; }
        public string EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public Nullable<decimal> BaseSalary { get; set; }
        public Nullable<decimal> BaseSalaryUSD { get; set; }
        public Nullable<decimal> AnnualisedSalary { get; set; }
        public Nullable<decimal> AnnualisedSalaryUSD { get; set; }
        public Nullable<decimal> HrlySalary { get; set; }
        public string Title { get; set; }
        public string Grade { get; set; }
        public int?  GradeNum { get; set; }
        public string CompComment { get; set; }
        public Nullable<int> MeritPerformanceRatingNum { get; set; }
        public string MeritPerformanceRating { get; set; }
        public string MeritRange { get; set; }
        public string MeritRangeExceed { get; set; }
        public Nullable<decimal> ProRation { get; set; }
        public string MeritPCT { get; set; }
        public Nullable<decimal> ProRatedAmt { get; set; }
        public Nullable<decimal> ProRatedAmtUSD { get; set; }
        public Nullable<decimal> HrlyMeritAmt { get; set; }
        public string NewTitle { get; set; }
        public string PromotionComment { get; set; }
        public int PromotionCommentNum { get; set; }
        public string NewGrade { get; set; }
        public string PromotionPct { get; set; }
        public string PromotionAmt { get; set; }
        public Nullable<decimal> PromotionAmtUSD { get; set; }
        public string AdjustmentPct { get; set; }
        public string AdjustmentAmt { get; set; }
        public Nullable<decimal> AdjustmentAmtUSD { get; set; }
        public Nullable<decimal> MktMin { get; set; }
        public Nullable<decimal> MktMid { get; set; }
        public Nullable<decimal> MktMax { get; set; }
        public Nullable<decimal> MktMinUSD { get; set; }
        public Nullable<decimal> MktMidUSD { get; set; }
        public Nullable<decimal> MktMaxUSD { get; set; }
        public Nullable<decimal> NewMktMin { get; set; }
        public Nullable<decimal> NewMktMid { get; set; }
        public Nullable<decimal> NewMktMax { get; set; }
        public Nullable<decimal> NewMktMinUSD { get; set; }
        public Nullable<decimal> NewMktMidUSD { get; set; }
        public Nullable<decimal> NewMktMaxUSD { get; set; }
        public Nullable<decimal> NewSalary { get; set; }
        public Nullable<decimal> NewSalaryUSD { get; set; }
        public Nullable<decimal> HrlyNewSalary { get; set; }
        public string NewQuartile { get; set; }
        public Nullable<decimal> CompaRatio { get; set; }
        public Nullable<decimal> NewCompaRatio { get; set; }
        public Nullable<decimal> BonusTarget { get; set; }
        public Nullable<decimal> BonusTargetUSD { get; set; }
        public string BonusGuideline { get; set; }
        public Nullable<decimal> BonusPCT { get; set; }
        public Nullable<decimal> BonusAMT { get; set; }
        public Nullable<decimal> BonusAMTUSD { get; set; }
        public Nullable<decimal> TotalNewComp { get; set; }
        public System.DateTime HireDate { get; set; }
        public string EmployeeSubStatus { get; set; }
        public string JobCode { get; set; }
        public Nullable<System.DateTime> SalaryEffectiveDate { get; set; }
        public string SalaryReasonCode { get; set; }
        public string PromotionRange { get; set; }
        public string AdjustmentRange { get; set; }
        public Nullable<decimal> ActualWorkHrs { get; set; }
        public string Department { get; set; }
        public string Division { get; set; }
        public string Category { get; set; }
        public int? ApprovalStatus { get; set; }
        public Nullable<decimal> LumpSumPct { get; set; }
        public Nullable<decimal> LumpSumAmt { get; set; }
        public Nullable<decimal> LumpSumAmtUSD { get; set; }
        public Nullable<decimal> HrlyPromotionAmt { get; set; }
        public Nullable<decimal> HrlyAdjustmentAmt { get; set; }
        public Nullable<decimal> HrlyLumpSumAmt { get; set; }
        public Nullable<decimal> MktQ1 { get; set; }
        public Nullable<decimal> MktQ2 { get; set; }
        public Nullable<decimal> MktQ3 { get; set; }
        public Nullable<decimal> MktQ4 { get; set; }
        public Nullable<decimal> MktQ5 { get; set; }
        public Nullable<decimal> HrlyMktQ1 { get; set; }
        public Nullable<decimal> HrlyMktQ2 { get; set; }
        public Nullable<decimal> HrlyMktQ3 { get; set; }
        public Nullable<decimal> HrlyMktQ4 { get; set; }
        public Nullable<decimal> HrlyMktQ5 { get; set; }
        public Nullable<decimal> HrlyMktMin { get; set; }
        public Nullable<decimal> HrlyMktMid { get; set; }
        public Nullable<decimal> HrlyMktMax { get; set; }
        public Nullable<decimal> HrlyBonusAmt { get; set; }
        public string EmailAddress { get; set; }
        public Nullable<decimal> ProRatedPCT { get; set; }
        public string MeritAmt { get; set; }
        public Nullable<decimal> MeritAmtUSD { get; set; }
        public string JobStatus { get; set; }
        public Nullable<decimal> FLSAPct { get; set; }
        public string CommentsList { get; set; }
        public Nullable<decimal> BudgetPCT { get; set; }
        public Nullable<decimal> Budget { get; set; }
        public Nullable<decimal> TotalWorkHrs { get; set; }
        public string SupervisorID { get; set; }
        public string SupervisorName { get; set; }
        public string Comments { get; set; }
        public string RatingAndGuideline { get; set; }
        public Nullable<decimal> MeritExchangeRate { get; set; }
        public string CurrencyCode { get; set; }
        public string Location { get; set; }
        public string BusinessUnit { get; set; }
        public string PayGroup { get; set; }
        public string EntityCode { get; set; }
        public bool PromotionRequest { get; set; }
        public bool HighLightHireDate { get; set; }
        public Nullable<int> JobMarketNum { get; set; }
        public Nullable<int> NewJobMarketNum { get; set; }
        public int MarketReferenceNum { get; set; }
        public int MeritPercentage { get; set; }
        public bool IsMeritLnkEnabled { get; set; }
        public int count{get;set;}
        public int Leavecount { get; set; }
        public string AdjustmentComments { get; set; }
        public string Country { get; set; }
        public string EmpJobFunction { get; set; }

        public decimal? BonusTargetPCT {get; set;}
        public decimal? BonusTargetAMT { get; set; }
        public decimal? AdjustedBonusAMT { get; set; }
             
    }
}
