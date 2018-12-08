using System.Data.Entity.Core.EntityClient;

namespace Laserbeam.EntityManager.Common
{
    public static class EntityConnector
    {

        // Author         :   Boobalan Ranganathan		
        // Creation Date  :   12-Sep-2015
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static EntityConnection CreateConnection(string connectionString)
        {
            string metaDataName = "LaserbeamEntities";
            var entityConnectionString = "metadata=res://*/" + metaDataName + ".csdl|res://*/" + metaDataName + ".ssdl|res://*/" + metaDataName + ".msl;" + connectionString;            
            return new EntityConnection(entityConnectionString);
        }
    }
}
