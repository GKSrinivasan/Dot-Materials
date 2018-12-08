// Copyright (c) 2014 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.

// Component Name  :  UserMessageModel
// Description     :  Contains the objects needed for application configuration
// Author          :  Shaheena Shaik		
// Creation Date   :  18-april-2017

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laserbeam.BusinessObject.Common
{
    public class UserMessageModel
    {
        public int UserMessageNum { get; set; }
        public int AppMessageID { get; set; }
        public int UserNum { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
    }
}
