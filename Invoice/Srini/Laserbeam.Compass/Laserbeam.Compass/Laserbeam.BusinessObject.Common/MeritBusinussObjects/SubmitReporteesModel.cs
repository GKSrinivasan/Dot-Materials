namespace Laserbeam.BusinessObject.Common.MeritBusinussObjects
{
    public class SubmitReporteesModel
    {
        public int EmpJobNum { get; set; }
        public string EmployeeName { get; set; }
        public string NextLevelManagerName { get; set; }
        public int TopLevelManager { get; set; }
        public int ApprovalLevel { get; set; }
        public string EmployeeID { get; set; }
        public bool SubmitIsChecked { get; set; }
        public bool ApprovalIsChecked { get; set; }
    }
}
