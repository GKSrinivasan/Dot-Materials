using System;
using System.Web.Caching;

namespace Laserbeam.UI.HR.UIExtensions
{
    public static class CacheExtensions
    {
        public static T GetCategory<T>(this Cache cache, String key, Func<T> generator)
        {
            var result = cache[key];

            if (result == null)
            {
                result = generator();
                cache[key] = result;
            }

            return (T)result;
        }
    }
}