//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Laserbeam.EntityManager.Common
{
    using System;
    using System.Collections.Generic;
    
    public partial class ChatDetail
    {
        public int ChatDetailNum { get; set; }
        public Nullable<int> SenderUserNum { get; set; }
        public Nullable<int> ReceiverUserNum { get; set; }
        public string Chat { get; set; }
        public Nullable<bool> ViewedStatus { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string Attachement { get; set; }
        public Nullable<int> FileType { get; set; }
        public string FileName { get; set; }
    }
}