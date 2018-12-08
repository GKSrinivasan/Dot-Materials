using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Laserbeam.UI.HR
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{*staticfiles}", new { staticfiles = @".*\.(css|js|gif|jpg|png|ico|svg)" });

            bool tenantNameFromVirtualPath = Convert.ToBoolean(ConfigurationManager.AppSettings["TenantNameFromVirtualPath"]);

            if (tenantNameFromVirtualPath)
            {
                routes.MapRoute("sso", url: "sso/", defaults: new { controller = "Account", action = "SSOLogIn" });
                routes.MapRoute(
                    name: "Default",
                    url: "{controller}/{action}",
                    defaults: new { controller = "Account", action = "LogIn" }
                );
            }
            else
            {
                string tenantName = HttpContext.Current.IsDebuggingEnabled ? "compass" : "";
                routes.MapRoute("sso", url: "{" + SiteExtension.Tenant + "}/sso/", defaults: new { controller = "Account", action = "SSOLogIn" });
                routes.MapRoute(
                    name: "Default",
                    url: "{" + SiteExtension.Tenant + "}",
                    defaults: new { controller = "Account", action = "LogIn", tenant = tenantName }
                );

                routes.MapRoute(name: "SiteNavigation", url: "{" + SiteExtension.Tenant + "}/{controller}/{action}");
            }
        }
    }
}
