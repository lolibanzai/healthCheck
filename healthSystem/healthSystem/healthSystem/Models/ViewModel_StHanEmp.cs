using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using healthSystem.Models;

namespace healthSystem.Models
{
    public class ViewModel_StHanEmp
    {
        HealthCheckEntities1 db = new HealthCheckEntities1();
        public IEnumerable<Employee> Employee
        {
            get;
            set;
        }
        public int StartId
        {
            get;
            set;
        }
    }
}