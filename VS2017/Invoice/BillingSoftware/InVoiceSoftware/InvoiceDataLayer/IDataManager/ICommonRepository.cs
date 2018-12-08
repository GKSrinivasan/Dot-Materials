using InvoiceDataLayer.EntityFramework;
using System.Collections.Generic;

namespace InvoiceDataLayer.IDataManager
{
    public interface ICommonRepository
    {
        AppUser GetAppUser(string userID);
        void UpdateAppUser(AppUser userModel);
        Employee GetEmployeeData(int empNum);
        AppUser GetAppUserData(int userNum);
        void UpdateEmployee(Employee employee);
        List<Employee> GetAllEmployee();
        IEnumerable<CommonCode> GetCommonCodes();
        IEnumerable<Currency> GetCurrencies();
        IEnumerable<CompanyLocation> GetCompanies();
        void AddEmployee(Employee employee);
    }
}
