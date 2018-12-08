// Copyright (c) 2014 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.

// Component Name  :  Enumeration
// Description     :  Contains objects for enumerations	
// Author          :  Boobalan		
// Creation Date   :  21-05-2014

namespace Laserbeam.Constant.HR
{
    public enum LogInType
    {
        LogIn=1,
        ChangePassword=2
    }
    public enum UserCredentialStatus
    {
        /// <summary>
        /// User credential succeeded
        /// </summary>
        Valid = 1,
        /// <summary>
        /// User default credential succeeded
        /// </summary>
        ValidFirstTime = 2,
        /// <summary>
        /// User not available
        /// </summary>
        InvalidUser = 3,
        /// <summary>
        /// User password is not valid
        /// </summary>
        InvalidPassword = 4,
        /// <summary>
        /// User is locked
        /// </summary>
        Locked = 5,
        /// <summary>
        /// User is inactive
        /// </summary>
        InActive = 6,
        /// <summary>
        /// User password has been changed
        /// </summary>
        PasswordChanged = 7,
        /// <summary>
        /// User password has been reset
        /// </summary>
        PasswordReset = 8,
        /// <summary>
        /// User secretkey is not valid
        /// </summary>
        InValidSecretKey = 9,
        /// <summary>
        /// User secretkey time expired
        /// </summary>
        SecretKeyTimeExpired = 10
    }

    public enum CommentType
    {        
        Compensation = 1,
        CompensationMeritMandit = 2,
        BonusComment = 3,
        BonusMandateComment = 4,        
        Promotion = 5,
        Adjustment = 6,
        FeedBack=7,
        LTIP=8,
    }
    public enum ApprovalStatus
    {
        Submit = 1,
        Approve = 2,
        Reject = 3,        
    }

    
    public enum ViewPageType
    {
        Default = 0,        
        Compensation=1,
        RatingDistribution = 2,
        Analytics = 3,
        LTIP=4        
    }

    
    public enum MenuType
    {
        MyTeam = 1,
        AssignedTeam = 2,
        AssignedGroup = 3
    }
    public enum CompFilterType
    {
        None = 0,
        AscendingOrder = 1,
        DescendingOrder = 2,
        StartsWith = 3,
        Contains = 4
    }
            
    public enum ManagerActionType
    {
        Filter = 1,
        ClearFilter = 2,
        Export = 3,
        Local = 4,
        USD =5
    }

        
    public enum FilterOperation
    {
        Equals,
        GreaterThan,
        LessThan,
        GreaterThanOrEqual,
        LessThanOrEqual,
        Contains,
        StartsWith,
        EndsWith
    }
    public enum SortDirection
    {
        Ascending = 0,
        Descending = 1
    }

    public enum DataFormat
    {
        Money = 1,
        Percentage = 2,
        Date = 3,
        Text = 4
    }

    public enum DataControlType
    {
        Label = 1,
        TextBox = 2,
        DropDown = 3,
        Link = 4
    }
}
