using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using healthSystem.Models;

namespace healthSystem.ViewModel
{
    public class healthSystemStartCheckMaster
    {
        public IEnumerable<int> year
        {
            get;
            set;
        }
        public IEnumerable<StartCheck> startCheck
        {
            get;
            set;
        }
    }
}