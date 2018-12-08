using InvoiceDataLayer.BussinessModel;
using InvoiceProcessLayer.IProcessManager;
using InVoiceSoftware.Models;
using System.Linq;
using System.Web.Mvc;

namespace InVoiceSoftware.Controllers
{
    public class UserDetailController : Controller
    {
        private readonly ICommonProcessManager m_commonProcessManager;

        public UserDetailController(ICommonProcessManager commonProcessManager)
        {
            m_commonProcessManager = commonProcessManager;

        }
        
        [HttpGet]
        public ActionResult UserDetail()
        {
            return View();
        }

        //public JsonResult GetUsers(string word, int page, int rows, string searchString)
        //{
        //    var Results = m_commonProcessManager.GetAllEmployee().Select(a => new { a.EmployeeID,a.EmployeeName,a.DOB,a.Email,a.Addressline1,a.City,a.Statecode,a.pincode }); 

        //        int totalRecords = Results.Count();
                  
        //        var jsonData = new
        //        {
        //            records = totalRecords,
        //            rows = Results
        //        };
        //        return Json(jsonData, JsonRequestBehavior.AllowGet);
        //}

        //public JsonResult GetStudents(string sidx="", string sort="", int page=1, int rows=1)
        //{
        //    sort = (sort == null) ? "" : sort;
        //    int pageIndex = Convert.ToInt32(page) - 1;
        //    int pageSize = rows;

        //    var StudentList = m_commonProcessManager.GetAllEmployee().Select(a => new { a.EmployeeID, a.EmployeeName, a.DOB, a.Email, a.Addressline1, a.City, a.Statecode, a.pincode });
        //    int totalRecords = StudentList.Count();
        //    var totalPages = (int)Math.Ceiling((float)totalRecords / (float)rows);
        //    if (sort.ToUpper() == "DESC")
        //    {
        //        StudentList = StudentList.OrderByDescending(t => t.EmployeeName);
        //        StudentList = StudentList.Skip(pageIndex * pageSize).Take(pageSize);
        //    }
        //    else
        //    {
        //        StudentList = StudentList.OrderBy(t => t.EmployeeName);
        //        StudentList = StudentList.Skip(pageIndex * pageSize).Take(pageSize);
        //    }
        //    var jsonData = new
        //    {
        //        total = totalPages,
        //        page,
        //        records = totalRecords,
        //        rows = StudentList
        //    };
        //    return Json(jsonData, JsonRequestBehavior.AllowGet);
        //}

        [HttpPost]
        public JsonResult GetEmployeeCommonCodes()
        {
            var data = m_commonProcessManager.GetCommonCodes().Where(x=>x.CodeType == "GENDER" || x.CodeType== "IDPROOFTYPE" || x.CodeType == "DEPARTMENT" || x.CodeType == "USERSTATUS" || x.CodeType == "USERROLL").ToList();
            return Json(data);
        }

        [HttpPost]
        public JsonResult GetCurrency()
        {
            var data = m_commonProcessManager.GetCurrencies().ToList();
            return Json(data);
        }

        [HttpPost]
        public JsonResult GetCompanyLocation()
        {
            var data = m_commonProcessManager.GetCompanies().Select(x=>x.City).Distinct().ToList();
            return Json(data);
        }

        [HttpPost]
        public JsonResult GetVendorCommonCodes()
        {
            var data = m_commonProcessManager.GetCommonCodes().Where(x => x.CodeType == "BUSNTYPE").ToList();
            return Json(data);
        }

        [HttpPost]
        public JsonResult GetCustomerCommonCodes()
        {
            var data = m_commonProcessManager.GetCommonCodes().Where(x => x.CodeType == "GENDER" || x.CodeType == "IDPROOFTYPE").ToList();
            return Json(data);
        }

        [HttpPost]
        public JsonResult GetGroupCommonCodes()
        {
            var data = m_commonProcessManager.GetCommonCodes().Where(x => x.CodeType == "STATEMENTTYP").ToList();
            return Json(data);
        }

        [HttpPost]
        public void Employeedetails(EmployeeModel model)
        {

        }

        [HttpPost]
        public void Vendordetails(VendorModel model)
        {

        }

        [HttpPost]
        public void Customerdetails(CustomerModel model)
        {

        }

        [HttpPost]
        public void AccountGroup(AccountGroupModel model)
        {

        }
    }
}