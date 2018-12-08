
// Copyright (c) 2014 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.
// Component Name  :    Comment controller
// Description     : 	Actions of objective and review comments are defined	
// Author          :	Raja Ganapathy		
// Creation Date   : 	05-Jul-2016 

using Laserbeam.BusinessObject.Common;
using Laserbeam.Constant.HR;
using Laserbeam.ProcessManager.Interfaces.Common;
using Laserbeam.UI.HR.Common;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Laserbeam.UI.HR.Controllers
{
    [AuthorizeUser(UserRoleConstants.AllRoles)]
    public class CommentController : Controller
    {        
        #region Fields
        private SessionManager m_sessionManager;
        private ICommentProcessManager m_commentProcessManager;
        #endregion

        #region Constructors
        // Author         :  Raja Ganapathy
        // Creation Date  :  05-Jul-2016  
        /// <summary>
        /// Initializes objects used in this class
        /// </summary>        
        /// <param name="commentProcessManager">commentProcessManager objects</param>
        /// <param name="sessionManager">sessionManager objects</param>
        public CommentController(ICommentProcessManager commentProcessManager, SessionManager sessionManager)
        {
            m_commentProcessManager = commentProcessManager;
            m_sessionManager = sessionManager;
        }
        #endregion


        #region Actions
        // Author         :  Raja Ganapathy
        // Creation Date  :  05-Jul-2016  
        /// <summary>
        /// Gets comment view based on comment key and comment type
        /// </summary>
        /// <param name="empJobNum">Key for which the comment data is needed</param>
        /// <param name="commentType">Type of the comment for which the view is rendered</param>
        /// <returns>Returns comment view</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxChildActionOnly]
        public async Task<PartialViewResult> _Comment(int empJobNum, CommentType commentType,int rowIndex=0)
        {
            ViewBag.rowIndex = rowIndex;                                
            ViewBag.EmpJobNum = empJobNum;
            ViewBag.CommentType = commentType;
            UserModel userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
            int loggedSelectedUserNum = m_sessionManager.GetSession<int>(SessionConstants.LoggedSelectedUserNum);
            IEnumerable<CommentModel> commentModel = m_commentProcessManager.GetComments(empJobNum, commentType,userModel.UserNum,loggedSelectedUserNum);
            ViewBag.RuleConfiguration = m_commentProcessManager.GetRuleConfiguration();
            if (commentType == CommentType.Compensation)
            {
                var res = await m_commentProcessManager.UpdateCommentStatus(empJobNum, loggedSelectedUserNum);
            }
            return PartialView(commentModel);
        }

        // Author         :  Raja Ganapathy
        // Creation Date  :  05-Jul-2016  
        /// <summary>
        /// Process comment data submitted by comment view
        /// </summary>
        /// <param name="commentKey">Key for which the comment data to be updated</param>
        /// <param name="comment">comment string submitted by comment view</param>
        /// <param name="commentType">Type of the comment for which the view is rendered</param>
        /// <param name="IsEditItem">Denotes the comment is edited or not</param>
        /// <param name="EmpCommentNum">Denotes edited comment num</param>
        /// <param name="CompensationTypeNum">Denotes the compensation type</param>
        /// <returns>Returns javascript methods that needs to be executed in client-side</returns>        
        [HttpPost,AjaxChildActionOnly,ValidateAntiForgeryToken]
        [ActionName("_CommentPost")]
        public JavaScriptResult _Comment(int commentKey, string comment, CommentType commentType,  bool IsEditItem = false, int EmpCommentNum = 0, int CompensationTypeNum = 0)
        {
            CommentType storedCommentType = commentType;            
            if (!string.IsNullOrEmpty(comment))
            {
                UserModel userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
                EmployeeModel employeeModel = m_sessionManager.GetSession<EmployeeModel>(SessionConstants.EmployeeModel);
                CommentInputModel commentInput = new CommentInputModel();
                commentInput.Comment = HttpUtility.HtmlDecode(comment);
                commentInput.CommentKey = commentKey;
                commentInput.EmpCommentNum = EmpCommentNum;
                commentInput.CompensationCommentTypeNum = CompensationTypeNum;
                commentInput.CommentedEmployeeNum = userModel.UserNum;
                storedCommentType = commentType;
                m_commentProcessManager.PutUpdateComments(commentInput, storedCommentType,IsEditItem);

            }
            return JavaScript("closeAfterSaveSlide();");
        }

        // Author         :  Raja Ganapathy
        // Creation Date  :  26-Jul-2016  
        /// <summary>
        /// To delete the comments
        /// </summary>
        /// <param name="commentKey">Denotes the deleted comments num</param>
        /// <returns>Returns the json result</returns>
        [HttpPost,ValidateAntiForgeryToken,AjaxChildActionOnly]
        public async Task<JsonResult> DeleteComment(int commentKey)
        {            
            await m_commentProcessManager.DeleteComments(commentKey);
            string message = "Deleted Successfully";
            return Json(message, JsonRequestBehavior.AllowGet);           
        }            
        
        #endregion
        
    }
}
