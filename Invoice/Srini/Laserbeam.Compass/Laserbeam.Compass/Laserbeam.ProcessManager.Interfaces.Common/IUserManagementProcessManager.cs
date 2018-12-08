// Copyright (c) 2015 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.

// Component Name :   IUserManagementProcessManager
// Description    :   Interface signature for UserManagementProcessManager
// Author         :   Karthikeyan Shanmugam
// Creation Date  :   09-Feb-2017

using Laserbeam.BusinessObject.Common;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace Laserbeam.ProcessManager.Interfaces.Common
{
    public interface IUserManagementProcessManager
    {

        Task<IEnumerable<AppUsers>> GetUserCount(int userNum);
        bool UserValidation(string userId);
        bool EmployeeIdValidation(string employeeId);
        IQueryable<AppUserRoleModel> GetDropdownUserRoles(int userNum);
        Task<int> AddUser(AppUserModel user, int userNum, string tenantName);
        Task<bool> DeleteUser(string id, int userNum, string tenantName);
        Task<DataTable> GetExportUserData(int userNum);
        Task<bool> SendEmailToUser(string requestURL, AppConfigModel appConfigModel);
        Task<bool> SendWelcomeEmailToUser(string requestURL, int userNum, string userID, AppConfigModel appConfigModel);
        AppUserModel GetUserInfo(int userNum);        
        Task<DataTable> GetUserDataTemplate();
        Task<DataTable> GetUserDataErrorExport();
        int UpdateXmlProcess(string sheetName, string fileSourcePath, int recordCount, int userNum);
        Task<bool> InsertStagingTableData(DataTable userData, string DbName, List<string> columnsName, int xmlProcessNum);
        void InitializeConnection(string filePath);
        String[] GetExcelSheetNames();
        DataTable GetDataTable(string sheetName);
        Task<int> ValidateUserData();
        Task<int> ProcessUserData(string tenantName);
        Task<List<TemplateErrorListModel>> GetUserDataErrorList();
        IQueryable<SendReminderNotificationModel> GetYetToLoginUsersList(int userNum);
        Task<bool> SendEmailReminderToUser(AppConfigModel appConfigModel, List<SendReminderNotificationModel> sendReminderMail, string emailBody, string emailSubject, string requestURL);
        UserManagementEmailDetails GetMailDetails();
        Task<int> GetErrorRecordCount();
        Task<IEnumerable<TemplateDataModel>> GetUserTemplateData(int userNum);
        Task<int> DeleteUserTemplateData(int xmlProcessNum);
        Task<IEnumerable<AppUserDataModel>> GetUserGridData(int userNum);
        Task<DataTable> GetExportXmlFile(int xmlProcessNum);
        bool UserDuplicateValidation(string userId,int userNum);
    }

}
