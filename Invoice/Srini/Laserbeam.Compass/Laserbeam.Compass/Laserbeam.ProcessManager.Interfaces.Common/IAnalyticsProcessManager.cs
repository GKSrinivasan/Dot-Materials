using Laserbeam.BusinessObject.Common;
using Laserbeam.BusinessObject.Common.AnalyticsBusinessObjects;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Laserbeam.ProcessManager.Interfaces.Common
{
    public interface IAnalyticsProcessManager
    {
        Task<IEnumerable<ManagerTree>> GetAnalyticsManagerTree(int managerNum, int loggedInEmployeeNum, int loggedInUserNum, int userNum);
        Task<IEnumerable<MeritIncreasebyRatings>> GetMeritIncreaseByDepartment(int loggedInEmpNum, int employeeNum, string groupBy, bool isRollup, bool isSelectedRollup, int loggedInUserNum,int currencyCodeNum);
        Task<IEnumerable<MeritIncreasebyRatings>> GetMeritIncreaseByGender(int loggedInEmpNum, int employeeNum, string groupBy, bool isRollup, bool isSelectedRollup, int loggedInUserNum, int currencyCodeNum);
        Task<IEnumerable<MeritIncreasebyRatings>> GetMeritIncreasebyGrade(int loggedInEmpNum, int employeeNum, string groupBy, bool isRollup, bool isSelectedRollup, int loggedInUserNum, int currencyCodeNum);
        Task<PayRangeDistribution> GetPayRangeData(int loggedInEmpNum, int employeeNum, bool isRollup, bool isSelectedRollup, int loggedInUserNum);
        Task<DataTable> IncreaseExport(int loggedInEmpNum, int employeeNum,string groupBy, bool isRollup, bool isSelectedRollup, int loggedInUserNum);
        Task<DataTable> PayRangeExport(int loggedInEmpNum, int employeeNum, string groupBy, bool isRollup, bool isSelectedRollup, int loggedInUserNum);
        Task<DataTable> CompExport(int loggedInEmpNum, int employeeNum, string groupBy, bool isRollup, bool isSelectedRollup, int loggedInUserNum);
        Task<DataTable> ProRateExport(int loggedInEmpNum, int employeeNum, string groupBy, bool isRollup, bool isSelectedRollup, int loggedInUserNum);
        Task<DataTable> AvgCostExport(int loggedInEmpNum, int employeeNum, string groupBy, bool isRollup, bool isSelectedRollup, int loggedInUserNum);
        Task<DataTable> IncreaseChartExport(int loggedInEmpNum, int employeeNum, string groupBy, bool isRollup, bool isSelectedRollup, int loggedInUserNum);
        Task<DataTable> IncreaseEmpCountExport(int loggedInEmpNum, int employeeNum, string groupBy, bool isRollup, bool isSelectedRollup, int loggedInUserNum);
        Task<DataTable> MarketDataExport(int loggedInEmpNum, int employeeNum, string groupBy, bool isRollup, bool isSelectedRollup, int loggedInUserNum);
        Task<DataTable> PopulationDataExport(int loggedInEmpNum, int employeeNum, string groupBy, bool isRollup, bool isSelectedRollup, int loggedInUserNum);
        Task<DataTable> OutlierExport(int loggedInEmpNum, int employeeNum, string groupBy, bool isRollup, bool isSelectedRollup, int loggedInUserNum);
        Task<ProRationChart> GetProrationChartData(int loggedInEmpNum, int employeeNum, bool isRollup, bool isSelectedRollup, int loggedInUserNum, int selectedCurrencyNum);
        Task<AverageCostChartData> GetAvgCostChartData(int loggedInEmpNum, int employeeNum, bool isRollup, bool isSelectedRollup, int loggedInUserNum, int selectedCurrencyNum);
        Task<IncreaseChart> GetChgSalaryChartData(int loggedInEmpNum, int employeeNum, bool isRollup, bool isSelectedRollup, int loggedInUserNum, int selectedCurrencyNum);
        Task<IncreaseEmpCount> GetIncreaseCountChartData(int loggedInEmpNum, int employeeNum, bool isRollup, bool isSelectedRollup, int loggedInUserNum);
        Task<CompRatio> GetCompRatioChartData(int loggedInEmpNum, int employeeNum, bool isRollup, bool isSelectedRollup, int loggedInUserNum, int selectedCurrencyNum);
        Task<PopulationCount> GetPopulationCountChartData(int loggedInEmpNum, int employeeNum, bool isRollup, bool isSelectedRollup, int loggedInUserNum);
        Task<OutlierChart> GetOutlierCountChartData(int loggedInEmpNum, int employeeNum, bool isRollup, bool isSelectedRollup, int loggedInUserNum, int selectedCurrencyNum);
        Task<IncreaseDistributionEmp> GetIncraeseDistribututionEmpChartData(int loggedInEmpNum, int employeeNum, bool isRollup, bool isSelectedRollup, int loggedInUserNum);
        Task<IncreaseDistributionMgr> GetIncraeseDistribututionMgrChartData(int loggedInEmpNum, int employeeNum, bool isRollup, bool isSelectedRollup, int loggedInUserNum);
        Task<CompRevenueChart> GetCompRevenueChartData(int loggedInEmpNum, int employeeNum, bool isRollup, bool isSelectedRollup, int loggedInUserNum);
        Task<BonusTargetChart> GetBonusTargetChartData(int loggedInEmpNum, int employeeNum, bool isRollup, bool isSelectedRollup, int loggedInUserNum);
        Task<BonusEligibilityCount> GetBonusEligibilityChartData(int loggedInEmpNum, int employeeNum, bool isRollup, bool isSelectedRollup, int loggedInUserNum);
        Task<BonusCompRatio> GetBonusCompRatioChartData(int loggedInEmpNum, int employeeNum, bool isRollup, bool isSelectedRollup, int loggedInUserNum);
        bool isBonusEnable();

    }
}
