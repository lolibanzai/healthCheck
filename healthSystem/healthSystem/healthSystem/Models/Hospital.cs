//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace healthSystem.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Hospital
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Hospital()
        {
            this.Program = new HashSet<Program>();
            this.HospitalProgramFile = new HashSet<HospitalProgramFile>();
            this.Book = new HashSet<Book>();
        }
    
        public int hospital_hospitalId { get; set; }
        public string hospital_name { get; set; }
        public string hospital_address { get; set; }
        public string hospital_contact { get; set; }
        public string hospital_phone { get; set; }
        public string hospital_uniform { get; set; }
        public string hospital_website { get; set; }
        public string hospital_email { get; set; }
        public string hospital_state { get; set; }
        public string hospital_updateuser { get; set; }
        public Nullable<System.DateTime> hospital_updatetime { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Program> Program { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HospitalProgramFile> HospitalProgramFile { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Book> Book { get; set; }
    }
}
