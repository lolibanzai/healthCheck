using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using healthSystem.Models;

namespace healthSystem.Models
{
    public class ViewModel_BookAction
    {
        public IEnumerable<VW_BookEmp> GetW_BookEmp {
            set;
            get;
        }
        public int GetAction {
            set;
            get;
        }
    }
}