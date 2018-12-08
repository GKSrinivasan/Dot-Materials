using Laserbeam.Constant.HR;
using Laserbeam.Libraries.Common;
using Laserbeam.ProcessManager.Interfaces.Common;
using Laserbeam.UI.HR.Models;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Laserbeam.UI.HR.Controllers
{
    public class AccountServiceController : ApiController
    {
        private readonly IAccountProcessManager m_accountProcessManager;
        private readonly ISessionProcessManager m_sessionProcessManager;
        public AccountServiceController(IAccountProcessManager accountProcessManager, ISessionProcessManager sessionProcessManager)
        {
            m_accountProcessManager = accountProcessManager;
            m_sessionProcessManager = sessionProcessManager;
        }

        [HttpPost]
        public HttpResponseMessage ValidateUser(UserServiceModel loginModel)
        {
            var appConfigModel = m_accountProcessManager.GetAppSetting();
            var userCredentialStatus = m_accountProcessManager.ValidateUserCredential(loginModel.UserId, loginModel.Password, appConfigModel);
            var xtenantHeader = Request.Headers.GetValues("X-Tenant").SingleOrDefault();


            string userId,tenantUrl=null;
            if (userCredentialStatus == UserCredentialStatus.Valid || userCredentialStatus == UserCredentialStatus.ValidFirstTime)
            {
                MayaLink.TryEncrypt(loginModel.UserId, out userId);
                MayaLink.TryDecrypt(xtenantHeader, out xtenantHeader);
                tenantUrl = Request.RequestUri.AbsoluteUri.Replace(Request.RequestUri.AbsolutePath,"") + HttpContext.Current.Request.ApplicationPath + "/" + xtenantHeader + "/Account/DirectLogIn?ghkdcipl=" + HttpUtility.UrlEncode(userId);
            }
            UserServiceResponseModel response = new UserServiceResponseModel { TenantUrl = tenantUrl, UserStatus = userCredentialStatus };
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        [HttpPut]
        public async Task< HttpResponseMessage> ResetUserPassword(UserServiceModel passwordModel)
        {
            var appConfigModel = m_accountProcessManager.GetAppSetting();
            UserCredentialStatus userCredentialStatus = m_accountProcessManager.ResetUserCredential(passwordModel.UserId, appConfigModel);
            var userModel = m_accountProcessManager.GetUser(passwordModel.UserId);
            int i = 0;
            var xtenantHeader = Request.Headers.GetValues("X-Tenant").SingleOrDefault();
            string tenantUrl = null;                            
                MayaLink.TryDecrypt(xtenantHeader, out xtenantHeader);
                tenantUrl = Request.RequestUri.AbsoluteUri.Replace(Request.RequestUri.AbsolutePath, "") + HttpContext.Current.Request.ApplicationPath + "/" + xtenantHeader ;
            if(userCredentialStatus== UserCredentialStatus.Locked || userCredentialStatus == UserCredentialStatus.Valid)
                i=await m_accountProcessManager.SendEmailToUser(userModel, "Forget Password Reset", tenantUrl);            
            return Request.CreateResponse(HttpStatusCode.OK, i);
        }
    }
}
