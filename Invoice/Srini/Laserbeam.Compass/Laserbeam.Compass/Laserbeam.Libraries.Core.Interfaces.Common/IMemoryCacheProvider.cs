using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laserbeam.Libraries.Core.Interfaces.Common
{
   public  interface IMemoryCacheProvider
    {
        // Author         :   Boobalan Ranganathan		
        // Creation Date  :   07-Apr-2017
        // Ticket ID      :   
        /// <summary>
        /// Gets data from cache
        /// </summary>
        /// <typeparam name="T">The type of data object</typeparam>
        /// <param name="cacheName">Name of the cache</param>
        /// <returns>Return T type object from cache</returns>
        T GetCache<T>(string cacheName);

        // Author         :   Boobalan Ranganathan		
        // Creation Date  :   07-Apr-2017
        // Ticket ID      :   
        /// <summary>
        /// Sets data into cache
        /// </summary>
        /// <typeparam name="T">The type of data object</typeparam>
        /// <param name="cacheName">Name of the cache</param>
        /// <param name="data">Object of type T</param>
        void SetCache<T>(string cacheName, T data);

        // Author         :   Boobalan Ranganathan		
        // Creation Date  :   07-Apr-2017
        // Ticket ID      :   
        /// <summary>
        /// Removes cached item from cache
        /// </summary>
        /// <param name="cacheName">Name of the cache</param>
        void RemoveCache(string cacheName);

        // Author         :   Boobalan Ranganathan		
        // Creation Date  :   07-Apr-2017
        // Ticket ID      :   
        /// <summary>
        /// Removes all cached item with keys ending with the match
        /// </summary>
        /// <param name="match">Ending part of the cache name</param>
        void RemoveCachesEndingWith(string endingLike);

        // Author         :   Boobalan Ranganathan		
        // Creation Date  :   07-Apr-2017
        // Ticket ID      :   
        /// <summary>
        /// Checks existence of a cache item
        /// </summary>
        /// <param name="cacheName">Name of the cache</param>
        /// <returns>Return true if cache exists and false if cache doesn't exists</returns>
        bool CacheExists(string cacheName);
    }
}
