using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace Laserbeam.UI.HR.Reports.ManagerReports
{
    public partial class ManagerBudgetByFunction : DevExpress.XtraReports.UI.XtraReport
    {
        decimal val = 0 ;
        decimal budgetAmt = 0;
        decimal spentAmt = 0;
        public ManagerBudgetByFunction(int loggedInEmpNum, int employeeNum, bool isRollup, bool isSelectedRollup, int UserNum,string updatedConnectionString)
        {
            InitializeComponent();
            sqlDataSource1.Connection.ConnectionString = updatedConnectionString;
            this.EmpNum.Value = loggedInEmpNum;
            this.ManagerNum.Value = employeeNum;
            this.RollUp.Value = isRollup;
            this.SelectedRollUp.Value = isSelectedRollup;
            this.UserNum.Value = UserNum;
        }

        private void GroupFooter2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
           
        }

        private void ManagerBudgetByFunction_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
     
        }

        private void xrLabel5_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            var outputValue = this.xrLabel5.Text = string.Format("{0:0.00;(0.00);''}", val);
            this.xrLabel5.Text = string.Format("{0:0.00;(0.00);''}", val) !="" ? string.Format("{0:#,0.00;(0.00);''}", val) :"0.00" ;
            val = 0;
            budgetAmt = 0;
            spentAmt = 0;
        }

        private void Detail_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
        }

        private void Detail_AfterPrint(object sender, EventArgs e)
        {
        }

        private void Detail_AfterPrint_1(object sender, EventArgs e)
        {
            decimal Budget = this.xrLabel8.Text != "" ? Convert.ToDecimal(this.xrLabel8.Text) : 0;
            budgetAmt = budgetAmt + Budget;
            decimal Spent = this.xrLabel9.Text != "" ? Convert.ToDecimal(this.xrLabel9.Text) : 0;
            spentAmt = spentAmt + Spent;
            val = budgetAmt != 0 ? (spentAmt / budgetAmt) * 100 : 0;
        }

    }
}
