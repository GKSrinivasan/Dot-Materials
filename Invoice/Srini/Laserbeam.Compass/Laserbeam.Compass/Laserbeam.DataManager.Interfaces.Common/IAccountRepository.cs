//using Laserbeam.HR.BusinessModels;
//using Laserbeam.HR.Core;
//using Laserbeam.HR.Core.BusinessModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Laserbeam.BusinessObject.Common;
namespace Laserbeam.HR.DataManagerInterface
{
    public interface IAccountRepository
    {
        AppConfigModel GetAppSetting();
        //TODO: Boobalan - Remove commented interface signature
        //UserModel GetUserSession(string employeeId, bool isSAMLRequest);
        //EmployeeModel GetEmployeeSession(int employeeNum, int year);
        //EmployeeModel GetSelectedEmployeeSession(int employeeNum, int year);
        //EmployeeModel GetLoggedInEmployeeSession(int employeeNum, int year);
        //UserModel GetLoggedUserCredential(LogInCredentialModel loginCredential);
        void AddCredential(AppUser appUser);
        AppUser GetUser(string userID);
        void PutUserDetails(UserModel user, string userID);
        // Author        :  Boobalan		
        // Creation Date :  12-01-2015
        /// <summary>
        /// Get page level access details for a user
        /// </summary>
        /// <param name="userNum">userNum as int, the unique number of the user</param>
        /// <returns>Returns a list of string collection with the list of page urls to which the user have access</returns>
        List<UserRights> GetUserAccess(int userNum);
    }
}
