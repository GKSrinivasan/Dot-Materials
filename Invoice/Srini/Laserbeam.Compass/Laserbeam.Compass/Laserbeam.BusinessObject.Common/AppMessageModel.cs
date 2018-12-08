using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laserbeam.BusinessObject.Common
{
    public class AppMessageModel
    {
        public int AppMessageID { get; set; }
        public string MessageSubject { get; set; }
        public string MessageBody { get; set; }
        public string MessageScript { get; set; }
        public string MessageDescr { get; set; }
        public bool MessageType { get; set; }
        public bool Active { get; set; }
        public Nullable<byte> MessageOrderby { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
    }
}
