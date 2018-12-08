using System.Net;
using System.Web.Mvc;
using System.Web.Routing;

namespace Laserbeam.UI.HR
{
    public class GlobalHandleErrorAttribute : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext exceptionContext)
        {
            LogApplicatonError applicationErrorLogger = new LogApplicatonError();
            applicationErrorLogger.RecordError(exceptionContext.Exception);
            exceptionContext.HttpContext.Response.TrySkipIisCustomErrors = true;
            exceptionContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            if (exceptionContext.HttpContext.Request.IsAjaxRequest())
            {
                exceptionContext.Result = new JavaScriptResult { Script = "window.location = '../Error/Index'" };
            }
            else {
                RouteValueDictionary routeValues = new RouteValueDictionary();
                routeValues.Add("controller", "Error");
                routeValues.Add("action", "Index");
                exceptionContext.Result = new RedirectToRouteResult(routeValues);
            }
            exceptionContext.ExceptionHandled = true;
        }
    }
}