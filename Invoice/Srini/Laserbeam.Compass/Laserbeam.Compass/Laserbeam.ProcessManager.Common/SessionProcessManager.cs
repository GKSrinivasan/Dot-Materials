// Copyright (c) 2014 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.

// Component Name  : 	Session Manager
// Description     : 	Session related logics are placed here	
// Author          :	Roopan		
// Creation Date   : 	05-11-2014  

using Laserbeam.BusinessObject.Common;
using Laserbeam.Constant.HR;
using Laserbeam.DataManager.Interfaces.Common;
using Laserbeam.ProcessManager.Interfaces.Common;
using System.Collections.Generic;

namespace Laserbeam.ProcessManager.HR.Common
{
    public class SessionProcessManager : ISessionProcessManager
    {
        #region Fields
        ISessionRepository m_sessionRepository;
        #endregion

        #region Constructors
        public SessionProcessManager(ISessionRepository sessionRepository)
        {
            m_sessionRepository = sessionRepository;
        }
        #endregion

        #region Implements
        // Author        :  Boobalan		
        // Creation Date :  05-01-2015
        /// <summary>
        /// To get user session data
        /// </summary>
        /// <param name="sessionId">EmployeeID or UserID for which the user session data is needed</param>
        /// <param name="isEmployeeId">Default is false, UserID should be provided as sessionId. True if EmployeeID is provided as sessionId</param>
        /// <returns>Returns UserModel object</returns>
        public UserModel GetUserSession(string sessionId,int year, bool isEmployeeId = false)
        {
            return m_sessionRepository.GetUserSession(sessionId,year, isEmployeeId);
        }
        public UserCredentialStatus GetUserSSOLoginSession( string emailAddress, out UserModel userModel)
        {
            return m_sessionRepository.GetUserSSOLoginSession( emailAddress, out userModel);
        }
        // Author        :  Boobalan		
        // Creation Date :  05-01-2015
        /// <summary>
        /// To get employee session
        /// </summary>
        /// <param name="employeeNum">Employee tool number</param>
        /// <param name="year">Employee job year</param>
        /// <returns>Returs EmployeeModel object</returns>
        public EmployeeModel GetEmployeeSession(int employeeNum, int year)
        {
            return m_sessionRepository.GetEmployeeSession(employeeNum, year);
        }

        // Author        :  Boobalan		
        // Creation Date :  05-01-2015
        /// <summary>
        /// To get user session data from database
        /// </summary>
        /// <param name="userNum">User tool number for which the user session data is needed</param>
        /// <returns>Returns UserModel object</returns>
        public UserModel GetUserSession(int userNum, int year)
        {
            return m_sessionRepository.GetUserSession(userNum,year);
        }
        public List<UserRights> GetUserAccess(int userNum)
        {
            return m_sessionRepository.GetUserAccess(userNum);
        }

        //public AppConfigModel GetAppSetting()
        //{
        //    return m_sessionRepository.GetAppSetting();
        //}



        #endregion
    }
}
