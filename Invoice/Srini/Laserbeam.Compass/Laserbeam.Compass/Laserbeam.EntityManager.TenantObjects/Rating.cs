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
    
    public partial class Rating
    {
        public Rating()
        {
            this.EmployeeCompRatings = new HashSet<EmployeeCompRating>();
            this.EmployeeCompRatings1 = new HashSet<EmployeeCompRating>();
        }
    
        public int RatingNum { get; set; }
        public string RatingID { get; set; }
        public string RatingDescr { get; set; }
        public string RatingType { get; set; }
        public Nullable<bool> IsCommentMandatory { get; set; }
        public string RatingDetailDescr { get; set; }
        public string HighRange { get; set; }
        public string LowRange { get; set; }
        public Nullable<int> RatingOrder { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string MinRange { get; set; }
        public string MaxRange { get; set; }
        public string RatingRange { get; set; }
    
        public virtual ICollection<EmployeeCompRating> EmployeeCompRatings { get; set; }
        public virtual ICollection<EmployeeCompRating> EmployeeCompRatings1 { get; set; }
    }
}