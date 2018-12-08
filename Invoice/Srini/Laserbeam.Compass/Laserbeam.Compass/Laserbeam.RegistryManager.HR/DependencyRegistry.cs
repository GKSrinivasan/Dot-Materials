using Laserbeam.DataManager.Common;
using Laserbeam.DataManager.Interfaces.Common;
using Laserbeam.DataManager.Interfaces.Core;
using Laserbeam.DataManager.Core;
using Laserbeam.Libraries.Common;
using Laserbeam.Libraries.Interfaces.Common;
using Laserbeam.ProcessManager.Common;
using Laserbeam.ProcessManager.HR.Common;
using Laserbeam.ProcessManager.Interfaces.Common;
using StructureMap;
using Laserbeam.EntityManager.Common;
using Laserbeam.Libraries.Core.Interfaces.Common;
using Laserbeam.Libraries.Core;
using System.Data.Entity.Core.EntityClient;
using Laserbeam.EntityManager.TenantMaster;

namespace Laserbeam.RegistryManager.HR
{
    public static class DependencyRegistry
    {
        #region Static Methods

        // Author         :   Raja Ganapathy
        // Creation Date  :   05-Jul-2016     
        // Reviewed By    :  Srinivasan Kamalakannan
        // Reviewed Date  :  31-Aug-2016 
        // Comments       :   Added process manager and repository in registry        
        /// <summary>
        /// Registers the process manager and repository in container
        /// </summary>        
        /// <param name="container">Instance of structuremap container</param>
        /// <returns>Returns true on successfull configuration and false on failure</returns>
        
        public static bool RegisterTenant(string tenantName, IContainer container, IContainer nestedContainer)

        {
            if (container.TryGetInstance<TenantEntityConnection>(tenantName) == null)
            {
                if (!TenantEntityConnection.CheckValidTenant(tenantName)) return false;
                container.Configure(c =>
                {
                    //DataManager.Core
                    c.For<TenantEntityConnection>().Use<TenantEntityConnection>().Ctor<string>().Is(tenantName).Named(tenantName).Singleton();
                    c.For<IMemoryCacheProvider>().Add(new MemoryCacheProvider(tenantName)).Named(tenantName).Singleton();
                });
            }
            nestedContainer.Configure(c =>
            {
                //DataManager.Core
                c.For<IBaseRepository>().Use(new BaseRepository<LaserbeamEntitiesContainer1>(new LaserbeamEntitiesContainer1(EntityConnector.CreateConnection(container.GetInstance<TenantEntityConnection>(tenantName).ConnectionString)))).Named(tenantName).Transient();
                c.For<IMasterBaseRepository>().Use(new BaseRepository<LaserbeamCompassMasterEntities>(LaserbeamCompassMasterEntities.Create())).Named(tenantName).Transient();
                ////DataManager
                //c.For<IBaseRepository>().Use(new BaseRepository<LaserbeamEntitiesContainer1>(new LaserbeamEntitiesContainer1())).Transient();
                c.For<IAppUserRepository>().Use<AppUserRepository>().Named(tenantName).Transient();
                c.For<ICompensationRepository>().Use<CompensationRepository>().Named(tenantName).Transient();
                c.For<IDashboardRepository>().Use<DashboardRepository>().Named(tenantName).Transient();
                c.For<IEmailDetailsRepository>().Use<EmailDetailsRepository>().Named(tenantName).Transient();
                c.For<IUserManagementRepository>().Use<UserManagementRepository>().Named(tenantName).Transient();
                c.For<IWorkForceRepository>().Use<WorkForceRepository>().Named(tenantName).Transient();
                c.For<ISessionRepository>().Use<SessionRepository>().Named(tenantName).Transient();
                c.For<IBudgetPlanRepository>().Use<BudgetPlanRepository>().Named(tenantName).Transient();
                c.For<IRatingRepository>().Use<RatingRepository>().Named(tenantName).Transient();
                c.For<IExchangeRateRepository>().Use<ExchangeRateRepository>().Named(tenantName).Transient();
                c.For<IWorkFlowRepository>().Use<WorkFlowRepository>().Named(tenantName).Transient();
                c.For<IRuleConfigurationRepository>().Use<RuleConfigurationRepository>().Named(tenantName).Transient();
                c.For<ICommunicationRepository>().Use<CommunicationRepository>().Named(tenantName).Transient();
                c.For<IPageCustomizationRepository>().Use<PageCustomizationRepository>().Named(tenantName).Transient();
                c.For<IProfileRepository>().Use<ProfileRepository>().Named(tenantName).Transient();
                c.For<IChatBoxRepository>().Use<ChatBoxRepository>().Named(tenantName).Transient();
                c.For<IAnalyticsRepository>().Use<AnalyticsRepository>().Named(tenantName).Transient();
                c.For<IMarketPayRangeRepository>().Use<MarketPayRangeRepository>().Named(tenantName).Transient();

                //ProcessManager
                c.For<IAccountProcessManager>().Use<AccountProcessManager>().Named(tenantName).Transient();
                c.For<ICompensationProcessManager>().Use<CompensationProcessManager>().Named(tenantName).Transient();
                c.For<ICommentProcessManager>().Use<CommentProcessManager>().Named(tenantName).Transient();
                c.For<IDashboardProcessManager>().Use<DashboardProcessManager>().Named(tenantName).Transient();
                c.For<IEmailProcessManager>().Use<EmailProcessManager>().Named(tenantName).Transient();
                c.For<IUserManagementProcessManager>().Use<UserManagementProcessManager>().Named(tenantName).Transient();
                c.For<IWorkForceProcessManager>().Use<WorkForceProcessManager>().Named(tenantName).Transient();
                c.For<ISessionProcessManager>().Use<SessionProcessManager>().Named(tenantName).Transient();
                c.For<IEmail>().Use<Email>().Named(tenantName).Transient();
                c.For<IExport>().Use<Export>().Named(tenantName).Transient();
                c.For<IKeyGenerator>().Use<KeyGenerator>().Named(tenantName).Transient();
                c.For<IPasswordEncryption>().Use<PasswordEncryption>().Named(tenantName).Transient();
                c.For<IBudgetPlanProcessManager>().Use<BudgetPlanProcessManager>().Named(tenantName).Transient();
                c.For<IRatingProcessManager>().Use<RatingProcessManager>().Named(tenantName).Transient();
                c.For<IExchangeRateProcessManager>().Use<ExchangeRateProcessManager>().Named(tenantName).Transient();
                c.For<IWorkFlowProcessManager>().Use<WorkflowProcessManager>().Named(tenantName).Transient();
                c.For<IRuleConfigurationProcessManager>().Use<RuleConfigurationProcessManager>().Named(tenantName).Transient();
                c.For<ICommunicationProcessManager>().Use<CommunicationProcessManager>().Named(tenantName).Transient();
                c.For<IPageCustomizationProcessManager>().Use<PageCustomizationProcessManager>().Named(tenantName).Transient();
                c.For<IProfileProcessManager>().Use<ProfileProcessManager>().Named(tenantName).Transient();
                c.For<IChatBoxProcessManager>().Use<ChatBoxProcessManager>().Named(tenantName).Transient();
                c.For<IAnalyticsProcessManager>().Use<AnalyticsProcessManager>().Named(tenantName).Transient();
                c.For<IMarketPayRangeProcessManager>().Use<MarketPayRangeProcessManager>().Named(tenantName).Transient();

            });
            nestedContainer.Configure(c => {
                //Libraries.Core
                c.For<ITenantCacheProvider>().Use(new TenantCacheProvider(nestedContainer.GetInstance<IBaseRepository>(tenantName), container.GetInstance<IMemoryCacheProvider>(tenantName))).Named(tenantName).Transient();
            });
            return true;
        }
        #endregion
    }
}
