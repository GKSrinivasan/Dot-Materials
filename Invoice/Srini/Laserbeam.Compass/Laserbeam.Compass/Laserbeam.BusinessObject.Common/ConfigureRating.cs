// Copyright (c) 2014 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.

// Component Name  :  ConfigureRating
// Description     :  Contains the objects needed for Rating
// Author          :  Shaheena Shaik		
// Creation Date   :  28-March-2017

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laserbeam.BusinessObject.Common
{
    public class ConfigureRating
    {
        public int RatingNum { get; set; }
        public string RatingId { get; set; }
        public string RatingType { get; set; }
        public int? UpdatedBy { get; set; }
        public string RatingDescription { get; set; }
        public string HighRange { get; set; }
        public string LowRange { get; set; }
        public int? RatingOrder { get; set; }
        public string MinRange { get; set; }
        public string MaxRange { get; set; }

    }
}
