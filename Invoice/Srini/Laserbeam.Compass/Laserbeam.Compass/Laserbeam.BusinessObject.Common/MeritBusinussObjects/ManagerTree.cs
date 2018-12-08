using Laserbeam.Constant.HR;
namespace Laserbeam.BusinessObject.Common
{
    public class ManagerTree
    {
        public int? RowNumber { get; set; }
        public string ManagerName { get; set; }
        public int? ManagerNum { get; set; }
        public int? ManagerEmpJobNum { get; set; }
        public int? ReportingManagerNum { get; set; }
        public string ManagerLineage { get; set; }
        public bool IsTreeTop { get; set; }
        public int? ReporteeCount { get; set; }
        public int? CompCompletedCount { get; set; }
        public string SortOrderEmployeeName { get; set; }
        public int? MenuType { get; set; }
        public int? LoggedInEmployeeNum { get; set; }
        public int? OrderByManagerTree { get; set; }
        public int? ManagerLevelAppStatus { get; set; }
        public bool IsOverSpent { get; set; }
    }
}
