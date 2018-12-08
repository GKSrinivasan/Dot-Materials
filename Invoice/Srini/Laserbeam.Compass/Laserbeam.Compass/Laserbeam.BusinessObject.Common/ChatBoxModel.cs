using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laserbeam.BusinessObject.Common
{
    public class ChatBoxModel
    {
        
        public List<ChatAccountModel> chatAccountModel { get; set; }
        public List<ChatStatus> chatStatusList { get; set; }
        public int UserStatus { get; set; }
        public int loggeedInUserNum { get; set; }
        public string loggedInUserName { get; set; }
        public string loggedInUserShortName { get; set; }      
        //public int ApiKey { get; set; }
        //public string SessionId { get; set; }
        //public string Token { get; set; }
    }
}
