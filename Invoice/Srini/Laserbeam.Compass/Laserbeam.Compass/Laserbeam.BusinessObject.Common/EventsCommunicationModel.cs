using System;

namespace Laserbeam.BusinessObject.Common
{
    public class EventsCommunicationModel
    {        
        public string EmailSubject { get; set; }
        public string EmailBody { get; set; }        
        public string UpdatedBy { get; set; }        
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string DateFormat { get; set; }
    }
}
