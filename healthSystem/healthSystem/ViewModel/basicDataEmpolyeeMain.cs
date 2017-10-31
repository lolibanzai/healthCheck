using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using healthSystem.Models;

namespace healthSystem.ViewModel
{
    public class basicDataEmpolyeeMain
    {
        public IEnumerable<Employee> employee
        {
            get;
            set;
        }
        public IEnumerable<WorkInfo> workInfo
        {
            get;
            set;
        }
        public IEnumerable<ReportManage> reportManage
        {
            get;
            set;
        }
        public IEnumerable<VW_EmpWorkInfo> vW_EmpWorkInfo
        {
            get;
            set;
        }
    }
}