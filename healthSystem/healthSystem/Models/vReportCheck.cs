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
    
    public partial class vReportCheck
    {
        public string ReportManage_id { get; set; }
        public string ReportManage_workNumber { get; set; }
        public Nullable<int> ReportManage_startId { get; set; }
        public Nullable<int> ReportManage_hospitalId { get; set; }
        public int ReportManage_programId { get; set; }
        public string ReportManage_medicalRecord { get; set; }
        public string ReportManage_notes { get; set; }
        public string ReportManage_IsDisable { get; set; }
        public string ReportManage_updateUser { get; set; }
        public Nullable<System.DateTime> ReportManage_updateTime { get; set; }
        public Nullable<int> report_id { get; set; }
        public string ReportCheckItem_employee_workNumber { get; set; }
        public string ReportCheckItem_checkDate { get; set; }
        public string checkYear { get; set; }
        public string ReportCheckItem_generalComment_1 { get; set; }
        public string ReportCheckItem_generalComment_2 { get; set; }
        public string ReportCheckItem_generalComment_3 { get; set; }
        public string ReportCheckItem_generalComment_4 { get; set; }
        public string ReportCheckItem_generalComment_5 { get; set; }
        public string ReportCheckItem_generalComment_6 { get; set; }
        public string ReportCheckItem_generalComment_7 { get; set; }
        public string ReportCheckItem_generalComment_8 { get; set; }
        public string ReportCheckItem_generalComment_9 { get; set; }
        public string ReportCheckItem_generalComment_10 { get; set; }
        public string ReportCheckItem_generalComment_11 { get; set; }
        public string ReportCheckItem_generalComment_12 { get; set; }
        public string ReportCheckItem_generalComment_13 { get; set; }
        public string ReportCheckItem_generalComment_14 { get; set; }
        public string ReportCheckItem_generalComment_15 { get; set; }
        public string ReportCheckItem_generalComment_16 { get; set; }
        public string ReportCheckItem_generalComment_17 { get; set; }
        public string ReportCheckItem_hw { get; set; }
        public string ReportCheckItem_pastHistory { get; set; }
        public string ReportCheckItem_symptoms { get; set; }
        public Nullable<double> ReportCheckItem_HeightValue { get; set; }
        public string ReportCheckItem_HVIsPass { get; set; }
        public Nullable<double> ReportCheckItem_WeightValue { get; set; }
        public string ReportCheckItem_WVIsPass { get; set; }
        public Nullable<double> ReportCheckItem_WaistValue { get; set; }
        public string ReportCheckItem_WaistIsPass { get; set; }
    }
}
