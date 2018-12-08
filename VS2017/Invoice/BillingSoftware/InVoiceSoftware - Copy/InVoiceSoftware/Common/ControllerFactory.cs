using InvoiceDependencyLibrary.DependencyResolution;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace InVoiceSoftware.Common
{
    public class ControllerFactory : DefaultControllerFactory
    {
        #region Fields

        private readonly IContainer m_container;

        #endregion

        #region Constructor

        public ControllerFactory(IContainer container)
        {
            m_container = container;
        }

        #endregion

        #region Methods

        // Author         :   Boobalan Ranganathan		
        // Creation Date  :   14-Sep-2015
        /// <summary>
        /// Creates controller instance based on tenant
        /// </summary>
        /// <param name="requestContext">Instance of RequestContext from Mvc framework</param>
        /// <param name="controllerType">Type of the controller</param>
        /// <returns>Returns instance of a controller</returns>
        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            if (controllerType == null || requestContext == null) return null;
            string tenantName = Convert.ToString(requestContext.RouteData.Values["tenantName"]);
            IContainer m_nestedContainer = m_container.GetNestedContainer(tenantName);
            if (!DefaultRegistry.RegisterTenant(tenantName, m_container, m_nestedContainer))
            {
                return TenantNotFoundErrorController(requestContext);
            }
            return m_nestedContainer.GetInstance(controllerType) as IController;
        }

        #endregion


        #region Private Methods

        private IController TenantNotFoundErrorController(RequestContext requestContext)
        {
            if (requestContext.HttpContext.Request.IsAjaxRequest())
            {
                requestContext.RouteData.Values["controller"] = "Error";
                requestContext.RouteData.Values["action"] = "RedirectToTenantNotFound";
            }
            else
            {
                requestContext.RouteData.Values["controller"] = "Error";
                requestContext.RouteData.Values["action"] = "TenantNotFound";
            }
            return new ErrorController();
        }
        #endregion
    }
}