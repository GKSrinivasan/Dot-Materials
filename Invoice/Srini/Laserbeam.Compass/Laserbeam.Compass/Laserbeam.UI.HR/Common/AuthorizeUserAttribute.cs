// Copyright (c) 2015 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.

// Component Name: 	AuthorizeUser
// Description: 	Extension of AuthorizeAttribute functionality. Authorizes a user for particular resource based on UserAccess session  
// Author:		    Boobalan		
// Creation Date: 	12-01-2015

using System;
using System.Web.Mvc;
using System.Web.Routing;
using Laserbeam.Constant.HR;
using System.Web.Configuration;
using System.Configuration;
using System.IdentityModel.Services;
using System.Net;
using Laserbeam.Libraries.Common;
using System.IdentityModel.Claims;
using Laserbeam.BusinessObject.Common;

namespace Laserbeam.UI.HR.Common
{
    public class AuthorizeUserAttribute : AuthorizeAttribute
    {
        #region Fields

        // Author         :   Boobalan Ranganathan		
        // Creation Date  :   18-Mar-2016
        /// <summary>
        /// User roles seperated by comma
        /// </summary>
        private readonly string UserRoles;
        #endregion

        #region Constructors

        // Author         :   Boobalan Ranganathan		
        // Creation Date  :   18-Mar-2016
        // Modified By    :   Boobalan Ranganathan
        // Modified Date  :   02-Apr-2016
        // Comment        :   Set UserRoles default value as SuperAdmin and Admin
        /// <summary>
        /// Constructor with comma seperated roles to allow AuthorizeUserAttribute to validate user's role
        /// </summary>
        /// <param name="roles">Comma seperated roles as string</param>
        public AuthorizeUserAttribute(string roles=null)
        {
            UserRoles = roles ?? UserRoleConstants.AllAdminRoles;
        }

        #endregion

        #region OverRiden Methods

        /// <summary>
        /// Redirects the unauthorized user to the login page
        /// </summary>
        /// <param name="filterContext">Instance of AuthorizationContext</param>
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            var httpContext = filterContext.HttpContext;
            var response = httpContext.Response;
            var user = httpContext.User;
            response.SuppressFormsAuthenticationRedirect = true;
            response.TrySkipIisCustomErrors = true;
            if (!user.Identity.IsAuthenticated || httpContext.Session.Count == 0)
            {
                FederatedAuthentication.SessionAuthenticationModule.SignOut();
                redirectToLogIn(filterContext);
            }
            else if ((!user.IsInTenant(filterContext.GetTenant())))
            {
                redirectToError(filterContext);
            }
            else if(!user.IsInRoles(UserRoles))
            {
                redirectToAccessDenied(filterContext);
            }
            else if (user.IsTenantExpired())
            {
                redirectToTenantExpire(filterContext);
            }
            else
            {
                redirectToLogIn(filterContext);
            }
        }


        // Author         :   Boobalan Ranganathan		
        // Creation Date  :   18-Mar-2016
        /// <summary>
        /// Custom OnAuthorization event for all action calls
        /// </summary>
        /// <param name="filterContext">Instance of AuthorizationContext</param>
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var user = filterContext.HttpContext.User;
            string tenantName = filterContext.GetTenant();
            if (user.Identity.IsAuthenticated && user.IsInTenant(tenantName)  && !(filterContext.HttpContext.Session.Count == 0) && magicKeyValidForUser(filterContext) && !user.IsTenantExpired() && user.IsInRoles(UserRoles))
            {
                base.OnAuthorization(filterContext);
            }
            else
            {
                HandleUnauthorizedRequest(filterContext);
            }
        }

        #endregion

        #region Private Methods

        // Modified By    : Sanjeeviram Kamalakkannan
        // Modified date  : 09-Oct-2016
        // Condition changed for checking magic key
        // Ticket ID      :    PSP-12813
        private bool magicKeyValidForUser(AuthorizationContext filterContext)
        {
            var user = filterContext.HttpContext.User;
            string magicKey = Convert.ToString(filterContext.HttpContext.Request.QueryString["magicKey"]);
            string decryptedMagicKey = string.Empty;
            bool decryptStatus = MayaLink.TryDecrypt(magicKey, out decryptedMagicKey);
            bool isValidMagicKey;
            if (decryptStatus && decryptedMagicKey.Length > 0)
            {
                string[] magicKeyData = decryptedMagicKey.Split('|');
                isValidMagicKey = (magicKeyData.Length > 2 && user.GetClaimValue(ClaimTypes.NameIdentifier) == magicKeyData[0]);
            }
            else
            {
                isValidMagicKey = true;
            }
            return isValidMagicKey;
        }

        // Author         :   Boobalan Ranganathan		
        // Creation Date  :   01-Aug-2017
        // Ticket ID      :   PSP-13508
        /// <summary>
        /// Redirects unauthorized users to Login page
        /// </summary>
        /// <param name="filterContext">A valid AuthorizationContext object</param>
        private void redirectToLogIn(AuthorizationContext filterContext)
        {
            var authenSection = (AuthenticationSection)ConfigurationManager.GetSection("system.web/authentication");
            var routes = authenSection.Forms.LoginUrl.Split('/');
            var requestUrl = "../" + filterContext.RouteData.Values["controller"] + "/" + filterContext.RouteData.Values["action"];
            var magicKey = Convert.ToString(filterContext.HttpContext.Request.QueryString["magicKey"]);

            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                string redirectUrl = "../" + routes[1] + "/" + routes[2]; //+ "?";
                filterContext.Result = new JavaScriptResult { Script = "window.location ='" + redirectUrl + "'" };
            }
            else
            {
                filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.Redirect;
                RouteValueDictionary routeValues = new RouteValueDictionary();
                routeValues.Add("controller", routes[1]);
                routeValues.Add("action", routes[2]);
                if (!string.IsNullOrWhiteSpace(requestUrl))
                {
                    routeValues.Add("returnUrl", requestUrl);
                }

                if (!string.IsNullOrWhiteSpace(magicKey))
                {
                    routeValues.Add("magicKey", magicKey);
                }
                filterContext.Result = new RedirectToRouteResult(routeValues);
            }
        }

        // Author         :   Boobalan Ranganathan		
        // Creation Date  :   01-Aug-2017
        // Ticket ID      :   PSP-13508
        /// <summary>
        /// Redirects unauthorized users to Login page
        /// </summary>
        /// <param name="filterContext">A valid AuthorizationContext object</param>
        private void redirectToError(AuthorizationContext filterContext)
        {

            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                filterContext.Result = new JavaScriptResult { Script = "window.location ='../Error/Index'" };
            }
            else
            {
                filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.Redirect;
                RouteValueDictionary routeValues = new RouteValueDictionary();
                routeValues.Add("controller", "Error");
                routeValues.Add("action", "Index");
                filterContext.Result = new RedirectToRouteResult(routeValues);
            }
        }
        private void redirectToAccessDenied(AuthorizationContext filterContext)
        {

            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                filterContext.Result = new JavaScriptResult { Script = "window.location ='../Error/AccessDenied'" };
            }
            else
            {
                filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.Redirect;
                RouteValueDictionary routeValues = new RouteValueDictionary();
                routeValues.Add("controller", "Error");
                routeValues.Add("action", "AccessDenied");
                filterContext.Result = new RedirectToRouteResult(routeValues);
            }
        }
        private void redirectToTenantExpire(AuthorizationContext filterContext)
        {

            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                filterContext.Result = new JavaScriptResult { Script = "window.location ='../Account/ExpireInfo'" };
            }
            else
            {
                filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.Redirect;
                RouteValueDictionary routeValues = new RouteValueDictionary();
                routeValues.Add("controller", "Account");
                routeValues.Add("action", "ExpireInfo");
                filterContext.Result = new RedirectToRouteResult(routeValues);
            }
        }
        #endregion



    }
}