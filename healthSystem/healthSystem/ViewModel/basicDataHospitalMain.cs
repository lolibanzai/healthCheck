using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using healthSystem.Models;

namespace healthSystem.ViewModel
{
    public class basicDataHospitalMain
    {
        public IEnumerable<Hospital> hospital
        {
            get;
            set;
        }
        public IEnumerable<Program> program
        {
            get;
            set;
        }
        public IEnumerable<HospitalProgramFile> hospitalProgramFile
        {
            get;
            set;
        }
    }
}