using Laserbeam.BusinessObject.Common;
using Laserbeam.Constant.HR;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;

namespace Laserbeam.UI.HR.Common
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class NoDirectAccessAttribute: ActionFilterAttribute
    {
        private SessionManager m_sessionManager = new SessionManager();
       
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
           List<UserRights> user = m_sessionManager.GetSession<List<UserRights>>(SessionConstants.UserAccess); 
           List<string> termsList = new List<string>();
           if (user != null)
           {
               foreach (UserRights userRight in user)
                   termsList.Add(userRight.MenuLinks != null ? userRight.MenuLinks.ToLower() : null);
               string[] links = termsList.ToArray();
               if (!termsList.Contains("/" + filterContext.RouteData.Values["controller"].ToString().ToLower() + "/" + filterContext.RouteData.Values["action"].ToString().ToLower()))
               {
                   filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Account", action = "Login", area = "" }));
               }
           }
        }
    }
}