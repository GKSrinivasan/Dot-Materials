// Copyright (c) 2015 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.

// Component Name  :    RegexPasswordAttribute
// Description     : 	This is used to change the regex expression
// Author          :	Roopan		
// Creation Date   : 	MAY-09-2015

using System.ComponentModel.DataAnnotations;

namespace Laserbeam.UI.HR.Common
{
    public class RegexPasswordAttribute : RegularExpressionAttribute
    {
        public RegexPasswordAttribute()
            : base(GetRegex())
        { }

        private static string GetRegex()
        {
            return @"^(?=(.*\d){0})(?=(.*[a-z]){0})(?=(.*[A-Z]){0})(?=(.*[@#$&]){0})([A-Za-z0-9@#$&]){4,8}$";
        }
    }
}