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
    
    public partial class ManagerRelation
    {
        public int Node { get; set; }
        public int EmployeeNum { get; set; }
        public Nullable<int> ParentNode { get; set; }
        public Nullable<byte> Depth { get; set; }
        public string Lineage { get; set; }
        public Nullable<int> ManagerNum { get; set; }
        public string ManagerLineage { get; set; }
    
        public virtual Employee Employee { get; set; }
    }
}
