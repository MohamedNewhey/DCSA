//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DCSA.Database
{
    using System;
    using System.Collections.Generic;
    
    public partial class Image
    {
        public int ID { get; set; }
        public Nullable<int> No { get; set; }
        public string URL { get; set; }
        public Nullable<int> UserID { get; set; }
        public Nullable<bool> Publish { get; set; }
    
        public virtual User User { get; set; }
    }
}
