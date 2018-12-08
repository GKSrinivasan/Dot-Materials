// Copyright (c) 2015 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.

// Component Name  : 	IAppUserRepository
// Description     : 	Interface for AppUserRepository	
// Author          :	Roopan		
// Creation Date   : 	APR-01-2015

using Laserbeam.BusinessObject.Common;
using Laserbeam.EntityManager.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Laserbeam.DataManager.Interfaces.Common
{
    public interface IAppUserRepository
    {
        UserModel GetUserDataModel(string userID);
        AppUser GetUser(string userID);
        AppUser GetUserWithStatus(string userID);
        IQueryable<AppUser> GetUsersByUserNum(List<int> userNum);
        IQueryable<AppUserRole> GetUserRoles();
        bool UpdateUser(AppUser appUser);
        AppUser GetUserData(int userNum);
        void PutUserLogoutDetails(int userNum, bool isSSOLogin);
        void PutUserLogInDetails(int userNum);
        string GetAppUserStatus(int? appUserStatusId);
        string GetPasswordMode();
        //int? GetloginAttempt();
        AppEmail GetEmailTemplate(string templateName);
        int GetAppUserStatusId(string appUserStatus);
        DateTime? GetLockedDate(string userId);
        Task PutUserPopulation(int userNum);
        int defaultPasswordLength();
        AppConfigModel GetAppSetting();
    }
}
