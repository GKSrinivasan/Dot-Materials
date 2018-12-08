using System.Collections.Generic;
namespace Laserbeam.BusinessObject.Common
{
    public class EmployeeTitleModel
    {
        public string EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string UserNameShort { get; set; }        
        public string JobTitle { get; set; }
        public string Location { get; set; }
        public string ImagePath { get; set; }
        public string Collobration { get; set; }
        public int Usernum { get; set; }
        public string UserName { get; set; }
        public string UserRole { get; set; }
        public string UserText { get; set; }
        public string ModuleName { get; set; }
        public string Instance { get; set; }
        public List<MyApproval> MyApprovals { get;set; }
        public int ApprovalCount { get; set; }
        public int ChatUnreadCount { get; set; }
    }
}