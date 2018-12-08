
// Copyright (c) 2014 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.

// Component Name :   StructureMapResolver
// Description    :   StructureMapResolver from WebApiContrib library modified to support StructureMap 3.0
// Author         :   Boobalan Ranganathan		
// Creation Date  :   06-May-2016
			
using StructureMap;
using System;
using System.Web.Http.Dependencies;

namespace Laserbeam.UI.HR.App_Start
{
    public class StructureMapResolver : StructureMapDependencyScope, IDependencyResolver
    {
        private readonly IContainer _container;

        public StructureMapResolver(IContainer container)
            : base(container)
        {
            if (container == null)
                throw new ArgumentNullException("container");

            _container = container;
        }

        public IDependencyScope BeginScope()
        {
            return new StructureMapDependencyScope(_container.GetNestedContainer());
        }
    }
}