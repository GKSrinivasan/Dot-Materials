using System.Linq;

namespace InvoiceDataLayer.EntityFramework
{
    public class TenantEntityConnection
    {

        #region Fields
        public string ConnectionString;
        public string TenantSchema;
        #endregion

        #region Constructor

        public TenantEntityConnection(string tenantName)
        {
            using (MasterDBEntities master = new MasterDBEntities())
            {
                var tenantPk = master.Tenants.Where(m => m.TenantName == tenantName).Select(m => m.TenantPK).FirstOrDefault();
                var databaseName = master.TenantDatabases.Where(x => x.TenantFK == tenantPk).Select(x => x.DataBaseName).FirstOrDefault();
                string dbServer = "LAPTOP-HGVP3VEO\\SQLSERVER";
                string dbUserId = "sa";
                string dbPassword = "SQL@2017";
                ConnectionString = "provider=System.Data.SqlClient;provider connection string=\"data source=" + dbServer + ";initial catalog=" + databaseName + ";persist security info=True;user id=" + dbUserId + ";password=" + dbPassword + ";App=Compass\";";
            }
        }
        #endregion
    }
}
