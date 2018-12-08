using Laserbeam.BusinessObject.Common;
using System.Collections.Generic;

namespace Laserbeam.UI.HR.Models
    {
    public class PromotionModel
        {
            public string newJobNum { get; set; }
            public string newTitle { get; set; }
            public string promotionComment { get; set; }
            public List<JobList> jobList { get; set; }
            public List<CommentModel> promotionComments { get; set; }
            public bool IsMeritLnkEnabled { get; set; }
        }
    }