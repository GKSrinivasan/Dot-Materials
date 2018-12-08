// Copyright (c) 2015 Laserbeam Software Pvt.ltd.  All rights reserved.
// Confidential and proprietary.

// Component Name  : 	IEncryptionLibrary
// Description     : 	Interface providing operations for EncryptionLibrary	
// Author          :	Roopan		
// Creation Date   : 	APR-09-2015


namespace Laserbeam.Libraries.Interfaces.Common
{
    public interface IPasswordEncryption
    {
        bool ValidatePassword(string password, string userPassword);
        string EncryptPassword(string password);
    }
}
