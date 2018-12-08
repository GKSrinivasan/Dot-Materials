using System;
using System.Data;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Text;
using System.Web.UI;
using Laserbeam.Libraries.Interfaces.Common;
using System.Text.RegularExpressions;
using System.Globalization;

namespace Laserbeam.Libraries.Common
{
    public class Export : IExport
    {
        #region Public Methods

        

        public void ExportWord<T>(List<T> data, string fileName)
        {
            HttpContext context = HttpContext.Current;
            context.Response.Clear();
            context.Response.Charset = "";
            context.Response.ContentEncoding = Encoding.Unicode;
            context.Response.BinaryWrite(Encoding.Unicode.GetPreamble());
            context.Response.ContentType = "application/msword";
            string strFileName = fileName + ".doc";
            context.Response.AddHeader("Content-Disposition", "inline;filename=" + strFileName);
            StringBuilder strHTMLContent = new StringBuilder();

            strHTMLContent.Append("<html xmlns:o='urn:schemas-microsoft-com:office:office'");
            strHTMLContent.Append("xmlns:w='urn:schemas-microsoft-com:office:word'");
            strHTMLContent.Append("xmlns='http://www.w3.org/TR/REC-html40'&gt;");
            strHTMLContent.Append("<head>");
            strHTMLContent.Append("<meta http-equiv=Content-Type content='text/html; charset=windows-1252'>");
            strHTMLContent.Append("<meta name=ProgId content=Word.Document>");
            strHTMLContent.Append("<meta name=Generator content='Microsoft Word 11'>");
            strHTMLContent.Append("<meta name=Originator content='Microsoft Word 11'>");
            strHTMLContent.Append("<link rel=File-List href='MyMemo_files/filelist.xml'>");
            strHTMLContent.Append("<!--[if gte mso 9]><xml>");
            strHTMLContent.Append("<o:DocumentProperties>");
            strHTMLContent.Append("<o:Author>SERVER2</o:Author>");
            strHTMLContent.Append("<o:LastAuthor>SERVER2</o:LastAuthor>");
            strHTMLContent.Append("<o:Revision>4</o:Revision>");
            strHTMLContent.Append("<o:TotalTime>2</o:TotalTime>");
            strHTMLContent.Append("<o:Created>2012-01-23T13:38:00Z</o:Created>");
            strHTMLContent.Append("<o:LastSaved>2012-01-23T14:28:00Z</o:LastSaved>");
            strHTMLContent.Append("<o:Pages>1</o:Pages>");
            strHTMLContent.Append("<o:Words>91</o:Words>");
            strHTMLContent.Append("<o:Characters>522</o:Characters>");
            strHTMLContent.Append("<o:Company>LaserBeam</o:Company>");
            strHTMLContent.Append("<o:Lines>4</o:Lines>");
            strHTMLContent.Append("<o:Paragraphs>1</o:Paragraphs>");
            strHTMLContent.Append("<o:CharactersWithSpaces>612</o:CharactersWithSpaces>");
            strHTMLContent.Append("<o:Version>11.5606</o:Version>");
            strHTMLContent.Append("</o:DocumentProperties>");
            strHTMLContent.Append("</xml><![endif]--><!--[if gte mso 9]><xml>");
            strHTMLContent.Append("<w:WordDocument>");
            strHTMLContent.Append("<w:View>Print</w:View>");
            strHTMLContent.Append("<w:Zoom>140</w:Zoom>");
            strHTMLContent.Append("<w:SpellingState>Clean</w:SpellingState>");
            strHTMLContent.Append("<w:GrammarState>Clean</w:GrammarState>");
            strHTMLContent.Append("<w:PunctuationKerning/>");
            strHTMLContent.Append("<w:ValidateAgainstSchemas/>");
            strHTMLContent.Append("<w:SaveIfXMLInvalid>false</w:SaveIfXMLInvalid>");
            strHTMLContent.Append("<w:IgnoreMixedContent>false</w:IgnoreMixedContent>");
            strHTMLContent.Append("<w:AlwaysShowPlaceholderText>false</w:AlwaysShowPlaceholderText>");
            strHTMLContent.Append("<w:Compatibility>");
            strHTMLContent.Append("<w:BreakWrappedTables/>");
            strHTMLContent.Append("<w:SnapToGridInCell/>");
            strHTMLContent.Append("<w:WrapTextWithPunct/>");
            strHTMLContent.Append("<w:UseAsianBreakRules/>");
            strHTMLContent.Append("<w:DontGrowAutofit/>");
            strHTMLContent.Append("</w:Compatibility>");
            strHTMLContent.Append("<w:BrowserLevel>MicrosoftInternetExplorer4</w:BrowserLevel>");
            strHTMLContent.Append("</w:WordDocument>");
            strHTMLContent.Append("</xml><![endif]--><!--[if gte mso 9]><xml>");
            strHTMLContent.Append("<w:LatentStyles DefLockedState='false' LatentStyleCount='156'>");
            strHTMLContent.Append("</w:LatentStyles>");
            strHTMLContent.Append("</xml><![endif]-->");
            strHTMLContent.Append("<style>");
            strHTMLContent.Append("<!--");
            strHTMLContent.Append("/* Style Definitions */");
            strHTMLContent.Append("p.MsoNormal, li.MsoNormal, div.MsoNormal");
            strHTMLContent.Append("{mso-style-parent:'';");
            strHTMLContent.Append("margin:0in;");
            strHTMLContent.Append("margin-bottom:.0001pt;");
            strHTMLContent.Append("mso-pagination:widow-orphan;");
            strHTMLContent.Append("font-size:12.0pt;");
            strHTMLContent.Append("font-family:'Times New Roman';");
            strHTMLContent.Append("mso-fareast-font-family:'Times New Roman';}");
            strHTMLContent.Append("p.page, li.page, div.page");
            strHTMLContent.Append("{mso-style-name:page;");
            strHTMLContent.Append("mso-margin-top-alt:auto;");
            strHTMLContent.Append("margin-right:0in;");
            strHTMLContent.Append("mso-margin-bottom-alt:auto;");
            strHTMLContent.Append("margin-left:0in;");
            strHTMLContent.Append("mso-pagination:widow-orphan;");
            strHTMLContent.Append("font-size:12.0pt;");
            strHTMLContent.Append("font-family:'Times New Roman';");
            strHTMLContent.Append("mso-fareast-font-family:'Times New Roman';}");
            strHTMLContent.Append("@page Section1");
            strHTMLContent.Append("{size:8.5in 11.0in;");
            strHTMLContent.Append("margin:1.0in 1.25in 1.0in 1.25in;");
            strHTMLContent.Append("mso-header-margin:.5in;");
            strHTMLContent.Append("mso-footer-margin:.5in;");
            strHTMLContent.Append("mso-paper-source:0;}");
            strHTMLContent.Append(".pageBreaks");
            strHTMLContent.Append("{");
            strHTMLContent.Append("page-break-before:always;");
            strHTMLContent.Append("clear:All;");
            strHTMLContent.Append("}");
            strHTMLContent.Append("-->");
            strHTMLContent.Append("</style>");
            strHTMLContent.Append("<!--[if gte mso 10]>");
            strHTMLContent.Append("<style>");
            strHTMLContent.Append("/* Style Definitions */");
            strHTMLContent.Append("table.MsoNormalTable");
            strHTMLContent.Append("{mso-style-name:'Table Normal';");
            strHTMLContent.Append("mso-tstyle-rowband-size:0;");
            strHTMLContent.Append("mso-tstyle-colband-size:0;");
            strHTMLContent.Append("mso-style-noshow:yes;");
            strHTMLContent.Append("mso-style-parent:'';");
            strHTMLContent.Append("mso-padding-alt:0in 5.4pt 0in 5.4pt;");
            strHTMLContent.Append("mso-para-margin:0in;");
            strHTMLContent.Append("mso-para-margin-bottom:.0001pt;");
            strHTMLContent.Append("mso-pagination:widow-orphan;");
            strHTMLContent.Append("font-size:10.0pt;");
            strHTMLContent.Append("font-family:'Times New Roman';");
            strHTMLContent.Append("mso-ansi-language:#0400;");
            strHTMLContent.Append("mso-fareast-language:#0400;");
            strHTMLContent.Append("mso-bidi-language:#0400;}");
            strHTMLContent.Append("</style>");
            strHTMLContent.Append("<![endif]-->");
            strHTMLContent.Append("</head>");
            strHTMLContent.Append("<body lang=EN-US style='tab-interval:.5in'>");
            foreach (T item in data)
            {
                strHTMLContent.Append(item);
                strHTMLContent.Append("<br class = pageBreaks />");
            }
            strHTMLContent.Append("</body>");
            strHTMLContent.Append("</html>");
            context.Response.Write(strHTMLContent);
            context.Response.End();
            context.Response.Flush();
        }

        public void ExporttoExcel(DataTable data, string fileName)
        {
            HttpContext context = HttpContext.Current;
            context.Response.Clear();
            context.Response.ContentType = "application/vnd.ms-excel";
            context.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName + ".xls");
            context.Response.ContentEncoding = Encoding.Unicode;
            context.Response.BinaryWrite(Encoding.Unicode.GetPreamble());
            context.Response.Charset = "";
            string strStyle =
                @"<style>table,td{border:thin solid black;border-collapse: collapse;} .text{mso-number-format:\@;}  .brtext{mso-data-placement:same-cell;}</style>  ";
            context.Response.Write(strStyle);
            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter htw = new HtmlTextWriter(sw))
                {
                    string exportHtmlString = "<table><thead><tr>";
                    for (int i = 0; i < data.Columns.Count; i++)
                    {
                        string propertyName;
                        propertyName = data.Columns[i].ColumnName.Replace("<br/>", " ").Replace("</br>", " ");
                        exportHtmlString += "<th>" + propertyName + "</th>";
                    }

                    exportHtmlString += "</tr></thead><tbody>";
                    context.Response.Write(exportHtmlString);
                    exportHtmlString = "";
                    CultureInfo culture = new CultureInfo("en-US");
                    foreach (DataRow row in data.Rows)
                    {
                        exportHtmlString += "<tr>";
                        for (int i = 0; i < data.Columns.Count; i++)
                        {
                            string propertyValue = Convert.ToString(row.ItemArray[i].ToString(), culture);
                            string propertyType = Convert.ToString(data.Columns[i].DataType.Name).ToLower();

                            if (!string.IsNullOrWhiteSpace(propertyValue))
                            {
                                if (propertyType.Contains("date"))
                                    propertyValue = string.Format(culture, "{0:d}", Convert.ToDateTime(propertyValue));
                                else if (propertyType.Contains("int"))
                                    propertyValue = string.Format(culture, "{0:n}", Convert.ToDecimal(propertyValue));
                                else if (propertyType.Contains("decimal"))
                                    propertyValue = string.Format(culture, "{0:n2}", Convert.ToDecimal(propertyValue));
                            }

                            //propertyValue = propertyValue.Replace("'", "\'");
                            propertyValue = HttpUtility.HtmlDecode(propertyValue).Replace("<br />", "br class='brtext'");
                            propertyValue = Regex.Replace((propertyValue), @"<[^>]*>", String.Empty).Replace("&nbsp;", " ");
                            propertyValue = propertyValue.Replace("br class='brtext'", "<br class='brtext'/>");
                            string classText = (propertyType.Contains("string") && propertyValue.Length < 256) ? " class='text'" : "";
                            exportHtmlString += "<td" + classText + ">" + propertyValue +
                                                "</td>";
                        }
                        exportHtmlString += "</tr>";
                        context.Response.Write(exportHtmlString);
                        exportHtmlString = "";
                    }

                    exportHtmlString += "</tbody></table>";
                    context.Response.Write(exportHtmlString);
                    context.Response.End();
                }
            }
        }

        #endregion

       
    }
}
