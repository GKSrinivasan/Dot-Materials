using Laserbeam.BusinessObject.Common;
using Laserbeam.Constant.HR;
using Laserbeam.ProcessManager.Interfaces.Common;
using Laserbeam.UI.HR.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using static System.Net.WebRequestMethods;

namespace Laserbeam.UI.HR.Controllers
{
    [AuthorizeUser]
    public class ChatboxController : Controller
    {
        #region Fields
        private IChatBoxProcessManager m_chatBoxProcessManager;
        private SessionManager m_sessionManager;// = new SessionManager();      
        private readonly IAccountProcessManager m_accountProcessManager;
        #endregion

        #region Constructors

        // Author         : 
        // Creation Date  :  
        /// <summary>
        /// Initializes objects used in this class
        /// </summary>        
        /// <param name="sessionProcessManager">sessionProcessManager objects</param>
        /// <param name="chatBoxProcessManager">chatBoxProcessManager objects</param>
        /// <param name="sessionManager">sessionManager objects</param>
        public ChatboxController( IChatBoxProcessManager chatBoxProcessManager, SessionManager sessionManager,IAccountProcessManager accountProcessManager)
        {
            m_chatBoxProcessManager = chatBoxProcessManager;
            m_sessionManager = sessionManager;
            m_accountProcessManager = accountProcessManager;
        }


        #endregion
        // GET: Chatbox
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Home()
        {
            return View();
        }
        public PartialViewResult _CompChat()
        {
            UserModel userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
            AppConfigModel appConfig = m_accountProcessManager.GetAppSetting();
            var ManagerList = m_chatBoxProcessManager.GetChatUserDetails(userModel.UserNum, appConfig);
            var UserStatus = m_chatBoxProcessManager.GetStatusList();
            var model = new ChatBoxModel
            {
                chatAccountModel = ManagerList,
                chatStatusList = UserStatus,
            };
            model.loggedInUserName = userModel.UserName;
            model.loggedInUserShortName = (userModel.UserName != null) ? userModel.UserName.Substring(0, 1) : " ";
            model.loggeedInUserNum = userModel.UserNum;
            model.UserStatus = m_chatBoxProcessManager.GetAppuserStatus(userModel.UserNum);
            return PartialView("_CompChat", model);
        }
        public async Task<JsonResult> UpdateStatus(int ID, int UserNum)
        {
            await m_chatBoxProcessManager.UpdateStatus(ID, UserNum);
            var userstatus = m_chatBoxProcessManager.GetAppuserStatus(UserNum);
            return Json(userstatus, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Chatbot()
        {
            return View();
        }
        public PartialViewResult _ChatDetails(int selectedUserNum = 0, string selectedUserName = "",string Chat="")
        {
            ViewBag.selectedUserNum = selectedUserNum;
            ViewBag.selectedUserName = selectedUserName;
            ViewBag.selectedUserShortName = (selectedUserName != "") ? selectedUserName.Substring(0, 1) : " ";
            var userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
            AppConfigModel appConfig = m_accountProcessManager.GetAppSetting();
            ViewBag.loggedInUserNum = userModel.UserNum;
            ViewBag.loggedInUserName = userModel.UserName;
            ViewBag.ApiKey = appConfig.TokBoxApiKey;
            List<ChatDetails> data = new List<ChatDetails>();
            if (Chat == "")
            {
                data = m_chatBoxProcessManager.GetChatDetails(userModel.UserNum, selectedUserNum, appConfig);
            }else
            {
                data = m_chatBoxProcessManager.GetSearchChatDetails(userModel.UserNum, selectedUserNum, Chat);
            }
           
                ViewBag.chatStatus = data[0].userChatStatus;
                ViewBag.SessionId = data[0].SessionID ?? "";
                ViewBag.Token = data[0].Token ?? "";
                ViewBag.Count = data[0].SenderUserShortName == null ? 0 : 1;
            return PartialView(data);
        }
        public PartialViewResult _ChatManagerDetails()
        {
            UserModel userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
            AppConfigModel appConfig = m_accountProcessManager.GetAppSetting();
            var ManagerList = m_chatBoxProcessManager.GetChatUserDetails(userModel.UserNum, appConfig);
            var model = new ChatBoxModel();
            model.chatAccountModel = ManagerList;
            return PartialView("_ChatManagerDetails", model);
        }

        public PartialViewResult UpdateChat(int loggedInUserNum, string loggedInUserName, int selectedUserNum, string selectedUserName, int chatStatus, string chat)
        {
            AppConfigModel appConfig = m_accountProcessManager.GetAppSetting();
            ViewBag.selectedUserNum = selectedUserNum;
            ViewBag.selectedUserName = selectedUserName;
            ViewBag.selectedUserShortName = (selectedUserName != "") ? selectedUserName.Substring(0, 1) : " ";
            ViewBag.loggedInUserNum = loggedInUserNum;
            ViewBag.chatStatus = chatStatus;
            ViewBag.loggedInUserName = loggedInUserName;
            ViewBag.ApiKey = appConfig.TokBoxApiKey;
            int FileType = 3;
            var result = m_chatBoxProcessManager.UpdateChat(loggedInUserNum, selectedUserNum, chat, null, FileType, null, appConfig);
            ViewBag.chatStatus = result[0].userChatStatus;
            ViewBag.SessionId = result[0].SessionID ?? "";
            ViewBag.Token = result[0].Token ?? "";
            ViewBag.Count = result[0].SenderUserShortName == null ? 0 : 1;
            return PartialView("_ChatDetails", result);
        }


        [HttpPost]
        public JsonResult _UploadAttachment()
        {
            UserModel userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
            AppConfigModel appConfig = m_accountProcessManager.GetAppSetting();
            try
            {
                HttpFileCollectionBase files = Request.Files;
                HttpPostedFileBase FileData = files[0];
                string filename = FileData.FileName;
                var FileExtension = Path.GetExtension(filename);
                int FileType = 0;
                string FilePath = "";
                string FolderName = "";
                if (FileExtension == ".pdf")
                {
                    FolderName = "~/Uploads/PdfFiles";
                    FileType = 1;
                }
                else if (FileExtension == ".xlsx" || FileExtension == ".xls")
                {
                    FolderName = "~/Uploads/ExcelFiles";
                    FileType = 1;
                }
                else if (FileExtension == ".docx")
                {
                    FolderName = "~/Uploads/WordFiles";
                    FileType = 1;
                }
                else if (FileExtension == ".zip")
                {
                    FolderName = "~/Uploads/ZipFiles";
                    FileType = 1;
                }
                else
                {
                    FolderName = "~/Uploads/Images";
                    FileType = 2;
                }
                HttpPostedFileBase SelecedUserData = files[1];
                int selectedUserNum = Convert.ToInt32(SelecedUserData.FileName);
                var result = m_chatBoxProcessManager.UpdateChat(userModel.UserNum, selectedUserNum, null, FolderName, FileType, filename, appConfig);
                FilePath = HostingEnvironment.MapPath(string.Concat(FolderName+"\\"+result[0].ChatDetailNum.ToString()+"_"+filename));
                FileData.SaveAs(FilePath);
                return Json(result);
            }
            catch (Exception ex)
            {
                return Json("Error occurred. Error details: " + ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult DownloadAttachment(int FileNum,string FileName, string PathName)
        {
            string file = HostingEnvironment.MapPath(string.Concat(PathName + '/' + FileNum.ToString() + "_" + FileName));
            string contentType = "application / vnd.openxmlformats - officedocument.spreadsheetml.sheet";
            return File(file, contentType, FileName);
        }
        public ActionResult ChatSearch(int loggedInUserNum, int selectedUserNum, string chat)
        {
            var result = m_chatBoxProcessManager.GetSearchChatDetails(loggedInUserNum, selectedUserNum, chat);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}