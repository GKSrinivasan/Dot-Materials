// Copyright (c) 2015 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.

// Component Name  :  EmployeeSalaryDetails Model
// Description     :  Contains the objects needed for application configuration
// Author          :  Arunraj		
// Creation Date   :  16-May-2017


namespace Laserbeam.BusinessObject.Common
{
    public class EmployeeSalaryDetails
    {
        public double? DirectBudget { get; set; }
        public double? IndirectBudget { get; set; }
        public double? DirectSpent { get; set; }
        public double? IndirectSpent { get; set; }
        public double? DirectBalance { get; set; }
        public double? IndirectBalance { get; set; }
        public int DirectMerit { get; set; }
        public int IndirectMerit { get; set; }
        public int DirectPromotion { get; set; }
        public int IndirectPromotion { get; set; }
        public int DirectAdjustment { get; set; }
        public int IndirectAdjustment { get; set; }
        public string MeritEnable { get; set; }
        public string PromotionEnable { get; set; }
        public string AdjustmentEnable { get; set; }
    }
}
