// Copyright (c) 2015 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.

// Component Name  : 	Session Manager
// Description     : 	Static class to hold user specific session object
// Author          :	Praveenkumar Selvaraj		
// Creation Date   : 	APL-09-2015

using System.Collections.Generic;
using System.Web;
using System.Web.SessionState;

namespace Laserbeam.UI.HR.Common
{
    public class SessionManager
    {
        #region Constructors

        public SessionManager()
        {
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds specified valueObject to Session using sessionKey
        /// </summary>
        /// <typeparam name="T">Any valid type</typeparam>
        /// <param name="sessionKey">unique key to identify a value from Session</param>
        /// <param name="valueObject">Actual value object which needs to be added to Session</param>
        public void SetSession<T>(string sessionKey, T valueObject)
        {
            HttpSessionState sessionObject = getSessionState();
            if (sessionObject != null)
            {
                sessionObject.Add(sessionKey, valueObject);
            }
        }

        /// <summary>
        /// Retrieves an object of type T from Session using the provided sessionKey
        /// </summary>
        /// <typeparam name="T">A valid type</typeparam>
        /// <param name="sessionKey">unique key to identify a value from Session</param>
        /// <returns>An object of type T</returns>
        public T GetSession<T>(string sessionKey)
        {
            T valueObject = default(T);
            HttpSessionState sessionObject = getSessionState();
            if (sessionObject != null && sessionObject[sessionKey] != null)
            {
                valueObject = (T)sessionObject[sessionKey];
            }
            return valueObject;
        }
        
        /// <summary>
        /// Retrieves an object list of type T from Session using the provided sessionKey
        /// </summary>
        /// <typeparam name="T">Valid object type</typeparam>
        /// <param name="sessionKey">unique key to identify a value from Session</param>
        /// <returns>An object list of type T</returns>
        public List<T> GetSessionCollection<T>(string sessionKey)
        {
            List<T> valueObject = default(List<T>);
            HttpSessionState sessionObject = getSessionState();
            if (sessionObject != null && sessionObject[sessionKey] != null)
            {
                valueObject = (List<T>)sessionObject[sessionKey];
            }
            return valueObject;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets current Session state object
        /// </summary>
        /// <returns>a reference to current session state</returns>
        private HttpSessionState getSessionState()
        {
            return HttpContext.Current.Session;
        }

        #endregion
    }
}