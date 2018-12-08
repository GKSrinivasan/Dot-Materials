
using ClosedXML.Excel;
using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Laserbeam.UI.HR.Common
{
    public class ExportExcel
    {       

        // Author         :   Raja Ganapathy
        // Creation Date  :   03-Mar-2017
        /// <summary>
        /// Export datatable to excel using ClosedXML
        /// </summary>
        /// <param name="table">Instance of DataTable</param>
        /// <param name="fileName">FileName as string</param>
        /// <param name="sheetName">sheetName as string</param>
        public static void ToExcel(DataTable table, string fileName,string sheetName = "Sheet1")
        {
            CultureInfo culture = new CultureInfo("en-US");
            using (XLWorkbook wbook = new XLWorkbook())
            {
                using (IXLWorksheet workSheet = wbook.AddWorksheet(sheetName))
                {
                    for (int i = 0; i < table.Columns.Count; i++)
                    {
                        workSheet.Cell(1, i + 1).Value = table.Columns[i].ColumnName;
                        workSheet.Cell(1, i + 1).Style.Alignment.WrapText = true;
                        workSheet.Cell(1, i + 1).Style.Font.SetFontSize(9);
                        workSheet.Cell(1, i + 1).Style.Font.SetBold(true);
                        workSheet.Cell(1, i + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    }

                    for (int rowNum = 0; rowNum < table.Rows.Count; rowNum++)
                    {
                        for (int i = 0; i < table.Columns.Count; i++)
                        {
                            var propertyValue = Convert.ToString(table.Rows[rowNum].ItemArray[i].ToString(), culture);
                            string propertyType = Convert.ToString(table.Columns[i].DataType.Name).ToLower();
                            if (!string.IsNullOrWhiteSpace(propertyValue))
                            {
                                if (propertyType.Contains("date"))
                                    propertyValue = string.Format("{0:d}", Convert.ToDateTime(propertyValue));
                                else if (propertyType.Contains("int"))
                                    propertyValue = string.Format("{0:n0}", Convert.ToInt32(propertyValue));
                                else if (propertyType.Contains("decimal"))
                                    propertyValue = string.Format("{0:n2}", Convert.ToDecimal(propertyValue));
                            }
                            propertyValue = HttpUtility.HtmlDecode(propertyValue).Replace("<br/>", "br class='brtext'");
                            propertyValue = Regex.Replace((propertyValue), @"<[^>]*>", String.Empty).Replace("&nbsp;", " ");
                            propertyValue = propertyValue.Replace("br class='brtext'", Environment.NewLine);
                            workSheet.Cell(rowNum + 2, i + 1).SetValue(propertyValue);
                            workSheet.Cell(rowNum + 2, i + 1).Style.Alignment.WrapText = true;
                            workSheet.Cell(rowNum + 2, i + 1).Style.Font.SetFontSize(9);
                        }
                    }


                    workSheet.Columns().AdjustToContents();
                    for (int i = 0; i < table.Columns.Count; i++)
                    {
                        var propertyHeaderValue = table.Columns[i].ColumnName.ToLower();
                        if (propertyHeaderValue.Contains("desc") || propertyHeaderValue.Contains("comment"))
                            workSheet.Column(i + 1).Width = 40;
                    }
                    HttpResponse response = HttpContext.Current.Response;
                    response.Clear();
                    response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    response.AddHeader("Content-Disposition", "attachment; filename=\"" + fileName + ".xlsx\"");
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        wbook.SaveAs(memoryStream);
                        memoryStream.WriteTo(response.OutputStream);
                        memoryStream.Close();

                    }
                    response.End();
                }
            }
        }

        
        /// <summary>
        /// Export datatable to excel using ClosedXML
        /// </summary>
        /// <param name="table">Instance of DataTable</param>
        /// <param name="fileName">FileName as string</param>
        public static void ToExcelMultiSheet(DataSet dataSet, string fileName)
        {
            DataTable table = dataSet.Tables[0];
            DataTable table2 = dataSet.Tables[1];
            CultureInfo culture = new CultureInfo("en-US");
            XLWorkbook wbook = new XLWorkbook();
            var workSheet = wbook.Worksheets.Add("Employee level budget breakup");
            var workSheet1 = wbook.Worksheets.Add(" Manager level budget breakup");
            for (int i = 0; i < table.Columns.Count; i++)
            {
                workSheet.Cell(1, i + 1).Value = table.Columns[i].ColumnName;
            }

            for (int rowNum = 0; rowNum < table.Rows.Count; rowNum++)
            {
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    var propertyValue = Convert.ToString(table.Rows[rowNum].ItemArray[i].ToString(), culture);
                    workSheet.Cell(rowNum + 2, i + 1).SetValue(propertyValue);
                }
            }
            for (int i = 0; i < table2.Columns.Count; i++)
            {
                workSheet1.Cell(1, i + 1).Value = table2.Columns[i].ColumnName;
            }

            for (int rowNum = 0; rowNum < table2.Rows.Count; rowNum++)
            {
                for (int i = 0; i < table2.Columns.Count; i++)
                {
                    var propertyValue = Convert.ToString(table2.Rows[rowNum].ItemArray[i].ToString(), culture);
                    workSheet1.Cell(rowNum + 2, i + 1).SetValue(propertyValue);
                }
            }
           
            HttpResponse response = HttpContext.Current.Response;
            response.Clear();
            response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            response.AddHeader("Content-Disposition", "attachment; filename=\"" + fileName + ".xlsx\"");
            using (MemoryStream memoryStream = new MemoryStream())
            {
                wbook.SaveAs(memoryStream);
                memoryStream.WriteTo(response.OutputStream);
                memoryStream.Close();
            }
            response.End();
        }

        
    }
}