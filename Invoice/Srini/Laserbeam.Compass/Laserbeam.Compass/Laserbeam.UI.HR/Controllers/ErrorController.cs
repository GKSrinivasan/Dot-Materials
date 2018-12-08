using Laserbeam.UI.HR.Common;
using System.Web.Mvc;

namespace Laserbeam.UI.HR.Controllers
{
    [AllowAnonymous]
    public class ErrorController : Controller
    {
        #region Action

        //// Author         :   Boobalan Ranganathan		
        //// Creation Date  :   19-Nov-2015
        ///// <summary>
        ///// Action to get Error view
        ///// </summary>
        ///// <param name="errorModel">Instance of ErrorModel</param>
        ///// <returns>Returns error view</returns>
        //[AllowAnonymous]
        //[HttpGet]
        //public ActionResult Index(ErrorModel errorModel)
        //{
        //    return View("~/View/Shared/Error.cshtml", errorModel);
        //}
        //[AllowAnonymous]
        //[HttpGet]
        //public ActionResult NotFound()
        //{
        //    return View();
        //}
        // Author         :   Karthikeyan Shanmugam		
        // Creation Date  :   27-Jul-2017
        // Ticket ID      :   PSP-13508
        /// <summary>
        /// Action to get UnhandledError view
        /// </summary>
        /// <returns>Returns UnhandledError view</returns>
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View("~/Views/Error/UnhandledError.cshtml");
        }

        [AllowAnonymous]
        public ActionResult AccessDenied()
        {
            return View("~/Views/Error/AccessDenied.cshtml");
        }

        // Author         :   Karthikeyan Shanmugam		
        // Creation Date  :   27-Jul-2017
        // Ticket ID      :   PSP-13508
        /// <summary>
        /// Action to get TenantNotFound view
        /// </summary>
        /// <returns>Returns TenantNotFound view</returns>
        [AllowAnonymous]
        public ActionResult TenantNotFound()
        {
            return View("~/Views/Error/TenantNotFound.cshtml");
        }

        // Author         :   Boobalan Ranganathan		
        // Creation Date  :   08-Aug-2017
        // Ticket ID      :   PSP-13508
        /// <summary>
        /// Get javascript result to redirect to error page
        /// </summary>
        /// <returns>Returns JavaScriptResult</returns>
        [AllowAnonymous]
        public JavaScriptResult RedirectToError()
        {
            return JavaScript("window.location = '../Error/Index'");
        }

        // Author         :   Boobalan Ranganathan		
        // Creation Date  :   08-Aug-2017
        // Ticket ID      :   PSP-13508
        /// <summary>
        /// Get javascript result to redirect to tenant not found page
        /// </summary>
        /// <returns>Returns JavaScriptResult</returns>
        [AllowAnonymous]
        public JavaScriptResult RedirectToTenantNotFound()
        {
            return JavaScript("window.location = '../Error/TenantNotFound'");
        }
        #endregion
    }
}