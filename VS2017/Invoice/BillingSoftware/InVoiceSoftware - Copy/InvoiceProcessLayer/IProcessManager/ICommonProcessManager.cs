using InvoiceDataLayer.EntityFramework;

namespace InvoiceProcessLayer.IProcessManager
{
    public interface ICommonProcessManager
    {
        AppUser GetAppUser(string userID);
    }
}
