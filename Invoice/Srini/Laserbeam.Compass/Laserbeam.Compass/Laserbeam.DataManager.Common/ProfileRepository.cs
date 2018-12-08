using Laserbeam.BusinessObject.Common;
using Laserbeam.BusinessObject.Common.Constants;
using Laserbeam.DataManager.Interfaces.Common;
using Laserbeam.DataManager.Interfaces.Core;
using Laserbeam.EntityManager.Common;
using Laserbeam.Libraries.Core.Interfaces.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laserbeam.DataManager.Common
{
    public class ProfileRepository : IProfileRepository
    {

        #region Fields
        // Author         :  
        // Creation Date  :          
        /// <summary>
        /// Object of Base Repository
        /// </summary>
        private IBaseRepository m_baseRepository;
        private ITenantCacheProvider m_tenantCacheProvider;

        #endregion

        #region Constructors
        // Author         :  
        // Creation Date  :    
        /// <summary>
        /// Initializes objects used in this class
        /// </summary>
        /// <param name="baseRepository">Base Repository Object</param>
        public ProfileRepository(IBaseRepository baseRepository, ITenantCacheProvider tenantCacheProvider)
        {
            m_baseRepository = baseRepository;
            m_tenantCacheProvider = tenantCacheProvider;
        }
        #endregion

        public UserProfileModel GetUserProfileDetails(int year, int userNum, string requestUrl)
        {
            UserProfileModel model = new UserProfileModel();
            var profileInfo = m_baseRepository.GetQuery<AppUser>(s => s.UserNum == userNum).FirstOrDefault();
            var appuserRole = m_baseRepository.GetQuery<AppUserRole>(s => s.UserRoleNum == profileInfo.UserRoleNum).FirstOrDefault();
            //var supportHours = m_baseRepository.GetQuery<BusSetting>(x => x.KeyId == "FeatureConfiguration" && x.KeyValue == "SupportHours").Select(x => x.KeyDataValue).FirstOrDefault();
            var supportHours = m_tenantCacheProvider.GetBusinessSetting().FeatureConfiguration.SupportHours.ToString();
            model.FirstName = profileInfo.FirstName;
            model.LastName = profileInfo.LastName;
            model.UserID = profileInfo.UserID;
            model.EmailAddress = profileInfo.EmailID;
            model.URL = requestUrl;
            model.SupportHours = supportHours;
            model.UserName = profileInfo.UserName;
            model.UserRole = appuserRole.UserRole;
            var ShortNameSplit = profileInfo.UserName.Trim().Split(' ');
            string ShortName = "";
            for (int s = 0; s < ShortNameSplit.Count(); s++)
            {
                ShortName = ShortName + ShortNameSplit[s].Substring(0, 1);
            }
            model.UserNameShort = ShortName.ToUpper();
            return model;
        }

        public bool updateUserDetails(int year, int userNum, UserProfileModel userDetails)
        {
            AppUser existingProfileInfo = m_baseRepository.GetQuery<AppUser>(s => s.UserNum == userNum).FirstOrDefault();
            existingProfileInfo.FirstName = userDetails.FirstName;
            existingProfileInfo.LastName = userDetails.LastName;
            existingProfileInfo.UserName = userDetails.FirstName +" "+userDetails.LastName; 
            existingProfileInfo.UserID = userDetails.UserID;
            existingProfileInfo.EmailID = userDetails.UserID;
            existingProfileInfo.UpdatedBy = userNum;
            existingProfileInfo.UpdatedDate = DateTime.Now;
            m_baseRepository.Edit<AppUser>(existingProfileInfo);
            bool result = m_baseRepository.SaveChanges() > 0;
            return result;
        }

        public IEnumerable<DropDownListModel> GetTeamTitle()
        {
            return m_baseRepository.GetQuery<Team>().Select(x => new DropDownListModel
            {
                Text = x.TeamTitle,
                Value = x.TeamNum.ToString()

            }).ToList();
        }

        public decimal GetTotalspent()
        {
            var Sum = (from emp in m_baseRepository.GetQuery<SupportTask>()
                             select decimal.Parse(emp.HoursSpent.Replace(":","."))).Sum();

            return Sum;
            

            //var s = Convert.ToDateTime(m_baseRepository.GetQuery<SupportTask>().Select(x => (decimal)x.HoursSpent).Sum()

        }

        public bool AddTaskDetails(TaskModel Model, int userNum)
        {
            var localTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.Local);
            if (Model.TaskNum == 0)
            {
                SupportTask supportTask = new SupportTask();
                supportTask.TeamNum = Model.TeamNum;
                supportTask.StartDate = Model.StartDate;
                supportTask.EndDate = Model.EndDate;
                supportTask.HoursSpent = Model.HoursSpent;
                supportTask.TaskTitle = Model.TaskTitle;
                supportTask.TaskDescription = Model.TaskDescription;
                //supportTask.UpdatedBy = userNum;
                supportTask.TaskCreatedDate = DateTime.Now;
                m_baseRepository.Add<SupportTask>(supportTask);
            }
            else
            {
                SupportTask existSupportTask = m_baseRepository.GetQuery<SupportTask>(x => x.SupportTaskNum == Model.TaskNum).FirstOrDefault();
                existSupportTask.TeamNum = Model.TeamNum;
                existSupportTask.StartDate = Model.StartDate;
                existSupportTask.EndDate = Model.EndDate;
                existSupportTask.HoursSpent = Model.HoursSpent;
                existSupportTask.TaskTitle = Model.TaskTitle;
                existSupportTask.TaskDescription = Model.TaskDescription;
                existSupportTask.UpdatedBy = userNum;
                existSupportTask.TaskUpdatedDate = DateTime.Now;
                m_baseRepository.Edit<SupportTask>(existSupportTask);

            }
            bool result = m_baseRepository.SaveChanges() > 0;
            return result;
        }


        public async Task<IEnumerable<TaskDataModel>> GetTaskData(int userNum)
        {
            SqlParameter[] parameters = { new SqlParameter("@UserNum", userNum) };
            return await m_baseRepository.GetData<TaskDataModel>("[Talent].[USP_PF_GET_TeamTaskGridData] @UserNum", parameters);

        }

        public TaskModel GetTaskDetails(int taskNum)
        {
            TaskModel model = new TaskModel();
            var data = (from s in m_baseRepository.GetQuery<SupportTask>()
                        join t in m_baseRepository.GetQuery<Team>() on s.TeamNum equals t.TeamNum
                        where s.SupportTaskNum == taskNum
                        select new
                        {
                            s.TaskTitle,
                            s.StartDate,
                            s.EndDate,
                            s.HoursSpent,
                            s.TaskDescription,
                            s.TeamNum,
                            s.SupportTaskNum
                        }).FirstOrDefault();

            model.TeamNum = data.TeamNum;
            model.TaskTitle = data.TaskTitle;
            model.TaskDescription = data.TaskDescription;
            model.HoursSpent = data.HoursSpent;
            model.StartDate = data.StartDate;
            model.EndDate = data.EndDate;
            model.TaskNum = data.SupportTaskNum;
            return model;
        }

        public IQueryable<SupportTaskComment> GetTaskComment(int supportTaskNum, int userNum)
        {
            return m_baseRepository.GetQuery<SupportTaskComment>(new string[] { "AppUser" }, s => s.SupportTaskNum == supportTaskNum);
        }

        public IQueryable<SupportTaskComment> GetSupportTaskComment(int supportTaskCommentsNum)
        {
            return m_baseRepository.GetQuery<SupportTaskComment>(s => s.SupportTaskCommentsNum == supportTaskCommentsNum);
        }

        public void PutUpdateComments(SupportTaskComment comment)
        {
            if (comment.SupportTaskCommentsNum == 0)
                m_baseRepository.Add<SupportTaskComment>(comment);
            else
                m_baseRepository.Edit<SupportTaskComment>(comment);
            m_baseRepository.SaveChanges();
        }

        public async Task DeleteComments(int SupportTaskCommentsNum)
        {
            await m_baseRepository.ExecuteStoredProcedure("[Talent].[USP_PF_Delete_SupportTaskComments]", new SqlParameter[] { new SqlParameter("@SupportTaskCommentNum", SupportTaskCommentsNum) });
        }

        public async Task<DataTable> GetTaskExportdata(string fromDate, string toDate)
        {
            SqlParameter[] parameters = { new SqlParameter("@FromDate", fromDate), new SqlParameter("@ToDate", toDate) };
            var taskDetails = await m_baseRepository.GetDataTableFromStoredProcedure("[Talent].[USP_PF_GET_TaskExportData]", parameters);
            return taskDetails;
        }

        public void updateSpentHours(string spentHours)
        {
            BusSetting data = m_baseRepository.GetQuery<BusSetting>(x => x.KeyId == "FeatureConfiguration" && x.KeyValue == "SupportHours").FirstOrDefault();
            data.KeyDataValue = spentHours;
            m_baseRepository.Edit<BusSetting>(data);
            m_baseRepository.SaveChanges();
            m_tenantCacheProvider.RemoveCache(ApplicationCacheConstants.BussinessSetting);
        }

        public bool GetUserIsDuplicate(string userID)
        {
            return m_baseRepository.GetQuery<AppUser>(s => s.UserID == userID).ToList().Count() > 0;
        }
    }
}
