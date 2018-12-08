// Copyright (c) 2014 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.

// Component Name  :  MessageModel
// Description     :  Contains the objects needed for application configuration
// Author          :  Shaheena Shaik		
// Creation Date   :  18-April-2017

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laserbeam.BusinessObject.Common
{
    public class MessageModel
    {
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string EmailID { get; set; }
        public int UserNum { get; set; }
        public int UserRoleNum { get; set; }
        public int orgGroupID { get; set; }
        public bool isChecked { get; set; }
        public string grpNum { get; set; }
        public string userType { get; set; }
        public string Status { get; set; }
        public bool? successStatus { get; set; }
        public string SecretKey { get; set; }
    }
}
