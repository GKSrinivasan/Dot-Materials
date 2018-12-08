using InvoiceDataLayer.EntityFramework;
using System.Collections.Generic;

namespace InvoiceDataLayer.IDataManager
{
    public interface ICommonRepository
    {
        AppUser GetAppUser(string userID);
    }
}
