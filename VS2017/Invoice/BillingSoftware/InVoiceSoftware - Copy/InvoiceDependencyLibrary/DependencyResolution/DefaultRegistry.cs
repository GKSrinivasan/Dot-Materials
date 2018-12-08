// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultRegistry.cs" company="Web Advanced">
// Copyright 2012 Web Advanced (www.webadvanced.com)
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace InvoiceDependencyLibrary.DependencyResolution
{
    using InvoiceDataLayer.DataManager;
    using InvoiceDataLayer.IDataManager;
    using InvoiceProcessLayer.IProcessManager;
    using InvoiceProcessLayer.ProcessManager;
    using StructureMap.Configuration.DSL;
    using StructureMap.Graph;
    using System.ComponentModel;

    public static class DefaultRegistry {
        //   #region Constructors and Destructors
        //   public DefaultRegistry() {
        //       Scan(
        //           scan => {
        //               scan.TheCallingAssembly();
        //               scan.WithDefaultConventions();
        //scan.With(new ControllerConvention());
        //           });
        //       For<IBaseRepository>().Use <BaseRepository>();
        //       For<ICommonRepository>().Use<CommonRepository>();
        //       For<ICommonProcessManager>().Use<CommonProcessManager>();
        //   }

        //   #endregion

        public static bool RegisterTenant(string tenantName, IContainer container, IContainer nestedContainer)
        {
            nestedContainer.Configure(c =>
            {
                //DataManager.Core
                c.For<IBaseRepository>().Use(new BaseRepository<LaserbeamEntitiesContainer1>(new LaserbeamEntitiesContainer1(EntityConnector.CreateConnection(container.GetInstance<TenantEntityConnection>(tenantName).ConnectionString)))).Named(tenantName).Transient();

                //ProcessManager
                c.For<IAccountProcessManager>().Use<AccountProcessManager>().Named(tenantName).Transient();
            });
            return true;
        }
    }
}