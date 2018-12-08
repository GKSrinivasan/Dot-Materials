using Laserbeam.DataManager.Interfaces.Core;
using Laserbeam.Libraries.Core.Interfaces.Common;

namespace Laserbeam.Libraries.Core
{
    public class CacheProvider : ICacheProvider
    {
        #region Fields

        private IBaseRepository m_baseRepository;


        private TenantCacheProvider m_tenantCacheProvider;
        #endregion

        #region Constructor


        public CacheProvider(IBaseRepository baseRepository, TenantCacheProvider tenantCacheProvider)
        {
            m_baseRepository = baseRepository;
            m_tenantCacheProvider = tenantCacheProvider;
        }
        #endregion


        //public ApplicationSettingModel GetApplicationSetting()
        //{
        //    if (m_tenantCacheProvider.CacheExists(ApplicationCacheConstants.ApplicationSetting))
        //        return m_tenantCacheProvider.GetCache<ApplicationSettingModel>(ApplicationCacheConstants.ApplicationSetting);

        //    var appSetting = m_baseRepository.GetQuery<AppSetting>().ToList();
        //    ApplicationSettingModel applicationSetting = new ApplicationSettingModel(appSetting);
        //    m_tenantCacheProvider.SetCache<ApplicationSettingModel>(ApplicationCacheConstants.ApplicationSetting, applicationSetting);
        //    return applicationSetting;
        //}


        //public BusinessSettingModel GetBusinessSetting()
        //{
        //    if (m_tenantCacheProvider.CacheExists(ApplicationCacheConstants.BussinessSetting))
        //        return m_tenantCacheProvider.GetCache<BusinessSettingModel>(ApplicationCacheConstants.BussinessSetting);

        //    var busSetting = m_baseRepository.GetQuery<BusSetting>().ToList();
        //    BusinessSettingModel businessSetting = new BusinessSettingModel(busSetting);
        //    m_tenantCacheProvider.SetCache<BusinessSettingModel>(ApplicationCacheConstants.ApplicationSetting, businessSetting);
        //    return businessSetting;
        //}


        //public SmtpClientSetting GetSmtpClientSetting()
        //{
        //    SmtpClientSetting smtpSetting = new SmtpClientSetting();
        //    var appSetting = GetApplicationSetting();
        //    smtpSetting.Password = appSetting.AdminPassword;
        //    smtpSetting.UserId = appSetting.AdminEmailID;
        //    smtpSetting.SmtpServer = appSetting.SMTPServer;
        //    smtpSetting.PortNumber = appSetting.SMTPPort;
        //    smtpSetting.EnableEmailAuthentication = appSetting.EnableEmailAuthentication;
        //    return smtpSetting;
        //}


        //public void RemoveCache(string cacheName)
        //{
        //    m_tenantCacheProvider.RemoveCache(cacheName);
        //}
    }
}
