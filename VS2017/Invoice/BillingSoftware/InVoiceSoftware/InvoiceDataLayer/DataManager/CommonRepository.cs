using InvoiceDataLayer.EntityFramework;
using InvoiceDataLayer.IDataManager;
using System.Collections.Generic;
using System.Linq;

namespace InvoiceDataLayer.DataManager
{
    public class CommonRepository:ICommonRepository
    {
        private readonly IBaseRepository m_baseRepository;

        public CommonRepository(IBaseRepository baseRepository)
        {
            m_baseRepository = baseRepository;
        }

        public AppUser GetAppUser(string userID)
        {
            return m_baseRepository.GetQuery<AppUser>(x => x.UserID == userID).FirstOrDefault();
        }

        public void UpdateAppUser(AppUser userModel)
        {
            m_baseRepository.Edit<AppUser>(userModel);
            m_baseRepository.SaveChanges();
        }

        public Employee GetEmployeeData(int empNum)
        {
            return m_baseRepository.GetQuery<Employee>().Where(x => x.EmployeePK == empNum).FirstOrDefault();
        }

        public AppUser GetAppUserData(int userNum)
        {
            return m_baseRepository.GetQuery<AppUser>().Where(x => x.AppUserPK == userNum).FirstOrDefault();
        }

        public void UpdateEmployee(Employee employee)
        {
            m_baseRepository.Edit<Employee>(employee);
            m_baseRepository.SaveChanges();
        }

        public void AddEmployee(Employee employee)
        {
            m_baseRepository.Add<Employee>(employee);
            m_baseRepository.SaveChanges();
        }

        public List<Employee> GetAllEmployee()
        {
            return m_baseRepository.GetQuery<Employee>().ToList();
        }

        public IEnumerable<CommonCode> GetCommonCodes()
        {
            return m_baseRepository.GetQuery<CommonCode>().Where(x => x.IsActive == true);
        }

        public IEnumerable<Currency> GetCurrencies()
        {
            return m_baseRepository.GetQuery<Currency>().Where(x => x.IsActive == true);
        }

        public IEnumerable<CompanyLocation> GetCompanies()
        {
            return m_baseRepository.GetQuery<CompanyLocation>().Where(x => x.IsActive == true);
        }
    }
}
