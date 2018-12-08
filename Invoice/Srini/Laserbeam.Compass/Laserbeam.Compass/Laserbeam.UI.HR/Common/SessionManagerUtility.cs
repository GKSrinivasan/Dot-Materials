// Copyright (c) 2015 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.

// Component Name  : 	Session Manager Utility
// Description     : 	Managing the application sessions	
// Author          :	Praveenkumar Selvaraj		
// Creation Date   : 	APR-09-2015

using System.Collections.Generic;
using System.Web;
using System.Web.SessionState;

namespace Laserbeam.UI.HR.Common
{
    public class SessionManagerUtility
    {
        private readonly HttpSessionState _session;

        #region Constructor
        public SessionManagerUtility()
        {
            _session = HttpContext.Current.Session;
        }

        #endregion

        #region Depends on SessionKey provied using constructor

        
        // Author        :  Praveenkumar Selvaraj		
        // Creation Date :  APR-09-2015
        /// <summary>
        /// Checking session has any key
        /// </summary>
        /// <returns></returns>
        public bool HasAnySessions()
        {
            return _session.Count > 0;
        }

        // Author        :  Praveenkumar Selvaraj		
        // Creation Date :  APR-09-2015
        /// <summary>
        /// Checking the provided key is exist
        /// </summary>
        /// <param name="sessionName">Session key</param>
        /// <returns>true if provided key exist, false as it is not added yet</returns>
        public bool DoesKeyExist(string sessionName)
        {
            if (!HasAnySessions())
            {
                return false;
            }
            bool exist = false;
            for (int i = 0; i < _session.Count; i++)
            {
                exist = _session.Keys[i] == sessionName;
                if (exist)
                {
                    break;
                }
            }
            return exist;
        }

        // Author        :  Praveenkumar Selvaraj		
        // Creation Date :  APR-09-2015
        /// <summary>
        /// Checking the provided key is null
        /// </summary>
        /// <param name="sessionName">Session key</param>
        /// <returns>false if provided key has value, true as it is not added yet or null</returns>
        public bool IsNull(string sessionName)
        {
            return _session[sessionName] == null;
        }

        // Author        :  Praveenkumar Selvaraj		
        // Creation Date :  APR-09-2015
        /// <summary>
        /// Set the provided key as null
        /// </summary>
        /// <param name="sessionName">Session key</param>
        public void SetNull(string sessionName)
        {
            _session[sessionName] = null;
        }

        // Author        :  Praveenkumar Selvaraj		
        // Creation Date :  APR-09-2015
        /// <summary>
        /// Returns the session value
        /// </summary>
        /// <typeparam name="TSource">Any valid object</typeparam>
        /// <param name="sessionName">Session key</param>
        /// <returns>Stored values based on the provided key</returns>
        public TSource Get<TSource>(string sessionName)
        {
            TSource valueObject = default(TSource);
            if (DoesKeyExist(sessionName) && !IsNull(sessionName))
            {
                valueObject = (TSource)_session[sessionName];
            }
            return valueObject;
        }

        // Author        :  Praveenkumar Selvaraj		
        // Creation Date :  APR-09-2015
        /// <summary>
        /// Returns the session value as list
        /// </summary>
        /// <typeparam name="TSource">Any valid object</typeparam>
        /// <param name="sessionName">Session key</param>
        /// <returns>Stored list values based on the provided key</returns>
        public List<TSource> GetList<TSource>(string sessionName)
        {
            List<TSource> valueObject = default(List<TSource>);
            if (DoesKeyExist(sessionName) && !IsNull(sessionName))
            {
                valueObject = (List<TSource>)_session[sessionName];
            }
            return valueObject;
        }

        // Author        :  Praveenkumar Selvaraj		
        // Creation Date :  APR-09-2015
        /// <summary>
        /// Create new session key
        /// </summary>
        /// <typeparam name="TSource">Any valid object</typeparam>
        /// <param name="sessionName">Session key</param>
        /// <param name="model">Object</param>
        public void Add<TSource>(string sessionName, TSource model)
        {
            _session.Add(sessionName, model);
        }

        // Author        :  Praveenkumar Selvaraj		
        // Creation Date :  APR-09-2015
        /// <summary>
        /// Repleace the session key value using the existing key
        /// </summary>
        /// <typeparam name="TSource">Any valid object</typeparam>
        /// <param name="sessionName">Session key</param>
        /// <param name="model">Object</param>
        public void Replace<TSource>(string sessionName, TSource model)
        {
            if (DoesKeyExist(sessionName) && (model.GetType() == _session[sessionName].GetType()))
            {
                _session[sessionName] = model;
            }
        }

        // Author        :  Praveenkumar Selvaraj		
        // Creation Date :  APR-09-2015
        /// <summary>
        /// Remove the session key from the session object
        /// </summary>
        /// <param name="sessionName">Session key</param>
        public void Remove(string sessionName)
        {
            if (DoesKeyExist(sessionName))
            {
                _session.Remove(sessionName);
            }
        }

        // Author        :  Praveenkumar Selvaraj		
        // Creation Date :  APR-09-2015
        /// <summary>
        /// Remove the all session key from the session object
        /// </summary>
        public void RemoveAll()
        {
            if (HasAnySessions())
            {
                _session.RemoveAll();
            }
        }

        // Author        :  Praveenkumar Selvaraj		
        // Creation Date :  APR-09-2015
        /// <summary>
        /// Get created session object id
        /// </summary>
        /// <returns></returns>
        public string GetSessionId()
        {
            string sessionID = string.Empty;
            if (HasAnySessions())
            {
                sessionID= _session.SessionID;
            }
            return sessionID;
        }

        // Author        :  Praveenkumar Selvaraj		
        // Creation Date :  APR-09-2015
        /// <summary>
        /// Abandon the session
        /// </summary>
        public void AbandonSessions()
        {
            if (HasAnySessions())
            {
                _session.Abandon();
            }
        }

        #endregion
    }
}