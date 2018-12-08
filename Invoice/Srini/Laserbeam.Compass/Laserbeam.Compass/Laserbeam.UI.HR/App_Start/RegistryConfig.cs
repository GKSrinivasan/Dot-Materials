using System.Web.Mvc;
using StructureMap;
using Laserbeam.UI.HR.Common;
using System.Web.Http.Dispatcher;
using System.Web.Http;

namespace Laserbeam.UI.HR.App_Start
{
    public static class RegistryConfig
    {
        private static IContainer m_container;
        // Author         :   Boobalan Ranganathan		
        // Creation Date  :   12-Sep-2015
        /// <summary>
        /// Registers dependecies into MVC application
        /// </summary>
        public static void RegisterDependecies()
        {
            m_container = new Container();
            m_container.Configure(x =>
            {
                x.For<IHttpControllerActivator>().Use<ApiControllerActivator>().Ctor<IContainer>().Is(m_container).Singleton();
            });
            GlobalConfiguration.Configuration.DependencyResolver = new StructureMapResolver(m_container);
            ControllerBuilder.Current.SetControllerFactory(new ControllerFactory(m_container));
        }

        // Author         :   Boobalan Ranganathan		
        // Creation Date  :   29-Sep-2015
        /// <summary>
        /// Dispose structure map container
        /// </summary>
        public static void DisposeContainer()
        {
            m_container.Dispose();
        }

    }
}