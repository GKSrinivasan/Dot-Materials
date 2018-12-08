// Copyright (c) 2015 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.

// Component Name  :    SetCultureAttribute
// Description     : 	This is used to set the culture
// Author          :	Roopan		
// Creation Date   : 	MAY-09-2015

using System.ComponentModel;
using System.Globalization;
using System.Threading;
using System.Web.Mvc;

namespace Laserbeam.UI.HR.Common
{
    public class SetCultureAttribute : ActionFilterAttribute
    {
        public SetCultureAttribute()
        {
            // Use the DefaultValue propety of each property to actually set it, via reflection.
            foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(this))
            {
                DefaultValueAttribute attr = (DefaultValueAttribute)prop.Attributes[typeof(DefaultValueAttribute)];
                if (attr != null)
                {
                    prop.SetValue(this, attr.Value);
                }
            }
        }

        [DefaultValue("en")]
        public string Language { get; set; }
        [DefaultValue("us")]
        public string Culture { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(string.Format("{0}-{1}", Language, Culture));
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(string.Format("{0}-{1}", Language, Culture));
        }
    }
}