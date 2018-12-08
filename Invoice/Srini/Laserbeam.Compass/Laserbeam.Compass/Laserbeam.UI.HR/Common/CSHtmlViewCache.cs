using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace Laserbeam.UI.HR
{
    public class CSHtmlViewCache : IViewLocationCache
    {
        private readonly static object s_key = new object();
        private readonly IViewLocationCache _cache;

        public CSHtmlViewCache(IViewLocationCache cache)
        {
            _cache = cache;
        }

        private static IDictionary<string, string> GetRequestCache(HttpContextBase httpContext)
        {
            var viewLookups = httpContext.Items[s_key] as IDictionary<string, string>;
            if (viewLookups == null)
            {
                viewLookups = new Dictionary<string, string>();
                httpContext.Items[s_key] = viewLookups;
            }
            return viewLookups;
        }

        public string GetViewLocation(HttpContextBase httpContext, string key)
        {
            var viewLookups = GetRequestCache(httpContext);
            string location;
            if (!viewLookups.TryGetValue(key, out location))
            {
                location = _cache.GetViewLocation(httpContext, key);
                viewLookups[key] = location;
            }
            return location;
        }

        public void InsertViewLocation(HttpContextBase httpContext, string key, string virtualPath)
        {
            _cache.InsertViewLocation(httpContext, key, virtualPath);
        }
    }
}