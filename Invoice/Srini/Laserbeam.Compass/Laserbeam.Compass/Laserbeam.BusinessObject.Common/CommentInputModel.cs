// Copyright (c) 2014 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.

// Component Name  :  Comment Input Model
// Description     :  To get the comments	
// Author          :  Boobalan		
// Creation Date   :  21-08-2014
namespace Laserbeam.BusinessObject.Common
{
    public class CommentInputModel
    {
        public int CommentKey { get; set; }
        public int CommentedEmployeeNum { get; set; }
        public string Comment { get; set; }
        public int CompensationCommentTypeNum { get; set; }
        public int EmpCommentNum { get; set; }
    }
}
