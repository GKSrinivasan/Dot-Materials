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
    
    public partial class AppScript
    {
        public AppScript()
        {
            this.ProxyScripts = new HashSet<ProxyScript>();
        }
    
        public int AppScriptID { get; set; }
        public string ScriptName { get; set; }
        public string Script { get; set; }
        public string ScriptDescr { get; set; }
        public byte ScriptGroup { get; set; }
        public Nullable<byte> ScriptOrderBy { get; set; }
        public Nullable<bool> Active { get; set; }
    
        public virtual ICollection<ProxyScript> ProxyScripts { get; set; }
    }
}
