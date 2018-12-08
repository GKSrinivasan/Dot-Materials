using Laserbeam.Libraries.Core.Interfaces.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace Laserbeam.Libraries.Core
{
   public class MemoryCacheProvider: IMemoryCacheProvider,IDisposable
    {
        #region Fields
        public string TenantName;
        // Author         :   Boobalan Ranganathan		
        // Creation Date  :   07-Apr-2017
        // Ticket ID      :   
        /// <summary>
        /// MemoryCache for application caching
        /// </summary>
        private MemoryCache Cache;

        #endregion

        #region Constructor
        public MemoryCacheProvider(string tenantName)
        {
            TenantName = tenantName;
            Cache = new MemoryCache(tenantName);
        }
        #endregion

        #region Public Methods

        // Author         :   Boobalan Ranganathan		
        // Creation Date  :   07-Apr-2017
        // Ticket ID      :   
        /// <summary>
        /// Gets data from cache
        /// </summary>
        /// <typeparam name="T">The type of data object</typeparam>
        /// <param name="cacheName">Name of the cache</param>
        /// <returns>Return T type object from cache</returns>
        public T GetCache<T>(string cacheName)
        {
            return (T)Cache.Get(cacheName);
        }

        // Author         :   Boobalan Ranganathan		
        // Creation Date  :   07-Apr-2017
        // Ticket ID      :   
        /// <summary>
        /// Sets data into cache
        /// </summary>
        /// <typeparam name="T">The type of data object</typeparam>
        /// <param name="cacheName">Name of the cache</param>
        /// <param name="data">Object of type T</param>
        public void SetCache<T>(string cacheName, T data)
        {
            CacheItemPolicy policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = MemoryCache.InfiniteAbsoluteExpiration;
            policy.SlidingExpiration = TimeSpan.FromMinutes(30);
            Cache.Add(new CacheItem(cacheName, data), policy);
        }

        // Author         :   Boobalan Ranganathan		
        // Creation Date  :   07-Apr-2017
        // Ticket ID      :   
        /// <summary>
        /// Removes cached item from cache
        /// </summary>
        /// <param name="cacheName">Name of the cache</param>
        public void RemoveCache(string cacheName)
        {
            if (Cache.Contains(cacheName))
                Cache.Remove(cacheName);
        }

        // Author         :   Boobalan Ranganathan		
        // Creation Date  :   07-Apr-2017
        // Ticket ID      :   
        /// <summary>
        /// Removes all cached item with keys ending with the match
        /// </summary>
        /// <param name="match">Ending part of the cache name</param>
        public void RemoveCachesEndingWith(string match)
        {
            List<string> keys = new List<string>();
            foreach (var item in Cache)
            {
                if (item.Key.EndsWith(match))
                {
                    keys.Add(item.Key);
                }
            }

            foreach (var key in keys)
            {
                Cache.Remove(key);
            }
        }

        // Author         :   Boobalan Ranganathan		
        // Creation Date  :   07-Apr-2017
        // Ticket ID      :   
        /// <summary>
        /// Checks existence of a cache item
        /// </summary>
        /// <param name="cacheName">Name of the cache</param>
        /// <returns>Return true if cache exists and false if cache doesn't exists</returns>
        public bool CacheExists(string cacheName)
        {
            return Cache.Contains(cacheName);
        }

        // Author         :   Boobalan Ranganathan		
        // Creation Date  :   07-Apr-2017
        // Ticket ID      :   
        /// <summary>
        /// Disposes memory cache
        /// </summary>
        public void Dispose()
        {
            Cache.Dispose();
        }
        #endregion
    }
}
