// Copyright (c) 2014 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.

// Component Name  :  RatingPopupModel
// Description     :  Contains the objects needed for Rating
// Author          :  Shaheena Shaik		
// Creation Date   :  3-March-2017

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;


namespace Laserbeam.BusinessObject.Common
{
    public class RatingPopupModel
    {
        public int RatingNum { get; set; }
        public string RatingId { get; set; }
        public string RatingType { get; set; }
        public int? UpdatedBy { get; set; }

        [Required(ErrorMessage="Please Enter Rating Description")]
        [MaxLength(50)]
       // [Remote("RatingValidation", "Rating", HttpMethod = "Post", ErrorMessage = "Enter valid Rating Description ")]
        public string RatingDescription { get; set; }

        [MaxLength(20)]
        public string HighRange { get; set; }

        [MaxLength(20)]
        public string LowRange { get; set; }
        [MaxLength(20)]
        public string MinRange { get; set; }

        [MaxLength(20)]
        public string MaxRange { get; set; }


        [Range(0, 9999)]
        public int? RatingOrder { get; set; }
    }
}
