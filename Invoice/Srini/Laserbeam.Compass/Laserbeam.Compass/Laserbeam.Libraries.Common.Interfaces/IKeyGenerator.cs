// Copyright (c) 2015 Laserbeam Software Pvt.ltd.  All rights reserved.
// Confidential and proprietary.

// Component Name  : 	IKeyGenerator
// Description     : 	Interface providing operations for KeyGenerator	
// Author          :	Roopan		
// Creation Date   : 	APR-09-2015


namespace Laserbeam.Libraries.Interfaces.Common
{
    public interface IKeyGenerator
    {
        string GenerateRandomKey(int maxSize);
    }
}
