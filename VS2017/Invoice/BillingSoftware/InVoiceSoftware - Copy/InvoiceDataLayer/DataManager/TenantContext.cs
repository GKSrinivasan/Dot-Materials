using InvoiceDataLayer.EntityFramework;
using System.Web;
using System.Web.Routing;
using System.Linq;

namespace InvoiceDataLayer.DataManager
{
    public class TenantContext
    {
        public string GetTenant()
        {
            MasterDBEntities masterDb = new MasterDBEntities();
            var routeData = RouteTable.Routes.GetRouteData(new HttpContextWrapper(HttpContext.Current));
            var tenantName = routeData.GetRequiredString("tenantName");
            var tenatPK = masterDb.Tenants.Where(x => x.TenantName == tenantName).Select(x => x.TenantPK).FirstOrDefault();
            return masterDb.TenantDatabases.Where(x => x.TenantFK == tenatPK).Select(x => x.DataBaseName).FirstOrDefault();
        }
    }
}
