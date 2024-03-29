//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace WGHotel.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Hotel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Hotel()
        {
            this.Room = new HashSet<Room>();
        }
    
        public int ID { get; set; }
        public string Name { get; set; }
        public string Game { get; set; }
        public int City { get; set; }
        public string Area { get; set; }
        public string Address { get; set; }
        public string Features { get; set; }
        public string LinkUrl { get; set; }
        public string Facilities { get; set; }
        public Nullable<bool> Enabled { get; set; }
        public int UserId { get; set; }
        public string Tel { get; set; }
        public Nullable<int> ParentId { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Room> Room { get; set; }
    }
}
