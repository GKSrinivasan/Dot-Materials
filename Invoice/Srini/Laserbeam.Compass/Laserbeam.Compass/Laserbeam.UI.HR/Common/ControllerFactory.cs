using Laserbeam.RegistryManager.HR;
using Laserbeam.UI.HR.Controllers;
using StructureMap;
using System;
using System.Configuration;
using System.Web.Mvc;
using System.Web.Routing;

namespace Laserbeam.UI.HR.Common
{

    public class ControllerFactory : DefaultControllerFactory
    {
        #region Fields

        // Author         :   Boobalan Ranganathan		
        // Creation Date  :   14-Sep-2015
        /// <summary>
        /// Instance of IContainer from StructureMap
        /// </summary>
        private readonly IContainer m_container;
        
        #endregion

        #region Constructor

        // Author         :   Boobalan Ranganathan		
        // Creation Date  :   14-Sep-2015
        /// <summary>
        /// Constructor to create instance of TenantControllerFactory
        /// </summary>
        /// <param name="container">Instance of IContainer from StructureMap</param>
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
            bool tenantNameFromVirtualPath = Convert.ToBoolean(ConfigurationManager.AppSettings["TenantNameFromVirtualPath"]);
            string tenantName;
            if (tenantNameFromVirtualPath)
            {
                var urlList = requestContext.HttpContext.Request.ApplicationPath.Split('/');
                tenantName = urlList[urlList.Length - 1];
            }
            else
            {
                tenantName = Convert.ToString(requestContext.RouteData.Values[SiteExtension.Tenant]);
            }
            IContainer m_nestedContainer = m_container.GetNestedContainer(tenantName);
            if (!DependencyRegistry.RegisterTenant(tenantName, m_container, m_nestedContainer))
            {
                //throw new ApplicationException("Tenant Not Found");
                return TenantNotFoundErrorController(requestContext);
            }
            //try
            //{
                return m_nestedContainer.GetInstance(controllerType) as IController;
            //}
            //catch(Exception ex)
            //{
            //    throw new Exception("Controller not available",ex);
            //}
        }

        #endregion


        #region Private Methods

        // Author         :   Boobalan Ranganathan		
        // Creation Date  :   03-Aug-2017
        // Ticket ID      :   PSP-13508
        /// <summary>
        /// Returns ErrorController with TenantNotFound action
        /// </summary>
        /// <param name="requestContext"></param>
        /// <returns></returns>
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