using System.Collections.Generic;

namespace Laserbeam.BusinessObject.Common
{
   public class CompensationGridModel
    {
      public List<CompensationReportees> CompensationReportee { get; set; }      
      public int compensationReporteesTotalCount { get; set; }
      public bool CompleteCompCommentsFlag { get; set; }
      public int CompCompleteCommentCount { get; set; }
      public int CompCompletedcount { get; set; }
    }
}
