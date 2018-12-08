using Laserbeam.BusinessObject.Common.CachedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laserbeam.Libraries.Core.Interfaces.Common
{
    public interface ITenantCacheProvider
    {
        // Author         :   Boobalan Ranganathan		
        // Creation Date  :   07-Apr-2017
        // Ticket ID      :   
        /// <summary>
        /// Method to get ApplicationSettingModel
        /// </summary>
        /// <returns>Returns an instance of ApplicationSettingModel</returns>
        ApplicationSettingModel GetApplicationSetting();

        // Author         :   Boobalan Ranganathan		
        // Creation Date  :   07-Apr-2017
        // Ticket ID      :   
        /// <summary>
        /// Method to remove cache
        /// </summary>
        /// <param name="cacheName">Name of the cache</param>
        void RemoveCache(string cacheName);


        // Author         :   Karthikeyan Shanmugam		
        // Creation Date  :   17-Apr-2017
        // Ticket ID      :   
        /// <summary>
        /// Method to get BusinessSettingModel
        /// </summary>
        /// <returns>Returns an instance of BusinessSettingModel</returns>
        BusinessSettingModel GetBusinessSetting();

        //// Author         :   Boobalan Ranganathan		
        //// Creation Date  :   07-Apr-2017
        //// Ticket ID      :   
        ///// <summary>
        ///// Method to get SmtpClientSetting
        ///// </summary>
        ///// <returns>Return instance of SmtpClientSetting</returns>
        //SmtpClientSetting GetSmtpClientSetting();

        //// Author         :   Boobalan Ranganathan		
        //// Creation Date  :   07-Apr-2017
        //// Ticket ID      :   
        ///// <summary>
        ///// Method to remove cache
        ///// </summary>
        ///// <param name="cacheName">Name of the cache</param>
        //void RemoveCache(string cacheName);

        //// Author         :   Muthuvel Sabarish.M		
        //// Creation Date  :   08-Sep-2017
        //// Ticket ID      :   
        //// Modified By    :   Karthikeyan Shanmugam		
        //// Modified Date  :   11-Sep-2017
        //// Ticket ID      :   PSP-15272
        //// Comment        :   Modified as per new table structure 
        ///// <summary>
        ///// Method to get CompensationJobCycle
        ///// </summary>
        ///// <returns>Return instance of CompensationJobCycle</returns>
        //CompensationCycleModel GetCompensationCycleStatus();


        //// Author         :   Karthikeyan Shanmugam		
        //// Creation Date  :   06-Nov-2017
        //// Ticket ID      :   PSP-16112
        ///// <summary>
        ///// Method to get MetaColumn data
        ///// </summary>
        ///// <returns>Returns an instance of MetaColumn</returns>
        //List<GridTitles> GetGridTitles();
    }
}
