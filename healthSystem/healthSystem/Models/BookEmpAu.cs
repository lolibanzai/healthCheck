using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using healthSystem.Models;

namespace healthSystem.Models
{
    public class BookEmpAu
    {
        HealthCheckEntities1 db = new HealthCheckEntities1();
        public IEnumerable<VW_BookEmp> BookEmp
        {
            get;
            set;
        }
        public IEnumerable<VW_EmpAuth> EmpAuth
        {
            get;
            set;
        }
    }
}