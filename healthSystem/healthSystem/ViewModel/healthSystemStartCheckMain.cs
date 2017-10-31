using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using healthSystem.Models;

namespace healthSystem.ViewModel
{
    public class healthSystemStartCheckMain
    {
        public IEnumerable<StartCheck> startCheck
        {
            get;
            set;
        }
        public IEnumerable<StartPlace> startPlace
        {
            get;
            set;
        }
        public IEnumerable<StartFile> startFile
        {
            get;
            set;
        }
    }
}