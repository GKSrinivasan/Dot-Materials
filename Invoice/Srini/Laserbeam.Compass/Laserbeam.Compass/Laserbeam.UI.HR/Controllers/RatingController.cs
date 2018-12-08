// Copyright (c) 2015 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.

// Component Name :   Rating 
// Description    :   Actions of Rating
// Author         :   Shaheena Shaik	
// Creation Date  :   27-March-2017 

using System.Web.Mvc;
using Laserbeam.BusinessObject.Common;
using Laserbeam.ProcessManager.Interfaces.Common;
using Laserbeam.UI.HR.Common;
using Kendo.Mvc.UI;
using Laserbeam.Constant.HR;
using Kendo.Mvc.Extensions;
using Laserbeam.UI.HR.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Laserbeam.UI.HR.Controllers
{
    [AuthorizeUser(UserRoleConstants.AllAdminRoles)]
    public class RatingController : Controller
    {

        #region Fields
        // Author         :   Shaheena Shaik
        // Creation Date  :   27-March-2017 

        /// <summary>
        /// Object of IRatingProcessManager
        /// </summary>
        private IRatingProcessManager m_ratingProcessManager;
        private ISessionProcessManager m_sessionProcessManager;
        private SessionManager m_sessionManager = new SessionManager();
        #endregion

        #region Constructors

        // Author           :  Shaheena Shaik
        // Creation Date    :  27-March-2017 
        // Reviewed By      :Hari.C
        // Reviewed Date    : 3-March-2017
        /// <summary>
        /// Initializes objects used in this class
        /// </summary>        
        /// <param name="sessionProcessManager">sessionProcessManager objects</param>
        /// <param name="dashboardBusinessManager">ratingBusinessManager objects</param>
        /// <param name="sessionManager">sessionManager objects</param>
        public RatingController(ISessionProcessManager sessionProcessManager, IRatingProcessManager ratingBusinessManager, SessionManager sessionManager)
        {
            m_sessionProcessManager = sessionProcessManager;
            m_ratingProcessManager = ratingBusinessManager;            
            m_sessionManager = sessionManager;
        }


        #endregion

        // GET: Rating
        public ActionResult Index()
        {
            return View();
        }

        // Author       : Shaheena Shaik
        // Creation Date:28-March-2017
        // Reviewed By  :Hari.C
        // Reviewed Date: 3-March-2017
        /// <summary>
        /// Getting the Rating page details
        /// </summary>
        /// <returns>returning a view</returns>
        public ActionResult Home()
        {
            return View();
        }

        // Author       : Shaheena Shaik
        // Creation Date:28-March-2017
        // Reviewed By  :Hari.C
        // Reviewed Date: 3-March-2017
        /// <summary>
        /// Getting RatingGrid data from database
        /// </summary>
        /// <param name="request"></param>
        /// <param name="Rating"></param>
        /// <returns>Returning Json object</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxChildActionOnly]
        public JsonResult GetConfigureRatingGridData([DataSourceRequest]DataSourceRequest request, string Rating)
        {
            UserModel userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
            var result =  m_ratingProcessManager.GetConfigureRatingGridData();
            return Json(result.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxChildActionOnly]
        public JsonResult GetConfigureRatingGridDataForRange([DataSourceRequest]DataSourceRequest request, string Rating)
        {
            UserModel userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
            var result = m_ratingProcessManager.GetConfigureRatingGridDataForRange();
            return Json(result.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        // Author       : Shaheena Shaik
        // Creation Date:28-March-2017
        // Reviewed By  :Hari.C
        // Reviewed Date: 3-March-2017
        /// <summary>
        /// Creating EditRating Popup
        /// </summary>
        /// <returns>Returning a partial view</returns>
        [HttpGet]
        public PartialViewResult _EditConfigureRatingPopup()
        {
            RatingViewModel configureRatingModel = new RatingViewModel();
            return PartialView(configureRatingModel);
        }

        // Author       : Shaheena Shaik
        // Creation Date:28-March-2017
        // Reviewed By  :Hari.C
        // Reviewed Date: 3-March-2017
        /// <summary>
        /// Creating AddRating Popup
        /// </summary>
        /// <param name="ratingNum"> To which ratingNum we are going to create a popup</param>
        /// <param name="ratingID">Rating Id of a row</param>
        /// <param name="ratingDescription">Rating Description of a row</param>
        /// <param name="ratingType">Rating type of a row</param>
        /// <param name="lowRange">Low range value</param>
        /// <param name="highRange">High range value</param>
        /// <param name="sortOrder">order of a newly creating rating</param>
        /// <returns>Returning partial view</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxChildActionOnly]
        public PartialViewResult _RatingPopUpModify(RatingViewModel ratingData)
        {
            return PartialView("_EditConfigureRatingPopup", ratingData);
        }
        // Author       : Shaheena Shaik
        // Creation Date:28-March-2017
        // Reviewed By  :Hari.C
        // Reviewed Date: 3-March-2017
        /// <summary>
        /// Adding And Updating Rating into the database
        /// </summary>
        /// <param name="configRatingObject">Model object which includes the edited or newly created Rating values</param>
        /// <returns>Returning javascript</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxChildActionOnly]
        public async Task<JsonResult> UpdateConfigureRatingGridData([DataSourceRequest]DataSourceRequest request, [Bind(Prefix = "models")] List<ConfigureRating> RatingGridData)
        {

            var userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
            var loginUserNum = userModel.UserNum;
            string message = string.Empty;
            bool updatesStatus = await m_ratingProcessManager.UpdateRatingRange(RatingGridData, loginUserNum);
            return Json(new
            {
                Message = "Great ! you have successfully set the Range."
            });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxChildActionOnly]
        public async Task<JavaScriptResult> _EditConfigureRatingPopup(RatingViewModel configRatingObject)
        {
            UserModel userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
            configRatingObject.UpdatedBy = userModel.UserNum;
            RatingPopupModel model = new RatingPopupModel();
            model.HighRange = configRatingObject.HighRange;
            model.LowRange = configRatingObject.LowRange;
            model.MinRange = configRatingObject.MinRange;
            model.MaxRange = configRatingObject.MaxRange;
            model.RatingDescription = configRatingObject.RatingDescription;
            model.RatingId = configRatingObject.RatingId;
            model.RatingNum = configRatingObject.RatingNum;
            model.RatingOrder = configRatingObject.RatingOrder;
            model.RatingType = configRatingObject.RatingType;
            
            bool isUpdated = await m_ratingProcessManager.UpdateAndAddRatingDetails(model);
         
            return JavaScript("CloseAfterUpdateRating();");      
        }
              
        

        // Author       : Shaheena Shaik
        // Creation Date:28-March-2017
        // Reviewed By  :Hari.C
        // Reviewed Date: 3-March-2017
        /// <summary>
        /// Deleting a rating from database
        /// </summary>
        /// <param name="ratingNum">The Rating num  which we are going to delete</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxChildActionOnly]
        public JsonResult DeleteRatingData(int ratingNum)
        {
            m_ratingProcessManager.DeleteRatingDetails(ratingNum);
            var message ="Deleted Successfully";
            return Json(message,JsonRequestBehavior.AllowGet);
        }


        // Author       : Hari.C
        // Creation Date:28-March-2017
        /// <summary>
        ///Remote Validation method 
        /// </summary>
        /// <param name="ratingNum">The Rating num  which we are going to delete</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult RatingValidation(RatingViewModel model)
        {
            return Json(m_ratingProcessManager.RatingValidation(model.RatingNum, model.RatingDescription) == false ? true : false);
        }
        

    }
}