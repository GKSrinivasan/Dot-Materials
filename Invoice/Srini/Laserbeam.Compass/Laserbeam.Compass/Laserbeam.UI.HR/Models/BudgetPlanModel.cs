using Laserbeam.BusinessObject.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Laserbeam.UI.HR.Models
{
    public class BudgetPlanModel
    {
        public BudgetPlanConfiguration BudgetPlanConfiguration { get; set; }
        public List<SelectListItem> Menu { get; set; }
        public List<ExchangeCurrencies> ExchangeCurrencies { get; set; }
        public string BaseCurrencyCode { get; set; }
        public string BaseCultureCode { get; set; }
        public int BaseCurrencyNum { get; set; }
        public Nullable<decimal> BaseExchangeRate { get; set; }
        public bool ProrationVisible { get; set; }
    }
}