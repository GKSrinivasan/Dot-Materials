using System;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace InVoiceSoftware.Common
{
    public static class CommonModel
    {
        public const string Tenant = "tenantName";

        public static string GetTenantLogo(this ViewContext viewContext)
        {
            if (File.Exists(HttpContext.Current.Server.MapPath("~/Images/Logo/" + viewContext.GetTenant() + ".jpg")))
                return "~/Images/Logo/" + viewContext.GetTenant() + ".jpg";
            else if (File.Exists(HttpContext.Current.Server.MapPath("~/Images/Logo/" + viewContext.GetTenant() + ".png")))
                return "~/Images/Logo/" + viewContext.GetTenant() + ".png";
            else
                return "~/Images/Logo/default.png";
        }

        public static string GetTenant(this ViewContext viewContext)
        {
            return getTenantName(viewContext.HttpContext.Request, viewContext.RouteData);
        }

        private static string getTenantName(HttpRequestBase request, System.Web.Routing.RouteData routeData)
        {
            string tenantName = string.Empty;
                tenantName = Convert.ToString(routeData.Values[CommonModel.Tenant]);
            return tenantName;
        }

        public static string UserName(this ViewContext viewContext)
        {
            return (string) System.Web.HttpContext.Current.Session["UserName"];
        }
    }
}