using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laserbeam.BusinessObject.Common
{
    public class AppEmailDetails
    {
        public int AppEmailID { get; set; }
        public string EmailSubject { get; set; }
        public string EmailBody { get; set; }
        public string EmailScript { get; set; }
        public string EmailDescr { get; set; }
        //public bool EmailType { get; set; }
        public bool Active { get; set; }
        public Nullable<byte> EmailOrderby { get; set; }
        public string EmailKey { get; set; }        
        public Nullable<bool> UserTemplate { get; set; }
    }
}
