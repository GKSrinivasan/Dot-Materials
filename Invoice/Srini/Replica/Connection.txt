using Laserbeam.EntityManager.TenantMaster;
using Laserbeam.Libraries.Common;
using System.Linq;

namespace Laserbeam.DataManager.Core
{
    public class TenantEntityConnection
    {

        #region Fields

        // Author         :   Boobalan Ranganathan		
        // Creation Date  :   25-Sep-2015
        /// <summary>
        /// Entity connection of a tenant
        /// </summary>
        public string ConnectionString;
        public string TenantSchema;
        #endregion

        #region Constructor

        // Author         :   Boobalan Ranganathan		
        // Creation Date  :   25-Sep-2015

        // Author         :   Boobalan Ranganathan		
        // Creation Date  :   17-Aug-2016
        // Ticket ID      :   PSP-12247
        // Comment : To decrypt the encrypted database connection details
        /// <summary>
        /// Creates instance of TenantEntityConnection for the specified tenant
        /// </summary>
        /// <param name="tenantName">Name of the tenant</param>
        public TenantEntityConnection(string tenantName)
        {
            TenantConnection tenantData;

            using (LaserbeamCompassMasterEntities master = LaserbeamCompassMasterEntities.Create())
            {
                tenantData = master.TenantConnections.SingleOrDefault(m => m.Tenant.TenantURLName == tenantName);
                string dbServer = "";
                string dbName = "";
                string dbUserId = "";
                string dbPassword = "";                            
                MayaLink.TryDecrypt(tenantData.DatabaseServer, out dbServer);
                MayaLink.TryDecrypt(tenantData.DatabaseName, out dbName);
                MayaLink.TryDecrypt(tenantData.DatabaseUserId, out dbUserId);
                MayaLink.TryDecrypt(tenantData.DatabasePassword, out dbPassword);
                ConnectionString = "provider=System.Data.SqlClient;provider connection string=\"data source=" + dbServer + ";initial catalog=" + dbName + ";persist security info=True;user id=" + dbUserId + ";password=" + dbPassword + ";App=Compass\";";
            }
        }

        /// <summary>
        /// Checks for the existence of a tenant
        /// </summary>
        /// <param name="tenantName">Name of the tenant</param>
        /// <returns>Returns true if the tenant exists or false</returns>
        public static bool CheckValidTenant(string tenantName)
        {
            using (LaserbeamCompassMasterEntities master = LaserbeamCompassMasterEntities.Create())
            {
                return master.Tenants.FirstOrDefault(m => m.TenantURLName == tenantName) != null;
            }
        }
        #endregion
    }
}
