using InvoiceProcessLayer.IProcessManager;
using InVoiceSoftware.Models;
using System;
using System.Text;
using System.Web.Mvc;

namespace InVoiceSoftware.Controllers
{
    public class AccountController : Controller
    {
        private readonly ICommonProcessManager m_commonProcessManager;

        public AccountController(ICommonProcessManager commonProcessManager)
        {
            m_commonProcessManager = commonProcessManager;
        }

        [HttpGet]
        public ActionResult Login(string tenantName)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            var data = m_commonProcessManager.GetAppUser(model.UserID);
            string message = "";
            if (data == null)
                message = "Invalid UserID";
            else
            {
                if (data.UserStatus != 1)
                {
                    if (data.UserStatus == 2)
                        message = "InActive User, Contact Administrator.";
                    else
                        message = "User Account is locked, Contact Administrator.";
                }
                string text = Encoding.UTF8.GetString(data.Passkey);
                if (text == model.Password)
                {
                    data.Logged = 1;
                    data.LoginAttempt = 1;
                }
                else
                {
                    data.LoginAttempt = data.LoginAttempt ?? 0 + 1;
                    if (data.LoginAttempt == 4)
                        data.UserStatus = 3;
                    message = "Incorrect Password";
                }
            }
            return View();
        }
    }
}