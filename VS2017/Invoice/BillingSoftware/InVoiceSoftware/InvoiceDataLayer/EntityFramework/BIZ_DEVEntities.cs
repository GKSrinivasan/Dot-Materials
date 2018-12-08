using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;

namespace InvoiceDataLayer.EntityFramework
{
    public partial class BIZ_DEVEntities : DbContext
    {
        public BIZ_DEVEntities(EntityConnection schemaConnection) : base(schemaConnection, true)
        {
            this.Configuration.LazyLoadingEnabled = false;
        }
    }
}
