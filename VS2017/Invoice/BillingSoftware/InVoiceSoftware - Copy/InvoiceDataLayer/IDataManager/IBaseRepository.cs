using InvoiceDataLayer.EntityFramework;
using System.Collections.Generic;

namespace InvoiceDataLayer.IDataManager
{
    public interface IBaseRepository
    {
        BIZ_DEVEntities dbcontext();
    }
}
