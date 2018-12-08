using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laserbeam.BusinessObject.Common
{
    public class EmailDetailsModel
    {
        public string UserName { get; set; }
        public string EmailSubject { get; set; }
        public string EmailBody { get; set; }
        public int UserID { get; set; }
        public int UserRoleNum { get; set; }
        public string DefaultPassword { get; set; }
        public string EmailID { get; set; }
        public string ClientUserID { get; set; }
        public string Password { get; set; }
        public string AdminEmailID { get; set; }
        public string EmailCC { get; set; }
        public string EmailPurpose { get; set; }
        public int WorkYear { get; set; }
        public string AdminPassword { get; set; }
        public int? SMTPPort { get; set; }
        public string SMTPServer { get; set; }
        public int AppEmailID { get; set; }
        public string UserGrp { get; set; }
        public int UserNum { get; set; }
        public int UserGrpNum { get; set; }
        public bool IsChanged { get; set; }
        public bool chkd { get; set; }
        public string hdnGrp { get; set; }
        public string ManagerName { get; set; }
        public string EmployeeName { get; set; }
        public string FromEmailID { get; set; }
        public string ToEmailID { get; set; }
        public System.DateTime SentDateTime { get; set; }
        public bool Success { get; set; }
        public string SuccessStatus { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public string ErrorDescription { get; set; }
        public int TotalEmailCount { get; set; }
        public int SuccessEmailCount { get; set; }
        public int ManagerNum { get; set; }
        public string EmployeeType { get; set; }
        public string EmployeeList { get; set; }
        public string ManagerID { get; set; }
        public int OrgGroupID { get; set; }
        public bool IsMessageSent { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedDateForEmail { get; set; }
        public string SecretKey { get; set; }
        public string referenceUrl { get; set; }
    }
}
