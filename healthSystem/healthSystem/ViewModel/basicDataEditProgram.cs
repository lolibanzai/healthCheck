using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using healthSystem.Models;

namespace healthSystem.ViewModel
{
    public class basicDataEditProgram
    {
        public IEnumerable<Program> program
        {
            get;
            set;
        }
        public IEnumerable<WorkInfo> workInfo
        {
            get;
            set;
        }
    }
}