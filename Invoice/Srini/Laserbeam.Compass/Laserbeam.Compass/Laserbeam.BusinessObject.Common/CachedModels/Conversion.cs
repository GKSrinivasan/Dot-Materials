// Copyright (c) 2014 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.

// Component Name :   Conversion Class
// Description    :   Model that converts a value to prefer type
// Author         :   Hariharasubramaniyan Chandrasekaran		
// Creation Date  :   08-March-2018
// Ticket ID      :   CL-1288
using System;

namespace Laserbeam.BusinessObject.Common.CachedModels
{
    public static class Conversion
    {
        public static bool convertToBool(string value)
        {
            if(value.ToLower().Trim()=="1" || value.ToLower().Trim()=="0")
                return (value != null && value.ToLower().Trim() == "1");
            else
            return (value != null && value.ToLower().Trim() == "yes");
        }
        public static int convertToInteger(string value)
        {

            return (string.IsNullOrWhiteSpace(value) ? 0 : Convert.ToInt32(value));
        }
        public static decimal convertToDecimal(string value)
        {

            return (string.IsNullOrWhiteSpace(value) ? 0 : Convert.ToDecimal(value));
        }
    }
}
