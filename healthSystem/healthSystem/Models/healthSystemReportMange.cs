using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using healthSystem.Models;

namespace healthSystem.ViewModel
{
    public class healthSystemReportMange
    {
        public IEnumerable<ReportManage> Manage { set; get; }
        public IEnumerable<ReportCheckItem> reportCheckItem { set; get; }

        public IEnumerable<vReportCheck> Report { set; get; }

        public List<ReportManage> reportList { set; get; }
    }

}