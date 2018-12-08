namespace Laserbeam.BusinessObject.Common
{
    public class LevelWiseApprovalDetails
    {        
        public int ManagerNum { get; set; }        
        public string ManagerName { get; set; }
        public string EmployeeName { get; set; }        
        public int IsApprove { get; set; }
        public int IsReopen { get; set; }
        public int IsApproved { get; set; }
        public int YetToStart { get; set; }
    }
}
