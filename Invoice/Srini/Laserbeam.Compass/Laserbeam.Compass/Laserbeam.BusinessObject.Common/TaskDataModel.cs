using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laserbeam.BusinessObject.Common
{
    public class TaskDataModel
    {
        public string Task { get; set; }
        public string TeamName { get; set; }
        public string HoursSpent { get; set; }
        public DateTime Date { get; set; }
        public int SupportTaskNum { get; set; }
        public string TotalSpent { get; set; }
        public string Hours { get; set; }
        public string Minutes { get; set; }
    }
}
