// Copyright (c) 2015 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.

// Component Name  : 	IAccountProcessManager
// Description     : 	Interface for AccountProcessManager	
// Author          :	Roopan		
// Creation Date   : 	APR-01-2015

using Laserbeam.BusinessObject.Common;
using Laserbeam.Constant.HR;
using System;
using System.Threading.Tasks;

namespace Laserbeam.ProcessManager.Interfaces.Common
{
    public interface IAccountProcessManager
    {
        UserModel GetUser(string userID);
        void PushUserLogoutDetails(int userNum, bool isSSOLogin);
        void PutUserLogInDetails(int userNum);
        UserCredentialStatus ValidateUserCredential(string userId, string password, AppConfigModel appConfig);
        UserCredentialStatus ValidateUserSecretKey(string userId, string secretKey, string requestTimeStamp, AppConfigModel appConfig, string mayaLink = null);
        string GetPasswordMode();
        UserCredentialStatus ResetUserCredential(string userId, AppConfigModel appConfig);
        Task<int> SendEmailToUser(UserModel user, string templateName, string requestURL);
        int SendContactEmail(string tenant, string toEmail);
        UserCredentialStatus ChangeUserCredential(string userId, string newPassword, AppConfigModel appConfig);
        DateTime? GetLockedDate(string userId);
        Task PutUserPopulation(int userNum);
        bool UserValidation(string userId);
        int getPasswordLength();
        AppConfigModel GetAppSetting();
    }
}
