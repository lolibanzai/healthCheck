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
    
    public partial class Book
    {
        public string book_id { get; set; }
        public string book_workNumber { get; set; }
        public string book_mobile { get; set; }
        public string book_state { get; set; }
        public Nullable<System.DateTime> book_Date { get; set; }
        public int book_hospitalId { get; set; }
        public int book_ProgramId { get; set; }
        public Nullable<int> book_special { get; set; }
        public string book_exceptDate { get; set; }
        public Nullable<System.DateTime> book_healthDate { get; set; }
        public Nullable<System.DateTime> book_costDate { get; set; }
        public Nullable<int> book_cost { get; set; }
        public string book_note { get; set; }
        public Nullable<int> book_Notice { get; set; }
        public string book_medicalHistory { get; set; }
        public string book_EatAspririn { get; set; }
        public string book_IsAllergy { get; set; }
        public string book_IsDisable { get; set; }
        public string book_updateUser { get; set; }
        public Nullable<System.DateTime> book_updateDate { get; set; }
        public Nullable<int> book_welfare { get; set; }
        public string book_host { get; set; }
        public Nullable<System.DateTime> book_hostTime { get; set; }
        public Nullable<int> book_serialNumber { get; set; }
    
        public virtual Hospital Hospital { get; set; }
        public virtual HealthNotice HealthNotice { get; set; }
        public virtual Program Program { get; set; }
        public virtual Program Program1 { get; set; }
        public virtual Employee Employee { get; set; }
    }
}
