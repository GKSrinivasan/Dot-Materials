// Copyright (c) 2014 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.

// Component Name  : 	Compensation Reportee Model
// Description     : 	This model contains the objects of grid display properties(Ex:column visibility,display name) 	
// Author          :	Roopan		
// Creation Date   : 	31-10-2014

using Laserbeam.BusinessObject.Common;
using Laserbeam.BusinessObject.Common.MeritBusinussObjects;
using Laserbeam.Constant.HR;
using System.Collections.Generic;
using System.Web.Mvc;
namespace Laserbeam.UI.HR.Models
{
    public class CompensationReporteeModel
    {
        public int EmployeeNum { get; set; }
        public string EmployeeName { get; set; }
        public List<SelectListItem> Menu { get; set; }        
        public List<MeritGridModel> compensationReportees { get; set; }        
        public AliasName aliasName { get; set; }
        public CompensationGridDisplay compensationGridDisplay { get; set; }
        public CompensationTypeConfiguration CompensationTypeConfiguration { get; set; }
        public List<ExchangeCurrencies> ExchangeCurrencies { get; set; }                
        public List<SelectListItem> rating { get; set; }
        public MenuType CompMenuType { get; set; }        
        public bool readOnly { get; set; }        
        public bool IsMyApproval { get; set; }
        public int RangeExceedPCT { get; set; }
    }
}
 