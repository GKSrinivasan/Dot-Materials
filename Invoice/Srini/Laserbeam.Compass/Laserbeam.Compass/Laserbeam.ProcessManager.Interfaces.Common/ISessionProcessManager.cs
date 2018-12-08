// Copyright (c) 2015 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.

// Component Name :   ISessionProcessManager
// Description    :   Interface signature for SessionProcessManager
// Author         :   Roopan
// Creation Date  :   05-11-2014  

using Laserbeam.BusinessObject.Common;
using Laserbeam.Constant.HR;
using System.Collections.Generic;

namespace Laserbeam.ProcessManager.Interfaces.Common
{
    public interface ISessionProcessManager
    {
        // Author:		      Boobalan		
        // Creation Date:   05-01-2015
        /// <summary>
        /// To get user session data from data tier
        /// </summary>
        /// <param name="sessionId">EmployeeID or UserID for which the user session data is needed</param>
        /// <param name="isEmployeeId">Default is false, UserID should be provided as sessionId. True if EmployeeID is provided as sessionId</param>
        /// <returns>Returns UserModel object</returns>
        UserModel GetUserSession(string sessionId,int year, bool isEmployeeId = false);
        // Author:		      Boobalan		
        // Creation Date:   05-01-2015
        /// <summary>
        /// To get employee session
        /// </summary>
        /// <param name="employeeNum">Employee tool number</param>
        /// <param name="year">Employee job year</param>
        /// <returns>Returs EmployeeModel object</returns>
        EmployeeModel GetEmployeeSession(int employeeNum, int year);

        // Author:		      Boobalan		
        // Creation Date:   05-01-2015
        /// <summary>
        /// To get user session data from database
        /// </summary>
        /// <param name="userNum">User tool number for which the user session data is needed</param>
        /// <returns>Returns UserModel object</returns>
        UserModel GetUserSession(int userNum, int year);
        List<UserRights> GetUserAccess(int userNum);
        //  AppConfigModel GetAppSetting();
        UserCredentialStatus GetUserSSOLoginSession( string emailAddress, out UserModel userModel);

    }
}
