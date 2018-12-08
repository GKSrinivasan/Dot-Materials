// Copyright (c) 2016 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.
// Component Name  :  IUserManagementRepository
// Description     :  Interface signature for UserManagementRepository
// Author         : HARIHARASUBRAMANIYAN CHANDRASEKARAN
// Creation Date  :  10-Feb-2017

using Laserbeam.BusinessObject.Common;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Laserbeam.EntityManager.Common;

namespace Laserbeam.DataManager.Interfaces.Common
{
    public interface IUserManagementRepository
    {
        #region User List Implementation
        Task<IEnumerable<AppUsers>> GetUserCount(int userNum);
        AppUser GetUser(string userID);
        bool UserDuplicateValidation(string userID,int userNum);
        
        Employee GetEmployee(string employeeId);
        IQueryable<AppUserRoleModel> GetDropdownUserRoles(int userNum);
        Task<int> AddUser(AppUserModel user, int userNum, string tenantName);
        Task<bool> DeleteUser(string id, int userNum, string tenantName);
        AppEmail GetEmailTemplate(string templateName);
        Task<DataTable> GetExportUserData(int userNum);
        List<AppUser> GetYettoLoginUsers();
        AppUser GetUserDetails(int userNum, string userID);
        AppUserModel GetUserInfo(int userNum);
        IQueryable<SendReminderNotificationModel> GetYetToLoginUsersList(int userNum);
        void UpdateAppEmailSubjectandBody(string emailBody, string emailSubject, string emailTemplate);
        Task<IEnumerable<AppUserDataModel>> GetUserGridData(int userNum);
        #endregion
        #region Upload User Implementation
        Task<DataTable> GetUserDataTemplate();
        int UpdateXmlProcess(string sheetName, string fileSourcePath, int recordCount, int userNum);
        Task<bool> InsertStagingTableData(DataTable userData, string DbName, List<string> columnsName, int xmlProcessNum);
        Task<int> ValidateUserData();
        Task<int> ProcessUserData(string tenantName);
        Task<DataTable> GetUserDataErrorExport();
        Task<IEnumerable<TemplateErrorListModel>> GetUserDataErrorList();
        Task<int> GetErrorRecordCount();
        Task<IEnumerable<TemplateDataModel>> GetUserTemplateData(int userNum);
        Task<int> DeleteUserTemplateData(int xmlProcessNum);
        Task<DataTable> GetExportXmlFile(int xmlProcessNum);
        #endregion




    }
}
