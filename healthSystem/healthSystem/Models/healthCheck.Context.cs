﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class HealthCheckEntities1 : DbContext
    {
        public HealthCheckEntities1()
            : base("name=HealthCheckEntities1")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Authority> Authority { get; set; }
        public virtual DbSet<book> book { get; set; }
        public virtual DbSet<CheckCollect> CheckCollect { get; set; }
        public virtual DbSet<CheckItem> CheckItem { get; set; }
        public virtual DbSet<DoubleCheck> DoubleCheck { get; set; }
        public virtual DbSet<Employee> Employee { get; set; }
        public virtual DbSet<Factory> Factory { get; set; }
        public virtual DbSet<HealthNotice> HealthNotice { get; set; }
        public virtual DbSet<Hospital> Hospital { get; set; }
        public virtual DbSet<InspectPerson> InspectPerson { get; set; }
        public virtual DbSet<Program> Program { get; set; }
        public virtual DbSet<ReportFile> ReportFile { get; set; }
        public virtual DbSet<ReportManage> ReportManage { get; set; }
        public virtual DbSet<StartCheck> StartCheck { get; set; }
        public virtual DbSet<StartFile> StartFile { get; set; }
        public virtual DbSet<StartHand> StartHand { get; set; }
        public virtual DbSet<StartPlace> StartPlace { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<WorkInfo> WorkInfo { get; set; }
        public virtual DbSet<HospitalProgramFile> HospitalProgramFile { get; set; }
    }
}
