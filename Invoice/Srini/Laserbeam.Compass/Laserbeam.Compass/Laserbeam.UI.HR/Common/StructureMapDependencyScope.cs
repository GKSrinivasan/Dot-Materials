
// Copyright (c) 2014 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.

// Component Name :   StructureMapDependencyScope
// Description    :   StructureMapResolver from WebApiContrib library modified to support StructureMap 3.0
// Author         :   Boobalan Ranganathan		
// Creation Date  :   06-May-2016

using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Dependencies;

namespace Laserbeam.UI.HR.App_Start
{
    public class StructureMapDependencyScope : IDependencyScope
    {
        private IContainer _container;

        public StructureMapDependencyScope(IContainer container)
        {
            if (container == null)
                throw new ArgumentNullException("container");

            _container = container;
        }

        public object GetService(Type serviceType)
        {
            if (_container == null)
                throw new ObjectDisposedException("this", "This scope has already been disposed.");
            try
            {
                // TryGetInstance does not return contcete types, 
                // ApiControllers are routinely registered without interfaces
                return _container.GetInstance(serviceType);
            }
            catch (StructureMapException)
            {
                return null;
            }
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            if (_container == null)
                throw new ObjectDisposedException("this", "This scope has already been disposed.");

            return _container.GetAllInstances(serviceType).Cast<object>();
        }

        public void Dispose()
        {
            if (_container != null)
                _container.Dispose();

            _container = null;
        }
    }
}