using InvoiceProcessLayer.IProcessManager;
using InVoiceSoftware.Models;
using System;
using System.Text;
using System.Web.Mvc;
using System.Web.SessionState;

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
        public string Login(LoginModel model)
        {
            var data = m_commonProcessManager.GetAppUser(model.UserID);
            System.Web.HttpContext.Current.Session.Add("UserName", data.UserName);
            System.Web.HttpContext.Current.Session.Add("UserID", data.UserID);
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
                else
                {
                    string text = Encoding.UTF8.GetString(data.Passkey);
                    if (text == model.Password)
                    {
                        data.Logged = 1;
                        data.LoginAttempt = 1;
                        data.Lastlogin = DateTime.Now;
                        message = "Success";
                    }
                    else
                    {
                        var attempt = data.LoginAttempt ?? 0;
                        data.LoginAttempt = Convert.ToInt16(attempt+1);
                        if (data.LoginAttempt == 4)
                            data.UserStatus = 3;
                        message = "Incorrect Password";
                    }
                    m_commonProcessManager.UpdateAppUser(data);
                }
            }
            return message;
        }

        public ActionResult Logout()
        {
            var userID = (string)System.Web.HttpContext.Current.Session["UserID"];
            var data = m_commonProcessManager.GetAppUser(userID);
            data.Lastlogout = DateTime.Now;
            m_commonProcessManager.UpdateAppUser(data);
            return View("Login");
        }
    }
}