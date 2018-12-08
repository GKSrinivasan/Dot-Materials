using Laserbeam.BusinessObject.Common.CachedModels;
using Laserbeam.BusinessObject.Common.Constants;
using Laserbeam.DataManager.Interfaces.Core;
using Laserbeam.EntityManager.Common;
using Laserbeam.Libraries.Core.Interfaces.Common;
using System;
using System.Linq;
using System.Runtime.Caching;

namespace Laserbeam.Libraries.Core
{
    public class TenantCacheProvider : ITenantCacheProvider
    {
        #region Fields

        // Author         :   Hariharasubramaniyan Chandrasekaran			
        // Creation Date  :   07-March-2017
        // Ticket ID      :   
        /// <summary>
        /// An instance of IBaseRepository
        /// </summary>
        private IBaseRepository m_baseRepository;

        // Author         :   Hariharasubramaniyan Chandrasekaran	
        // Creation Date  :   07-March-2017
        // Ticket ID      :   
        /// <summary>
        /// An instance of TenantCacheProvider
        /// </summary>
        private IMemoryCacheProvider m_memoryCacheProvider;
        #endregion

        #region Constructor

        // Author         :   Hariharasubramaniyan Chandrasekaran		
        // Creation Date  :   07-March-2017
        // Ticket ID      :   
        /// <summary>
        /// Constructor of CacheRepositroy
        /// </summary>
        /// <param name="baseRepository">An instance of IBaseRepository</param>
        /// <param name="tenantCacheProvider">An instance of TenantCacheProvider</param>
        public TenantCacheProvider(IBaseRepository baseRepository, IMemoryCacheProvider memoryCacheProvider)
        {
            m_baseRepository = baseRepository;
            m_memoryCacheProvider = memoryCacheProvider;
        }
        #endregion

        // Author         :   Hariharasubramaniyan Chandrasekaran		
        // Creation Date  :   07-March-2017
        // Ticket ID      :   
        /// <summary>
        /// Method to get ApplicationSettingModel
        /// </summary>
        /// <returns>Returns an instance of ApplicationSettingModel</returns>
        public ApplicationSettingModel GetApplicationSetting()
        {

            if (m_memoryCacheProvider.CacheExists(ApplicationCacheConstants.ApplicationSetting))
                return m_memoryCacheProvider.GetCache<ApplicationSettingModel>(ApplicationCacheConstants.ApplicationSetting);

            var appSetting = m_baseRepository.GetQuery<AppSetting>().ToList();
            ApplicationSettingModel applicationSetting = new ApplicationSettingModel(appSetting);
            m_memoryCacheProvider.SetCache<ApplicationSettingModel>(ApplicationCacheConstants.ApplicationSetting, applicationSetting);
            return applicationSetting;
        }

        // Author         :   Hariharasubramaniyan Chandrasekaran	
        // Creation Date  :   17-March-2017
        // Ticket ID      :   
        /// <summary>
        /// Method to get BusinessSettingModel
        /// </summary>
        /// <returns>Returns an instance of BusinessSettingModel</returns>
        public BusinessSettingModel GetBusinessSetting()
        {
            if (m_memoryCacheProvider.CacheExists(ApplicationCacheConstants.BussinessSetting))
                return m_memoryCacheProvider.GetCache<BusinessSettingModel>(ApplicationCacheConstants.BussinessSetting);

            var busSetting = m_baseRepository.GetQuery<BusSetting>().ToList();
            BusinessSettingModel businessSetting = new BusinessSettingModel(busSetting);
            m_memoryCacheProvider.SetCache<BusinessSettingModel>(ApplicationCacheConstants.BussinessSetting, businessSetting);
            return businessSetting;
        }
        // Author         :   Hariharasubramaniyan Chandrasekaran		
        // Creation Date  :   07-March-2017
        // Ticket ID      :   
        /// <summary>
        /// Method to get SmtpClientSetting
        /// </summary>
        /// <returns>Return instance of SmtpClientSetting</returns>
        public SmtpClientSettingModel GetSmtpClientSetting()
        {
            SmtpClientSettingModel smtpSetting = new SmtpClientSettingModel();
            var appSetting = GetApplicationSetting();
            smtpSetting.Password = appSetting.AdminPassword;
            smtpSetting.UserId = appSetting.AdminEmailID;
            smtpSetting.SmtpServer = appSetting.SMTPServer;
            smtpSetting.PortNumber = appSetting.SMTPPort;
            return smtpSetting;
        }


        // Author         :  Hariharasubramaniyan Chandrasekaran			
        // Creation Date  :   07-March-2017
        // Ticket ID      :   
        /// <summary>
        /// Method to remove cache
        /// </summary>
        /// <param name="cacheName">Name of the cache</param>
        public void RemoveCache(string cacheName)
        {
            m_memoryCacheProvider.RemoveCache(cacheName);
        }
    }
}
