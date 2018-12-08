// Copyright (c) 2016 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.
// Component Name  :  Email
// Description     :  This page is used to retrieve the Email related details. 
// Author         :  Raja Ganapathy
// Creation Date  :  05-Jul-2016  

using Laserbeam.DataManager.Interfaces.Common;
using Laserbeam.DataManager.Interfaces.Core;
using Laserbeam.EntityManager.Common;
using System.Linq;
namespace Laserbeam.DataManager.Common
{
    public class EmailDetailsRepository : IEmailDetailsRepository
    {
        // Author         :  Raja Ganapathy
        // Creation Date  :  05-Jul-2016        
        /// <summary>
        /// Object of Base Repository
        /// </summary>
        private IBaseRepository m_baseRepository;

        #region Constructors
        // Author         :  Raja Ganapathy
        // Creation Date  :  05-Jul-2016  
        /// <summary>
        /// Initializes objects used in this class
        /// </summary>
        /// <param name="baseRepository">Base Repository Object</param>
        public EmailDetailsRepository(IBaseRepository baseRepository)           
        {
            m_baseRepository = baseRepository;
        }
        #endregion

        #region Implements
        #region Public Methods        
        // Author         :  Raja Ganapathy
        // Creation Date  :  05-Jul-2016        
        /// <summary>
        /// To get Email details like EmailSubject,EmailBody etc from the database.
        /// </summary>
        /// <param name="emailKey">Using EmailKey the respective subject,body and script is selected for sending a particular email</param>
        /// <returns>AppEmail</returns>
        public AppEmail GetAppEmailDetails(string emailKey)
        {
            return m_baseRepository.GetQuery<AppEmail>(appEmailInfo => appEmailInfo.EmailKey == emailKey).FirstOrDefault();
        }
        
         
        #endregion

        #endregion
    }
}
