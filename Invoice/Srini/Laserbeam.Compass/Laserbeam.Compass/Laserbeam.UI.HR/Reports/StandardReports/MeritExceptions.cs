using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Web;
using System.Text.RegularExpressions;

namespace Laserbeam.UI.HR.Reports.ManagerReports
{
    public partial class MeritExceptions : DevExpress.XtraReports.UI.XtraReport
    {
        public MeritExceptions(int loggedInEmpNum, int employeeNum, bool isRollup, bool isSelectedRollup, int UserNum,string updatedConnectionString)
        {
            InitializeComponent();
            sqlDataSource1.Connection.ConnectionString = updatedConnectionString;
            this.EmpNum.Value = loggedInEmpNum;
            this.ManagerNum.Value = employeeNum;
            this.RollUp.Value = isRollup;
            this.SelectedRollUp.Value = isSelectedRollup;
            this.UserNum.Value = UserNum;
        }

        private void Detail_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            this.xrRichText1.Html = GetCurrentColumnValue("Comments") != null ? "<div style='text-align:left;vertical-align:middle;font-family:Calibri;font-size:10pt;padding-top:10%;'>" + Regex.Replace(HttpUtility.HtmlDecode(HttpUtility.HtmlDecode(HttpUtility.HtmlDecode(GetCurrentColumnValue("Comments").ToString()))), @"<[^>]+>|&nbsp;", "").Trim().Replace("\\e\\v", "<span style='font-style:italic !important;'>").Replace("\\e\\e", "<span style=' font-style:normal !important;'>").Replace("\\r\\n", "<br/>") + "</div>" : "";
        }

    }
}
