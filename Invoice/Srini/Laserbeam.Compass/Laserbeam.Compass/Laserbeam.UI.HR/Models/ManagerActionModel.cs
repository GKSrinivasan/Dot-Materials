// Copyright (c) 2014 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.

// Component Name  : 	Manager Action Model
// Description     : 	Contains the properties needed for manager action
// Author          :	Roopan		
// Creation Date   : 	08-11-2014

using Laserbeam.BusinessObject.Common;
using Laserbeam.BusinessObject.Common.MeritBusinussObjects;
using Laserbeam.Constant.HR;
using System.Collections.Generic;
using System.Web.Mvc;
namespace Laserbeam.UI.HR.Models
{
    public class ManagerActionModel
    {
        public List<SelectListItem> Menu { get; set; }
        public List<SelectListItem> filterSortList { get; set; }
        public BudgetModel budgetModel { get; set; }
        public int EmployeeNum { get; set; }
        public MenuType CompMenuType { get; set; }        
        public CompensationTypeConfiguration CompensationTypeConfiguration { get; set; }
        public CompensationGridDisplay compensationGridDisplay { get; set; }
        public string EmployeeName { get; set; }
        public List<ExchangeCurrencies> ExchangeCurrencies { get; set; }
        public int Year { get; set; }
        public int userNum { get; set; }
        public int LoggedInEmployeeNum { get; set; }
        public int DefaultCurrencyNum { get; set; }
    }
}