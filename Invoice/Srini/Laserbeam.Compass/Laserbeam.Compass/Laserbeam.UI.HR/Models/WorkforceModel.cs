using Laserbeam.BusinessObject.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Laserbeam.UI.HR.Models
{
    public class WorkforceModel
    {
        public RuleConfiguration RuleConfiguration { get; set; }
        public List<SelectListItem> RatingRange { get; set; }
    }
}