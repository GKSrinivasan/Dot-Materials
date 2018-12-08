using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laserbeam.BusinessObject.Common.MeritBusinussObjects
{
    public class MeritGridModel
    {
        public string EmployeeID { get; set; }
        public string MyHRID { get; set; }
        public string EmployeeName { get; set; }
        public string EmpFunction { get; set; }
        public int EmployeeNum { get; set; }
        public int Empjobnum { get; set; }                
        public Nullable<decimal> CompaRatio { get; set; }
        public string MeritPerformanceRating { get; set; }        
        public string MeritRange { get; set; }
        public string MeritRangeExceed { get; set; }
        public string NewTitle { get; set; }
        public string NewGrade { get; set; }
        public string HireDate { get; set; }
        public string EmployeeSubStatus { get; set; }
        public string JobCode { get; set; }
        public string Title { get; set; }
        public string Grade { get; set; }
        public string PromotionRange { get; set; }
        public string AdjustmentRange { get; set; }        
        public string Country { get; set; }
        public string Department { get; set; }
        public string Location { get; set; }
        public string Division { get; set; }
        public string Category { get; set; }
        public Nullable<int> ApprovalStatus { get; set; }
        public Nullable<decimal> BaseSalaryLocal { get; set; }
        public Nullable<decimal> BaseSalaryUSD { get; set; }
        public Nullable<decimal> CurrentAnnualSalaryLocalForCalc { get; set; }
        public Nullable<decimal> CurrentAnnualSalaryUSDForCalc { get; set; }
        public Nullable<decimal> CurrentAnnualSalaryLocalForLumpSumCalc { get; set; }
        public Nullable<decimal> CurrentAnnualSalaryUSDForLumpSumCalc { get; set; }
        public Nullable<decimal> CurrentAnnualSalaryLocalForNewSalaryCalc { get; set; }

        public Nullable<decimal> CurrentHourlyRateLocalForCalc { get; set; }
        public Nullable<decimal> CurrentHourlyRateUSDForCalc { get; set; }
        public Nullable<decimal> ProRation { get; set; }                
        public Nullable<decimal> CountryBudgetLocal { get; set; }
        public Nullable<decimal> CountryBudgetUSD { get; set; }
        public Nullable<decimal> BudgetLocal { get; set; }
        public Nullable<decimal> BudgetUSD { get; set; }
        public Nullable<decimal> BudgetPCT { get; set; }
        public Nullable<decimal> MeritPCT { get; set; }
        public Nullable<decimal> MeritAmtLocal { get; set; }
        public Nullable<decimal> MeritAmtUSD { get; set; }
        public Nullable<decimal> PromotionPct { get; set; }
        public Nullable<decimal> PromotionAmtLocal { get; set; }
        public Nullable<decimal> PromotionAmtUSD { get; set; }
        public Nullable<decimal> AdjustmentPct { get; set; }
        public Nullable<decimal> AdjustmentAmtLocal { get; set; }
        public Nullable<decimal> AdjustmentAmtUSD { get; set; }
        public Nullable<decimal> NewSalaryLocal { get; set; }
        public Nullable<decimal> NewSalaryUSD { get; set; }
        public Nullable<decimal> NewCompaRatio { get; set; }        
        //public Nullable<decimal> TotalNewCompAmtLocal { get; set; }
        //public Nullable<decimal> TotalNewCompAmtUSD { get; set; }        
        public Nullable<decimal> LumpSumPct { get; set; }
        public Nullable<decimal> LumpSumAmtLocal { get; set; }
        public Nullable<decimal> LumpSumAmtUSD { get; set; }
        public Nullable<decimal> MktMinLocal { get; set; }
        public Nullable<decimal> MktMinUSD { get; set; }
        public Nullable<decimal> MktMidLocal { get; set; }
        public Nullable<decimal> MktMidUSD { get; set; }
        public Nullable<decimal> MktMaxLocal { get; set; }
        public Nullable<decimal> MktMaxUSD { get; set; }        
        public Nullable<decimal> NewMktMinLocal { get; set; }
        public Nullable<decimal> NewMktMinUSD { get; set; }
        public Nullable<decimal> NewMktMidLocal { get; set; }
        public Nullable<decimal> NewMktMidUSD { get; set; }
        public Nullable<decimal> NewMktMaxLocal { get; set; }
        public Nullable<decimal> NewMktMaxUSD { get; set; }                                    
        public string SupervisorID { get; set; }
        public string SupervisorName { get; set; }
        public string CurrencyCodeLocal { get; set; }
        public string CurrencyCodeUSD { get; set; }
        public string CultureCode { get; set; }
        public Nullable<decimal> MeritExchangeRate { get; set; }        
        public Nullable<int> MeritPerformanceRatingNum { get; set; }        
        public string BusinessUnit { get; set; }
        public string PayGroup { get; set; }
        public string EntityCode { get; set; }        
        public string RatingAndGuideline { get; set; }                        
        public Nullable<int> PromotionCommentNum { get; set; }
        public string Comments { get; set; }
        public string PromotionComment { get; set; }
        public string AdjustmentComments { get; set; }        
        public bool IsMeritEdited { get; set; }        
        public bool IsPromotionEdited { get; set; }
        public bool IsAdjustmentEdited { get; set; }
        public bool IsBonusEdited { get; set; }
        public bool IsSubmit { get; set; }
        public bool IsApprove { get; set; }        
        public bool IsLocked { get; set; }
        public bool IsChecked { get; set; }
        public bool IsTopLevelManager { get; set; }
        public string FlagColor { get; set; }
        public string FlagTooltip { get; set; }
        public bool isApprovalLevel { get; set; }            
        public bool SubmitIsChecked { get; set; }
        public bool ApprovalIsChecked { get; set; }
        public int TotalCommentsCount { get; set; }
        public int PromotionCommentsCount { get; set; }        
        public bool IsMktMinHigh { get; set; }
        public bool IsMeritEligible { get; set; }        
        public bool IsAdjustmentEligible { get; set; }
        public bool IsPromotionEligible { get; set; }
        public bool IsSuperAdmin { get; set; }
        public int CompCommentReadCount { get; set; }
        public int WorkflowCommentReadCount { get; set; }
        public string EmployeeClass { get; set; }
        public string EmployeeStatus { get; set; }
        public string FLSAStatus { get; set; }
        public string WorkFlowStatus { get; set; }
        public int IsFirstLevelManager { get; set; }
        public int submitCount { get; set; }
        public int approvalCount { get; set; }
        public int reopenCount { get; set; }

        public string MoreInfo1 { get; set; }
        public string MoreInfo2 { get; set; }
        public string MoreInfo3 { get; set; }
        public string MoreInfo4 { get; set; }
        public string MoreInfo5 { get; set; }
        public string WorkLocation { get; set; }
        public string MeritProrationDate { get; set; }
        public string MeritIncreaseGuideline { get; set; }
        public string PromoteTo { get; set; }
        public decimal? CurrentAnnualizedSalaryLocal { get; set; }
        public decimal? CurrentAnnualizedSalaryUSD { get; set; }
        public decimal? CurrentHourlyRateLocal { get; set; }
        public decimal? CurrentHourlyRateUSD { get; set; }
        public decimal? NewHourlyRateLocal { get; set; }
        public decimal? NewHourlyRateUSD { get; set; }
        public decimal? TCCLocal { get; set; }
        public decimal? TCCUSD { get; set; }
        public decimal?  TotalWorkHrs { get; set; }

        public string WorkFlow { get; set; }

        public decimal? FTE { get; set; }
        public int TotalReadComment
        {
            get
            {
                return CompCommentReadCount + WorkflowCommentReadCount;
            }
        }

        public int TotalUnReadComment
        {
            get
            {
                return TotalCommentsCount - TotalReadComment;
            }
        }

        public Nullable<decimal> BonusTargetPct { get; set; }
        public Nullable<decimal> BonusTargetAmt { get; set; }

        public Nullable<decimal> IndividualPortion { get; set; }
        public Nullable<decimal> GlobalPortion { get; set; }


        public Nullable<decimal> BonusPct { get; set; }
        public Nullable<decimal> BonusAmt { get; set; }

        public Nullable<decimal> Payout { get; set; }
        public bool IsBonusEligible { get; set; }
        


    }
}
