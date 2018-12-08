using Laserbeam.BusinessObject.Common;
using Laserbeam.DataManager.Common;
using Laserbeam.DataManager.Interfaces.Common;
using Laserbeam.EntityManager.Common;
using Laserbeam.ProcessManager.Interfaces.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laserbeam.ProcessManager.Common
{
    public class ProfileProcessManager : IProfileProcessManager
    {

        #region Fields
        // Author         :   
        // Creation Date  :     
        /// <summary>
        /// Instance of ProfileRepository
        /// </summary>
        IProfileRepository m_profileRepository;
        #endregion

        #region Constructors
        public ProfileProcessManager(IProfileRepository ProfileRepository)
        {
            m_profileRepository = ProfileRepository;
        }
        #endregion


        public UserProfileModel GetUserProfileDetails(int year, int userNum, string requestUrl)
        {
            return m_profileRepository.GetUserProfileDetails(year, userNum, requestUrl);
        }

        public bool updateUserDetails(int year, int userNum, UserProfileModel userDetails)
        {
            return m_profileRepository.updateUserDetails(year, userNum, userDetails);
        }

        public IEnumerable<DropDownListModel> GetTeamTitle()
        {
            return m_profileRepository.GetTeamTitle();
        }

        public decimal GetTotalspent()
        {
            return m_profileRepository.GetTotalspent();
        }

        public bool AddTaskDetails(TaskModel Model, int userNum)
        {
            return m_profileRepository.AddTaskDetails(Model, userNum);

        }

        public async Task<IEnumerable<TaskDataModel>> GetTaskData(int userNum)
        {
            return await m_profileRepository.GetTaskData(userNum);
        }

        public TaskModel GetTaskDetails(int taskNum)
        {
            return m_profileRepository.GetTaskDetails(taskNum);
        }

        public IQueryable<SupportTaskCommentsModel> GetTaskComment(int supportTaskNum, int userNum)
        {
            IQueryable<SupportTaskCommentsModel> comments = m_profileRepository.GetTaskComment(supportTaskNum, userNum).Select(o => new SupportTaskCommentsModel
            {
                SupportTaskCommentsNum = o.SupportTaskCommentsNum,
                CreatedDate = o.CreatedDate,
                UpdatedBy = o.UpdatedBy,
                CreatedBy = o.CreatedBy,
                UpdatedDate = o.UpdatedDate,
                Comments = o.Comments,
                SupportTaskNum = o.SupportTaskNum,
                EmployeeName = o.AppUser.UserName,
                FirstName = o.AppUser.FirstName,
                LastName = o.AppUser.LastName
            });
            return comments;
        }
 
        public void PutUpdateComments(SupportTaskCommentsModel comments, bool isEditItem)
        {
            //var localTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.Local);
            if (isEditItem)
            {
                SupportTaskComment existComment = m_profileRepository.GetSupportTaskComment(comments.SupportTaskCommentsNum).FirstOrDefault();
                existComment.SupportTaskNum = comments.SupportTaskNum;
                existComment.Comments = comments.Comments;
                existComment.UpdatedBy = comments.UpdatedBy;
                existComment.UpdatedDate = DateTime.UtcNow;
                m_profileRepository.PutUpdateComments(existComment);
            }
            else
            {
                SupportTaskComment comment = new SupportTaskComment();
                comment.SupportTaskNum = comments.SupportTaskNum;
                comment.Comments = comments.Comments;
                comment.CreatedBy = comments.CreatedBy;
                comment.CreatedDate = DateTime.UtcNow;
                m_profileRepository.PutUpdateComments(comment);
            }
        }
        public async Task DeleteComments(int SupportTaskCommentsNum)
        {
           await m_profileRepository.DeleteComments(SupportTaskCommentsNum);
        }

        public async Task<DataTable> GetTaskExportdata(string fromDate, string toDate)
        {
            return await m_profileRepository.GetTaskExportdata(fromDate, toDate);
        }

        public void updateSpentHours(string spentHours)
        {
            m_profileRepository.updateSpentHours(spentHours);
        }

        public bool GetUserIsDuplicate(string userID)
        {
            return m_profileRepository.GetUserIsDuplicate(userID);
        }

    }
}
