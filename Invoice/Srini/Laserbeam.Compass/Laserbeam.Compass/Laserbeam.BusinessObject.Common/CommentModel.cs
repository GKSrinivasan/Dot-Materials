// Copyright (c) 2014 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.

// Component Name  :  Comment Model
// Description     :  To get all the necessary comments based on the comment type	
// Author          :  Boobalan		
// Creation Date   :  21-08-2014
      
using System;
using System.ComponentModel.DataAnnotations;
namespace Laserbeam.BusinessObject.Common

{
    public class CommentModel
    {
        public string EmployeeName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmployeeNameShort
        {
            get
            {
                return string.Concat((this.EmployeeName != null?this.EmployeeName[0]:' '));
            }
        }
        public string Comment { get; set; }
        public string CommentKey { get; set; }
        [DisplayFormat(DataFormatString="{0:d}")]
        public DateTime? CommentUpdatedDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? CommentCreatedDate { get; set; }
        public int? CreatedByUserNumOrEmpNum { get; set; }
        public int? UpdatedByUserNumOrEmpNum { get; set; }
        public int EmpCommentNum { get; set; }
        public bool IsExceedMeritGuideline { get; set; }
        public bool IsGeneralComments { get; set; }
        public bool IsExceedAdjustmentGuideline { get; set; }
        public bool IsPromotionComments { get; set; }
        public bool IsWorkFlowComments { get; set; } 
        public int CompensationTypeNum { get; set; }
        public string DateFormat { get; set; }
        public bool IsExceedBonusGuideline { get; set; }
        public string Label { get; set; }
       
    }
}
