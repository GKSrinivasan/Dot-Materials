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
    using InvoiceDataLayer.EntityFramework;
    using InvoiceDataLayer.IDataManager;
    using InvoiceProcessLayer.IProcessManager;
    using InvoiceProcessLayer.ProcessManager;
    using StructureMap;

    public static class DefaultRegistry {
        public static bool RegisterTenant(string tenantName, IContainer container, IContainer nestedContainer)
        {
            var connectionString = new TenantEntityConnection(tenantName).ConnectionString;
            nestedContainer.Configure(c =>
            {
                //DataManager.Core
                //c.For<IBaseRepository>().Use(new BaseRepository<BIZ_DEVEntities>(new BIZ_DEVEntities(EntityConnector.CreateConnection(container.GetInstance<TenantEntityConnection>(tenantName).ConnectionString)))).Named(tenantName).Transient();
                c.For<IBaseRepository>().Use(new BaseRepository<BIZ_DEVEntities>(new BIZ_DEVEntities(EntityConnector.CreateConnection(connectionString)))).Named(tenantName).Transient();
                c.For<IMasterBaseRepository>().Use(new BaseRepository<MasterDBEntities>(new MasterDBEntities())).Named(tenantName).Transient();
                c.For<ICommonRepository>().Use<CommonRepository>().Named(tenantName).Transient();
                //ProcessManager
                c.For<ICommonProcessManager>().Use<CommonProcessManager>().Named(tenantName).Transient();
            });
            return true;
        }
    }
}