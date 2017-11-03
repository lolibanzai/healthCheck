using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using healthSystem.Models;

namespace healthSystem.ViewModel
{
    public class healthSystemCheckCollectMaster
    {
        public IEnumerable<StartCheck> beginSstartCheck
        {
            set;
            get;
        }
        public IEnumerable<StartCheck> startCheck
        {
            set;
            get;
        }
    }
}