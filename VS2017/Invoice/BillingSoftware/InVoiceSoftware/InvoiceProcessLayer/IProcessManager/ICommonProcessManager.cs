using InvoiceDataLayer.EntityFramework;
using System.Collections.Generic;

namespace InvoiceProcessLayer.IProcessManager
{
    public interface ICommonProcessManager
    {
        AppUser GetAppUser(string userID);
        void UpdateAppUser(AppUser model);
        List<Employee> GetAllEmployee();
        IEnumerable<CommonCode> GetCommonCodes();
        IEnumerable<Currency> GetCurrencies();
        IEnumerable<CompanyLocation> GetCompanies();
    }
}
