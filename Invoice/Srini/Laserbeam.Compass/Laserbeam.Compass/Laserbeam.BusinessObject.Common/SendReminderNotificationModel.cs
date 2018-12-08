using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laserbeam.BusinessObject.Common
{
    public class SendReminderNotificationModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string EmailAddress { get; set; }
        public string Role { get; set;}
        public bool IsMessageSent { get; set; }
        public string SecretKey { get; set; }
        //public bool dirty { get; set; }
        //public string uid { get; set; }
        //public string id { get; set; }
    }
}
