using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Web;
using System.Text.RegularExpressions;

namespace Laserbeam.UI.HR.Reports.ManagerReports
{
    public partial class IncraeaseDistributionManager : DevExpress.XtraReports.UI.XtraReport
    {
        decimal value = 0;
        decimal SalaryNew = 0;
        decimal SalaryCurrent=0;
        public IncraeaseDistributionManager(int loggedInEmpNum, int employeeNum, bool isRollup, bool isSelectedRollup, int UserNum,string updatedConnectionString)
        {
            InitializeComponent();
            sqlDataSource1.Connection.ConnectionString = updatedConnectionString;
            sqlDataSource2.Connection.ConnectionString = updatedConnectionString;
            this.EmpNum.Value = loggedInEmpNum;
            this.ManagerNum.Value = employeeNum;
            this.IsRollup.Value = isRollup;
            this.SelectedRollUp.Value = isSelectedRollup;
            this.UserNum.Value = UserNum;
        }

        private void GroupFooter1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {   
        }

        private void Detail1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
           
        }

        private void xrLabel4_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            var outputValue = string.Format("{0:#,0.00;(0.00);''}", value);
            this.xrLabel4.Text = outputValue != "" ? outputValue : "0.00";
            value = 0;
        }

        private void Detail1_AfterPrint(object sender, EventArgs e)
        {
            decimal newSalary = (this.xrLabel6.Text != null && this.xrLabel6.Text!="") ? Convert.ToDecimal(this.xrLabel6.Text) : 0;
            SalaryNew = SalaryNew + newSalary;
            decimal currentSalary = (this.xrLabel8.Text != null && this.xrLabel6.Text!="") ? Convert.ToDecimal(this.xrLabel8.Text) : 0;
            SalaryCurrent = SalaryCurrent + currentSalary;
            value = SalaryCurrent!=0 ?((SalaryNew - SalaryCurrent) / SalaryCurrent) * 100 : 0;
        }

    }
}
