﻿// Copyright (c) 2016 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.
// Component Name  :  ISessionRepository
// Description     :  Interface signature for SessionRepository
// Author         : Boobalan
// Creation Date  :  03-01-2015

using Laserbeam.BusinessObject.Common;
using Laserbeam.Constant.HR;
using System.Collections.Generic;

namespace Laserbeam.DataManager.Interfaces.Common
{
   // public interface ISessionRepository : BaseRepository<LaserbeamCommonEntities>
    public interface ISessionRepository 
    {
        // Author:		      Boobalan		
        // Creation Date:   03-01-2015
        /// <summary>
        /// To get user session data from database
        /// </summary>
        /// <param name="sessionId">EmployeeID or UserID for which the user session data is needed</param>
        /// <param name="isEmployeeId">Default is false, UserID should be provided as sessionId. True if EmployeeID is provided as sessionId</param>
        /// <returns>Returns UserModel object</returns>
        UserModel GetUserSession(string sessionId,int year, bool isEmployeeId = false);

        // Author:		      Boobalan		
        // Creation Date:   03-01-2015
        /// <summary>
        /// To get employee session from database
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

        //AppConfigModel GetAppSetting();

        UserCredentialStatus GetUserSSOLoginSession(string emailAddress, out UserModel userModel);


    }
}
