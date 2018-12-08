using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laserbeam.BusinessObject.Common
{
   public class TemplateDataModel
    {
        public string XmlFileName { get; set; }
        public int? RecordCount { get; set; }
        public int? ProcessedCount { get; set; }
        public string ProcessDate { get; set; }
        public string UserName { get; set; }
        public bool IsProcessed { get; set; }
        public int XMLProcessNum { get; set; }
    }
}
