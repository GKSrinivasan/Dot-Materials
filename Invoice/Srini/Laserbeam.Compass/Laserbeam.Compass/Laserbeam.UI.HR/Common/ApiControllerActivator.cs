using Laserbeam.Libraries.Common;
using Laserbeam.RegistryManager.HR;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;

namespace Laserbeam.UI.HR.Common
{
    public class ApiControllerActivator: IHttpControllerActivator
    {
        // Author         :   Boobalan Ranganathan		
        // Creation Date  :   06-May-2016
        /// <summary>
        /// An instance of IContainer
        /// </summary>
        private IContainer m_container;
        public ApiControllerActivator(IContainer container)
        {
            m_container = container;
        }

        public object RuntimeRegistry { get; private set; }

        // Author         :   Boobalan Ranganathan		
        // Creation Date  :   06-May-2016
        /// <summary>
        /// Creates an instance of IHttpController
        /// </summary>
        /// <param name="request">An instance of HttpRequestMessage</param>
        /// <param name="controllerDescriptor">An instance of HttpControllerDescriptor</param>
        /// <param name="controllerType">Type of the controller</param>
        /// <returns>Returns an instance of IHttpController</returns>
        public IHttpController Create(HttpRequestMessage request, HttpControllerDescriptor controllerDescriptor, Type controllerType)
        {
            var scope = request.GetDependencyScope();
            var nestedContainer = (IContainer)scope.GetService(typeof(IContainer));
            try
            {
                IEnumerable<string> tenantNames;
                if (!request.Headers.TryGetValues("X-Tenant", out tenantNames) && (controllerType.Name == "ReflectReportController" || controllerType.Name == "ReflectAttributesController"))
                {
                    return (IHttpController)nestedContainer.GetInstance(controllerType);
                }

                string tenantName = request.Headers.GetValues("X-Tenant").SingleOrDefault() ?? "";
                MayaLink.TryDecrypt(tenantName, out tenantName);
                string[] excludeController = {
                    "SSOController",
                    "VerifyTenantController",
                    };
                if (!DependencyRegistry.RegisterTenant(tenantName, m_container, nestedContainer) && !excludeController.Contains(controllerType.Name)) throw new ApplicationException("Tenant not found");
                return (IHttpController)nestedContainer.GetInstance(controllerType);
            }
            catch (StructureMapException)
            {
                return null;
            }
        }
    }
}