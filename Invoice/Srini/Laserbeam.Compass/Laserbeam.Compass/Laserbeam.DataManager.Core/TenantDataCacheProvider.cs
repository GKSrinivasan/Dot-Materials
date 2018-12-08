using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace Laserbeam.DataManager.Core
{
    public class TenantDataCacheProvider : IDisposable
    {
        private ObjectCache Cache { get { return MemoryCache.Default; } }
        private string TenantName;
        public TenantDataCacheProvider(string tenantName)
        {
            TenantName = tenantName;
        }
        public T GetCache<T>()
        {
            return (T)Cache[typeof(T).FullName + TenantName];
        }

        public void SetCache<T>(object data)
        {
            CacheItemPolicy policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = DateTime.Now + TimeSpan.FromMinutes(30);
            Cache.Add(new CacheItem(typeof(T).FullName + TenantName, data), policy);
        }

        public void RemoveCache<T>()
        {
            Cache.Remove(typeof(T).FullName + TenantName);
        }

        public bool CheckCache<T>()
        {
            return (Cache[typeof(T).FullName + TenantName] != null);
        }

        public void Dispose()
        {
            ((MemoryCache)Cache).Dispose();
        }
    }
}
