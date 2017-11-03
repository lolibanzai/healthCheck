using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using healthSystem.Models;

namespace healthSystem.Models
{
    public class ViewModel_empAu
    {
        public IEnumerable<Employee> EmpView {
            get;
            set;
        }
        public IEnumerable<Authority> AuthView {
            get;
            set;
        }
        public IEnumerable<Factory> Factory {
            get;
            set;
        }
    }
}