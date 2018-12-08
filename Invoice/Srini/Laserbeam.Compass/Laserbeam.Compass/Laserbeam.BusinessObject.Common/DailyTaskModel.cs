// Copyright (c) 2015 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.

// Component Name  :  DailyTaskModel
// Description     :  Contains the objects needed for application configuration
// Author          :  Arunraj		
// Creation Date   :  16-May-2017

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Laserbeam.BusinessObject.Common
{
    public class DailyTaskModel
    {
        public int TaskNum { get; set; }
        public int UserNum { get; set; }
        public string TaskTitle { get; set; }
        [Required(ErrorMessage = "The content is required")]
        public string TaskDescr { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<System.DateTime> TaskCompletedDate { get; set; }
        public bool TaskCompleted { get; set; }
        public Nullable<bool> Active { get; set; }
        public int ManagerNum { get; set; }
        public int SubmitCount { get; set; }
        public int ApproveCount { get; set; }
        public int ReopenCount { get; set; }
        public int TotalCount { get; set; }        
        public string ManagerName { get; set; }
        public decimal YetToSubmitPct { get; set; }        
        public List<DailyTaskModel> DailyTaskList { get; set; }
        public List<DailyTaskModel> ApprovalList { get; set; }

    }
}
