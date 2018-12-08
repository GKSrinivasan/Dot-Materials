// Copyright (c) 2015 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.

// Component Name :   PageCustomization 
// Description    :   Actions of PageCustomization
// Author         :   Muthuvel Sabarish M	
// Creation Date  :   27-Mar-2017

using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Laserbeam.BusinessObject.Common;
using Laserbeam.Constant.HR;
using Laserbeam.ProcessManager.Interfaces.Common;
using Laserbeam.UI.HR.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Laserbeam.UI.HR.Controllers
{
    [AuthorizeUser(UserRoleConstants.AllAdminRoles)]
    public class PageCustomizationController : Controller
    {

        #region Fields
        // Author         :   Muthuvel Sabarish M	
        // Creation Date  :   27-Mar-2017 
        /// <summary>
        /// Object of IPageCustomizationProcessManager
        /// </summary>
        private IPageCustomizationProcessManager m_pageCustomizationProcessManager;

        // Author         :   Muthuvel Sabarish M	
        // Creation Date  :   27-Mar-2017 
        /// <summary>
        /// Object of session manager
        /// </summary>
        private SessionManager m_sessionManager;
        #endregion

        #region Constructor
        // Author         :   Muthuvel Sabarish M	
        // Creation Date  :   27-Mar-2017 
        /// <summary>
        // Initializes the objects in this class
        // </summary>
        // <param name="PageCustomizationProcessManager">Object of IPageCustomizationProcessManager</param>
        public PageCustomizationController(IPageCustomizationProcessManager PageCustomizationProcessManager)
        {
            m_pageCustomizationProcessManager = PageCustomizationProcessManager; ;
            m_sessionManager = new SessionManager();
        }
        #endregion



        // Author         :   Muthuvel Sabarish M	
        // Creation Date  :   27-Mar-2017 
        // <summary>
        //  It returns the main view 
        // </summary>
        // <returns>Returns the view</returns>
        [HttpGet]
        public ActionResult Home()
        {
            return View();
        }

        //// Author         :   Muthuvel Sabarish M	
        //// Creation Date  :   27-Mar-2017 
        //// <summary>
        ////  It returns the main view 
        //// </summary>
        //// <returns>Returns the view</returns>
        //public ActionResult _PageCustomization()
        //{
        //    return PartialView();
        //}
        // Author         :   Hari C	
        // Creation Date  :   02-May-2017 
        // <summary>
        //  It returns the main view 
        // </summary>
        // <returns>Returns the view</returns>
        [HttpGet]
        public ActionResult _GridDisplay()
        {
            return PartialView();
        }
        // Author         :   Hari C	
        // Creation Date  :   02-May-2017 
        // <summary>
        //  It returns the main view 
        // </summary>
        // <returns>Returns the view</returns>
        [HttpGet]
        public ActionResult _PopupDisplay()
        {
            return PartialView();
        }
        // Author         :   Hari C	
        // Creation Date  :   02-May-2017 
        // <summary>
        //  It returns the main view 
        // </summary>
        // <returns>Returns the view</returns>
        [HttpGet]
        public ActionResult _FilterDisplay()
        {
            return PartialView();
        }
        // Author         :   Hari C	
        // Creation Date  :   02-May-2017 
        // <summary>
        //  It returns the main view 
        // </summary>
        // <returns>Returns the view</returns>
        [HttpGet]
        public ActionResult _ExportDisplay()
        {
            return PartialView();
        }

        // Author        :  Muthuvel Sabarish.M
        // Creation Date :  25-April-2017
        // <summary>
        //Reset the data and bind to grid 
        //</summary>        
        // <param name=""></param>  
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxChildActionOnly]
        public async Task<JsonResult> ResetAction()
        {
            int value = await m_pageCustomizationProcessManager.ResetPageCustomization();
            if (value > 0)
            {
                return Json("Reset successfully");
            }
            return Json("An error occurred while Reset");
        }

        // Author        :  Muthuvel Sabarish.M
        // Creation Date :  25-April-2017
        // <summary>
        //update the data and bind to grid 
        //</summary>        
        // <param name=""></param>  
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxChildActionOnly]
        public JsonResult GetUserDetails([DataSourceRequest] DataSourceRequest request,string display)
        {
            var PageCustomizationDetails = m_pageCustomizationProcessManager.getUserCustomizationDetails(display);
            return Json(PageCustomizationDetails.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        // Author        :  Muthuvel Sabarish.M
        // Creation Date :  25-April-2017
        // <summary>
        //Reset the data and bind to grid 
        //</summary>        
        // <param name=""></param>  
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxChildActionOnly]
        public JsonResult UpdatePageCustomization(List<PageCustomization> pageCustomizationDetails)
        {
            try
            {
                UserModel userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
                m_pageCustomizationProcessManager.UpdatePageCustomization(pageCustomizationDetails, userModel.UserNum);
                if (pageCustomizationDetails != null)
                {
                    return Json("Updated successfully");
                }
            }
            catch //(Exception ex)
            {
                return Json("An error occurred while updating user information");

            }
            return Json("Updated successfully");
        }

        // Author        :  Muthuvel Sabarish.M
        // Creation Date :  25-April-2017
        // <summary>
        //filter and sort view
        //</summary>        
        // <param name=""></param>  
        [HttpGet]
        public PartialViewResult _FilterSort()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem { Text = "Fields", Value = "ColumnName"  });
            list.Add(new SelectListItem { Text = "Display Name", Value = "AliasName" });
            list.Add(new SelectListItem { Text = "Popup Display", Value = "PopupDisplay"});
            list.Add(new SelectListItem { Text = "Grid Display", Value = "GridDisplay" });
            list.Add(new SelectListItem { Text = "Export Display", Value = "ExportDisplay" });
            list.Add(new SelectListItem { Text = "Filter Display", Value = "FilterDisplay" });
            return PartialView(list);
        }

    }
}