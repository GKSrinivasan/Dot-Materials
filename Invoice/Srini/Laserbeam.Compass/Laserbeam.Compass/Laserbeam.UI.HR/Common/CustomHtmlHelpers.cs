
// Copyright (c) 2015 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.
//
// Component Name :   CustomHtmlHelpers
// Description    :   Customized Mvc helpers
// Author         :   Boobalan Ranganathan
// Creation Date  :   Apr-16-2015

using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Xsl;

namespace Laserbeam.UI.HR
{
    public static class CustomHtmlHelpers
    {

        // Author         :   Boobalan Ranganathan
        // Creation Date  :   Apr-17-2015
        /// <summary>
        /// Transforms Xml file into Html using an Xsl file
        /// </summary>
        /// <param name="helper">Current instance of HtmlHelper</param>
        /// <param name="xmlPath">Virtual path of Xml file</param>
        /// <param name="xslPath">Virtual path of Xsl file</param>
        /// <returns></returns>
        public static MvcHtmlString XmlView(this HtmlHelper helper,string xmlPath,string xslPath)
        {
            XmlDocument xdoc = new XmlDocument();
            XslCompiledTransform xsl = new XslCompiledTransform();
            xdoc.Load(HttpContext.Current.Server.MapPath(xmlPath));
            xsl.Load(HttpContext.Current.Server.MapPath(xslPath));
            XmlReader xReader = XmlReader.Create(new StringReader(xdoc.InnerXml));
            StringBuilder stringBuilder = new StringBuilder();
            XmlWriter xWriter = XmlWriter.Create(stringBuilder);
            xsl.Transform(xReader,xWriter);
            return MvcHtmlString.Create(stringBuilder.ToString());
        }
    }
}