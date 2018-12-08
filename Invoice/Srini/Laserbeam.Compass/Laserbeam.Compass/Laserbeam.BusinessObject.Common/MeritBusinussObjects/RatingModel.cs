using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laserbeam.BusinessObject.Common.MeritBusinussObjects
{
    public class RatingModel
    {
        public int RatingNum { get; set; }
        public string RatingID { get; set; }
        public string RatingDescr { get; set; }
        public string RatingType { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<bool> IsCommentMandatory { get; set; }
        public string RatingDetailDescr { get; set; }
        public Nullable<decimal> HighRange { get; set; }
        public Nullable<decimal> LowRange { get; set; }
        public Nullable<int> RatingOrder { get; set; }
        public string RatingRange { get; set; }
    }
}
