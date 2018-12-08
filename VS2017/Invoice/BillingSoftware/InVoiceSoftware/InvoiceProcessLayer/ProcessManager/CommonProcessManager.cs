using InvoiceDataLayer.BussinessModel;
using InvoiceDataLayer.EntityFramework;
using InvoiceDataLayer.IDataManager;
using InvoiceProcessLayer.IProcessManager;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InvoiceProcessLayer.ProcessManager
{
    public class CommonProcessManager : ICommonProcessManager
    {
        private readonly ICommonRepository m_commonRepository;

        public CommonProcessManager(ICommonRepository commonRepository)
        {
            m_commonRepository = commonRepository;
        }

        public AppUser GetAppUser(string userID)
        {
            return m_commonRepository.GetAppUser(userID);
        }

        public void UpdateAppUser(AppUser model)
        {
            m_commonRepository.UpdateAppUser(model);
        }

        public List<Employee> GetAllEmployee()
        {
           return m_commonRepository.GetAllEmployee();
        }

        public IEnumerable<CommonCode> GetCommonCodes()
        {
            return m_commonRepository.GetCommonCodes();
        }

        public IEnumerable<Currency> GetCurrencies()
        {
            return m_commonRepository.GetCurrencies();
        }

        public IEnumerable<CompanyLocation> GetCompanies()
        {
            return m_commonRepository.GetCompanies();
        }

        public void AddEmployee(EmployeeModel model)
        {
            var commonCode = m_commonRepository.GetCommonCodes();
            var gender = commonCode.Where(x => x.CodeType == "GENDER" && x.Code == model.Gender).Select(x => x.CodeID).FirstOrDefault();
            var department = commonCode.Where(x => x.CodeType == "DEPARTMENT" && x.Code == model.Department).Select(x => x.CodeID).FirstOrDefault();
            var idType = commonCode.Where(x => x.CodeType == "IDPROOFTYPE" && x.Code == model.IDType).Select(x => x.CodeID).FirstOrDefault();
            Employee emp = new Employee();
            emp.EmployeeID =model.EmployeeID;
            emp.EmployedDate =model.HireDate;
            emp.FirstName =model.FirstName;
            emp.LastName =model.LastName;
            emp.Email =model.EmailID;
            emp.PhoneNo = model.PhoneNo;
            emp.Addressline1 =model.Address1;
            emp.Addressline2 =model.Address2;
            emp.City =model.City;
            emp.Statecode =model.State;
            emp.country =model.Country;
            emp.pincode =Convert.ToString(model.Pincode);
            emp.DOB =model.DateOfBirth;
            emp.IDType =idType;
            //emp.ReferenceID =model;
            emp.DeptType =department;
            //emp.Designation =model.Designation;
            //emp.Location =model;
        //emp.ManagerEmpFK=model;
        //gender
        emp.ContactName=model.ContactPerson;
        emp.ContactPhoneNo=model.ContactPersonNo;
        //emp.ISAppUser=model.;
        //emp.AppuserFK=model;
        emp.EmployeeName=model.FirstName+","+model.LastName;
    }
    }
}
