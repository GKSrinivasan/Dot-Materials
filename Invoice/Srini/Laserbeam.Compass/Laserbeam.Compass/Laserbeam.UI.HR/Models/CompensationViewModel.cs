using Laserbeam.BusinessObject.Common;
using Laserbeam.BusinessObject.Common.MeritBusinussObjects;
using Laserbeam.Constant.HR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Laserbeam.UI.HR.Models
{
    public class CompensationViewModel
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
        public List<SelectListItem> RatingRange { get; set; }
        public MenuType CompMenuType { get; set; }
        public bool readOnly { get; set; }
        public bool IsMyApproval { get; set; }
        public int RangeExceedPCT { get; set; }
        public List<SelectListItem> filterSortList { get; set; }
        public BudgetModel budgetModel { get; set; }
        public int Year { get; set; }
        public int userNum { get; set; }
        public int LoggedInEmployeeNum { get; set; }
        public int DefaultCurrencyNum { get; set; }
       
    }
}