
// Copyright (c) 2015 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.


// Component Name  : 	Praveenkumar Selvaraj
// Description     : 	Email utility class interface
// Author          :	Praveenkumar Selvaraj
// Creation Date   : 	APR-13-2015
using System;
using System.Collections.Generic;
using Laserbeam.BusinessObject.Common;
using System.Threading.Tasks;
using Laserbeam.EntityManager.Common;

namespace Laserbeam.Libraries.Interfaces.Common
{
    public interface IEmail
    {
        Int16 SendEmail(AppConfigModel appConfigModel, EmailDetails emailDetailList, bool isBodyHtml);
        Int16 SendEmail(AppConfigModel appConfigModel, List<EmailDetails> emailDetailList, bool isBodyHtml);
        Task<Int16> SendEmailAsync(AppConfigModel appConfigModel, EmailDetails emailDetailList, bool isBodyHtml);
        Task<Int16> SendEmailAsync(AppConfigModel appConfigModel, List<EmailDetails> emailDetailList, bool isBodyHtml);
        List<EmailTracker> SendEmail(List<EmailDetails> emailContentDetails, int templateID, AppConfigModel appConfigModel, int userNum, bool isBodyHtml);
    }
}
