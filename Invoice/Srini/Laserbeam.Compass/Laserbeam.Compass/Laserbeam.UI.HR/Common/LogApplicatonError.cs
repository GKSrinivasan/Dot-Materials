using System;
using System.Configuration;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using Laserbeam.UI.HR.ServiceReference;
using Laserbeam.BusinessObject.Common;

namespace Laserbeam.UI.HR
{
    public class LogApplicatonError
    {
        public void RecordError(Exception exception)
        {
            try
            {
                if ((typeof(ApplicationException) != exception.GetType()))
                {
                    string message = string.Empty;
                    ErrorLogClient errorLog = new ErrorLogClient();
                    ExceptionLogModel exceptionLog = new ExceptionLogModel();
                    var userModel = HttpContext.Current.Session["UserSession"];
                    exceptionLog.ApplicationName = ((AuthenticationSection)ConfigurationManager.GetSection("system.web/authentication")).Forms.Name;
                    exceptionLog.AssemblyName = exception.TargetSite.ReflectedType.Name;
                    exceptionLog.ExceptionDateTime = DateTime.Now;
                    exceptionLog.ExceptionType = exception.GetType().Name;
                    exceptionLog.IsServerSide = true;
                    exceptionLog.MethodName = exception.TargetSite.Name;
                    exceptionLog.SessionID = HttpContext.Current.Session.SessionID;
                    exceptionLog.URL = HttpContext.Current.Request.Url.AbsolutePath;
                    exceptionLog.UserID = (userModel == null) ? "" : ((UserModel)userModel).UserID;
                    if (exception.GetType() == typeof(DbEntityValidationException))
                    {
                        var entityErrors = ((DbEntityValidationException)exception).EntityValidationErrors;
                        foreach (var entityError in entityErrors)
                        {
                            var fieldErrors = entityError.ValidationErrors;
                            foreach (var fieldError in fieldErrors)
                            {
                                message += entityError.Entry.Entity.GetType().Name.Split('_')[0] + " : " + fieldError.PropertyName + " : " + fieldError.ErrorMessage + " <br/> ";
                            }
                        }
                    }
                    else message = exception.Message;
                    message = string.IsNullOrWhiteSpace(message) ? exception.Message : message;
                    exceptionLog.ExceptionMessage = (message.Count() > 1000) ? message.Substring(0, 1000) : message;
                    if (exception.InnerException != null)
                    {
                        exceptionLog.InnerExceptionType = exception.InnerException.GetType().Name;
                        exceptionLog.InnerExceptionMessage = exception.InnerException.Message;
                    }
                    errorLog.LogSingleExceptionAsync(exceptionLog);
                }
            }
            catch
            {

            }
        }
    }
}