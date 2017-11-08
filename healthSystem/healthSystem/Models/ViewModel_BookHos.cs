using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using healthSystem.Models;

namespace healthSystem.Models
{
    public class ViewModel_BookHos
    {
        public IEnumerable<VW_BookEmp> GetW_BookEmp {
            set;
            get;
        }
        public IEnumerable<VW_HospitalProgram> GetW_HosProgram {
            set;
            get;
        }
    }
}