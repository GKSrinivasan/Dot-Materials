// Copyright (c) 2015 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.

// Component Name :   IEmailDetailsRepository
// Description    :   Interface signature for EmailDetailsRepository
// Author         :   Raja Ganapathy
// Creation Date  :   05-Jul-2016  

using Laserbeam.EntityManager.Common;

namespace Laserbeam.DataManager.Interfaces.Common
{
    public interface IEmailDetailsRepository
    {
        AppEmail GetAppEmailDetails(string emailKey);                                
    }
}
