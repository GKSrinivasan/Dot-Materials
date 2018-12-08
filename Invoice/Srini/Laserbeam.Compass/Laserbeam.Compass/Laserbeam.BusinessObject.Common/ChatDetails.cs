using System;
using System.ComponentModel.DataAnnotations;

namespace Laserbeam.BusinessObject.Common
{
    public class ChatDetails
    {
        public Nullable<int> SenderUserNum { get; set; }
        public string SenderUserName { get; set; }
        public string SenderUserShortName { get; set; }
        public int userChatStatus { get; set; }
        public string SessionID { get; set; }
        public string Token { get; set; }
        public int ApiKey { get; set; }
        public string Chat { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string Time { get; set; }
        public string Attachment { get; set; }
        public int FileType { get; set; }
        public string FileName { get; set; }
        public int ChatDetailNum { get; set; }
        public string FilePath { get; set; }
    }
}
