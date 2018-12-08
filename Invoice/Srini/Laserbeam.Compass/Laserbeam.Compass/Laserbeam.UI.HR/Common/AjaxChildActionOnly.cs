
// Copyright (c) 2015 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.

// Component Name:  AjaxChildActionOnlyAttribute
// Description: 	Extension of ChildActionOnlyAttribute functionality. This allows a request to be either an Ajax call or a ChildAction call
// Author:		    Boobalan		
// Creation Date: 	12-01-2015
      
using System.Web.Mvc;

namespace Laserbeam.UI.HR.Common
{
    public class AjaxChildActionOnlyAttribute : ActionMethodSelectorAttribute
    {
        
        // Author        :  Boobalan		
        // Creation Date :  12-01-2015
        /// <summary>
        /// Event to validate a request is either Ajax call or a ChildAction call
        /// </summary>
        /// <param name="controllerContext">An instance of ControllerContex, automatically passed by MVC framework</param>
        /// <param name="methodInfo">An instance of MethodInfo, automatically passed by MVC framework</param>
        /// <returns>Returns true if the request is Ajax or ChildAction call else retruns false</returns>
        public override bool IsValidForRequest(ControllerContext controllerContext, System.Reflection.MethodInfo methodInfo)
        {
            return controllerContext.RequestContext.HttpContext.Request.IsAjaxRequest() || controllerContext.IsChildAction;
        }
    }
}