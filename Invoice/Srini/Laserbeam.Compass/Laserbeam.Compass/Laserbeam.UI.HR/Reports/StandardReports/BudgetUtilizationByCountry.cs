using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace Laserbeam.UI.HR.Reports.ManagerReports
{
    public partial class BudgetUtilizationByCountry : DevExpress.XtraReports.UI.XtraReport
    {
        public BudgetUtilizationByCountry(int loggedInEmpNum, int employeeNum, bool isRollup, bool isSelectedRollup, int UserNum,string updatedConnectionString)
        {
            InitializeComponent();
            sqlDataSource1.Connection.ConnectionString = updatedConnectionString;
            this.EmpNum.Value = loggedInEmpNum;
            this.ManagerNum.Value = employeeNum;
            this.IsRollUp.Value = isRollup;
            this.SelectedRollUp.Value = isSelectedRollup;
            this.UserNum.Value = UserNum;
        }

        private void Detail_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

        }

        private void Detail_BeforePrint_1(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
        }

    }
}
