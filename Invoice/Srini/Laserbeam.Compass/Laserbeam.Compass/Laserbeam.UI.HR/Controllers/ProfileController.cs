using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Laserbeam.BusinessObject.Common;
using Laserbeam.Constant.HR;
using Laserbeam.ProcessManager.Interfaces.Common;
using Laserbeam.UI.HR.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Laserbeam.UI.HR.Controllers
{
    [AuthorizeUser(UserRoleConstants.SuperAdmin)]
    public class ProfileController : Controller
    {

        #region Fields
        private SessionManager m_sessionManager;
        private IProfileProcessManager m_profileProcessManager;
        private readonly IAccountProcessManager m_accountProcessManager;
        #endregion

        #region Constructors
        // Author         :  
        // Creation Date  :    
        /// <summary>
        /// Initializes objects used in this class
        /// </summary>        
        /// <param name="commentProcessManager">profileProcessManager objects</param>
        /// <param name="sessionManager">sessionManager objects</param>
        public ProfileController(IProfileProcessManager profileProcessManager, SessionManager sessionManager, IAccountProcessManager accountProcessManager)
        {
            m_profileProcessManager = profileProcessManager;
            m_sessionManager = sessionManager;
            m_accountProcessManager = accountProcessManager;
        }
        #endregion

        #region Public Methods
        [HttpGet]
        public ActionResult Home()
        {
            UserProfileModel model = new UserProfileModel();
            string requestURL = this.GetTenantUrl();
            UserModel user = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
            AppConfigModel appConfigModel = m_accountProcessManager.GetAppSetting();
            int year = Convert.ToInt32(appConfigModel.MeritCycleYear);
            model = m_profileProcessManager.GetUserProfileDetails(year, user.UserNum, requestURL);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxChildActionOnly]
        public ActionResult updateUserDetails(UserProfileModel userDetails)
        {
            UserModel user = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
            AppConfigModel appConfigModel = m_accountProcessManager.GetAppSetting();
            int year = Convert.ToInt32(appConfigModel.MeritCycleYear);
            m_profileProcessManager.updateUserDetails(year, user.UserNum, userDetails);
            user.UserName = userDetails.FirstName+" "+userDetails.LastName;
            m_sessionManager.SetSession<UserModel>(SessionConstants.UserModel, user);
            return null;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult UpdateUploadedImage()
        {
            HttpPostedFileBase fileContent = null;
            fileContent = Request.Files[0];
            string tenant = this.GetTenant();
            var fileName = Path.GetFileName(fileContent.FileName);
            string currFilePath = "~/Images/Logo/";
            string path = Path.Combine(currFilePath,fileName.Replace(Path.GetFileNameWithoutExtension(fileName), tenant));
            string fileArray = Server.MapPath(path);
            var pngFile = Server.MapPath("~/Images/Logo/"+ tenant +".png");
            var jpgFile = Server.MapPath("~/Images/Logo/" + tenant + ".jpg");
            System.IO.File.Delete(pngFile);
            System.IO.File.Delete(jpgFile);
            fileContent.SaveAs(fileArray);
            return null;
        }


        [HttpGet]
        public PartialViewResult _AddTask()
        {
            TaskModel model = new TaskModel();
           return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxChildActionOnly]
        public JsonResult TeamTitle()
        {
            var teamTitles = m_profileProcessManager.GetTeamTitle();
            return Json(teamTitles, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxChildActionOnly]
        public ActionResult _AddTask(TaskModel Model)
        {
            UserModel user = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
            m_profileProcessManager.AddTaskDetails(Model, user.UserNum);
            return null;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxChildActionOnly]
        public ActionResult _EditTask(int taskNum)
        {

            var model = m_profileProcessManager.GetTaskDetails(taskNum);
            return PartialView("_EditTask", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxChildActionOnly]
        public async Task<JsonResult> GetTaskData([DataSourceRequest]  DataSourceRequest request)
        {
            UserModel user = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
            var result = await m_profileProcessManager.GetTaskData(user.UserNum);
            return Json(result.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxChildActionOnly]
        public PartialViewResult _SupportTaskComment(int supportTaskNum)
        {
            UserModel userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
            IEnumerable<SupportTaskCommentsModel> commentModel = m_profileProcessManager.GetTaskComment(supportTaskNum, userModel.UserNum);
            return PartialView(commentModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxChildActionOnly]
        public JavaScriptResult SupportTaskComment(string taskComment, bool IsEditItem = false, int SupportTaskCommentsNum = 0 ,int SupportTaskNum=0)
        {
            if (!string.IsNullOrEmpty(taskComment))
            {
                UserModel user = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
                SupportTaskCommentsModel comments = new SupportTaskCommentsModel();
                comments.Comments = HttpUtility.HtmlDecode(taskComment);
                comments.CreatedBy = user.UserNum;
                comments.SupportTaskCommentsNum = SupportTaskCommentsNum;
                comments.SupportTaskNum = SupportTaskNum;
                m_profileProcessManager.PutUpdateComments(comments, IsEditItem);
            }
            return JavaScript("closeAfterCommentsSlide();");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxChildActionOnly]
        public async Task<JsonResult> DeleteComment (int SupportTaskCommentsNum)
        {
            await m_profileProcessManager.DeleteComments(SupportTaskCommentsNum);
            string message = "Deleted Successfully";
            return Json(message, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task TaskExport( string fromDate,string toDate)
        {
            var taskdata = await m_profileProcessManager.GetTaskExportdata(fromDate, toDate);
            ExportExcel.ToExcel(taskdata, "TaskDetail");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxChildActionOnly]
        public JsonResult updateSpentHours(string spentHours)
        {
            m_profileProcessManager.updateSpentHours(spentHours);
            return null;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxChildActionOnly]
        public JsonResult GetUserIsDuplicate(string userID)
        {
            bool result = m_profileProcessManager.GetUserIsDuplicate(userID);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}