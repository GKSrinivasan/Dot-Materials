
using Laserbeam.BusinessObject.Common.MeritBusinussObjects;
using System.Collections.Generic;
namespace Laserbeam.BusinessObject.Common
{
    public class CompensationReporteesCount
    {
        public List<MeritGridModel> CompensationReportee { get; set; }
        public int CompCompletedcount { get; set; }
        public int Totalcount { get; set; }
        public bool IsEmployeeLocked { get; set; }
        public int TotalGridcount { get; set; }
        public bool IsCompCompleteComments { get; set; }
        public int CompCommentCount { get; set; } 

    }
}
