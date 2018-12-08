using Laserbeam.BusinessObject.Common;
using Laserbeam.Constant.HR;
using System;
using System.Configuration;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Linq;
using System.IO;

namespace Laserbeam.UI.HR
{
    public static class SiteExtension
    {
        public const string Tenant = "tenant";
        public const string TenantId = "TenantGUID";
        public const string TenantExpire = "TenantExpire";
        /// <summary>
        /// Extension method for ViewContext to get user name from session in views
        /// </summary>
        /// <param name="viewContext">Instance of ViewContext on which the extension method is applied</param>
        /// <returns>Returns user name as string</returns>
        public static string GetUserName(this ViewContext viewContext)
        {
            var userSession = HttpContext.Current.Session[SessionConstants.UserModel];
            return (userSession != null) ? ((UserModel)userSession).UserName : "";
        }
        public static string GetUserRole(this ViewContext viewContext)
        {
            var userSession = HttpContext.Current.Session[SessionConstants.UserModel];
            return (userSession != null) ? ((UserModel)userSession).UserRole : "";
        }
        public static bool GetEnableSwitchAdmin(this ViewContext viewContext)
        {
            var userSession = HttpContext.Current.Session[SessionConstants.UserModel];
            return (userSession != null) ? ((UserModel)userSession).enableSwitchtoAdmin : false;
        }
        public static bool GetIsSampleData(this ViewContext viewContext)
        {
            var userSession = HttpContext.Current.Session[SessionConstants.UserModel];
            return (userSession != null) ? ((UserModel)userSession).IsSampleData : false;
        }
        public static int GetUserNum(this ViewContext viewContext)
        {
            var userSession = HttpContext.Current.Session[SessionConstants.UserModel];
            return (userSession != null) ? ((UserModel)userSession).UserNum : 0;
        }
        public static bool GetNotificationStatus(this ViewContext viewContext)
        {
            var appConfigSession = HttpContext.Current.Session[SessionConstants.AppConfigModel];
            return (appConfigSession != null) ? ((AppConfigModel)appConfigSession).EnableNotification : false;
        }

        public static bool GetFeedBackStatus(this ViewContext viewContext)
        {
            var appConfigSession = HttpContext.Current.Session[SessionConstants.AppConfigModel];
            return (appConfigSession != null) ? ((AppConfigModel)appConfigSession).EnableFeedBack : false;
        }

        public static string GetClaimValue(this IPrincipal user, string identityKeyName)
        {
            return ((ClaimsIdentity)user.Identity).FindFirst(identityKeyName).Value;
        }

        public static string GetTenant(this ViewContext viewContext)
        {
            return getTenantName(viewContext.HttpContext.Request, viewContext.RouteData);
        }

        public static bool IsInTenant(this IPrincipal user, string tenant)
        {
            return ((ClaimsIdentity)user.Identity).HasClaim(SiteExtension.Tenant, tenant);
        }

        public static string GetTenant(this System.Web.Mvc.AuthorizationContext filterContext)
        {
            return getTenantName(filterContext.HttpContext.Request, filterContext.RouteData);
        }

        public static string GetTenant(this Controller controller)
        {
            return getTenantName(controller.Request, controller.RouteData);
        }

        public static string GetTenantUrl(this Controller controller)
        {
            bool tenantNameFromVirtualPath = Convert.ToBoolean(ConfigurationManager.AppSettings["TenantNameFromVirtualPath"]);
            var request = controller.Request;
            string siteUrl = request.Url.ToString();
            siteUrl = siteUrl.Remove(siteUrl.IndexOf((VirtualPathUtility.MakeRelative("~", request.Url.AbsolutePath))));
            siteUrl = (tenantNameFromVirtualPath) ? siteUrl : siteUrl + Convert.ToString(controller.RouteData.Values[SiteExtension.Tenant]);
            return siteUrl;
        }

        private static string getTenantName(HttpRequestBase request, System.Web.Routing.RouteData routeData)
        {
            bool tenantNameFromVirtualPath = Convert.ToBoolean(ConfigurationManager.AppSettings["TenantNameFromVirtualPath"]);
            string tenantName = string.Empty;
            if (tenantNameFromVirtualPath)
            {
                var urlList = request.ApplicationPath.Split('/');
                tenantName = urlList[urlList.Length - 1];
            }
            else
            {
                tenantName = Convert.ToString(routeData.Values[SiteExtension.Tenant]);
            }
            return tenantName;
        }

        // Author         :   Boobalan Ranganathan		
        // Creation Date  :   18-Mar-2016
        /// <summary>
        /// Extension method to verify the user has a specific role
        /// </summary>
        /// <param name="user">Instance of IPrincipal to be extended</param>
        /// <param name="roles">Comma seperated roles as string</param>
        /// <returns>Returns true if user has any one of the provided roles or no roles are configured else returns false</returns>
        public static bool IsInRoles(this IPrincipal user, string roles)
        {
            if (string.IsNullOrWhiteSpace(roles)) return false;
            string[] roleArray = roles.ToLower().Split(',');
            var claim = ((ClaimsIdentity)user.Identity).FindFirst(ClaimTypes.Role);
            if (claim == null) return false;
            string userRole = claim.Value.ToLower();
            return roleArray.Select(m => m.Trim()).Contains(userRole);
        }

        public static bool IsTenantExpired(this IPrincipal user)
        {
            return ((ClaimsIdentity)user.Identity).HasClaim(SiteExtension.TenantExpire, "True");
        }

        public static string GetTenantLogo(this ViewContext viewContext)
        {
            if (File.Exists(HttpContext.Current.Server.MapPath("~/Images/Logo/" + viewContext.GetTenant() + ".jpg")))
                return "~/Images/Logo/" + viewContext.GetTenant() + ".jpg";
            else if(File.Exists(HttpContext.Current.Server.MapPath("~/Images/Logo/" + viewContext.GetTenant() + ".png")))
                    return "~/Images/Logo/" + viewContext.GetTenant() + ".png";
            else 
                return "~/Images/Logo/default.png";
        }

    }
}