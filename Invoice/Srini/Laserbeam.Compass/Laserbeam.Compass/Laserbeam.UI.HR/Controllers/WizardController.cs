// Copyright (c) 2015 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.
// Component Name  :    Wizard Controller
// Description     : 	Wizard Methods
// Author          :	Raja Ganapathy
// Creation Date   : 	30-Mar-2017 

using Laserbeam.BusinessObject.Common;
using Laserbeam.Constant.HR;
using Laserbeam.ProcessManager.Interfaces.Common;
using Laserbeam.UI.HR.Common;
using System.Web.Mvc;
using System;
using System.Threading.Tasks;

namespace Laserbeam.UI.HR.Controllers
{
    [AuthorizeUser(UserRoleConstants.SuperAdmin)]
    public class WizardController : Controller
    {
        #region Fields
        // Author         :   Raja Ganapathy
        // Creation Date  :   30-Mar-2017 
        /// <summary>
        /// Object of IWorkForceProcessManager
        /// </summary>
        private IRuleConfigurationProcessManager m_ruleConfigurationProcessManager;
        private SessionManager m_sessionManager;
        #endregion


        #region Constructor
        // Author         :   Raja Ganapathy
        // Creation Date  :   30-Mar-2017 
        /// <summary>
        /// Initialize the processManager and session 
        /// </summary>
        /// <param name="workForceProcessManager">Object of IWorkForceProcessManager</param>
        /// <param name="sessionManager">Object of SessionManager</param>
        public WizardController(IRuleConfigurationProcessManager ruleConfigurationProcessManager, SessionManager sessionManager)
        {
            m_ruleConfigurationProcessManager = ruleConfigurationProcessManager;
            m_sessionManager = sessionManager;
        }
        #endregion
               
        [HttpGet]
        public ActionResult Home()
        {
            UserModel userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
             ViewBag.Step=m_ruleConfigurationProcessManager.GetWizardDetails(userModel.UserNum);
            ViewBag.UserName = userModel.UserName;
            return View();
        }
       
        [HttpGet]
        public PartialViewResult _Welcome()
        {
            UserModel userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
            ViewBag.UserName = userModel.UserName;
            return PartialView();
        }

        [HttpGet]
        public PartialViewResult _DefineBudget()
        {
            ViewBag.BudgetPct = m_ruleConfigurationProcessManager.GetBudgetPct();
            return PartialView();
        }

        [HttpGet]
        public PartialViewResult _Complete()
        {
            UserModel userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
            BusSettingModel busSettingModel = m_ruleConfigurationProcessManager.GetBusSetting();
            ViewBag.UserName = userModel.UserName;
            return PartialView(busSettingModel);
        }

       [HttpPost]
       [ValidateAntiForgeryToken]
       [AjaxChildActionOnly]
       public JsonResult UpdateStepCount(int Step)
        {
            UserModel userModel = m_sessionManager.GetSession<UserModel>(SessionConstants.UserModel);
            bool isWizard = Step == 5 ? true : false;
          
            bool status=m_ruleConfigurationProcessManager.PutWizardDetails(userModel.UserNum, Convert.ToByte(Step),isWizard);
            //if(isWizard)
            //m_ruleConfigurationProcessManager.clearAllData();
            return Json((status) ? 1 : 0);

        }
       
    }
}