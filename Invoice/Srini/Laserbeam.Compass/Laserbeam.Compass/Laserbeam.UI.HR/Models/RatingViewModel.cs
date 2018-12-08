using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Laserbeam.UI.HR.Models
{
    public class RatingViewModel
    {
        public int RatingNum { get; set; }
        public string RatingId { get; set; }
        public string RatingType { get; set; }
        public int? UpdatedBy { get; set; }

        [Required(ErrorMessage = "Please Enter Rating Description")]
        [MaxLength(50)]
        [Remote("RatingValidation", "Rating", AdditionalFields = "RatingNum", HttpMethod = "Post", ErrorMessage = "Rating Description already exists")]
        public string RatingDescription { get; set; }

        [RegularExpression(@"[0-9]{0,}\.[0-9]{2}", ErrorMessage = "Enter only two decimal places")]
        [MaxLength(20)]
        public string HighRange { get; set; }
                
        [RegularExpression(@"[0-9]{0,}\.[0-9]{2}", ErrorMessage = "Enter only two decimal places")]
        [MaxLength(20)]
        public string LowRange { get; set; }

      //  [RegularExpression(@"[0-9]{0,}\.[0-9]{2}", ErrorMessage = "Enter only two decimal places")]
        [MaxLength(20)]
        public string MinRange { get; set; }

       // [RegularExpression(@"[0-9]{0,}\.[0-9]{2}", ErrorMessage = "Enter only two decimal places")]
        [MaxLength(20)]
        public string MaxRange { get; set; }

        [Range(0, 9999)]
        public int? RatingOrder { get; set; }
    }
}