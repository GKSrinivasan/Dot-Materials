using InVoiceSoftware.Common;
using System.ComponentModel;
using System.Web.Mvc;

namespace InVoiceSoftware.App_Start
{
    public static class RegistryConfig
    {
        private static IContainer m_container;

        public static void RegisterDependecies()
        {
            m_container = new Container();
            ControllerBuilder.Current.SetControllerFactory(new ControllerFactory(m_container));
        }

        public static void DisposeContainer()
        {
            m_container.Dispose();
        }

    }
}