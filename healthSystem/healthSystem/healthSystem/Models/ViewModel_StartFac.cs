using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace healthSystem.Models {
    public class ViewModel_StartFac {
        public IEnumerable<Factory> Factory {
            get;
            set;
        }
        public IEnumerable<StartPlace> Start {
            get;
            set;
        }
        public int StartId {
            get;
            set;
        }
    }
}