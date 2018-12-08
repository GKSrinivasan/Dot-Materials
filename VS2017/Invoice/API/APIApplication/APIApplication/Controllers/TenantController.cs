using APIApplication.Entity;
using Nitin.Sms.Api;
using System;
using System.Web.Http;

namespace APIApplication.Controllers
{
    public class TenantController : ApiController
    {
        TenantEntities dbContext = new TenantEntities();

        [HttpPost]
        public void AddNewTenant()
        {
            var contactNo = dbContext.USP_PUT_NewDataBase();
            Way2Sms way2Sms = new Way2Sms("8807865404", "kamalakannan");
            var a = way2Sms.Login();
            var b = way2Sms.SendSms(Convert.ToString(contactNo), "Hi, Your tenant has been successfully create. to login in to tool use this URL:http://localhost:53565/");
        }
    }
}
