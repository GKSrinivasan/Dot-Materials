using InvoiceDependencyLibrary.App_Start;
using InvoiceDependencyLibrary.DependencyResolution;
using InVoiceSoftware.App_Start;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace InVoiceSoftware
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            RegistryConfig.RegisterDependecies();
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            StructuremapMvc.Start();
        }
    }
}
