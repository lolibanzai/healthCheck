using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using healthSystem.Models;

namespace healthSystem.ViewModel
{
    public class basicDataEmployeeMaster
    {
        public IEnumerable<VW_EmpWorkInfo> vW_EmpWorkInfo
        {
            get;
            set;
        }
        public IEnumerable<WorkInfo> workInfo
        {
            get;
            set;
        }
        public IEnumerable<Factory> factory
        {
            get;
            set;
        }
    }
}