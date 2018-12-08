using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laserbeam.BusinessObject.Common.AnalyticsBusinessObjects
{
    public class ChartData
    {
        public List<ProRationChart> ProRationChart { get; set; }
        public List<AverageCostChartData> AverageCostChartData { get; set; }
        public List<IncreaseChart> IncreaseChart { get; set; }
        public List<IncreaseEmpCount> IncreaseEmpCount { get; set; }
        public List<PopulationCount> PopulationCount { get; set; }
        public List<OutlierChart> OutlierChart { get; set; }
        public List<CompRevenueChart> CompRevenueChart { get; set; }
        public List<ExchangeCurrencies> ExchangeCurrencies { get; set; }
        public CompensationTypeConfiguration CompensationTypeConfiguration { get; set; }
        public string ProRationEECount { get; set; }
        public string ProRationTotalIncrease { get; set; }
        public string ProRationTotalSalary { get; set; }
        public string AvgCostTotalEligibleEEs { get; set; }
        public string AvgCostTotalCompSalary { get; set; }
        public string AvgCostPerEEcost { get; set; }
        public string ChnSalaryCurrent { get; set; }
        public string ChnSalaryNew { get; set; }
        public string Diffence { get; set; }
        public int Merit { get; set; }
        public int Promotion { get; set; }
        public int LumpSum { get; set; }
        public int Adjustment { get; set; }
        public string CRNewSalary { get; set; }
        public string CRMarketMid { get; set; }
        public decimal CRratio { get; set; }
        public int Active { get; set; }
        public int Termed { get; set; }
        public int Total { get; set; }
        public int OutliersCount { get; set; }
        public string OutlierTotalSalary { get; set; }
        public string OutlierTotalIncrease { get; set; }
        public string TotalRevenue { get; set; }
        public decimal RevenuePct { get; set; }
        public string MeritAmt { get; set; }
        public string MeritPct { get; set; }
        public string LumpSumAmt { get; set; }
        public string LumpSumPct { get; set; }
        public string PromotionAmt { get; set; }
        public string PromotionAmtPct { get; set; }
        public string AdjustmentAmt { get; set; }
        public string AdjustmentPct { get; set; }
        public int EmpCount { get; set; }
        public string TotalIncrease { get; set; }
        public decimal UtilizationPct { get; set; }
        public int MgrCount { get; set; }
        public string MgrTotalIncrease { get; set; }
        public decimal MgrUtilizationPct { get; set; }
        public string TargetAmt { get; set; }
        public string PayoutAmt { get; set; }
        public decimal TargetRatio { get; set; }
        public int TotalCount { get; set; }
        public int EligibleCount { get; set; }
        public int IncrBonusCount { get; set; }
        public string ReceivedPct { get; set; }
        public string TotalCash { get; set; }
        public string TotalBonusAmt { get; set; }
        public string BonusCompensationRatio { get; set; }
        public int TargetAmtValue { get; set; }

    }
}
