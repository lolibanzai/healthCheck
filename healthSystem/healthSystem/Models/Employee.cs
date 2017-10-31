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
    
    public partial class Employee
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Employee()
        {
            this.Authority = new HashSet<Authority>();
            this.book = new HashSet<book>();
            this.InspectPerson = new HashSet<InspectPerson>();
            this.StartHand = new HashSet<StartHand>();
            this.ReportManage = new HashSet<ReportManage>();
            this.EmployeeWork = new HashSet<EmployeeWork>();
        }
    
        public string employee_workNumber { get; set; }
        public int employee_workId { get; set; }
        public string employee_name { get; set; }
        public string employee_identityCard { get; set; }
        public string employee_blood { get; set; }
        public string employee_gender { get; set; }
        public System.DateTime employee_dateOfBirth { get; set; }
        public int employee_age { get; set; }
        public string employee_groupId { get; set; }
        public string employee_corporation { get; set; }
        public string employee_team { get; set; }
        public string employee_factoryId { get; set; }
        public string employee_jobTitle { get; set; }
        public string employee_department { get; set; }
        public Nullable<System.DateTime> employee_managerDate { get; set; }
        public string employee_homeSite { get; set; }
        public string employee_IsAbroad { get; set; }
        public string employee_IaborId { get; set; }
        public System.DateTime employee_laborInsurance { get; set; }
        public System.DateTime employee_comingDate { get; set; }
        public Nullable<System.DateTime> employee_quitDate { get; set; }
        public string employee_accFactory { get; set; }
        public string employee_mailingAddress { get; set; }
        public string employee_cellphone { get; set; }
        public string employee_home { get; set; }
        public string employee_email { get; set; }
        public string employee_reportAddress { get; set; }
        public string employee_reportMobile { get; set; }
        public string employee_note { get; set; }
        public Nullable<System.DateTime> employee_changedDate { get; set; }
        public string employee_isDisabled { get; set; }
        public string employee_username { get; set; }
        public string employee_password { get; set; }
        public string employee_role { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Authority> Authority { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<book> book { get; set; }
        public virtual WorkInfo WorkInfo { get; set; }
        public virtual Factory Factory { get; set; }
        public virtual Factory Factory1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InspectPerson> InspectPerson { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StartHand> StartHand { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReportManage> ReportManage { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EmployeeWork> EmployeeWork { get; set; }
    }
}
