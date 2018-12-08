using Laserbeam.UI.HR.App_Start;
using Laserbeam.UI.HR.Common;
using System;
using System.IdentityModel.Claims;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Laserbeam.UI.HR
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new CSHtmlViewEngine());
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            RegistryConfig.RegisterDependecies();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            GlobalFilters.Filters.Add(new GlobalHandleErrorAttribute());
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(RegexPasswordAttribute), typeof(RegularExpressionAttributeAdapter));
            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.NameIdentifier;
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            Response.AddHeader("X-Frame-Options", "ALLOW-FROM SAMEDOMAIN");
            Response.AddHeader("Content-Security-Policy", "frame-ancestors self");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            Response.Cache.SetNoStore();
        }

        protected void Application_Error()
        {
            Exception exception = Server.GetLastError();
            LogApplicatonError applicationErrorLogger = new LogApplicatonError();
            applicationErrorLogger.RecordError(exception);
            Server.ClearError();
            Response.TrySkipIisCustomErrors = true;
            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            bool isAjaxRequest = (new HttpRequestWrapper(Context.Request)).IsAjaxRequest();
            if (isAjaxRequest)
            {
                Response.ContentType = "text/javascript";
                Response.Write("window.location = '../Error/Index'");
            }
            else
            {
                RouteValueDictionary routeValues = new RouteValueDictionary();
                routeValues.Add("controller", "Error");
                routeValues.Add("action", "Index");
                Response.RedirectToRoute(routeValues);
            }

        }

        protected void Application_End()
        {
            RegistryConfig.DisposeContainer();
        }
    }
}
