// Copyright (c) 2015 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.
// Component Name  :    AccountController
// Description     : 	Actions of login are specified.	
// Author          :	Roopan		
// Creation Date   : 	APR-01-2015

using Laserbeam.BusinessObject.Common;
using Laserbeam.Constant.HR;
using Laserbeam.Libraries.Common;
using Laserbeam.Libraries.Interfaces.Common;
using Laserbeam.Resource.HR.LoginResources;
using Laserbeam.SAML2;
using Laserbeam.UI.HR.Common;
using Laserbeam.UI.HR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IdentityModel.Services;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Laserbeam.ProcessManager.Interfaces.Common;
using System.Threading.Tasks;
using System.Web.Routing;
using System.Web.Configuration;
using System.Configuration;
using System.Net;
using Newtonsoft.Json.Linq;
using Laserbeam.Libraries.Core.Interfaces.Common;

namespace Laserbeam.UI.HR.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        #region Fields
        private readonly IAccountProcessManager m_accountProcessManager;
        private readonly IPasswordEncryption m_passwordEncryption;
        private SessionManager m_sessionManager = new SessionManager();
        private ISessionProcessManager m_sessionProcessManager;
        private ITenantCacheProvider m_tenantCacheProvider;
        #endregion
        #region Constructor
        //private string message;
        //private bool isDefaultPassword;
        // Author         :  Raja Ganapathy
        // Creation Date  :  05-Jul-2016  
        /// <summary>
        /// Initializes objects used in this class
        /// </summary>        
        /// <param name="accountProcessManager">accountProcessManager objects</param>
        /// <param name="encryptionLibrary">encryptionLibrary objects</param>
        /// <param name="sessionManager">sessionManager objects</param>
        /// <param name="sessionProcessManager">sessionProcessManager objects</param>
        public AccountController(IAccountProcessManager accountProcessManager, IPasswordEncryption encryptionLibrary, SessionManager sessionManager, ISessionProcessManager sessionProcessManager, ITenantCacheProvider tenantCacheProvider)
        {
            m_accountProcessManager = accountProcessManager;
            m_passwordEncryption = encryptionLibrary;
            m_sessionManager = sessionManager;
            m_sessionProcessManager = sessionProcessManager;
            m_tenantCacheProvider = tenantCacheProvider;
        }
        #endregion
        #region Public Methods Implementation
        // Author         :   Boobalan Ranganathan
        // Creation Date  :   Nov-10-2017
        public ActionResult DirectLogIn(string ghkdcipl)
        {
            string userId = ghkdcipl;
            MayaLink.TryDecrypt(userId, out userId);
            setLogInSession(userId);
            var userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
            setUserSessionAndClaims(userModel);
            var AppUserStatus = m_accountProcessManager.GetUser(userModel.UserID);
            return RedirectToRoute(getDefaultRedirectRoute(AppUserStatus));
        }

        [HttpGet]
        public ActionResult SSOLogIn()
        {
            AppConfigModel appConfig = m_accountProcessManager.GetAppSetting();
            bool isEnableSSO = m_tenantCacheProvider.GetBusinessSetting().SSOConfiguration.EnableSSO;

            if (!isEnableSSO)
            {
                ViewBag.IsLogout = false;
                RecaptchaViewbagData();
                return RedirectToAction("LogIn", new { mayaLink = Convert.ToString(Request.QueryString["mayaLink"]) });
            }

            var targetUrl = m_tenantCacheProvider.GetBusinessSetting().SSOConfiguration.IDPEndPoint;
            SsoViewModel data = new SsoViewModel();
            data.SAMLRequest = generateSamlRequest(targetUrl);
            data.TargetURL = targetUrl;
            return View(data);
        }

        [HttpPost]
        public ActionResult SSOLogIn(string SAMLResponse)
        {
            AppConfigModel appConfig = m_accountProcessManager.GetAppSetting();

            try
            {
                SAMLResponseReader samlResponseReader = new SAMLResponseReader(SAMLResponse, null);
                var samlAttributes = samlResponseReader.GetAttributeStatement().Attributes;
                string emailAddress = null;
                samlAttributes.TryGetValue("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress", out emailAddress);
                UserModel userData;
                var userCredentialStatus = m_sessionProcessManager.GetUserSSOLoginSession(emailAddress, out userData);
                if (userCredentialStatus == UserCredentialStatus.Valid)
                {
                    setLogInSession(userData.UserID);
                    var userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
                    setUserSessionAndClaims(userModel);
                    var AppUserStatus = m_accountProcessManager.GetUser(userModel.UserID);
                    return RedirectToRoute(getDefaultRedirectRoute(AppUserStatus));
                }
                else
                {
                    switch (userCredentialStatus)
                    {
                        case UserCredentialStatus.InvalidUser:
                            ModelState.AddModelError("errorsummary", LoginResource.SSOInvalidUser);
                            break;
                        case UserCredentialStatus.InActive:
                            ModelState.AddModelError("errorsummary", LoginResource.SSOInactiveUser);
                            break;
                        case UserCredentialStatus.Locked:
                            ModelState.AddModelError("errorsummary", LoginResource.SSOInactiveUser);
                            break;
                        default:
                            ModelState.AddModelError("errorsummary", LoginResource.SSOValidationMessage);
                            break;
                    }
                    ViewBag.IsLogout = false;
                    RecaptchaViewbagData();
                    return View("~/Views/Account/LogIn.cshtml", new LoginModel { });
                }
            }
            catch (Exception)
            {
                ViewBag.IsLogout = false;
                RecaptchaViewbagData();
                return RedirectToAction("LogIn", new { ssoFailed = true });
            }

        }

        // Author        :  Roopan		
        // Creation Date :  APR-03-2015
        // Modified By    :  N Thuishithaa
        // Modified Date  :  12-Oct-2016  
        // Reviewed By    :  Hari
        // Reviewed Date  :  12-Oct-2016
        // Comment        :  Added condition for InActive status and return to login screen with message
        /// <summary>
        /// Get login view
        /// </summary>
        /// <returns>Returns login view</returns>
        [HttpGet]
        public ActionResult LogIn(bool ssoFailed = false, string mayaLink = null)
        {
            SignOut();
            RecaptchaViewbagData();
            AppConfigModel appConfig = m_accountProcessManager.GetAppSetting();
            //if (appConfig == null)
            //    appConfig = setAppSettingSession();
            ViewBag.ToolVersion = appConfig.ToolVersion;
            ViewBag.ToolName = appConfig.ToolName;
            ViewBag.SSOFailed = ssoFailed;
            ViewBag.SSOEnabled = m_tenantCacheProvider.GetBusinessSetting().SSOConfiguration.EnableSSO;
            var encryptedMagicKey = mayaLink;
            bool decryptResult = false;
            string decryptedMagicKey;
            string[] decryptedValues = null;
            ViewBag.IsLogout = false;
            if (encryptedMagicKey != "")
            {
                decryptResult = MayaLink.TryDecrypt(encryptedMagicKey, out decryptedMagicKey);
                decryptedValues = (decryptResult) ? decryptedMagicKey.Split('|') : null;
            }
            if (decryptResult == true && decryptedValues.Length == 3 )
            {
                var userCredentialStatus = m_accountProcessManager.ValidateUserSecretKey(decryptedValues[0], decryptedValues[1], decryptedValues[2],appConfig,mayaLink);
                var passwordLength = m_accountProcessManager.getPasswordLength();
                
                ActionResult result;
              
                switch (userCredentialStatus)
                {
                    case UserCredentialStatus.InvalidUser:
                        setUserStatusMessage(userCredentialStatus);
                        result = View(new LoginModel { UserID = decryptedValues[0] });
                        break;
                    case UserCredentialStatus.InActive:
                        setUserStatusMessage(userCredentialStatus);
                        result = View(new LoginModel { UserID = decryptedValues[0] });
                        break;
                    case UserCredentialStatus.Valid:
                        result = View("~/Views/Account/ChangePassword.cshtml", new ChangePasswordModel { UserID = decryptedValues[0],  PasswordLength = passwordLength });
                        break;
                    default:
                        result = View(new LoginModel { UserID = decryptedValues[0] });
                        break;
                }
                return result;
            }
            return View();
        }

        // Author        :  Hariharasubramaniyan C	
        // Creation Date :  APR-09-2017
        /// <summary>
        /// User validation on run time
        /// </summary>
        /// <param name="model"> Model having user id for login</param>
        /// <returns>Returns validation for the current user ID</returns>
        [HttpPost]
        public JsonResult UserValidation(LoginModel model)
        {
            bool b = m_accountProcessManager.UserValidation(model.UserID);
            return (b == true) ? Json(true) : Json(false);
        }


        // Author        :  Roopan		
        // Creation Date :  APR-09-2015
        /// <summary>
        /// Get forgot password view
        /// </summary>
        /// <param name="userID">ID of user</param>
        /// <returns>Returns forgot password view</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(string hndForgotPasswordUserId)
        {
            AppConfigModel appConfig = m_accountProcessManager.GetAppSetting();
            //if (appConfig == null)
            //    appConfig = setAppSettingSession();
            ViewBag.ToolVersion = appConfig.ToolVersion;
            ViewBag.ToolName = appConfig.ToolName;
            ForgotPasswordModel forgotPasswordmodel = new ForgotPasswordModel
            {
                UserID = hndForgotPasswordUserId
            };
            return View(forgotPasswordmodel);
        }

        // Author        :  Hari		
        // Creation Date :  Nov-10-2017
        // Comment       :  Send Contact email
        [HttpPost,ValidateAntiForgeryToken]
        public ActionResult SendContactMail()
        {
            string tenant = this.GetTenant();
            string masterEmail = ConfigurationManager.AppSettings["LaserbeamContactMail"].ToString();
            m_accountProcessManager.SendContactEmail(tenant, masterEmail);
            ViewBag.SSOEnabled = m_tenantCacheProvider.GetBusinessSetting().SSOConfiguration.EnableSSO;
            RecaptchaViewbagData();
            return RedirectToAction("Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ForgotPasswordModel model)
        {
            string requestURL = this.GetTenantUrl();
            AppConfigModel appConfig = m_accountProcessManager.GetAppSetting();
            //if (appConfig == null)
            //    appConfig = setAppSettingSession();
            ViewBag.ToolVersion = appConfig.ToolVersion;
            ViewBag.ToolName = appConfig.ToolName;
            ViewBag.SSOEnabled = m_tenantCacheProvider.GetBusinessSetting().SSOConfiguration.EnableSSO;
            int i = 0;
            ActionResult result = View("~/Views/Account/ForgotPassword.cshtml", model);
            if (ModelState.IsValid)
            {
                UserCredentialStatus userCredentialStatus = m_accountProcessManager.ResetUserCredential(model.UserID, appConfig);
                var userModel = m_accountProcessManager.GetUser(model.UserID);
                switch (userCredentialStatus)
                {
                    case UserCredentialStatus.Locked:
                        i = await m_accountProcessManager.SendEmailToUser(userModel, "Forget Password Reset", requestURL);
                        if (i == 0)
                            ModelState.AddModelError("errorsummary", LoginResource.MessageForgotPasswordFailure);
                        else
                            ViewData["SuccessMessage"] = LoginResource.MessageForgotPasswordSuccess;
                        ViewBag.IsLogout = false;
                        RecaptchaViewbagData();
                        result = View("~/Views/Account/LogIn.cshtml", new LoginModel { UserID = model.UserID });
                        break;

                    case UserCredentialStatus.Valid:
                        i = await m_accountProcessManager.SendEmailToUser(userModel, "Forget Password Reset", requestURL);
                        if (i == 0)
                            ModelState.AddModelError("errorsummary", LoginResource.MessageForgotPasswordFailure);
                        else
                            ViewData["SuccessMessage"] = LoginResource.MessageForgotPasswordSuccess;
                        ViewBag.IsLogout = false;
                        RecaptchaViewbagData();
                        result = View("~/Views/Account/LogIn.cshtml", new LoginModel { UserID = model.UserID });
                        break;
                    default:
                        setUserStatusMessage(userCredentialStatus);
                        break;
                }
            }
            return result;
        }

        // Author        :  Roopan		
        // Creation Date :  APR-09-2015
        /// <summary>
        /// If validation success it change the user password and redirect to login page
        /// else it return change password view 
        /// </summary>
        /// <param name="changePasswordModel">It has user credential with new password and confirm password</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ChangePassword(string UserID)
        {
            AppConfigModel appConfig = m_accountProcessManager.GetAppSetting();
            //if (appConfig == null)
            //    appConfig = setAppSettingSession();
            ViewBag.ToolVersion = appConfig.ToolVersion;
            ViewBag.ToolName = appConfig.ToolName;
            ChangePasswordModel changePasswordModel = new ChangePasswordModel
            {
                UserID = UserID
            };
            return View(changePasswordModel);
        }

        [HttpPost]
        public  ActionResult ChangePasswordTo(ChangePasswordModel changePasswordModel)
        {
            AppConfigModel appConfig = m_accountProcessManager.GetAppSetting();
            //if (appConfig == null)
            //    appConfig = setAppSettingSession();
            ViewBag.ToolVersion = appConfig.ToolVersion;
            ViewBag.ToolName = appConfig.ToolName;
            
            ActionResult result = View("~/Views/Account/ChangePassword.cshtml", changePasswordModel);
            if (ModelState.IsValid)
            {
                    var userCredentialStatus = m_accountProcessManager.ChangeUserCredential(changePasswordModel.UserID, changePasswordModel.NewPassword, appConfig);
                    switch (userCredentialStatus)
                    {
                        case UserCredentialStatus.PasswordChanged:
                            setLogInSession(changePasswordModel.UserID);
                            var userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
                            setUserSessionAndClaims(userModel);
                            var AppUserStatus = m_accountProcessManager.GetUser(userModel.UserID);
                            result = RedirectToRoute(getDefaultRedirectRoute(AppUserStatus));
                            break;
                        default:
                            setUserStatusMessage(userCredentialStatus);
                            break;
                    }
            }
            return result;
        }
        // Author        :  Roopan		
        // Creation Date :  APR-03-2015
        // Modified By    :  N Thuishithaa
        // Modified Date  :  04-August-2016 
        // Reviewed By    :  Praveen Kumar
        // Reviewed Date  :  20-August-2016 
        // Comment        :  Pass ToolVersion,ToolName and lockedDate to view
        /// <summary>
        /// Redirects to dashboard
        /// </summary>
        /// <param name="LoginModel">It has user credentials</param>
        /// <returns>Redirects to dashboard if all validations are passed</returns>

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel loginModel)
        {
          
            AppConfigModel appConfigModel = m_accountProcessManager.GetAppSetting();
            //if (appConfigModel == null)
            //    appConfigModel = setAppSettingSession();
            ViewBag.ToolVersion = appConfigModel.ToolVersion;
            ViewBag.IsLogout = false;
            ViewBag.ToolName = appConfigModel.ToolName;
            ViewBag.SSOEnabled = m_tenantCacheProvider.GetBusinessSetting().SSOConfiguration.EnableSSO;
            ActionResult result = View();
            bool isRecaptcha = false;
            string siteKey = ConfigurationManager.AppSettings["ReCaptchaSiteKey"].ToString();
            bool isReCaptchaEnabled = ConfigurationManager.AppSettings["ReCaptchaValidation"].ToLower() == "yes" ? true : false;
            ViewBag.IsRecaptcha = isReCaptchaEnabled;
            ViewBag.Sitekey = siteKey;
            if (isReCaptchaEnabled == true)
                isRecaptcha = ReCaptchaValidation();
            if (isRecaptcha == false && isReCaptchaEnabled == true)
            {
                ModelState.AddModelError("ReCaptcha", "Your recaptcha verification process is failed. Please try again");
                result = View(new LoginModel { UserID = loginModel.UserID});
            }
            else
            {
                var userCredentialStatus = m_accountProcessManager.ValidateUserCredential(loginModel.UserID, loginModel.UserPassword, appConfigModel);
                switch (userCredentialStatus)
                {
                    case UserCredentialStatus.InvalidUser:
                        setUserStatusMessage(userCredentialStatus);
                        break;
                    case UserCredentialStatus.InActive:
                        setUserStatusMessage(userCredentialStatus);
                        break;
                    case UserCredentialStatus.InvalidPassword:
                        setUserStatusMessage(userCredentialStatus);
                        result = View(new LoginModel { UserID = loginModel.UserID});
                        break;
                    case UserCredentialStatus.Valid:
                        setLogInSession(loginModel.UserID);
                        var userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
                        setUserSessionAndClaims(userModel);
                        var AppUserStatus = m_accountProcessManager.GetUser(userModel.UserID);
                        result = RedirectToRoute(getDefaultRedirectRoute(AppUserStatus));
                        break;
                    case UserCredentialStatus.Locked:
                        setUserStatusMessage(userCredentialStatus);
                        result = View(new LoginModel { UserID = loginModel.UserID});
                        break;
                    default:
                        setUserStatusMessage(userCredentialStatus);
                        DateTime? lockedDate = m_accountProcessManager.GetLockedDate(loginModel.UserID.TrimEnd());
                        lockedDate = lockedDate.Value.AddMinutes(appConfigModel.PasswordDisableDuration);
                        result = View(new LoginModel { UserID = loginModel.UserID, LockedDate = lockedDate });
                        break;
                }
            }
            return result;
        }


        [AuthorizeUser(UserRoleConstants.AllRoles)]
        [HttpGet]
        public ActionResult LogOut()
        {
            RecaptchaViewbagData();
            var user = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
            m_accountProcessManager.PushUserLogoutDetails(user.UserNum, user.IsSSOLogin);
            SignOut();
            ViewBag.IsLogout = true;
            ViewBag.SSOEnabled = m_tenantCacheProvider.GetBusinessSetting().SSOConfiguration.EnableSSO;
            return View("Login");
        }

        [AllowAnonymous]
        public ActionResult ExpireInfo()
        {
            return View();
        }

        // Modified By    :  Hari
        // Modified Date  :  21-Nov-2017  
        // Comment        :  Sign out logic
        public void SignOut()
        {
            Session.Clear();
            Session.Abandon();
            HttpCookie cookie2 = new HttpCookie("ASP.NET_SessionId", "");
            cookie2.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie2);

            FormsAuthentication.SignOut();
            HttpCookie cookie1 = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            cookie1.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie1);
        }

        #endregion
        #region Private Methods Implementation
        // Modified By    :  N Thuishithaa
        // Modified Date  :  04-August-2016  
        // Reviewed By    :  Praveen Kumar
        // Reviewed Date  :  20-August-2016 
        // Comment        :  Pass message to view if user has crossed the failedloginattempts
        private void setUserSessionAndClaims(UserModel userModel)
        {
          
            AppConfigModel appConfig = m_accountProcessManager.GetAppSetting();
            var isTenantExpired = Convert.ToBoolean(DateTime.Now.Date >= appConfig.TenantExpireDate.Date);
            var claims = new List<Claim>();            
            claims.Add(new Claim(ClaimTypes.Name, userModel.UserName));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, userModel.UserID));
            claims.Add(new Claim(ClaimTypes.Role, userModel.KeyName));
            claims.Add(new Claim(SiteExtension.Tenant, this.GetTenant()));
            claims.Add(new Claim(SiteExtension.TenantExpire, isTenantExpired.ToString()));

            var claimsIdentity = new ClaimsIdentity(claims, "Forms");
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            var token = new System.IdentityModel.Tokens.SessionSecurityToken(claimsPrincipal);
            FederatedAuthentication.SessionAuthenticationModule.WriteSessionTokenToCookie(token);
        }

        private void setUserStatusMessage(UserCredentialStatus userStatus)
        {
            
            AppConfigModel appConfigModel = m_accountProcessManager.GetAppSetting();
            var message = "You have tried "+appConfigModel.LoginAttempt+" failed login attempts. Please try again after " + appConfigModel.PasswordDisableDuration + " minutes, if you don’t remember the password please click on the Forgot Password?";
            switch (userStatus)
            {
                case UserCredentialStatus.InvalidUser:
                    ModelState.AddModelError("errorsummary", LoginResource.MessageInvalidUser);
                    break;
                case UserCredentialStatus.InvalidPassword:
                    ModelState.AddModelError("errorsummary", LoginResource.MessageInvalidPassword);
                    break;
                case UserCredentialStatus.Locked:
                    ModelState.AddModelError("errorsummary", message);
                    break;
                case UserCredentialStatus.InActive:
                    ModelState.AddModelError("errorsummary", LoginResource.MessageInactive);
                    break;
                default:
                    break;
            }
        }
      
        private void setLogInSession(string sessionId, UserModel userModel = null)
        {
            
            AppConfigModel appConfig = m_accountProcessManager.GetAppSetting();

            int year = Convert.ToInt32(appConfig.CurrentYear);
            if (userModel == null || ( userModel != null && userModel.UserID != sessionId))
                userModel = m_sessionProcessManager.GetUserSession(sessionId,year);
            m_accountProcessManager.PutUserLogInDetails(userModel.UserNum);
            EmployeeModel employeeModel = m_sessionProcessManager.GetEmployeeSession(userModel.EmployeeNum, year);
            List<UserRights> userAccess = m_sessionProcessManager.GetUserAccess(userModel.UserNum);
            m_sessionManager.SetSession<UserModel>(SessionConstants.UserModel, userModel);
            m_sessionManager.SetSession<EmployeeModel>(SessionConstants.SelectedEmployeeModel, employeeModel);
            m_sessionManager.SetSession<EmployeeModel>(SessionConstants.EmployeeModel, employeeModel);
            m_sessionManager.SetSession<List<UserRights>>(SessionConstants.UserAccess, userAccess);
            m_sessionManager.SetSession<int>(SessionConstants.LoggedSelectedUserNum, userModel.UserNum);
            m_sessionManager.SetSession<string>(SessionConstants.LoggedSelectedUserText, userModel.UserID + " - " + userModel.UserName);
            m_sessionManager.SetSession<int>(SessionConstants.LoggedSelectedUserRoleNum, userModel.UserRoleNum);            
        }
        
        // Author        :  Boobalan		
        // Creation Date :  MAY-06-2015
        /// <summary>
        /// Generates SAML request to be sent to SSO URL and signs the request with X.509 certificate
        /// </summary>
        /// <param name="samlRequest">An instance of LogInModel to append SAML request to it</param>
        private string generateSamlRequest(string targetUrl)
        {
            SAMLAuthenRequestConfiguration _samlRequestConfig = new SAMLAuthenRequestConfiguration();
            string ssoIdentityURL = this.GetTenantUrl() + "/sso";
            _samlRequestConfig.AssertionConsumerServiceURL = ssoIdentityURL;
            _samlRequestConfig.Issuer = ssoIdentityURL;
            _samlRequestConfig.ProviderName = ConfigurationManager.AppSettings.Get("ProviderName");
            _samlRequestConfig.NameIDPolicy = NameIDPolicy.None;
            _samlRequestConfig.TargetURL = targetUrl;
            SAMLAuthenRequestWriter _authenRequest = new SAMLAuthenRequestWriter(_samlRequestConfig);
            return _authenRequest.GetSAMLAuthenRequestInBase64Format();
        }

        private RouteValueDictionary getDefaultRedirectRoute(UserModel userModel)
        {
            AppConfigModel appConfig = m_accountProcessManager.GetAppSetting();
            var authenSection = (AuthenticationSection)ConfigurationManager.GetSection("system.web/authentication");
                var routes = authenSection.Forms.DefaultUrl.Split('/');
                RouteValueDictionary routeValues = new RouteValueDictionary();
                routeValues.Add("controller", "Dashboard");
                routeValues.Add("action", "DashboardView");
                if (userModel.WizardStepsCompleted < 5 && userModel.IsDefaultUser == true)
                {
                    routeValues["controller"] = "Wizard";
                    routeValues["action"] = "Home";
                }
                return routeValues;
        }

        private bool ReCaptchaValidation()
        {
            string secretKey = ConfigurationManager.AppSettings["ReCaptchaSecretKey"].ToString();
            var response = Request["g-recaptcha-response"];
            var client = new WebClient();
            var result = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secretKey, response));
            var obj = JObject.Parse(result);
            var status = (bool)obj.SelectToken("success");
            return status;
        }

        private void RecaptchaViewbagData()
        {
            string siteKey = ConfigurationManager.AppSettings["ReCaptchaSiteKey"].ToString();
            bool isReCaptcha = ConfigurationManager.AppSettings["ReCaptchaValidation"].ToLower() == "yes" ? true : false;
            ViewBag.IsRecaptcha = isReCaptcha;
            ViewBag.Sitekey = siteKey;
        }

        #endregion

    }
}