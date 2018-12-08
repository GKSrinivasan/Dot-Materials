using System.Data.Entity.Core.EntityClient;

namespace InvoiceDataLayer.EntityFramework
{
    public static class EntityConnector
    {
        public static EntityConnection CreateConnection(string connectionString)
        {
            string metaDataName = "EntityFramework.TenantDBEntity";
            var entityConnectionString = "metadata=res://*/" + metaDataName + ".csdl|res://*/" + metaDataName + ".ssdl|res://*/" + metaDataName + ".msl;" + connectionString;
            return new EntityConnection(entityConnectionString);
        }
    }
}
