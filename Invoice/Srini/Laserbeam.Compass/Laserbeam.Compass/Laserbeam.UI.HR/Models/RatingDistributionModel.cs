// Copyright (c) 2014 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.

// Component Name  :  Rating Distribution model
// Description     :  Contains the objects needed for rating distribution 	
// Author          :  Roopan		
// Creation Date   :  03-11-2014

using Laserbeam.BusinessObject.Common;
using System.Collections.Generic;
namespace Laserbeam.UI.HR.Models
{
    public class RatingDistributionModel
    {        
        public List<MeritBudget> MeritBudget { get; set; }        
        public List<MeritIncreasebyRatings> MeritIncreasebyRating { get; set; }
        public List<MeritIncreasebyRatings> MeritIncreasebyRatingGrid { get; set; }
        public List<MeritIncreasebyRatings> MeritIncreasebyRatingChart { get; set; }
        public List<PayRangeDistribution> PayRangeDistribution { get; set; }
        public List<PayRangeDistribution> PayRangeDistributionChart { get; set; }        
        public bool CompExclusion  { get; set; }
        public int UserNum { get; set; }
        public string EmployeeId { get; set; }        
    }
}