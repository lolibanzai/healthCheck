using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using healthSystem.Models;

namespace healthSystem.Models {
    public class VWModel_BookEmp {
        public IEnumerable<Book> BookRow {
            set;
            get;
        }
        public IEnumerable<Employee> EmpRow {
            set;
            get;
        }
        public IEnumerable<VW_HospitalProgram> HosProg {
            set;
            get;
        }
    }
}