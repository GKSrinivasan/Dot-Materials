namespace Laserbeam.BusinessObject.Common
{
    public class WorkFlowBudgetSpendCount
    {
        public int TotalEmpCount { get; set; }
        public int YetToSubmit { get; set; }
        public int Reopen { get; set; }
        public int Pending { get; set; }
        public int Approved { get; set; }
        public int OverSpend { get; set; }
        public int WihInBudget { get; set; }
        public int YetToSpend { get; set; }
        public int TotalEmployeeCount
        {
            get
            {
                return YetToSubmit + Reopen + Pending+ Approved;
            }
        }
       
    }
}
