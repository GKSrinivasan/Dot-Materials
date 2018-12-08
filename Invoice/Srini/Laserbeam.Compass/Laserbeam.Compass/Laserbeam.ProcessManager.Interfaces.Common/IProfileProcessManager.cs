using Laserbeam.BusinessObject.Common;
using Laserbeam.EntityManager.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laserbeam.ProcessManager.Interfaces.Common
{
     public interface IProfileProcessManager
    {
        UserProfileModel GetUserProfileDetails(int year,int userNum,string requestUrl);
        bool updateUserDetails(int year, int userNum, UserProfileModel userDetails);
        IEnumerable<DropDownListModel> GetTeamTitle();
        bool AddTaskDetails(TaskModel Model, int userNum);
        Task<IEnumerable<TaskDataModel>> GetTaskData(int userNum);
        TaskModel GetTaskDetails(int taskNum);
        IQueryable<SupportTaskCommentsModel> GetTaskComment(int supportTaskNum, int userNum);
        void PutUpdateComments(SupportTaskCommentsModel comments, bool isEditItem);
        Task DeleteComments(int SupportTaskCommentsNum);
        decimal GetTotalspent();
        Task<DataTable> GetTaskExportdata(string fromDate, string toDate);
        void updateSpentHours(string spentHours);
        bool GetUserIsDuplicate(string userID);
    }
}
