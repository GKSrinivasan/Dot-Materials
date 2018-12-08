using InvoiceDataLayer.EntityFramework;
using InvoiceDataLayer.IDataManager;

namespace InvoiceDataLayer.DataManager
{
    public class BaseRepository : IBaseRepository
    {
        public TenantContext context = new TenantContext();
        public BIZ_DEVEntities dbContext;
        public BaseRepository()
        {
            string database = context.GetTenant();
            dbContext = new BIZ_DEVEntities(database);
        }

        public BIZ_DEVEntities dbcontext()
        {
            return dbContext;
        }
    }
}

