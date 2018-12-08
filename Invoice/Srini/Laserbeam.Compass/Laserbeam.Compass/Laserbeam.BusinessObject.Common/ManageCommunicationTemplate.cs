using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laserbeam.BusinessObject.Common
{
    public class ManageCommunicationTemplate
    {
        public int AppEmailId { get; set; }
        public string EmailSubject { get; set; }
        public string EmailBody { get; set; }
        public int? ParentMenuNum { get; set; }
        public bool IsTreeTop { get; set; }
        public int OrderByTree { get; set; } 
    }
}
