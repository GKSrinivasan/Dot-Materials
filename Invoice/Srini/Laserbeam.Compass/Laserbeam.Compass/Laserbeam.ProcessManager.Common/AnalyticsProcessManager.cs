using Laserbeam.BusinessObject.Common;
using Laserbeam.BusinessObject.Common.AnalyticsBusinessObjects;
using Laserbeam.DataManager.Interfaces.Common;
using Laserbeam.ProcessManager.Interfaces.Common;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Laserbeam.ProcessManager.Common
{
    public class AnalyticsProcessManager : IAnalyticsProcessManager
    {
        #region Fields
        IAnalyticsRepository m_analyticsRepository;
        #endregion

        #region Constructors
        public AnalyticsProcessManager(IAnalyticsRepository analyticsRepository)
        {
            m_analyticsRepository = analyticsRepository;
        }
        #endregion
        #region Public Methods
        public bool isBonusEnable()
        {
            return m_analyticsRepository.isBonusEnable();
        }
        public async Task<IEnumerable<ManagerTree>> GetAnalyticsManagerTree(int managerNum, int loggedInEmployeeNum, int loggedInUserNum, int userNum)
        {
            return await m_analyticsRepository.GetAnalyticsManagerTree(managerNum, loggedInEmployeeNum, loggedInUserNum, userNum);
        }
        public async Task<ProRationChart> GetProrationChartData(int loggedInEmpNum, int employeeNum, bool isRollup, bool isSelectedRollup, int loggedInUserNum, int selectedCurrencyNum)
        {
            return await m_analyticsRepository.GetProrationChartData(loggedInEmpNum, employeeNum, isRollup, isSelectedRollup, loggedInUserNum,selectedCurrencyNum);
        }
        public async Task<AverageCostChartData> GetAvgCostChartData(int loggedInEmpNum, int employeeNum, bool isRollup, bool isSelectedRollup, int loggedInUserNum, int selectedCurrencyNum)
        {
            return await m_analyticsRepository.GetAvgCostChartData(loggedInEmpNum, employeeNum, isRollup, isSelectedRollup, loggedInUserNum,selectedCurrencyNum);
        }
        
  public async Task<IncreaseChart> GetChgSalaryChartData(int loggedInEmpNum, int employeeNum, bool isRollup, bool isSelectedRollup, int loggedInUserNum, int selectedCurrencyNum)
        {
            return await m_analyticsRepository.GetChgSalaryChartData(loggedInEmpNum, employeeNum, isRollup, isSelectedRollup, loggedInUserNum,selectedCurrencyNum);
        }
        public async  Task<IncreaseEmpCount> GetIncreaseCountChartData(int loggedInEmpNum, int employeeNum, bool isRollup, bool isSelectedRollup, int loggedInUserNum)
        {
            return await m_analyticsRepository.GetIncreaseCountChartData(loggedInEmpNum, employeeNum, isRollup, isSelectedRollup, loggedInUserNum);
        }
        public async Task<CompRatio> GetCompRatioChartData(int loggedInEmpNum, int employeeNum, bool isRollup, bool isSelectedRollup, int loggedInUserNum, int selectedCurrencyNum)
        {
            return await m_analyticsRepository.GetCompRatioChartData(loggedInEmpNum, employeeNum, isRollup, isSelectedRollup, loggedInUserNum,selectedCurrencyNum);
        }
        public async Task<PopulationCount> GetPopulationCountChartData(int loggedInEmpNum, int employeeNum, bool isRollup, bool isSelectedRollup, int loggedInUserNum)
        {
            return await m_analyticsRepository.GetPopulationCountChartData(loggedInEmpNum, employeeNum, isRollup, isSelectedRollup, loggedInUserNum);
        }
        public async Task<OutlierChart> GetOutlierCountChartData(int loggedInEmpNum, int employeeNum, bool isRollup, bool isSelectedRollup, int loggedInUserNum, int selectedCurrencyNum)
        {
            return await m_analyticsRepository.GetOutlierCountChartData(loggedInEmpNum, employeeNum, isRollup, isSelectedRollup, loggedInUserNum,selectedCurrencyNum);
        }
        public async Task<IncreaseDistributionEmp> GetIncraeseDistribututionEmpChartData(int loggedInEmpNum, int employeeNum, bool isRollup, bool isSelectedRollup, int loggedInUserNum)
        {
            return await m_analyticsRepository.GetIncraeseDistribututionEmpChartData(loggedInEmpNum, employeeNum, isRollup, isSelectedRollup, loggedInUserNum);
        }
        public async Task<IncreaseDistributionMgr> GetIncraeseDistribututionMgrChartData(int loggedInEmpNum, int employeeNum, bool isRollup, bool isSelectedRollup, int loggedInUserNum)
        {
            return await m_analyticsRepository.GetIncraeseDistribututionMgrChartData(loggedInEmpNum, employeeNum, isRollup, isSelectedRollup, loggedInUserNum);
        }
       
        public async Task<CompRevenueChart> GetCompRevenueChartData(int loggedInEmpNum, int employeeNum, bool isRollup, bool isSelectedRollup, int loggedInUserNum)
        {
            return await m_analyticsRepository.GetCompRevenueChartData(loggedInEmpNum, employeeNum, isRollup, isSelectedRollup, loggedInUserNum);
        }
        
        public async Task<BonusTargetChart> GetBonusTargetChartData(int loggedInEmpNum, int employeeNum, bool isRollup, bool isSelectedRollup, int loggedInUserNum)
        {
            return await m_analyticsRepository.GetBonusTargetChartData(loggedInEmpNum, employeeNum, isRollup, isSelectedRollup, loggedInUserNum);
        }
        public async Task<BonusEligibilityCount> GetBonusEligibilityChartData(int loggedInEmpNum, int employeeNum, bool isRollup, bool isSelectedRollup, int loggedInUserNum)
        {
            return await m_analyticsRepository.GetBonusEligibilityChartData(loggedInEmpNum, employeeNum, isRollup, isSelectedRollup, loggedInUserNum);
        }
        public async Task<BonusCompRatio> GetBonusCompRatioChartData(int loggedInEmpNum, int employeeNum, bool isRollup, bool isSelectedRollup, int loggedInUserNum)
        {
            return await m_analyticsRepository.GetBonusCompRatioChartData(loggedInEmpNum, employeeNum, isRollup, isSelectedRollup, loggedInUserNum);
        }
       

        public async Task<IEnumerable<MeritIncreasebyRatings>> GetMeritIncreaseByDepartment(int loggedInEmpNum, int employeeNum, string groupBy, bool isRollup, bool isSelectedRollup, int loggedInUserNum, int currencyCodeNum)
        {
            return await m_analyticsRepository.GetMeritIncreaseByDepartment(loggedInEmpNum, employeeNum, groupBy, isRollup, isSelectedRollup, loggedInUserNum, currencyCodeNum);
        }
        public async Task<IEnumerable<MeritIncreasebyRatings>> GetMeritIncreaseByGender(int loggedInEmpNum, int employeeNum, string groupBy, bool isRollup, bool isSelectedRollup, int loggedInUserNum, int currencyCodeNum)
        {
            return await m_analyticsRepository.GetMeritIncreaseByGender(loggedInEmpNum, employeeNum, groupBy, isRollup, isSelectedRollup, loggedInUserNum,currencyCodeNum);
        }
        public async Task<IEnumerable<MeritIncreasebyRatings>> GetMeritIncreasebyGrade(int loggedInEmpNum, int employeeNum, string groupBy, bool isRollup, bool isSelectedRollup, int loggedInUserNum, int currencyCodeNum)
        {
            return await m_analyticsRepository.GetMeritIncreasebyGrade(loggedInEmpNum, employeeNum, groupBy, isRollup, isSelectedRollup, loggedInUserNum,currencyCodeNum);
        }
        public async Task<PayRangeDistribution> GetPayRangeData(int loggedInEmpNum, int employeeNum, bool isRollup, bool isSelectedRollup, int loggedInUserNum)
        {
            return await m_analyticsRepository.GetPayRangeData(loggedInEmpNum, employeeNum, isRollup, isSelectedRollup, loggedInUserNum);
        }
        public async Task<DataTable> IncreaseExport(int loggedInEmpNum, int employeeNum, string groupBy, bool isRollup, bool isSelectedRollup, int loggedInUserNum)
        {
            return await m_analyticsRepository.IncreaseExport(loggedInEmpNum, employeeNum, groupBy, isRollup, isSelectedRollup, loggedInUserNum);
        }
        public async Task<DataTable> PayRangeExport(int loggedInEmpNum, int employeeNum, string groupBy, bool isRollup, bool isSelectedRollup, int loggedInUserNum)
        {
            return await m_analyticsRepository.PayRangeExport(loggedInEmpNum, employeeNum, groupBy, isRollup, isSelectedRollup, loggedInUserNum);
        }
        public async Task<DataTable> CompExport(int loggedInEmpNum, int employeeNum, string groupBy, bool isRollup, bool isSelectedRollup, int loggedInUserNum)
        {
            return await m_analyticsRepository.CompExport(loggedInEmpNum, employeeNum, groupBy, isRollup, isSelectedRollup, loggedInUserNum);
        }
        public async Task<DataTable> ProRateExport(int loggedInEmpNum, int employeeNum, string groupBy, bool isRollup, bool isSelectedRollup, int loggedInUserNum)
        {
            return await m_analyticsRepository.ProRateExport(loggedInEmpNum, employeeNum, groupBy, isRollup, isSelectedRollup, loggedInUserNum);
        }
        
        public async Task<DataTable> AvgCostExport(int loggedInEmpNum, int employeeNum, string groupBy, bool isRollup, bool isSelectedRollup, int loggedInUserNum)
        {
            return await m_analyticsRepository.AvgCostExport(loggedInEmpNum, employeeNum, groupBy, isRollup, isSelectedRollup, loggedInUserNum);
        }
        public async Task<DataTable> IncreaseChartExport(int loggedInEmpNum, int employeeNum, string groupBy, bool isRollup, bool isSelectedRollup, int loggedInUserNum)
        {
            return await m_analyticsRepository.IncreaseChartExport(loggedInEmpNum, employeeNum, groupBy, isRollup, isSelectedRollup, loggedInUserNum);
        }
        
        public async Task<DataTable> IncreaseEmpCountExport(int loggedInEmpNum, int employeeNum, string groupBy, bool isRollup, bool isSelectedRollup, int loggedInUserNum)
        {
            return await m_analyticsRepository.IncreaseEmpCountExport(loggedInEmpNum, employeeNum, groupBy, isRollup, isSelectedRollup, loggedInUserNum);
        }
        public async Task<DataTable> MarketDataExport(int loggedInEmpNum, int employeeNum, string groupBy, bool isRollup, bool isSelectedRollup, int loggedInUserNum)
        {
            return await m_analyticsRepository.MarketDataExport(loggedInEmpNum, employeeNum, groupBy, isRollup, isSelectedRollup, loggedInUserNum);
        }
        public async Task<DataTable> PopulationDataExport(int loggedInEmpNum, int employeeNum, string groupBy, bool isRollup, bool isSelectedRollup, int loggedInUserNum)
        {
            return await m_analyticsRepository.PopulationDataExport(loggedInEmpNum, employeeNum, groupBy, isRollup, isSelectedRollup, loggedInUserNum);
        }
        public async Task<DataTable> OutlierExport(int loggedInEmpNum, int employeeNum, string groupBy, bool isRollup, bool isSelectedRollup, int loggedInUserNum)
        {
            return await m_analyticsRepository.OutlierExport(loggedInEmpNum, employeeNum, groupBy, isRollup, isSelectedRollup, loggedInUserNum);
        }
        #endregion

    }
}
