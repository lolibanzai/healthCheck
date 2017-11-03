using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using healthSystem.Models;

namespace healthSystem.Models
{
    public class ViewModel_BookEmp
    {
        public IEnumerable<VW_BookEmp> BookView
        {
            get;
            set;
        }
        public IEnumerable<VW_EmpAuth> EmpView
        {
            get;
            set;
        }
        public IEnumerable<Employee> Emp
        {
            get;
            set;
        }
    }
}