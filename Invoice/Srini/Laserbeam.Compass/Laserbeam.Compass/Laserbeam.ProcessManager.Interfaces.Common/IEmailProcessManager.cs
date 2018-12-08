// Copyright (c) 2016 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.
// Component Name  : 	IEmailProcessManager
// Description     : 	Contains Email related logics	
// Author          :    Thiyagu	
// Creation Date   :     04-11-2014

namespace Laserbeam.ProcessManager.Interfaces.Common
{
    public interface IEmailProcessManager
    {           
        int ForgotPassword(string toEmailID, string userName, string defaultPassword);       
    }
}
