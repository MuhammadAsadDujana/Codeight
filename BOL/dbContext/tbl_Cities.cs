//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BOL.dbContext
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbl_Cities
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_Cities()
        {
            this.tbl_Users = new HashSet<tbl_Users>();
        }
    
        public int CityId { get; set; }
        public string CityName { get; set; }
        public Nullable<int> StateId { get; set; }
        public Nullable<int> is_launched { get; set; }
        public Nullable<int> drivers_count { get; set; }
        public Nullable<decimal> toll_charges { get; set; }
        public Nullable<bool> IsActive { get; set; }
    
        public virtual tbl_States tbl_States { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_Users> tbl_Users { get; set; }
    }
}
