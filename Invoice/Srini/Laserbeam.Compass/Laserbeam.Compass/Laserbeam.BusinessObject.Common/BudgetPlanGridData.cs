using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laserbeam.BusinessObject.Common
{
    public class BudgetPlanGridData
    {
        public string ManagerName { get; set; }
        public string Division { get; set; }
        public int ManagerNum { get; set; }
        public int DivisionNum { get; set; }
        public decimal ProratedBudget { get; set; }
        public decimal Budget { get; set; }
        public decimal BudgetPercent { get; set; }
        public decimal Spent { get; set; }
        public decimal MeritSpent { get; set; }
        public decimal PromotionSpent { get; set; }
        public decimal LumpSumSpent { get; set; }
        public decimal AdjustmentSpent { get; set; }
        public decimal Balance { get; set; }
        public Int64 RowId { get; set; }
        public int SubmittedCount { get; set; }
        public int FinalApprovedCount { get; set; }
        public bool IsManagerLink { get; set; }
        public string ManagerLineage { get; set; }
        public string CountryCode { get; set; }
        public string Country { get; set; }
        public string BaseCurrency { get; set; }
        public decimal BudgetPct { get; set; }
        public decimal BaseCurrentSalary { get; set; }
        public Int16 CountryNum { get; set; }
        public int EmployeeCount { get; set; }
        public bool ProrationVisible { get; set; }
        public decimal ProratedBudgetPct { get; set; }      
        public decimal AdjustedBudget { get; set; }
        public decimal AdjustedBudgetPct { get; set; }
        public decimal AdjustedBudgetRoundedPct { get; set; }
        public string ProrationStartDate { get; set; }
        public string ProrationEndDate { get; set; }
        public string StartDate { get { return ProrationStartDate != "" ? ProrationStartDate : " "; } }
        public string EndDate { get { return ProrationEndDate != "" ? ProrationEndDate : " "; } }
        //public string StartDate { get { return ProrationStartDate != "" ? Convert.ToDateTime(ProrationStartDate).ToShortDateString() : " "; } }
        //public string EndDate { get { return ProrationEndDate != "" ? Convert.ToDateTime(ProrationEndDate).ToShortDateString() : " "; } }
        public string BaseCurrencyCode { get; set; }
        public string SelectedCurrencyCode { get; set; }
        public decimal SelectedExchangeRate { get; set; }
        public decimal AverageBudgetPct { get; set; }
        public decimal TotalSalary { get; set; }
        public decimal TotalAdjustedBudget { get; set; }
        public decimal TotalProratedBudget { get; set; }
        public decimal TotalBudget { get; set; }
        public decimal TotalSpent { get; set; }
        public int TotalEmployeeCount { get; set; }
        public decimal TotalMeritSpent { get; set; }
        public decimal TotalLumpSumSpent { get; set; }
        public decimal TotalAdjustmentSpent { get; set; }
        public decimal TotalPromotionSpent { get; set; }
        public decimal BudgetPctValue { get; set; }



    }
}
