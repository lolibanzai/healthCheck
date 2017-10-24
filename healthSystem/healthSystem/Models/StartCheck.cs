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
    
    public partial class StartCheck
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public StartCheck()
        {
            this.CheckCollect = new HashSet<CheckCollect>();
            this.ReportManage = new HashSet<ReportManage>();
            this.StartFile = new HashSet<StartFile>();
            this.StartHand = new HashSet<StartHand>();
            this.StartPlace = new HashSet<StartPlace>();
        }
    
        public int Start_id { get; set; }
        public int start_year { get; set; }
        public Nullable<System.DateTime> start_startDate { get; set; }
        public Nullable<System.DateTime> start_CheckEndDate { get; set; }
        public string start_principal { get; set; }
        public string start_state { get; set; }
        public string start_note { get; set; }
        public string start_updateUser { get; set; }
        public Nullable<System.DateTime> start_updateDate { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CheckCollect> CheckCollect { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReportManage> ReportManage { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StartFile> StartFile { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StartHand> StartHand { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StartPlace> StartPlace { get; set; }
    }
}
