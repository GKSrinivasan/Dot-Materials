using Laserbeam.BusinessObject.Common;
using Laserbeam.BusinessObject.Common.AnalyticsBusinessObjects;
using Laserbeam.Constant.HR;
using Laserbeam.DataManager.Interfaces.Common;
using Laserbeam.DataManager.Interfaces.Core;
using Laserbeam.EntityManager.Common;
using Laserbeam.Libraries.Core.Interfaces.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Laserbeam.DataManager.Common
{
    public class AnalyticsRepository: IAnalyticsRepository
    {
        #region Fields
        private IBaseRepository m_baseRepository;
        private ITenantCacheProvider m_tenantCacheProvider;

        #endregion

        #region Constructors
        public AnalyticsRepository(IBaseRepository baseRepository, ITenantCacheProvider tenantCacheProvider)
        {
            m_baseRepository = baseRepository;
            m_tenantCacheProvider = tenantCacheProvider;
        }
        #endregion
        #region Public Methods Implementation
        public async Task<IEnumerable<ManagerTree>> GetAnalyticsManagerTree(int managerNum, int loggedInEmployeeNum, int loggedInUserNum, int userNum)
        {
            string moduleKey = ModuleConstants.Analytics;
            string pageTypeValue = "Analytics";
            SqlParameter[] sqlParameter = {   new SqlParameter("@ManagerNum", managerNum),
                                              new SqlParameter("@LoggedInEmployeeNum",loggedInEmployeeNum),
                                              new SqlParameter("@UserNum",userNum),
                                              new SqlParameter("@PageType",pageTypeValue),
                                              new SqlParameter("@ModuleKey",moduleKey),
                                              new SqlParameter("@loggedInUserNum",loggedInUserNum)
                                          };
            var managerTree = await m_baseRepository.GetData<ManagerTree>("Common.USP_GET_ManagerTree @ManagerNum,@LoggedInEmployeeNum,@UserNum,@PageType,@ModuleKey,@loggedInUserNum", sqlParameter);            
            return managerTree;
        }
        public async Task<ProRationChart> GetProrationChartData(int loggedInEmpNum, int employeeNum, bool isRollup, bool isSelectedRollup, int loggedInUserNum, int selectedCurrencyNum)
        {
            SqlParameter[] sqlParameter = {   new SqlParameter("@loggedInEmpNum", loggedInEmpNum),
                                              new SqlParameter("@managerNum",(int)employeeNum),
                                              new SqlParameter("@rollup", (bool)isRollup),
                                              new SqlParameter("@isSelectedRollup", (bool)isSelectedRollup),
                                               new SqlParameter("@loggedInUserNum",(int)loggedInUserNum),
                                               new SqlParameter("@selectedCurrencyNum",selectedCurrencyNum)
            };
            var prorationData = await m_baseRepository.GetData<ProRationChart>("Talent.USP_ALS_GET_ProRationChartData @loggedInEmpNum,@managerNum,@rollup,@isSelectedRollup,@loggedInUserNum,@selectedCurrencyNum", sqlParameter);
            return prorationData.FirstOrDefault();
        }
        public async Task<AverageCostChartData> GetAvgCostChartData(int loggedInEmpNum, int employeeNum, bool isRollup, bool isSelectedRollup, int loggedInUserNum, int selectedCurrencyNum)
        {
            SqlParameter[] sqlParameter = {   new SqlParameter("@loggedInEmpNum", loggedInEmpNum),
                                              new SqlParameter("@managerNum",(int)employeeNum),
                                              new SqlParameter("@rollup", (bool)isRollup),
                                              new SqlParameter("@isSelectedRollup", (bool)isSelectedRollup),
                                               new SqlParameter("@loggedInUserNum",(int)loggedInUserNum),
                                               new SqlParameter("@selectedCurrencyNum",selectedCurrencyNum),
            };
            var avgCostChartData = await m_baseRepository.GetData<AverageCostChartData>("Talent.USP_ALS_GET_AverageCostChartData @loggedInEmpNum,@managerNum,@rollup,@isSelectedRollup,@loggedInUserNum,@selectedCurrencyNum", sqlParameter);
            return avgCostChartData.FirstOrDefault();
        }
        public async Task<IncreaseChart> GetChgSalaryChartData(int loggedInEmpNum, int employeeNum, bool isRollup, bool isSelectedRollup, int loggedInUserNum, int selectedCurrencyNum)
        {
            SqlParameter[] sqlParameter = {   new SqlParameter("@loggedInEmpNum", loggedInEmpNum),
                                              new SqlParameter("@managerNum",(int)employeeNum),
                                              new SqlParameter("@rollup", (bool)isRollup),
                                              new SqlParameter("@isSelectedRollup", (bool)isSelectedRollup),
                                               new SqlParameter("@loggedInUserNum",(int)loggedInUserNum),
                                               new SqlParameter("@selectedCurrencyNum",selectedCurrencyNum),
            };
            var increasetChartData = await m_baseRepository.GetData<IncreaseChart>("Talent.USP_ALS_GET_ChangeSalaryChartData @loggedInEmpNum,@managerNum,@rollup,@isSelectedRollup,@loggedInUserNum,@selectedCurrencyNum", sqlParameter);
            return increasetChartData.FirstOrDefault();
        }
        public async Task<IncreaseEmpCount> GetIncreaseCountChartData(int loggedInEmpNum, int employeeNum, bool isRollup, bool isSelectedRollup, int loggedInUserNum)
        {
            SqlParameter[] sqlParameter = {   new SqlParameter("@loggedInEmpNum", loggedInEmpNum),
                                              new SqlParameter("@managerNum",(int)employeeNum),
                                              new SqlParameter("@rollup", (bool)isRollup),
                                              new SqlParameter("@isSelectedRollup", (bool)isSelectedRollup),
                                               new SqlParameter("@loggedInUserNum",(int)loggedInUserNum)
            };
            var increasetEmpCountData = await m_baseRepository.GetData<IncreaseEmpCount>("Talent.USP_ALS_GET_IncreaseCountChartData @loggedInEmpNum,@managerNum,@rollup,@isSelectedRollup,@loggedInUserNum", sqlParameter);
            return increasetEmpCountData.FirstOrDefault();
        }
        public async Task<CompRatio> GetCompRatioChartData(int loggedInEmpNum, int employeeNum, bool isRollup, bool isSelectedRollup, int loggedInUserNum, int selectedCurrencyNum)
        {
            SqlParameter[] sqlParameter = {   new SqlParameter("@loggedInEmpNum", loggedInEmpNum),
                                              new SqlParameter("@managerNum",(int)employeeNum),
                                              new SqlParameter("@rollup", (bool)isRollup),
                                              new SqlParameter("@isSelectedRollup", (bool)isSelectedRollup),
                                               new SqlParameter("@loggedInUserNum",(int)loggedInUserNum),
                                               new SqlParameter("@selectedCurrencyNum",selectedCurrencyNum),
            };
            var compRatioEmpCountData = await m_baseRepository.GetData<CompRatio>("Talent.USP_ALS_GET_MarketMidChartData @loggedInEmpNum,@managerNum,@rollup,@isSelectedRollup,@loggedInUserNum,@selectedCurrencyNum", sqlParameter);
            return compRatioEmpCountData.FirstOrDefault();
        }
        public async Task<PopulationCount> GetPopulationCountChartData(int loggedInEmpNum, int employeeNum, bool isRollup, bool isSelectedRollup, int loggedInUserNum)
        {
            SqlParameter[] sqlParameter = {   new SqlParameter("@loggedInEmpNum", loggedInEmpNum),
                                              new SqlParameter("@managerNum",(int)employeeNum),
                                              new SqlParameter("@rollup", (bool)isRollup),
                                              new SqlParameter("@isSelectedRollup", (bool)isSelectedRollup),
                                               new SqlParameter("@loggedInUserNum",(int)loggedInUserNum)
            };
            var populationEmpCountData = await m_baseRepository.GetData<PopulationCount>("Talent.USP_ALS_GET_PopulationCount @loggedInEmpNum,@managerNum,@rollup,@isSelectedRollup,@loggedInUserNum", sqlParameter);
            return populationEmpCountData.FirstOrDefault();
        }
        public async Task<OutlierChart> GetOutlierCountChartData(int loggedInEmpNum, int employeeNum, bool isRollup, bool isSelectedRollup, int loggedInUserNum, int selectedCurrencyNum)
        {
            SqlParameter[] sqlParameter = {   new SqlParameter("@loggedInEmpNum", loggedInEmpNum),
                                              new SqlParameter("@managerNum",(int)employeeNum),
                                              new SqlParameter("@rollup", (bool)isRollup),
                                              new SqlParameter("@isSelectedRollup", (bool)isSelectedRollup),
                                               new SqlParameter("@loggedInUserNum",(int)loggedInUserNum),
                                               new SqlParameter("@selectedCurrencyNum",selectedCurrencyNum)
            };
            var outlierChartEmpCountData = await m_baseRepository.GetData<OutlierChart>("Talent.USP_ALS_GET_OutlierChartData @loggedInEmpNum,@managerNum,@rollup,@isSelectedRollup,@loggedInUserNum,@selectedCurrencyNum", sqlParameter);
            return outlierChartEmpCountData.FirstOrDefault();
        }
        public async Task<IncreaseDistributionEmp> GetIncraeseDistribututionEmpChartData(int loggedInEmpNum, int employeeNum, bool isRollup, bool isSelectedRollup, int loggedInUserNum)
        {
            SqlParameter[] sqlParameter = {   new SqlParameter("@loggedInEmpNum", loggedInEmpNum),
                                              new SqlParameter("@managerNum",(int)employeeNum),
                                              new SqlParameter("@rollup", (bool)isRollup),
                                              new SqlParameter("@isSelectedRollup", (bool)isSelectedRollup),
                                               new SqlParameter("@loggedInUserNum",(int)loggedInUserNum)
            };
            var IncraeseDistribututionEmpChartData = await m_baseRepository.GetData<IncreaseDistributionEmp>("Talent.USP_ALS_GET_IncreaseDistributionEmpChartData @loggedInEmpNum,@managerNum,@rollup,@isSelectedRollup,@loggedInUserNum", sqlParameter);
            return IncraeseDistribututionEmpChartData.FirstOrDefault();
        }
        public async Task<IncreaseDistributionMgr> GetIncraeseDistribututionMgrChartData(int loggedInEmpNum, int employeeNum, bool isRollup, bool isSelectedRollup, int loggedInUserNum)
        {
            SqlParameter[] sqlParameter = {   new SqlParameter("@loggedInEmpNum", loggedInEmpNum),
                                              new SqlParameter("@managerNum",(int)employeeNum),
                                              new SqlParameter("@rollup", (bool)isRollup),
                                              new SqlParameter("@isSelectedRollup", (bool)isSelectedRollup),
                                               new SqlParameter("@loggedInUserNum",(int)loggedInUserNum)
            };
            var IncraeseDistribututionMgrChartData = await m_baseRepository.GetData<IncreaseDistributionMgr>("Talent.USP_ALS_GET_IncreaseDistributionMgrChartData @loggedInEmpNum,@managerNum,@rollup,@isSelectedRollup,@loggedInUserNum", sqlParameter);
            return IncraeseDistribututionMgrChartData.FirstOrDefault();
        }
        public bool isBonusEnable()
        {
            return m_tenantCacheProvider.GetBusinessSetting().FeatureConfiguration.Bonus;
         }
        public async Task<CompRevenueChart> GetCompRevenueChartData(int loggedInEmpNum, int employeeNum, bool isRollup, bool isSelectedRollup, int loggedInUserNum)
        {
            SqlParameter[] sqlParameter = {   new SqlParameter("@loggedInEmpNum", loggedInEmpNum),
                                              new SqlParameter("@managerNum",(int)employeeNum),
                                              new SqlParameter("@rollup", (bool)isRollup),
                                              new SqlParameter("@isSelectedRollup", (bool)isSelectedRollup),
                                               new SqlParameter("@loggedInUserNum",(int)loggedInUserNum)
            };
            var compRevenueChartData = await m_baseRepository.GetData<CompRevenueChart>("Talent.USP_ALS_GET_CompChartData @loggedInEmpNum,@managerNum,@rollup,@isSelectedRollup,@loggedInUserNum", sqlParameter);
            return compRevenueChartData.FirstOrDefault();
        }
        public async Task<BonusTargetChart> GetBonusTargetChartData(int loggedInEmpNum, int employeeNum, bool isRollup, bool isSelectedRollup, int loggedInUserNum)
        {
            SqlParameter[] sqlParameter = {   new SqlParameter("@loggedInEmpNum", loggedInEmpNum),
                                              new SqlParameter("@managerNum",(int)employeeNum),
                                              new SqlParameter("@rollup", (bool)isRollup),
                                              new SqlParameter("@isSelectedRollup", (bool)isSelectedRollup),
                                               new SqlParameter("@loggedInUserNum",(int)loggedInUserNum)
            };
            var BonusTargetChartData = await m_baseRepository.GetData<BonusTargetChart>("Talent.USP_ALS_GET_BounsTargetChartData @loggedInEmpNum,@managerNum,@rollup,@isSelectedRollup,@loggedInUserNum", sqlParameter);
            return BonusTargetChartData.FirstOrDefault();
        }
        public async Task<BonusEligibilityCount> GetBonusEligibilityChartData(int loggedInEmpNum, int employeeNum, bool isRollup, bool isSelectedRollup, int loggedInUserNum)
        {
            SqlParameter[] sqlParameter = {   new SqlParameter("@loggedInEmpNum", loggedInEmpNum),
                                              new SqlParameter("@managerNum",(int)employeeNum),
                                              new SqlParameter("@rollup", (bool)isRollup),
                                              new SqlParameter("@isSelectedRollup", (bool)isSelectedRollup),
                                               new SqlParameter("@loggedInUserNum",(int)loggedInUserNum)
            };
            var BonusEligibility = await m_baseRepository.GetData<BonusEligibilityCount>("Talent.USP_ALS_GET_BounsEligiblityCountChartData @loggedInEmpNum,@managerNum,@rollup,@isSelectedRollup,@loggedInUserNum", sqlParameter);
            return BonusEligibility.FirstOrDefault();
        }
        public async Task<BonusCompRatio> GetBonusCompRatioChartData(int loggedInEmpNum, int employeeNum, bool isRollup, bool isSelectedRollup, int loggedInUserNum)
        {
            SqlParameter[] sqlParameter = {   new SqlParameter("@loggedInEmpNum", loggedInEmpNum),
                                              new SqlParameter("@managerNum",(int)employeeNum),
                                              new SqlParameter("@rollup", (bool)isRollup),
                                              new SqlParameter("@isSelectedRollup", (bool)isSelectedRollup),
                                               new SqlParameter("@loggedInUserNum",(int)loggedInUserNum)
            };
            var BonusCompRatio = await m_baseRepository.GetData<BonusCompRatio>("Talent.USP_ALS_GET_BounsCompRatioChartData @loggedInEmpNum,@managerNum,@rollup,@isSelectedRollup,@loggedInUserNum", sqlParameter);
            return BonusCompRatio.FirstOrDefault();
        }
        

        public async Task<IEnumerable<MeritIncreasebyRatings>> GetMeritIncreaseByDepartment(int loggedInEmpNum, int employeeNum, string groupBy, bool isRollup, bool isSelectedRollup, int loggedInUserNum, int currencyCodeNum)
        {
            string culturalCode = m_baseRepository.GetQuery<Currency>(x=>x.CurrencyCodeNum== currencyCodeNum).Select(x=>x.CultureCode).FirstOrDefault();
            CultureInfo culture = CultureInfo.CreateSpecificCulture(culturalCode);
            List<MeritIncreasebyRatings> result = new List<MeritIncreasebyRatings>();
            SqlParameter[] sqlParameter = {   new SqlParameter("@loggedInEmpNum", loggedInEmpNum),
                                              new SqlParameter("@managerNum",(int)employeeNum),
                                              new SqlParameter("@rollup", (bool)isRollup),
                                              new SqlParameter("@isSelectedRollup", (bool)isSelectedRollup),
                                              new SqlParameter("@groupBy",groupBy),
                                              new SqlParameter("@loggedInUserNum",loggedInUserNum),
                                              new SqlParameter("@selectedCurrencyNum",currencyCodeNum)
                                          };

            var budgetData = await m_baseRepository.GetData<BindChart>("[Talent].[USP_ALS_GET_GetMeritBudgetChartData] @loggedInEmpNum,@managerNum,@rollup,@isSelectedRollup,@groupBy,@loggedInUserNum,@selectedCurrencyNum", sqlParameter);
            result = (
                      from b in budgetData
                      select new MeritIncreasebyRatings
                      {
                          DepartmentName = b.name,
                          CountValue = b.Count > 0 ? b.Count : 0,
                          AverageValue = Math.Round(Convert.ToDecimal(b.value), 2).ToString() + "%",
                          value = b.value,
                          name = b.name,
                          BudgetAmount = (Math.Round(((b.Budget != null) ? Convert.ToDecimal(b.Budget) : 0), 0, MidpointRounding.ToEven)).ToString("C0", culture),
                          SpentAmount = (Math.Round(((b.Spent != null) ? Convert.ToDecimal(b.Spent) : 0), 0, MidpointRounding.ToEven)).ToString("C0", culture),
                          CurrentSalaryAmount = (Math.Round(((b.CurrentSalary != null) ? Convert.ToDecimal(b.CurrentSalary) : 0), 0, MidpointRounding.ToEven)).ToString("C0", culture),
                          NewSalaryAmount = (Math.Round(((b.NewSalary != null) ? Convert.ToDecimal(b.NewSalary) : 0), 0, MidpointRounding.ToEven)).ToString("C0", culture),

                      }).ToList();
            return result;
        }
        public async Task<IEnumerable<MeritIncreasebyRatings>> GetMeritIncreaseByGender(int loggedInEmpNum, int employeeNum, string groupBy, bool isRollup, bool isSelectedRollup, int loggedInUserNum, int currencyCodeNum)
        {
            string culturalCode = m_baseRepository.GetQuery<Currency>(x => x.CurrencyCodeNum == currencyCodeNum).Select(x => x.CultureCode).FirstOrDefault();
            CultureInfo culture = CultureInfo.CreateSpecificCulture(culturalCode);
            List<MeritIncreasebyRatings> result = new List<MeritIncreasebyRatings>();
            SqlParameter[] sqlParameter = {   new SqlParameter("@loggedInEmpNum", loggedInEmpNum),
                                              new SqlParameter("@managerNum",(int)employeeNum),
                                              new SqlParameter("@rollup", (bool)isRollup),
                                              new SqlParameter("@isSelectedRollup", (bool)isSelectedRollup),
                                              new SqlParameter("@groupBy",groupBy),
                                              new SqlParameter("@loggedInUserNum",loggedInUserNum),
                                              new SqlParameter("@selectedCurrencyNum",currencyCodeNum)
                                          };

            var budgetData = await m_baseRepository.GetData<BindChart>("[Talent].[USP_ALS_GET_GetMeritBudgetChartData] @loggedInEmpNum,@managerNum,@rollup,@isSelectedRollup,@groupBy,@loggedInUserNum,@selectedCurrencyNum", sqlParameter);
            result = (
                      from b in budgetData
                      select new MeritIncreasebyRatings
                      {
                          DepartmentName = b.name,
                          CountValue = b.Count > 0 ? b.Count : 0,
                          AverageValue = Math.Round(Convert.ToDecimal(b.value), 2).ToString() + "%",
                          value = b.value,
                          name = b.name,
                          BudgetAmount = (Math.Round(((b.Budget != null) ? Convert.ToDecimal(b.Budget) : 0), 0, MidpointRounding.ToEven)).ToString("C0", culture),
                          SpentAmount = (Math.Round(((b.Spent != null) ? Convert.ToDecimal(b.Spent) : 0), 0, MidpointRounding.ToEven)).ToString("C0", culture),
                          CurrentSalaryAmount = (Math.Round(((b.CurrentSalary != null) ? Convert.ToDecimal(b.CurrentSalary) : 0), 0, MidpointRounding.ToEven)).ToString("C0", culture),
                          NewSalaryAmount = (Math.Round(((b.NewSalary != null) ? Convert.ToDecimal(b.NewSalary) : 0), 0, MidpointRounding.ToEven)).ToString("C0", culture),

                      }).ToList();
            return result;
        }
        public async Task<IEnumerable<MeritIncreasebyRatings>> GetMeritIncreasebyGrade(int loggedInEmpNum, int employeeNum, string groupBy, bool isRollup, bool isSelectedRollup, int loggedInUserNum, int currencyCodeNum)
        {
            string culturalCode = m_baseRepository.GetQuery<Currency>(x => x.CurrencyCodeNum == currencyCodeNum).Select(x => x.CultureCode).FirstOrDefault();
            CultureInfo culture = CultureInfo.CreateSpecificCulture(culturalCode);
            List<MeritIncreasebyRatings> result = new List<MeritIncreasebyRatings>();
            SqlParameter[] sqlParameter = {   new SqlParameter("@loggedInEmpNum", loggedInEmpNum),
                                              new SqlParameter("@managerNum",(int)employeeNum),
                                              new SqlParameter("@rollup", (bool)isRollup),
                                              new SqlParameter("@isSelectedRollup", (bool)isSelectedRollup),
                                              new SqlParameter("@groupBy",groupBy),
                                              new SqlParameter("@loggedInUserNum",loggedInUserNum),
                                              new SqlParameter("@selectedCurrencyNum",currencyCodeNum)
                                          };

            var budgetData = await m_baseRepository.GetData<BindChart>("[Talent].[USP_ALS_GET_GetMeritBudgetChartData] @loggedInEmpNum,@managerNum,@rollup,@isSelectedRollup,@groupBy,@loggedInUserNum,@selectedCurrencyNum", sqlParameter);
            result = (
                      from b in budgetData
                      select new MeritIncreasebyRatings
                      {
                          DepartmentName = b.name,
                          CountValue = b.Count > 0 ? b.Count : 0,
                          AverageValue = Math.Round(Convert.ToDecimal(b.value), 2).ToString() + "%",
                          value = b.value,
                          name = b.name,
                          BudgetAmount = (Math.Round(((b.Budget != null) ? Convert.ToDecimal(b.Budget) : 0), 0, MidpointRounding.ToEven)).ToString("C0", culture),
                          SpentAmount = (Math.Round(((b.Spent != null) ? Convert.ToDecimal(b.Spent) : 0), 0, MidpointRounding.ToEven)).ToString("C0", culture),
                          CurrentSalaryAmount = (Math.Round(((b.CurrentSalary != null) ? Convert.ToDecimal(b.CurrentSalary) : 0), 0, MidpointRounding.ToEven)).ToString("C0", culture),
                          NewSalaryAmount = (Math.Round(((b.NewSalary != null) ? Convert.ToDecimal(b.NewSalary) : 0), 0, MidpointRounding.ToEven)).ToString("C0", culture),

                      }).ToList();
            return result;
        }
        public async Task<PayRangeDistribution> GetPayRangeData(int loggedInEmpNum, int employeeNum, bool isRollup, bool isSelectedRollup, int loggedInUserNum)
        {
            SqlParameter[] sqlParameter = {   new SqlParameter("@loggedInEmpNum", loggedInEmpNum),
                                              new SqlParameter("@managerNum",(int)employeeNum),
                                              new SqlParameter("@rollup", (bool)isRollup),
                                              new SqlParameter("@isSelectedRollup", (bool)isSelectedRollup),
                                               new SqlParameter("@loggedInUserNum",(int)loggedInUserNum)
                                          };
            var payrRangeData = await m_baseRepository.GetData<PayRangeDistribution>("[Talent].[USP_ALS_GET_PayRangeDistributionChart] @loggedInEmpNum,@managerNum,@rollup,@isSelectedRollup,@loggedInUserNum", sqlParameter);
            return payrRangeData.FirstOrDefault();
        }
        public async Task<DataTable> IncreaseExport(int loggedInEmpNum, int employeeNum, string groupBy, bool isRollup, bool isSelectedRollup, int loggedInUserNum)
        {
            SqlParameter[] sqlParameter = {   new SqlParameter("@loggedInEmpNum", loggedInEmpNum),
                                              new SqlParameter("@managerNum",(int)employeeNum),
                                              new SqlParameter("@rollup", (bool)isRollup),
                                              new SqlParameter("@isSelectedRollup", (bool)isSelectedRollup),
                                              new SqlParameter("@groupBy",groupBy),
                                              new SqlParameter("@loggedInUserNum",loggedInUserNum)
                                          };
            var increaseExport = await m_baseRepository.GetDataTableFromStoredProcedure("[Talent].[USP_ALS_GET_GetMeritBudgetExportData]", sqlParameter);
            return increaseExport;
        }
        public async Task<DataTable> PayRangeExport(int loggedInEmpNum, int employeeNum, string groupBy, bool isRollup, bool isSelectedRollup, int loggedInUserNum)
        {
            SqlParameter[] sqlParameter = {   new SqlParameter("@loggedInEmpNum", loggedInEmpNum),
                                              new SqlParameter("@managerNum",(int)employeeNum),
                                              new SqlParameter("@rollup", (bool)isRollup),
                                              new SqlParameter("@isSelectedRollup", (bool)isSelectedRollup),
                                              new SqlParameter("@loggedInUserNum",loggedInUserNum)
                                          };
            var increaseExport = await m_baseRepository.GetDataTableFromStoredProcedure("[Talent].[USP_ALS_GET_PayRangeDistributionExport]", sqlParameter);
            return increaseExport;
        }
        public async Task<DataTable> CompExport(int loggedInEmpNum, int employeeNum, string groupBy, bool isRollup, bool isSelectedRollup, int loggedInUserNum)
        {
            SqlParameter[] sqlParameter = {   new SqlParameter("@managerNum",(int)employeeNum),
                                              new SqlParameter("@loggedInEmpNum", loggedInEmpNum),
                                              new SqlParameter("@rollup", (bool)isRollup),
                                               new SqlParameter("@loggedInUserNum",loggedInUserNum),
                                              new SqlParameter("@isSelectedRollup", (bool)isSelectedRollup)
                                          };
            var CompExport = await m_baseRepository.GetDataTableFromStoredProcedure("[Talent].[USP_Comp_GET_ReporteesExport]", sqlParameter);
            return CompExport;
        }
        public async Task<DataTable> ProRateExport(int loggedInEmpNum, int employeeNum, string groupBy, bool isRollup, bool isSelectedRollup, int loggedInUserNum)
        {
            SqlParameter[] sqlParameter = {   new SqlParameter("@loggedInEmpNum", loggedInEmpNum),
                                              new SqlParameter("@managerNum",(int)employeeNum),
                                              new SqlParameter("@rollup", (bool)isRollup),
                                              new SqlParameter("@isSelectedRollup", (bool)isSelectedRollup),
                                               new SqlParameter("@loggedInUserNum",(int)loggedInUserNum)
            };
            var proRateExport = await m_baseRepository.GetDataTableFromStoredProcedure("[Talent].[USP_ALS_GET_ProRationChartDataExport]", sqlParameter);
            return proRateExport;
        }
        public async Task<DataTable> AvgCostExport(int loggedInEmpNum, int employeeNum, string groupBy, bool isRollup, bool isSelectedRollup, int loggedInUserNum)
        {
            SqlParameter[] sqlParameter = {   new SqlParameter("@loggedInEmpNum", loggedInEmpNum),
                                              new SqlParameter("@managerNum",(int)employeeNum),
                                              new SqlParameter("@rollup", (bool)isRollup),
                                              new SqlParameter("@isSelectedRollup", (bool)isSelectedRollup),
                                               new SqlParameter("@loggedInUserNum",(int)loggedInUserNum)
            };
            var avgCostExport = await m_baseRepository.GetDataTableFromStoredProcedure("[Talent].[USP_ALS_GET_AvgCostChartDataExport]", sqlParameter);
            return avgCostExport;
        }
        public async Task<DataTable> IncreaseChartExport(int loggedInEmpNum, int employeeNum, string groupBy, bool isRollup, bool isSelectedRollup, int loggedInUserNum)
        {
            SqlParameter[] sqlParameter = {   new SqlParameter("@loggedInEmpNum", loggedInEmpNum),
                                              new SqlParameter("@managerNum",(int)employeeNum),
                                              new SqlParameter("@rollup", (bool)isRollup),
                                              new SqlParameter("@isSelectedRollup", (bool)isSelectedRollup),
                                               new SqlParameter("@loggedInUserNum",(int)loggedInUserNum)
            };
            var increaseChartExport = await m_baseRepository.GetDataTableFromStoredProcedure("[Talent].[USP_ALS_GET_IncreaseChartDataExport]", sqlParameter);
            return increaseChartExport;
        }
        public async Task<DataTable> IncreaseEmpCountExport(int loggedInEmpNum, int employeeNum, string groupBy, bool isRollup, bool isSelectedRollup, int loggedInUserNum)
        {
            SqlParameter[] sqlParameter = {   new SqlParameter("@loggedInEmpNum", loggedInEmpNum),
                                              new SqlParameter("@managerNum",(int)employeeNum),
                                              new SqlParameter("@rollup", (bool)isRollup),
                                              new SqlParameter("@isSelectedRollup", (bool)isSelectedRollup),
                                               new SqlParameter("@loggedInUserNum",(int)loggedInUserNum)
            };
            var increaseEmpCountExport = await m_baseRepository.GetDataTableFromStoredProcedure("[Talent].[USP_ALS_GET_IncreaseEmpCountChartDataExport]", sqlParameter);
            return increaseEmpCountExport;
        }
        public async Task<DataTable> MarketDataExport(int loggedInEmpNum, int employeeNum, string groupBy, bool isRollup, bool isSelectedRollup, int loggedInUserNum)
        {
            SqlParameter[] sqlParameter = {   new SqlParameter("@loggedInEmpNum", loggedInEmpNum),
                                              new SqlParameter("@managerNum",(int)employeeNum),
                                              new SqlParameter("@rollup", (bool)isRollup),
                                              new SqlParameter("@isSelectedRollup", (bool)isSelectedRollup),
                                               new SqlParameter("@loggedInUserNum",(int)loggedInUserNum)
            };
            var marketDataExport = await m_baseRepository.GetDataTableFromStoredProcedure("[Talent].[USP_ALS_GET_MarketMidChartDataExport]", sqlParameter);
            return marketDataExport;
        }
        public async Task<DataTable> PopulationDataExport(int loggedInEmpNum, int employeeNum, string groupBy, bool isRollup, bool isSelectedRollup, int loggedInUserNum)
        {
            SqlParameter[] sqlParameter = {   new SqlParameter("@loggedInEmpNum", loggedInEmpNum),
                                              new SqlParameter("@managerNum",(int)employeeNum),
                                              new SqlParameter("@rollup", (bool)isRollup),
                                              new SqlParameter("@isSelectedRollup", (bool)isSelectedRollup),
                                               new SqlParameter("@loggedInUserNum",(int)loggedInUserNum)
            };
            var populationDataExport = await m_baseRepository.GetDataTableFromStoredProcedure("[Talent].[USP_ALS_GET_PopulationChartDataExport]", sqlParameter);
            return populationDataExport;
        }
        public async Task<DataTable> OutlierExport(int loggedInEmpNum, int employeeNum, string groupBy, bool isRollup, bool isSelectedRollup, int loggedInUserNum)
        {
            SqlParameter[] sqlParameter = {   new SqlParameter("@loggedInEmpNum", loggedInEmpNum),
                                              new SqlParameter("@managerNum",(int)employeeNum),
                                              new SqlParameter("@rollup", (bool)isRollup),
                                              new SqlParameter("@isSelectedRollup", (bool)isSelectedRollup),
                                               new SqlParameter("@loggedInUserNum",(int)loggedInUserNum)
            };
            var populationDataExport = await m_baseRepository.GetDataTableFromStoredProcedure("[Talent].[USP_ALS_GET_OutlierChartDataExport]", sqlParameter);
            return populationDataExport;
        }
        #endregion
    }
}
