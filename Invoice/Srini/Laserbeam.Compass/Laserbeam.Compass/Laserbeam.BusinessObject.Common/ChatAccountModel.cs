using Laserbeam.EntityManager.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laserbeam.BusinessObject.Common
{
    public class ChatAccountModel
    {
        public int EmployeeNum { get; set; }
        public int Usernum { get; set; }
        public string UserName { get; set; }
        public int UserStatus { get; set; }
        public int unReadChatCount { get; set; }
        public string sessionID { get; set; }
        public int ApiKey { get; set; }
        public string token { get; set; }

    }
}
