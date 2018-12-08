using Laserbeam.BusinessObject.Common;
using Laserbeam.EntityManager.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laserbeam.DataManager.Interfaces.Common
{
     public interface IProfileRepository
    {
        UserProfileModel GetUserProfileDetails(int year,int userNum,string requestUrl);
        bool updateUserDetails(int year, int userNum, UserProfileModel userDetails);
        IEnumerable<DropDownListModel> GetTeamTitle();
        bool AddTaskDetails(TaskModel Model, int userNum);
        Task<IEnumerable<TaskDataModel>> GetTaskData(int userNum);
        TaskModel GetTaskDetails(int taskNum);
        IQueryable<SupportTaskComment> GetTaskComment(int supportTaskNum, int userNum);
        void PutUpdateComments(SupportTaskComment comment);
        Task DeleteComments(int SupportTaskCommentsNum);
        IQueryable<SupportTaskComment> GetSupportTaskComment(int supportTaskCommentsNum);
        decimal GetTotalspent();
        Task<DataTable> GetTaskExportdata(string fromDate, string toDate);
        void updateSpentHours(string spentHours);
        bool GetUserIsDuplicate(string userID);
    }
}
