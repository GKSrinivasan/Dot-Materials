using Laserbeam.Libraries.Common;
using System;
using System.Configuration;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;

namespace Laserbeam.EntityManager.TenantMaster
{

    public partial class LaserbeamCompassMasterEntities : DbContext
    {

        // Author         :   Boobalan Ranganathan		
        // Creation Date  :   16-Aug-2016
        // Ticket ID      :   PSP-12247
        /// <summary>
        /// Constructor for LaserbeamTMSMasterEntities accepting entity connectionstring
        /// </summary>
        /// <param name="connectionString">A valid entity connectionstring</param>
        public LaserbeamCompassMasterEntities(string connectionString)
            : base(connectionString)
        {
            this.Configuration.LazyLoadingEnabled = false;
        }

        // Author         :   Boobalan Ranganathan		
        // Creation Date  :   16-Aug-2016
        // Ticket ID      :   PSP-12247
        /// <summary>
        /// Static method to create an instance of LaserbeamTMSMasterEntities
        /// </summary>
        /// <returns>Returns an instance of LaserbeamTMSMasterEntities</returns>
        public static LaserbeamCompassMasterEntities Create()
        {
            var entityConnectionString = new EntityConnectionStringBuilder(ConfigurationManager.ConnectionStrings["LaserbeamCompassMasterEntities"].ConnectionString);
            var databaseConnection = new DbConnectionStringBuilder();
            databaseConnection.ConnectionString = entityConnectionString.ProviderConnectionString;
            string dbServer = Convert.ToString(databaseConnection["Data Source"]);
            string dbName = Convert.ToString(databaseConnection["Initial Catalog"]);
            string dbUserId = Convert.ToString(databaseConnection["User ID"]);
            string dbPassword = Convert.ToString(databaseConnection["Password"]);
            MayaLink.TryDecrypt(dbServer, out dbServer);
            MayaLink.TryDecrypt(dbName, out dbName);
            MayaLink.TryDecrypt(dbUserId, out dbUserId);
            MayaLink.TryDecrypt(dbPassword, out dbPassword);
            databaseConnection["Data Source"] = dbServer;
            databaseConnection["Initial Catalog"] = dbName;
            databaseConnection["User ID"] = dbUserId;
            databaseConnection["Password"] = dbPassword;
            entityConnectionString.ProviderConnectionString = databaseConnection.ConnectionString;
            return new LaserbeamCompassMasterEntities(entityConnectionString.ConnectionString);
        }
    }
}
