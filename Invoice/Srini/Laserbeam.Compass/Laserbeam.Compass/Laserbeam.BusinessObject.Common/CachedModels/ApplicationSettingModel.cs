
// Copyright (c) 2014 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.

// Component Name :   ApplicationSettingModel
// Description    :   Model that converts AppSetting table to ApplicationSettingModel
// Author         :   Hariharasubramaniyan Chandrasekaran		
// Creation Date  :   08-March-2018
// Ticket ID      :   CL-1288

using Laserbeam.BusinessObject.Common.Constants;
using Laserbeam.EntityManager.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Laserbeam.BusinessObject.Common.CachedModels
{
    public class ApplicationSettingModel
    {
        #region Fields
        public readonly int CurrentYear;
        public readonly string SMTPServer;
        public readonly string AdminEmailID;
        public readonly byte LoginAttempts;
        public readonly string AdminPassword;
        public readonly int SMTPPort;
        public readonly int MeritCycleYear;
        public readonly string ToolName;
        public readonly string ToolVersion;
        public readonly string BaseCurrency;
        public readonly string CurrencyApiAccessKey;
        public readonly int PasswordLength;
        public readonly string TenantExpireDate;
        public readonly string PayChangeURL;
        public readonly string PasswordMode;
        public readonly string IsSAMLLogin;
        public readonly string Instance;
        public readonly bool EnableNotification;
        public readonly bool EnableFeedBack;
        public readonly bool TriggerEmailSameServer;
        public readonly string Version;
        public readonly int PasswordDisableDuration;
        public readonly int RangeExceed;
        public readonly int BaseCurrencyNum;
        public readonly string AdminUserID;
        public readonly string CurrencyConversionPreview;
        public readonly string CurrencyDisplayPreview;

        #endregion
        #region Constructor
        // Author         :   Hariharasubramaniyan Chandrasekaran
        // Creation Date  :   08-March-2018
        // Ticket ID      :   CL-1288
        /// <summary>
        /// Constructor of ApplicationSettingModel
        /// </summary>
        /// <param name="appSetting">List of AppSetting data</param>
        public ApplicationSettingModel(List<AppSetting> appSetting)
        {
            var currentYearValue = appSetting.Single(m => m.AppSettingID == AppSettingConstants.CurrentYear).AppSettingValue;
            var loginAttemptValue = appSetting.Single(m => m.AppSettingID == AppSettingConstants.LoginAttempts).AppSettingValue;
            var portNumerValue = appSetting.Single(m => m.AppSettingID == AppSettingConstants.SMTPPort).AppSettingValue;
            var meritCycleYearValue = appSetting.Single(m => m.AppSettingID == AppSettingConstants.MeritCycleYear).AppSettingValue;
            CurrentYear = string.IsNullOrWhiteSpace(currentYearValue) ? Convert.ToInt32(DateTime.Now.Year) : Convert.ToInt32(currentYearValue);
            SMTPServer = appSetting.Single(m => m.AppSettingID == AppSettingConstants.SMTPServer).AppSettingValue;
            AdminEmailID = appSetting.Single(m => m.AppSettingID == AppSettingConstants.AdminEmailID).AppSettingValue;
            LoginAttempts = string.IsNullOrWhiteSpace(loginAttemptValue) ? Convert.ToByte(5) : Convert.ToByte(loginAttemptValue);
            AdminPassword = appSetting.Single(m => m.AppSettingID == AppSettingConstants.AdminPassword).AppSettingValue;
            SMTPPort = string.IsNullOrWhiteSpace(portNumerValue) ? 0 : Convert.ToInt32(portNumerValue);
            MeritCycleYear = string.IsNullOrWhiteSpace(meritCycleYearValue) ? Convert.ToInt32(DateTime.Now.Year) : Convert.ToInt32(meritCycleYearValue);
            ToolName = appSetting.Single(m => m.AppSettingID == AppSettingConstants.ToolName).AppSettingValue;
            ToolVersion = appSetting.Single(m => m.AppSettingID == AppSettingConstants.ToolVersion).AppSettingValue;
            BaseCurrency = appSetting.Single(m => m.AppSettingID == AppSettingConstants.BaseCurrency).AppSettingValue;
            CurrencyApiAccessKey = appSetting.Single(m => m.AppSettingID == AppSettingConstants.CurrencyApiAccessKey).AppSettingValue;
            PasswordLength = Conversion.convertToInteger(appSetting.Single(m => m.AppSettingID == AppSettingConstants.PasswordLength).AppSettingValue);
            TenantExpireDate = appSetting.Single(m => m.AppSettingID == AppSettingConstants.TenantExpireDate).AppSettingValue;
            PayChangeURL = appSetting.Single(m => m.AppSettingID == AppSettingConstants.PayChangeURL).AppSettingValue;
            PasswordMode = appSetting.Single(m => m.AppSettingID == AppSettingConstants.PasswordMode).AppSettingValue;
            IsSAMLLogin = appSetting.Single(m => m.AppSettingID == AppSettingConstants.IsSAMLLogin).AppSettingValue;
            Instance= appSetting.Single(m => m.AppSettingID == AppSettingConstants.Instance).AppSettingValue;
            EnableNotification = Conversion.convertToBool(appSetting.Single(m => m.AppSettingID == AppSettingConstants.EnableNotification).AppSettingValue);
            EnableFeedBack= Conversion.convertToBool(appSetting.Single(m => m.AppSettingID == AppSettingConstants.EnableFeedBack).AppSettingValue);
            TriggerEmailSameServer= Conversion.convertToBool(appSetting.Single(m => m.AppSettingID == AppSettingConstants.TriggerEmailSameServer).AppSettingValue);
           Version = appSetting.Single(m => m.AppSettingID == AppSettingConstants.Version).AppSettingValue;
            PasswordDisableDuration = Conversion.convertToInteger(appSetting.Single(m => m.AppSettingID == AppSettingConstants.PasswordDisableDuration).AppSettingValue);
            RangeExceed = Conversion.convertToInteger(appSetting.Single(m => m.AppSettingID == AppSettingConstants.RangeExceed).AppSettingValue);
            AdminUserID = appSetting.Single(m => m.AppSettingID == AppSettingConstants.AdminUserID).AppSettingValue;
            CurrencyConversionPreview = appSetting.Single(m => m.AppSettingID == AppSettingConstants.CurrencyConversionPreview).AppSettingValue;
            CurrencyDisplayPreview = appSetting.Single(m => m.AppSettingID == AppSettingConstants.CurrencyDisplayPreview).AppSettingValue;


        }
        #endregion
    }
}
