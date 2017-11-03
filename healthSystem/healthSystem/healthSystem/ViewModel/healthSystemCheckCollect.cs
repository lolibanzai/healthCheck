using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using healthSystem.Models;

namespace healthSystem.ViewModel
{
    public class healthSystemCheckCollect
    {
        public IEnumerable<StartHand> startHand
        {
            get;
            set;
        }
        public IEnumerable<StartPlace> startPlace
        {
            get;
            set;
        }
        public IEnumerable<Employee> employee
        {
            get;
            set;
        }
        public int StartID
        {
            get;
            set;
        }
    }
}