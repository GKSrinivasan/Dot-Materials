using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laserbeam.BusinessObject.Common
{
    public class TaskModel
    {
        public string TaskTitle { get; set; }
        public int TeamNum { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string TaskDescription { get; set; }
        public int TaskNum { get; set; }
        public string HoursSpent { get; set; }
   }
}
