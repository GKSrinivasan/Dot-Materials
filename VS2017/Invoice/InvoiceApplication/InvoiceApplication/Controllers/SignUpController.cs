using InvoiceApplication.Entity;
using InvoiceApplication.Models;
using System.Linq;
using System.Web.Mvc;

namespace InvoiceApplication.Controllers
{
    public class SignUpController : Controller
    {
        TenantMaster dbContext = new TenantMaster();

        // GET: SignUp
        [HttpGet]
        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public JsonResult SignUp(SignUpModel sign)
        {
            var tenantName = dbContext.Tenants.Where(x => x.TenantName == sign.TenantName).FirstOrDefault();
            if (tenantName != null)
                return Json("Tenant Exists", JsonRequestBehavior.AllowGet);
            Tenant tenant = new Tenant();
            tenant.FirstName = sign.FirstName;
            tenant.LastName = sign.LastName;
            tenant.PhoneNo = sign.PhoneNumber;
            tenant.TenantName = sign.TenantName;
            tenant.UserID = sign.UserID;
            tenant.Email = sign.EmailAddress;
            tenant.CompanyName = sign.CompanyName;
            dbContext.Tenants.Add(tenant);
            dbContext.SaveChanges();
            return Json("Successfully Tenant Created", JsonRequestBehavior.AllowGet);
        }

    }
}